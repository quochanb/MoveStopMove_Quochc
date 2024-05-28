using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : GameUnit
{
    [SerializeField] private int totalEnemy = 50;
    [SerializeField] private int initialEnemyCount = 20;

    private int spawnedEnemies;
    private int currentEnemyCount;
    //List<Enemy> enemies = new List<Enemy>();
    

    private void Start()
    {
        for(int i = 0; i < initialEnemyCount; i++)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        if (spawnedEnemies > totalEnemy || spawnedEnemies > initialEnemyCount)
        {
            return;
        }
        Enemy enemy = SimplePool.Spawn<Enemy>(PoolType.Enemy, transform.position, Quaternion.identity);
        enemy.Tf.position = enemy.GetRandomPoint();
        //enemies.Add(enemy);
        spawnedEnemies++;
        currentEnemyCount++;
        enemy.OnDeath += HandleOnDeath;
    }

    public void Despawn(Enemy enemy)
    {
        //enemies.Remove(enemy);
        SimplePool.Despawn(enemy);
    }

    private void HandleOnDeath(Enemy enemy)
    {
        if(enemy != null)
        {
            enemy.OnDeath -= HandleOnDeath;
        }
        currentEnemyCount--;
        Despawn(enemy);
        //StartCoroutine(RespawnEnemy(2f));
    }

    IEnumerator RespawnEnemy(float time)
    {
        yield return Cache.GetWFS(time);
        Spawn();
    }
}
