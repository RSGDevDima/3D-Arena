using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectFollower))]
public class RedEnemy : Enemy
{
    [SerializeField] float _hitDamage = 15f;
    [SerializeField] float _flyUpSpeed = 3f;
    [SerializeField] float _flyUpDistance = 2f;

    ObjectFollower objectFollower;
    float startBaseOffset;
    Coroutine flyUpProcess;

    public override void Init()
    {
        base.Init();

        objectFollower = GetComponent<ObjectFollower>();
        startBaseOffset = agent.baseOffset;

        flyUpProcess = StartCoroutine(flyUp());
    }

    protected override void FixedUpdate()
    {
        if (agent.enabled && isFliedUp)
        {
            base.FixedUpdate();
        }
    }

    override protected void attackPlayer()
    {
        // stop the enemy
        agent.SetDestination(transform.position);
        agent.enabled = false;
        objectFollower.StartMoving(player.transform);
    }

    IEnumerator flyUp()
    {
        agent.baseOffset = -_flyUpDistance;
        while (agent.baseOffset < startBaseOffset)
        {
            agent.baseOffset += _flyUpSpeed * Time.deltaTime;
            yield return null;
        }

        flyUpProcess = null;
        isFliedUp = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;
        if (tag == "Player")
        {
            player.GetComponent<Player>().ApplyHealthChanges(_hitDamage);
            GlobalEventManager.OnEnemyDeath.Fire(this);
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        agent.enabled = true;
        if(flyUpProcess != null)
        {
            StopCoroutine(flyUpProcess);
        }
    }
}
