using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn settings")]
    [SerializeField] float _maxSpawnSpeed = 2f;
    [SerializeField] float _minSpawnSpeed = 5f;
    [SerializeField] float _speedDecreasingStep = 0.5f;
    [Tooltip("The radius from player of enemy spawning area")]
    [SerializeField] float _enemySpawnRadius = 5f;

    [Header("Enemy settings")]
    [SerializeField] float _redEnemyYSpawnPoint = 0.4f;
    [SerializeField] int _startBlueEmount = 1;
    [SerializeField] int _blueAmountStep = 1;

    Player player;
    float currentRedAmount = 0;
    float currentBlueAmount = 0;

    const string RED_ENEMY_TAG = "RedEnemy";
    const string BLUE_ENEMY_TAG = "BlueEnemy";

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        StartCoroutine(spawnProcess());
        GlobalEventManager.OnEnemyDeath.AddListener(OnEmenyDeath);
    }

    IEnumerator spawnProcess()
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
            while (!RandomMeshPoint(player.transform.position, _enemySpawnRadius, out spawnPoint)) ;

            float neededRedAmount = currentBlueAmount * 4;
            float availableRedAmount = neededRedAmount - currentRedAmount;

            if (blueAmount > currentBlueAmount && availableRedAmount <= 0)
            {
                spawnEnemy(BLUE_ENEMY_TAG, spawnPoint);
                currentBlueAmount++;
            }
            else if (availableRedAmount > 0)
            {
                spawnPoint.y = _redEnemyYSpawnPoint; // To spawn red on the "second floor"
                spawnEnemy(RED_ENEMY_TAG, spawnPoint);
                currentRedAmount++;
            }
        }
    }

    void spawnEnemy(string enemyTag, Vector3 spawnPoint)
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
            currentRedAmount--;
        }
        else
        {
            currentBlueAmount--;
        }
    }

    private void OnDisable()
    {
        GlobalEventManager.OnEnemyDeath.RemoveListener(OnEmenyDeath);
    }
}
