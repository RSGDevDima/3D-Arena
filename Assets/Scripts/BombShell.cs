using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombShell : MonoBehaviour
{
    [SerializeField] float _lifeTime = 10f;
    [SerializeField] float _minDamage = 15f;
    [SerializeField] float _maxDamage = 30f;

    bool isRebounded = false;
    Coroutine movingProcess = null;
    float speed;

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
        this.speed = speed;
        startMovingProcess(direction, speed);
        StartCoroutine(timerForDestroy());
    }

    void startMovingProcess(Vector3 direction, float speed){
        if(movingProcess != null){
            StopCoroutine(movingProcess);
        }

        movingProcess = StartCoroutine(moveInDirectionProcess(direction, speed));
    }

    IEnumerator moveInDirectionProcess(Vector3 direction, float speed)
    {
        while (true)
        {
            transform.position += direction * speed * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator timerForDestroy()
    {
        yield return new WaitForSeconds(_lifeTime);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;

        if (tag == "TeleportZone") return;
        if (tag != "BlueEnemy" && tag != "RedEnemy")
        {
            gameObject.SetActive(false);
            return;
        }

        handleDamage(other);
    }

    void handleDamage(Collider obj)
    {
        Debug.Log("Handle damage");
        Enemy enemy = obj.gameObject.GetComponent<Enemy>();

        float enemyMaxHealth = enemy.MaxHealth;
        float remainEnemyHealth;

        float damage = getDamage();
        enemy.ApplyHealthChanges(-damage, out remainEnemyHealth);

        if(remainEnemyHealth <= 0 && isRebounded){
            GlobalEventManager.OnExtraDeath.Fire();
            gameObject.SetActive(false);

            return;
        }

        handleExtra(remainEnemyHealth, enemyMaxHealth);
    }

    void handleExtra(float remainEnemyHealth, float enemyMaxHealth){
        float extraChance = (100 - (remainEnemyHealth / enemyMaxHealth * 100)) / 2;
        bool isExtra = Random.Range(0, 100) <= extraChance;

        if (remainEnemyHealth <= 0 && isExtra)
        {
            isRebounded = true;

            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit);

            Vector3 surfaceNormal = hit.normal;
            float hitAngle = (float)(Vector3.AngleBetween(transform.forward,surfaceNormal) * 180 / 3.14);

            if(hitAngle > 100 && hitAngle < 150){
                // new shell direction
                Vector3 reboundedDirection = Vector3.Reflect(transform.forward, surfaceNormal);
                startMovingProcess(reboundedDirection, speed / 3);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    float getDamage()
    {
        return Random.Range(_minDamage, _maxDamage);
    }
}
