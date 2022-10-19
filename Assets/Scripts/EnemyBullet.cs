using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float strengthDamage = 25f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.ApplyStrenghtChanges(strengthDamage);

            gameObject.SetActive(false);
        }
        else if (other.tag == "PlayerShell")
        {
            gameObject.SetActive(false);
        }
    }
}
