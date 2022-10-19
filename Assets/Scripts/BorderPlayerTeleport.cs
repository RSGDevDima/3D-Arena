using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderPlayerTeleport : MonoBehaviour
{
    Player player;
    Spawner enemySpawner;

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        enemySpawner = GameObject.FindObjectOfType<Spawner>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            List<Vector3> enemiesPositions = new List<Vector3>();

            List<Enemy> enemiesList = Utils.GetActiveEnemies();
            

            // Get all enemy positions
            foreach (var enemy in enemiesList)
            {
                if (enemy != null)
                    enemiesPositions.Add(enemy.transform.position);
            }

            // Calculate the avarage position value
            Vector3 avaregeEnemyPos;
            float avarageX = 0;
            float avarageZ = 0;

            if (enemiesList.Count != 0)
            {
                foreach (var position in enemiesPositions)
                {
                    avarageX += position.x;
                    avarageZ += position.z;
                }

                avarageX /= enemiesPositions.Count;
                avarageZ /= enemiesPositions.Count;

                avaregeEnemyPos = new Vector3(avarageX, 0, avarageZ);
            }
            else
            {
                avaregeEnemyPos = new Vector3(1, player.transform.position.y, 1);
            }


            Vector3 enemyPosDir = transform.position - avaregeEnemyPos;
            Vector3 oppositeCorner = enemyPosDir.normalized * (transform.localScale.x / 2 - 0.2f);
            oppositeCorner.y = player.transform.position.y;

            GlobalEventManager.OnOppositeCornerMove.Fire();
            player.transform.position = oppositeCorner;
            player.transform.LookAt(transform, player.transform.up);
        }
    }

    
}
