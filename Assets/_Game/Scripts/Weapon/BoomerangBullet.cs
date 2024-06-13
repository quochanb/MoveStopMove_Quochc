using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBullet : Bullet
{
    private Transform attackerTransform;
    private Vector3 endPoint;
    private bool isReturning = false;

    public override void OnInit(Character attacker, Action<Character, Character> onHit, Vector3 target)
    {
        base.OnInit(attacker, onHit, target);
        attackerTransform = attacker.Tf;

    }

    public override void Move()
    {
        Tf.eulerAngles += new Vector3(0, 1000, 0) * Time.deltaTime;

        if (!isReturning)
        {
            Tf.position += direction * speed * Time.deltaTime;
            if (Vector3.Distance(Tf.position, target) < 0.1f)
            {
                isReturning = true;
            }
        }
        else
        {
            endPoint = (attackerTransform.position - Tf.position).normalized;
            Tf.position += endPoint * speed * Time.deltaTime;

            if (Vector3.Distance(Tf.position, endPoint) < 0.1f)
            {
                OnDespawn();
            }
        }
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        isReturning = false;
    }

}
