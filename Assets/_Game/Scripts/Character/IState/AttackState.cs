using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    public void OnEnter(Enemy enemy)
    {
        
    }

    public void OnExecute(Enemy enemy)
    {
        if(enemy.GetTarget() == null)
        {
            enemy.ChangeState(new PatrolState());
        }
        else
        {
            enemy.Attack();
        }
    }

    public void OnExit(Enemy enemy)
    {
        
    }
}
