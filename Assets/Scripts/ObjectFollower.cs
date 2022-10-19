using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ObjectFollower : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _timeBeforeDestroy = 2f;
    private Coroutine _moving;
    private Vector3 _vectorToTarget;

    public void StartMoving(Transform target)
    {
        _moving = StartCoroutine(StartMovingProcess(target));
        GlobalEventManager.OnOppositeCornerMove.AddListener(StopFollowing);
    }

    private IEnumerator StartMovingProcess(Transform target)
    {
  
        while (enabled)
        {
            _vectorToTarget = target.transform.position - transform.position;
            transform.position = transform.position + _vectorToTarget.normalized * _speed * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    private void StopFollowing()
    {
        StartCoroutine(StopFollowingProcess());
    }

    private IEnumerator StopFollowingProcess()
    {
        StopCoroutine(_moving);

        while (_timeBeforeDestroy > 0)
        {
            transform.position += _vectorToTarget * Time.deltaTime;
            _timeBeforeDestroy -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        GlobalEventManager.OnOppositeCornerMove.RemoveListener(StopFollowing);
    }
}
