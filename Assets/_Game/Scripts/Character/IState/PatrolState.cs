using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    public void OnEnter(Enemy enemy)
    {
        Vector3 destination = enemy.GetRandomPoint();
        enemy.SetDestination(destination);
        enemy.ChangeAnim(Constants.ANIM_RUN);
    }

    public void OnExecute(Enemy enemy)
    {
        if (enemy.IsDead)
        {
            return;
        }
        if (enemy.IsDestination)
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void OnExit(Enemy enemy)
    {
        
    }
}
