using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour
{
    public LayerMask WhatIsGround, WhatIsPlayer;
    public Vector3 WalkPoint;
    public float WalkPointRange;

    [SerializeField] protected float _sightRange, _attackRange;
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _strengthReward = 50f;

    protected NavMeshAgent Agent;
    protected Transform Player;
    protected float Health;

    protected bool PlayerInSightRange, PlayerInAttackRange;
    protected bool WalkPointSet;
    protected bool IsFliedUp = false;

    public float StrengthReward { get => _strengthReward; }
    public float MaxHealth { get => _maxHealth; }

    private void OnEnable() {
        Init();
    }

    public virtual void Init()
    {
        Health = _maxHealth;
        Player = GameObject.Find("Player").transform;
        Agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void FixedUpdate()
    {

        PlayerInSightRange = Physics.CheckSphere(transform.position, _sightRange, WhatIsPlayer);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, WhatIsPlayer);

        RaycastHit forwardObstacleHit = ObstacleInWay(transform.position - Player.transform.position, _attackRange);

        if (forwardObstacleHit.distance <= _sightRange && forwardObstacleHit.collider.tag == "Player")
        {
            if (PlayerInAttackRange)
            {
                AttackPlayer();
            }
            else if (PlayerInSightRange)
            {
                ChasePlayer();
            }
        }
        else
        {
            Patroling();
        }
    }

    protected abstract void AttackPlayer();

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-WalkPointRange, WalkPointRange);
        float randomX = Random.Range(-WalkPointRange, WalkPointRange);

        WalkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // is that point on the ground
        if (Physics.Raycast(WalkPoint, -transform.up, 2f, WhatIsGround))
            WalkPointSet = true;
    }

    private RaycastHit ObstacleInWay(Vector3 vector, float rayLenght)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -vector.normalized, out hit);

        return hit;
    }

    private void Patroling()
    {
        if (!WalkPointSet) SearchWalkPoint();

        if (WalkPointSet)
            Agent.SetDestination(WalkPoint);

        Vector3 distanceToWalkPoint = transform.position - WalkPoint;

        Debug.DrawLine(transform.position, WalkPoint);
        // WalkPoint reched
        if (distanceToWalkPoint.magnitude < 1f)
        {
            WalkPointSet = false;
        }
    }

    private void ChasePlayer()
    {
        Agent.SetDestination(Player.position);
    }

    public void ApplyHealthChanges(float points, out float remainHealth)
    {
        Health += points;
        remainHealth = Health;
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (Health <= 0)
        {
            GlobalEventManager.OnRewardedEnemyDeath.Fire(this, _strengthReward);
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _sightRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
