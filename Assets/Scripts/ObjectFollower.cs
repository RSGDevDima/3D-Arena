using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ObjectFollower : MonoBehaviour
{
    [SerializeField] float _speed = 2f;
    [SerializeField] float _timeBeforeDestroy = 2f;
    Coroutine moving;
    Vector3 vectorToTarget;

    public void StartMoving(Transform target)
    {
        moving = StartCoroutine(startMoving(target));
        GlobalEventManager.OnOppositeCornerMove.AddListener(stopFollowing);
    }

    IEnumerator startMoving(Transform target)
    {
  
        while (enabled)
        {
            vectorToTarget = target.transform.position - transform.position;
            transform.position = transform.position + vectorToTarget.normalized * _speed * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    void stopFollowing()
    {
        StartCoroutine(stopFollowingProcess());
    }

    IEnumerator stopFollowingProcess()
    {
        StopCoroutine(moving);

        while (_timeBeforeDestroy > 0)
        {
            transform.position += vectorToTarget * Time.deltaTime;
            _timeBeforeDestroy -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        GlobalEventManager.OnOppositeCornerMove.RemoveListener(stopFollowing);
    }
}
