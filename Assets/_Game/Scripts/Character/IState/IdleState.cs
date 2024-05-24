using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    float randomTime;
    float timer;

    public void OnEnter(Enemy enemy)
    {
        enemy.ChangeAnim(Constants.ANIM_IDLE);
        enemy.StopMove();
        randomTime = Random.Range(0,3);
        timer = 0f;
    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if(timer >= randomTime)
        {
            enemy.ChangeState(new PatrolState());
            
        }
    }

    public void OnExit(Enemy enemy)
    {
        
    }
}
