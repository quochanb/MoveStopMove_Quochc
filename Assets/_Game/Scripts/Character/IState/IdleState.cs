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
        randomTime = Random.Range(1, 4);
        timer = 0f;
    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (timer >= randomTime)
        {
            if (enemy.GetTarget() != null)
            {
                enemy.ChangeState(new AttackState());
            }
            if(enemy.GetTarget() == null)
            {
                enemy.ChangeState(new PatrolState());
            }
        }

    }

    public void OnExit(Enemy enemy)
    {

    }
}
