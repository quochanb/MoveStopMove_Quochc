using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    Character target;

    public void OnEnter(Enemy enemy)
    {
        target = enemy.GetTarget();
    }

    public void OnExecute(Enemy enemy)
    {
        if (target != null)
        {
            enemy.Attack(target);
        }
        else
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Enemy enemy)
    {
        
    }
}
