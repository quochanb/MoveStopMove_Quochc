using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Transform[] startPoint;
    [SerializeField] private int totalEnemy = 50;
    [SerializeField] private int initialEnemyCount = 10;

    private int spawnedEnemies;

    private void Start()
    {
        for(int i = 0; i < initialEnemyCount; i++)
        {
            Spawn(startPoint[i].position);
        }
    }

    private void OnEnable()
    {
        Enemy.onDeathEvent += HandleOnDeath;
    }

    private void OnDisable()
    {
        Enemy.onDeathEvent -= HandleOnDeath;
    }

    public void Spawn(Vector3 point)
    {
        if (spawnedEnemies == totalEnemy)
        {
            return;
        }
        Enemy enemy = SimplePool.Spawn<Enemy>(PoolType.Enemy, point, Quaternion.identity);
        enemy.OnInit();
        spawnedEnemies++;
    }


    private void HandleOnDeath()
    {
        StartCoroutine(RespawnEnemy(Random.Range(3, 5)));
    }

    IEnumerator RespawnEnemy(float time)
    {
        yield return Cache.GetWFS(time);
        Spawn(startPoint[Random.Range(0, startPoint.Length)].position);
    }
}
