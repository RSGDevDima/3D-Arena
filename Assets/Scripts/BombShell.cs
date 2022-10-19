using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombShell : MonoBehaviour
{
    [SerializeField] float _lifeTime = 10f;
    [SerializeField] float _minDamage = 15f;
    [SerializeField] float _maxDamage = 30f;

    private bool _isRebounded = false;
    private Coroutine _movingProcess = null;
    private float _speed;

    private void OnEnable()
    {
        if (_minDamage > _maxDamage || (_maxDamage <= 0 || _minDamage <= 0))
        {
            _maxDamage = _minDamage;
            _minDamage = 1;
        }
    }

    public void MoveInDirection(Vector3 direction, float speed)
    {
        this._speed = speed;
        StartMovingProcess(direction, speed);
        StartCoroutine(TimerForDestroy());
    }

    private void StartMovingProcess(Vector3 direction, float speed)
    {
        if (_movingProcess != null)
        {
            StopCoroutine(_movingProcess);
        }

        _movingProcess = StartCoroutine(MoveInDirectionProcess(direction, speed));
    }

    private IEnumerator MoveInDirectionProcess(Vector3 direction, float speed)
    {
        while (true)
        {
            transform.position += direction * speed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator TimerForDestroy()
    {
        yield return new WaitForSeconds(_lifeTime);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;

        if (tag == "TeleportZone")
            return;

        if (tag != "BlueEnemy" && tag != "RedEnemy")
        {
            SpawnParticle("ExplosionParticle");
            gameObject.SetActive(false);
            return;
        }

        SpawnParticle("HitParticle");

        GlobalEventManager.OnEnemyDamage.Fire();
        HandleDamage(other);
    }

    private void HandleDamage(Collider obj)
    {
        Enemy enemy = obj.gameObject.GetComponent<Enemy>();

        float enemyMaxHealth = enemy.MaxHealth;
        float remainEnemyHealth;

        float damage = GetDamage();
        enemy.ApplyHealthChanges(-damage, out remainEnemyHealth);

        if (remainEnemyHealth <= 0 && _isRebounded)
        {
            GlobalEventManager.OnExtraDeath.Fire();
            gameObject.SetActive(false);

            return;
        }

        HandleExtra(remainEnemyHealth, enemyMaxHealth);
    }

    private void HandleExtra(float remainEnemyHealth, float enemyMaxHealth)
    {
        float extraChance = (100 - (remainEnemyHealth / enemyMaxHealth * 100)) / 2;
        bool isExtra = Random.Range(0, 100) <= extraChance;

        if (remainEnemyHealth <= 0 && isExtra)
        {
            _isRebounded = true;

            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit);

            Vector3 surfaceNormal = hit.normal;
            float hitAngle = (float)(Vector3.AngleBetween(transform.forward, surfaceNormal) * 180 / 3.14);

            if (hitAngle > 100 && hitAngle < 150)
            {
                // new shell direction
                Vector3 reboundedDirection = Vector3.Reflect(transform.forward, surfaceNormal);
                StartMovingProcess(reboundedDirection, _speed / 3);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private float GetDamage()
    {
        return Random.Range(_minDamage, _maxDamage);
    }

    private void SpawnParticle(string particleTag)
    {
        GameObject particle = ObjectPooller.Current.GetPooledObject(particleTag);
        particle.transform.position = transform.position;
        particle.SetActive(true);
    }
}
