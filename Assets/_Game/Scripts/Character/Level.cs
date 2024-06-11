using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    [SerializeField] private Transform[] startPoints;
    [SerializeField] private int totalEnemy = 50;
    [SerializeField] private int initialEnemyCount = 15;

    private int aliveEnemy;
    private int spawnedEnemies;
    private List<Vector3> spawnPointList = new List<Vector3>();

    public static UnityAction winGameEvent;

    private void Start()
    {
        aliveEnemy = totalEnemy;
        //UIManager.Instance.GetUI<UIGamePlay>().UpdateAlive(aliveEnemy);

        for (int i = 0; i < startPoints.Length - 3; i++)
        {
            spawnPointList.Add(startPoints[i].position);
        }

        for (int i = 0; i < initialEnemyCount; i++)
        {
            Spawn(GetRandomStartPoint());
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

    private void Spawn(Vector3 point)
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
        if (aliveEnemy > 0)
        {
            aliveEnemy--;
            ReturnStartPoint();
            UIManager.Instance.GetUI<UIGamePlay>().UpdateAlive(aliveEnemy);
            StartCoroutine(RespawnEnemy(Random.Range(4, 7)));

            if (aliveEnemy == 0)
            {
                winGameEvent?.Invoke();
                GameManager.Instance.OnVictory();
            }
        }
        
    }

    private Vector3 GetRandomStartPoint()
    {
        int randomIndex = Random.Range(0, spawnPointList.Count);
        Vector3 randomPoint = spawnPointList[randomIndex];
        spawnPointList.RemoveAt(randomIndex);

        return randomPoint;
    }

    private void ReturnStartPoint()
    {
        for (int i = 0; i < startPoints.Length - 3; i++)
        {
            if (!spawnPointList.Contains(startPoints[i].position))
            {
                spawnPointList.Add(startPoints[i].position);
                break;
            }
        }
    }

    public void DespawnEnemy()
    {
        SimplePool.Collect(PoolType.Enemy);
    }

    public Vector3 GetPlayerStartPoint()
    {
        return startPoints[Random.Range(20, 23)].position;
    }

    IEnumerator RespawnEnemy(float time)
    {
        yield return Cache.GetWFS(time);
        Spawn(GetRandomStartPoint());
    }
}
