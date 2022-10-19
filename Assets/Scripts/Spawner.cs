using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn settings")]
    [SerializeField] private float _maxSpawnSpeed = 2f;
    [SerializeField] private float _minSpawnSpeed = 5f;
    [SerializeField] private float _speedDecreasingStep = 0.5f;
    [Tooltip("The radius from player of enemy spawning area")]
    [SerializeField] private float _enemySpawnRadius = 5f;

    [Header("Enemy settings")]
    [SerializeField] private float _redEnemyYSpawnPoint = 0.4f;
    [SerializeField] private int _startBlueEmount = 1;
    [SerializeField] private int _blueAmountStep = 1;

    private Player _player;
    private float _currentRedAmount = 0;
    private float _currentBlueAmount = 0;

    private const string _redEnemyTag = "RedEnemy";
    private const string _blueEnemyTag = "BlueEnemy";

    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
        StartCoroutine(SpawnProcess());
        GlobalEventManager.OnEnemyDeath.AddListener(OnEmenyDeath);
    }

    private IEnumerator SpawnProcess()
    {
        float currentSpawnSpeed = _minSpawnSpeed;
        float blueAmount = _startBlueEmount;


        while (true)
        {
            yield return new WaitForSeconds(currentSpawnSpeed);

            if (currentSpawnSpeed > _maxSpawnSpeed)
            {
                currentSpawnSpeed = Mathf.Clamp(currentSpawnSpeed - _speedDecreasingStep, _maxSpawnSpeed, _minSpawnSpeed);
            }
            else
            {
                blueAmount += _blueAmountStep;
            };

            Vector3 spawnPoint;
            while (!RandomMeshPoint(_player.transform.position, _enemySpawnRadius, out spawnPoint)) ;

            float neededRedAmount = _currentBlueAmount * 4;
            float availableRedAmount = neededRedAmount - _currentRedAmount;

            if (blueAmount > _currentBlueAmount && availableRedAmount <= 0)
            {
                SpawnEnemy(_blueEnemyTag, spawnPoint);
                _currentBlueAmount++;
            }
            else if (availableRedAmount > 0)
            {
                spawnPoint.y = _redEnemyYSpawnPoint; // To spawn red on the "second floor"
                SpawnEnemy(_redEnemyTag, spawnPoint);
                _currentRedAmount++;
            }
        }
    }

    void SpawnEnemy(string enemyTag, Vector3 spawnPoint)
    {
        GameObject enemy =  ObjectPooller.Current.GetPooledObject(enemyTag);
        enemy.transform.position = spawnPoint;
        enemy.SetActive(true);
    }

    bool RandomMeshPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = new Vector3();
        return false;
    }

    public void OnEmenyDeath(Enemy enemy)
    {
        if (enemy.GetComponent<RedEnemy>())
        {
            _currentRedAmount--;
        }
        else
        {
            _currentBlueAmount--;
        }
    }

    private void OnDisable()
    {
        GlobalEventManager.OnEnemyDeath.RemoveListener(OnEmenyDeath);
    }
}
