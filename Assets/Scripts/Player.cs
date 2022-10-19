using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Player : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _maxStrength = 100f;

    [Space(5)]
    [Header("Strenght")]
    [SerializeField] private float _initialHealth = 100f;
    [SerializeField] private float _initialStrength = 50f;

    
    public float Health{
        get{ return _health;}
    }

    public float MaxHealth{
        get{ return _maxHealth;}
    }

    public float Strenght{
        get{ return _strength;}
    }

    public float MaxStrenght{
        get{ return _maxStrength;}
    }

    private float _health = 0;
    private float _strength = 0;

    private void Start()
    {
        // Check if initial > max
        _health = Mathf.Clamp(_initialHealth, 0, _maxHealth);
        _strength = Mathf.Clamp(_initialStrength, 0, _maxStrength);

        GlobalEventManager.OnPlayerInit.Fire(_health, _maxHealth, _strength, _maxStrength);
    }

    public void ApplyHealthChanges(float changePoints){
        _health -= changePoints;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        GlobalEventManager.OnHealthChange.Fire(_health, _maxHealth);
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (_health == 0)
            GlobalEventManager.OnEndgame.Fire();
    }

    public void ApplyStrenghtChanges(float changePoints){
        _strength -= changePoints;
        _strength = Mathf.Clamp(_strength, 0, _maxStrength);
        GlobalEventManager.OnStrenghtChange.Fire(_strength, _maxStrength);
    }
}
