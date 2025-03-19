using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public interface IPoolObject
{
    GameObject Prefab { get; }
    GameObject gameObject { get; }
    public void ResetState();
    public void AddToPool();
}


public class PoolManager
{
    public static PoolManager _instance;

    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PoolManager();
            }
            return _instance;
        }
    }

    private Dictionary<GameObject, List<IPoolObject>> prefabToObjects = new Dictionary<GameObject, List<IPoolObject>>();
    private GameObject pool;

    public GameObject GetGameObject(GameObject prefab)
    {
        GameObject go;
        if (prefabToObjects.ContainsKey(prefab) && prefabToObjects[prefab].Count > 0)
        {
            IPoolObject po = prefabToObjects[prefab][0];
            go = po.gameObject;
            if (go.IsDestroyed())
            {
                go = GameObject.Instantiate(prefab);
                go.SetActive(true);
            }
            else
            {
                go.SetActive(true);
                po.ResetState();
            }
            prefabToObjects[prefab].RemoveAt(0);
        }
        else
        {
            go = GameObject.Instantiate(prefab);
            go.SetActive(true);
        }
        go.transform.SetParent(null);
        return go;
    }

    public void AddGameObject(IPoolObject po)
    {
        GameObject gameObject = po.gameObject;
        if (!prefabToObjects.ContainsKey(po.Prefab))
        {
            prefabToObjects[po.Prefab] = new List<IPoolObject>();
        }
        prefabToObjects[po.Prefab].Add(po);

        if (pool == null)
        {
            pool = new GameObject("pool");
        }
        if(pool.transform.Find(po.Prefab.name) == null)
        {
            GameObject goParent = new GameObject(po.Prefab.name);
            goParent.transform.position = Vector3.zero;
            goParent.transform.SetParent(pool.transform);
        }
        gameObject.transform.SetParent(pool.transform.Find(po.Prefab.name));
        gameObject.SetActive(false);
    }

    public void Clear()
    {
        prefabToObjects.Clear();
    }
}
