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

    public int AliveEnemy { get { return aliveEnemy; } }
    public static UnityAction winGameEvent;

    private void Start()
    {
        aliveEnemy = totalEnemy;

        for (int i = 0; i < startPoints.Length - 3; i++) //lay tu index 0 -> 19
        {
            spawnPointList.Add(startPoints[i].position);
        }

        for (int i = 0; i < initialEnemyCount; i++) //spawn theo so luong khoi tao ban dau
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

    //spawn enemy
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

    //xu ly su kien khi 1 enemy chet
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

    //lay random diem spawn enemy
    private Vector3 GetRandomStartPoint()
    {
        int randomIndex = Random.Range(0, spawnPointList.Count);
        Vector3 randomPoint = spawnPointList[randomIndex];
        spawnPointList.RemoveAt(randomIndex);

        return randomPoint;
    }

    //tra lai vi tri spawn enemy
    private void ReturnStartPoint()
    {
        if (spawnPointList.Count < 5)
        {
            for (int i = 0; i < startPoints.Length - 3; i++) //lay tu index 0 -> 19
            {
                if (!spawnPointList.Contains(startPoints[i].position))
                {
                    spawnPointList.Add(startPoints[i].position);
                }
            }
        }
    }

    //respawn enemy
    IEnumerator RespawnEnemy(float time)
    {
        yield return Cache.GetWFS(time);
        if (spawnedEnemies < totalEnemy)
        {
            Spawn(GetRandomStartPoint());
        }
    }

    //huy tat ca enemy con tren map
    public void DespawnEnemy()
    {
        SimplePool.Collect(PoolType.Enemy);
    }

    //lay start point cho Player
    public Vector3 GetPlayerStartPoint()
    {
        return startPoints[Random.Range(20, 23)].position; //lay tu index 20 -> 22
    }
}
