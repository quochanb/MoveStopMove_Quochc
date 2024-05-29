using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    Character target;
    float timer;
    float delayTime;
    public void OnEnter(Enemy enemy)
    {
        target = enemy.GetTarget();
        timer = 0;
        delayTime = 1f;
    }

    public void OnExecute(Enemy enemy)
    {
        if (enemy.IsDead)
        {
            return;
        }
        if (target != null)
        {
            enemy.Attack(target);
        }
        if (target == null || enemy.IsOutOfAttackRange(target) || target.IsDead)
        {
            timer += Time.deltaTime;
            if (timer >= delayTime)
            {
                enemy.ChangeState(new PatrolState());
            }
        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}
