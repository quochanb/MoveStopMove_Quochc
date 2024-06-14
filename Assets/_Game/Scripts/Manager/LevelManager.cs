using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Level[] levels;
    [SerializeField] private Player playerPrefab;

    private Player player;
    private Level currentLevel;
    private Transform tf;

    public Transform Tf
    {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }

    private void Start()
    {
        CameraFollow.Instance.enabled = false;
    }

    //reset level
    public void OnReset()
    {
        if (currentLevel != null)
        {
            currentLevel.DespawnEnemy();
            Destroy(currentLevel.gameObject);
        }
        if (player != null)
        {
            Destroy(player.gameObject);
            CameraFollow.Instance.enabled = false;
        }
    }

    //load level
    public void OnLoadLevel(int level)
    {
        if (level < levels.Length)
        {
            currentLevel = Instantiate(levels[level], Tf);
        }
        else
        {
            Debug.LogError("No more level to load !");
        }

        Invoke(nameof(OnLoadPlayer), 0.01f);
    }

    //load player
    public void OnLoadPlayer()
    {
        if (player == null)
        {
            Vector3 spawnPoint = currentLevel.GetPlayerStartPoint();
            player = Instantiate(playerPrefab, spawnPoint, Quaternion.Euler(0, 180, 0));
            player.Tf.SetParent(Tf);

            CameraFollow.Instance.enabled = true;
            CameraFollow.Instance.SetTarget(player.Tf);
        }
    }

    //lay coin nguoi choi
    public int GetPlayerCoin()
    {
        return player.Coin;
    }

    //lay so enemy con lai tren map
    public int GetAliveEnemy()
    {
        return currentLevel.AliveEnemy;
    }

    public string GetNameAttacker()
    {
        return "";
    }
}
