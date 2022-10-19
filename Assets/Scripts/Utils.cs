using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void AddPooledObjectToList(List<GameObject> list, string objectTag)
    {
        List<GameObject> enemyList = ObjectPooller.Current.GetActiveObjects(objectTag);

        if (enemyList != null)
        {
            foreach (var item in enemyList)
            {
                list.Add(item);
            }
        }
    }

    public static List<Enemy> GetActiveEnemies()
    {
        List<GameObject> enemiesObjectList = new List<GameObject>();
        List<Enemy> enemiesList = new List<Enemy>();

        AddPooledObjectToList(enemiesObjectList, "BlueEnemy");
        AddPooledObjectToList(enemiesObjectList, "RedEnemy");

        foreach (var item in enemiesObjectList)
        {
            Enemy enemy = item.GetComponent<Enemy>();
            enemiesList.Add(enemy);
        }
        
        return enemiesList;
    }
}
