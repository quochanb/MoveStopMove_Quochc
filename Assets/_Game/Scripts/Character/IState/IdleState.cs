using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private float randomTime;
    private float timer;

    public void OnEnter(Enemy enemy)
    {
        enemy.ChangeAnim(Constants.ANIM_IDLE);
        randomTime = Random.Range(1, 4);
        timer = 0f;
    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (enemy.IsDead)
        {
            return;
        }

        if (enemy.GetTarget() != null && !enemy.IsOutOfAttackRange(enemy.GetTarget()))
        {
            enemy.ChangeState(new AttackState());

        }

        else if (timer >= randomTime)
        {
            enemy.ChangeState(new PatrolState());

        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}
