using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour
{
    public LayerMask WhatIsGround, WhatIsPlayer;
    public Vector3 WalkPoint;
    public float WalkPointRange;

    [SerializeField] protected float _sightRange, _attackRange;
    [SerializeField] float _maxHealth = 100f;
    [SerializeField] float _strengthReward = 50f;

    protected NavMeshAgent agent;
    protected Transform player;
    protected float health;

    protected bool playerInSightRange, playerInAttackRange;
    protected bool walkPointSet;
    protected bool isFliedUp = false;

    public float StrengthReward { get => _strengthReward; }
    public float MaxHealth { get => _maxHealth; }

    private void OnEnable() {
        Init();
    }

    public virtual void Init()
    {
        health = _maxHealth;
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void FixedUpdate()
    {

        playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, WhatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, WhatIsPlayer);

        RaycastHit forwardObstacleHit = obstacleInWay(transform.position - player.transform.position, _attackRange);

        if (forwardObstacleHit.distance <= _sightRange && forwardObstacleHit.collider.tag == "Player")
        {
            if (playerInAttackRange)
            {
                attackPlayer();
            }
            else if (playerInSightRange)
            {
                chasePlayer();
            }
        }
        else
        {
            patroling();
        }
    }

    protected abstract void attackPlayer();

    void searchWalkPoint()
    {
        float randomZ = Random.Range(-WalkPointRange, WalkPointRange);
        float randomX = Random.Range(-WalkPointRange, WalkPointRange);

        WalkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // is that point on the ground
        if (Physics.Raycast(WalkPoint, -transform.up, 2f, WhatIsGround))
            walkPointSet = true;
    }

    RaycastHit obstacleInWay(Vector3 vector, float rayLenght)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -vector.normalized, out hit);

        return hit;
    }

    void patroling()
    {
        if (!walkPointSet) searchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(WalkPoint);

        Vector3 distanceToWalkPoint = transform.position - WalkPoint;

        Debug.DrawLine(transform.position, WalkPoint);
        // WalkPoint reched
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    void chasePlayer()
    {
        agent.SetDestination(player.position);
    }

    public void ApplyHealthChanges(float points, out float remainHealth)
    {
        health += points;
        remainHealth = health;
        checkHealth();
    }

    void checkHealth()
    {
        if (health <= 0)
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
