using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(ObjectFollower))]
public class RedEnemy : Enemy
{
    bool IsFliedUp;

    [SerializeField] private float _hitDamage = 15f;
    [SerializeField] private float _flyUpSpeed = 3f;
    [SerializeField] private float _flyUpDistance = 2f;

    private ObjectFollower _objectFollower;
    private float _startBaseOffset;
    private Coroutine _flyUpProcess;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        _startBaseOffset = Agent.baseOffset;
    }

    public override void Init()
    {
        base.Init();
        _objectFollower = GetComponent<ObjectFollower>();

        IsFliedUp = false;
        _flyUpProcess = StartCoroutine(FlyUp());
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

    private IEnumerator FlyUp()
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
