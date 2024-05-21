using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePool : MonoBehaviour
{
    private static Dictionary<PoolType, Pool> poolInstance = new Dictionary<PoolType, Pool>();

    public static void PreLoad(GameUnit prefab, int amount, Transform parent)
    {
        if(prefab == null)
        {
            Debug.LogError("PREFAB IS EMPTY!!!");
            return;
        }
        if(!poolInstance.ContainsKey(prefab.poolType) || poolInstance[prefab.poolType] == null)
        {
            Pool p = new Pool();
            p.PreLoad(prefab, amount, parent);
            poolInstance[prefab.poolType] = p;
        }
    }

    public static T Spawn<T>(PoolType poolType, Vector3 pos, Quaternion rot) where T : GameUnit
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError(poolType + "IS NOT PRELOAD!!!");
            return null;
        }
        return poolInstance[poolType].Spawn(pos, rot) as T;
    }

    public static T Spawn<T>(PoolType poolType) where T : GameUnit
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError(poolType + "IS NOT PRELOAD!!!");
            return null;
        }
        return poolInstance[poolType].Spawn() as T;
    }

    public static void Despawn(GameUnit unit)
    {
        if (!poolInstance.ContainsKey(unit.poolType))
        {
            Debug.LogError(unit.poolType + "IS NOT PRELOAD!!!");
        }
        poolInstance[unit.poolType].Despawn(unit);
    }

    public static void Collect(PoolType poolType)
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError(poolType + "IS NOT PRELOAD!!!");
        }
        poolInstance[poolType].Collect();
    }

    public static void CollectAll()
    {
        foreach(var item in poolInstance.Values)
        {
            item.Collect();
        }
    }

    public static void Release(PoolType poolType)
    {
        if (!poolInstance.ContainsKey(poolType))
        {
            Debug.LogError(poolType + "IS NOT PRELOAD!!!");
        }
        poolInstance[poolType].Release();
    }

    public static void ReleaseAll()
    {
        foreach(var item in poolInstance.Values)
        {
            item.Release();
        }
    }
}

public class Pool
{
    GameUnit prefab;
    Transform parent;

    Queue<GameUnit> inactives = new Queue<GameUnit>();
    List<GameUnit> actives = new List<GameUnit>();

    public void PreLoad(GameUnit prefab, int amount, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < amount; i++)
        {
            Despawn(GameObject.Instantiate(prefab, parent));
        }
    }

    public GameUnit Spawn()
    {
        GameUnit unit;
        if (inactives.Count == 0)
        {
            unit = GameObject.Instantiate(prefab, parent);
        }
        else
        {
            unit = inactives.Dequeue();
        }
        actives.Add(unit);
        unit.gameObject.SetActive(true);

        return unit;
    }

    public GameUnit Spawn(Vector3 pos, Quaternion rot)
    {
        GameUnit unit = Spawn();
        
        unit.Tf.SetPositionAndRotation(pos, rot);

        return unit;
    }

    public void Despawn(GameUnit unit)
    {
        if(unit != null && unit.gameObject.activeSelf)
        {
            inactives.Enqueue(unit);
            actives.Remove(unit);
            unit.gameObject.SetActive(false);
        }
    }

    public void Collect()
    {
        while(actives.Count > 0)
        {
            Despawn(actives[0]);
        }
    }

    public void Release()
    {
        Collect();
        while(inactives.Count > 0)
        {
            GameObject.Destroy(inactives.Dequeue().gameObject);
        }
        inactives.Clear();
    }
}
