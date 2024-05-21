using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBullet : Bullet
{
    [SerializeField] float returnTime = 1.5f;
    private Vector3 startPosition;

    private void Update()
    {
        Move();
    }

    public override void OnInit(Character attacker, Action<Character, Character> onHit)
    {
        base.OnInit(attacker, onHit);
        startPosition = Tf.position;
    }

    public override void Move()
    {
        float distance = Vector3.Distance(Tf.position, startPosition);
        if (distance < speed * returnTime)
        {
            //di chuyen ve phia truoc
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            //quay tro lai
            Vector3 directionToAttacker = (attacker.Tf.position - Tf.position).normalized;
            transform.Translate(directionToAttacker * speed * Time.deltaTime);
        }
    }

}
