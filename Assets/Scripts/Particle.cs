using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    ParticleSystem _particleSystem;

    private void OnEnable()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particleSystem.Play(); 
    }
}
