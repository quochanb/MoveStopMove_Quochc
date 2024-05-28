using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Transform[] startPoint;
    [SerializeField] private int totalEnemy = 50;
    [SerializeField] private int initialEnemyCount = 10;

    private int spawnedEnemies;
    //private int currentEnemyCount;
    //List<Enemy> enemies = new List<Enemy>();
    

    private void Start()
    {
        for(int i = 0; i < initialEnemyCount; i++)
        {
            Spawn(startPoint[i].position);
        }
    }

    public void Spawn(Vector3 point)
    {
        if (spawnedEnemies > totalEnemy || spawnedEnemies > initialEnemyCount)
        {
            return;
        }
        Enemy enemy = SimplePool.Spawn<Enemy>(PoolType.Enemy, point, Quaternion.identity);
        //enemy.Tf.position = enemy.GetRandomPoint();
        //enemies.Add(enemy);
        spawnedEnemies++;
        //currentEnemyCount++;
    }


    private void HandleOnDeath(Enemy enemy)
    {
        //if(enemy != null)
        //{
        //    enemy.OnDeath -= HandleOnDeath;
        //}
        //currentEnemyCount--;
        StartCoroutine(DespawnEnemy(enemy));
        Spawn(startPoint[Random.Range(0, startPoint.Length)].position);
        //StartCoroutine(RespawnEnemy(2f));
    }

    IEnumerator RespawnEnemy(float time)
    {
        yield return Cache.GetWFS(time);
        Spawn(startPoint[Random.Range(0, startPoint.Length)].position);
    }

    IEnumerator DespawnEnemy(Enemy enemy)
    {
        yield return Cache.GetWFS(2f);
        SimplePool.Despawn(enemy);
    }
}
