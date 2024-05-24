using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    public void OnEnter(Enemy enemy)
    {
        Vector3 destination = enemy.GetNextPoint();
        enemy.SetDestination(destination);
        enemy.Move();
    }

    public void OnExecute(Enemy enemy)
    {
        if (enemy.IsDestination)
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void OnExit(Enemy enemy)
    {
        
    }
}
