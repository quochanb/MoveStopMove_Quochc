using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    float delayTime;
    float timer;

    public void OnEnter(Enemy enemy)
    {
        delayTime = 1f;
        timer = 0f;
    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if(timer >= delayTime)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Enemy enemy)
    {
        
    }
}
