using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    float randomTime;
    float timer;

    public void OnEnter(Enemy enemy)
    {
        enemy.StopMove();
        randomTime = Random.Range(0,3);
        timer = 0f;
    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if(timer >= randomTime && enemy.GetTarget()!= null)
        {
            //enemy.ChangeState(new PatrolState());
            enemy.Attack(enemy.GetTarget());
            
        }
    }

    public void OnExit(Enemy enemy)
    {
        
    }
}
