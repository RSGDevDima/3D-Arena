using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerUltra : MonoBehaviour
{
    public bool IsUltraReady { get => _isUltraReady; }

    [SerializeField] private Spawner _spawner;

    private Player _player;
    private bool _isUltraReady = false;

    private void OnEnable()
    {
        _player = GetComponent<Player>();
        GlobalEventManager.OnStrenghtChange.AddListener(checkUltra);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space) && Input.touchCount == 0){
            UseUltra();
        }
    }

    private void checkUltra(float strenght, float maxStrenght)
    {
        if (strenght == maxStrenght)
        {
            setUltra(true);
        }
    }

    private void setUltra(bool state)
    {
        _isUltraReady = state;
        GlobalEventManager.OnUltraStateChanged.Fire(state);
    }

    public void UseUltra()
    {
        if (_isUltraReady)
        {
            List<Enemy> activeEnemies = Utils.GetActiveEnemies();

            foreach (var enemy in activeEnemies)
            {
                GlobalEventManager.OnRewardedEnemyDeath.Fire(enemy, enemy.StrengthReward);
                enemy.gameObject.SetActive(false);
            }

            _player.ApplyStrenghtChanges(_player.MaxStrenght);

            setUltra(false);
        }
    }

    private void OnDisable()
    {
        GlobalEventManager.OnStrenghtChange.RemoveListener(checkUltra);
    }
}
