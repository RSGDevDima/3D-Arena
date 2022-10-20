using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectFollower))]
public class RedEnemy : Enemy
{
    [SerializeField] private float _hitDamage = 15f;
    [SerializeField] private float _flyUpSpeed = 3f;
    [SerializeField] private float _flyUpDistance = 2f;

    private ObjectFollower _objectFollower;
    private float _startBaseOffset;
    private Coroutine _flyUpProcess;

    public override void Init()
    {
        base.Init();
        _objectFollower = GetComponent<ObjectFollower>();
        _startBaseOffset = Agent.baseOffset;

        _flyUpProcess = StartCoroutine(flyUp());
    }

    protected override void FixedUpdate()
    {
        if (Agent.enabled && IsFliedUp)
        {
            base.FixedUpdate();
        }
    }

    override protected void AttackPlayer()
    {
        // stop the enemy
        Agent.SetDestination(transform.position);
        Agent.enabled = false;
        _objectFollower.StartMoving(Player.transform);
    }

    private IEnumerator flyUp()
    {
        Agent.baseOffset = -_flyUpDistance;
        while (Agent.baseOffset < _startBaseOffset)
        {
            Agent.baseOffset += _flyUpSpeed * Time.deltaTime;
            yield return null;
        }

        _flyUpProcess = null;
        IsFliedUp = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;
        if (tag == "Player")
        {
            Player.GetComponent<Player>().ApplyHealthChanges(_hitDamage);
            GlobalEventManager.OnEnemyDeath.Fire(this);
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        Agent.enabled = true;
        if(_flyUpProcess != null)
        {
            StopCoroutine(_flyUpProcess);
        }
    }
}
