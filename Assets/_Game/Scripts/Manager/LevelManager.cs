using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Level[] levels;

    private Level currentLevel;

    public void OnReset()
    {
        if(currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }
    }

    public void OnLoadLevel(int level)
    {
        if(level < levels.Length)
        {
            currentLevel = Instantiate(levels[level], transform);
        }
        else
        {
            Debug.LogError("No more level to load !");
        }
    }
}
