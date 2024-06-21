using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    public void OnEnter(Enemy enemy)
    {
        enemy.ChangeAnim(Constants.ANIM_RUN);
        Vector3 destination = enemy.GetRandomPoint();
        enemy.SetDestination(destination);
    }

    public void OnExecute(Enemy enemy)
    {
        if (enemy.IsDead)
        {
            return;
        }

        if (enemy.IsDestination)
        {
            enemy.IsMoving = false;
            enemy.ChangeState(new IdleState());
        }
    }

    public void OnExit(Enemy enemy)
    {
        
    }
}
