using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolControl : MonoBehaviour
{
    [SerializeField] PoolAmount[] poolAmounts;

    private void Start()
    {
        for(int i = 0; i < poolAmounts.Length; i++)
        {
            SimplePool.PreLoad(poolAmounts[i].prefab, poolAmounts[i].amount, poolAmounts[i].parent);
        }
    }
}

[System.Serializable]
public class PoolAmount
{
    public GameUnit prefab;
    public int amount;
    public Transform parent;
}


