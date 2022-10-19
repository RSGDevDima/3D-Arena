using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueEnemy : Enemy
{
    public float TimeBetweenAttacks;

    [SerializeField] private ObjectFollower _enemyBulletObject;
    
    private bool _alreadyAttacked;

    override protected void AttackPlayer()
    {
        WalkPoint = Player.position;

        // stop the enemy
        Agent.SetDestination(transform.position);

        transform.LookAt(Player);

        if (!_alreadyAttacked)
        {
            SpawnBullet();

            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), TimeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }

    private void SpawnBullet()
    {
        Vector3 spawnPos = transform.position + transform.forward * transform.localScale.z;

        GameObject bulletObject = ObjectPooller.Current.GetPooledObject("EnemyShell");
        bulletObject.transform.position = spawnPos;
        bulletObject.transform.rotation = transform.rotation;
        bulletObject.SetActive(true);

        ObjectFollower objectFollower = bulletObject.GetComponent<ObjectFollower>();
        objectFollower.StartMoving(Player);
    }
}
