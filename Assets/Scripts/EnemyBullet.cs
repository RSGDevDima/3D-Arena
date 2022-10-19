using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float _strengthDamage = 25f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.ApplyStrenghtChanges(_strengthDamage);

            gameObject.SetActive(false);
        }
        else if (other.tag == "PlayerShell")
        {
            gameObject.SetActive(false);
        }
    }
}
