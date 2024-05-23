using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    public void OnEnter(Enemy enemy)
    {
        
    }

    public void OnExecute(Enemy enemy)
    {
        if(enemy.GetTarget() != null)
        {
            enemy.ChangeState(new AttackState());
        }
        else
        {
            enemy.Move();
        }
    }

    public void OnExit(Enemy enemy)
    {
        
    }
}
