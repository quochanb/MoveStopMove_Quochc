using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Character target;
    private float timer;
    private float delayTime;

    public void OnEnter(Enemy enemy)
    {
        enemy.ChangeAnim(Constants.ANIM_ATTACK);
        target = enemy.GetTarget();
        timer = 0;
        delayTime = Random.Range(1, 4);
    }

    public void OnExecute(Enemy enemy)
    {
        if (enemy.IsDead)
        {
            return;
        }

        timer += Time.deltaTime;

        if (target != null)
        {
            enemy.Attack(target);
        }

        if (timer >= delayTime)
        {
            enemy.IsMoving = true;
            enemy.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}
