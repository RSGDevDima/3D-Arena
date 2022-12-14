using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PoollerItem
{
    public GameObject ObjectToPool;
    public int PremadeAmount;
}

public class ObjectPooller : MonoBehaviour
{
    [SerializeField] private List<PoollerItem> _itemsToPull = new List<PoollerItem>();
    public List<GameObject> Pool;
    [HideInInspector]
    public static ObjectPooller Current;

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        foreach (var item in _itemsToPull)
        {
            for (int i = 0; i < item.PremadeAmount; i++)
            {
                addObject(item.ObjectToPool);
            }
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        foreach (var item in Pool)
        {
            if (item.tag == tag && !item.activeInHierarchy)
            {
                return item;
            }
        }

        foreach (var item in _itemsToPull)
        {
            if (item.ObjectToPool.tag == tag)
            {
                GameObject newObject = addObject(item.ObjectToPool);
                return newObject;
            }
        }

        return null;
    }

    public List<GameObject> GetActiveObjects(string tag)
    {
        List<GameObject> resultList = new List<GameObject>();

        foreach (var item in Pool)
        {
            if (item.tag == tag && item.active)
            {
                resultList.Add(item);
            }
        }

        if (resultList.Count == 0)
            return null;

        return resultList;
    }

    public GameObject addObject(GameObject gameObject)
    {
        GameObject instance = Instantiate(gameObject);
        instance.SetActive(false);
        Pool.Add(instance);

        return instance;
    }
}
