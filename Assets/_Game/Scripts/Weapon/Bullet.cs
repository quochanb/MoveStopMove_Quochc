using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : GameUnit
{
    [SerializeField] protected float speed = 5f;
    protected Character attacker;
    protected Vector3 target;
    protected Action<Character, Character> onHit;

    public virtual void OnInit(Character attacker, Action<Character, Character> onHit, Vector3 target)
    {
        this.attacker = attacker;
        this.onHit = onHit;
        this.target = target;
    }

    public virtual void Move()
    {
        if (target != null)
        {
            Tf.position = Vector3.MoveTowards(Tf.position, target, speed * Time.deltaTime);
        }
    }

    public void OnDespawn()
    {
        SimplePool.Despawn(this);
    }

    public void DelayDespawnBullet()
    {
        Invoke(nameof(OnDespawn), 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_CHARACTER))
        {
            Character victim = Cache.GetCharacter(other);
            if (victim != attacker)
            {
                attacker.UpSize();
                onHit?.Invoke(attacker, victim);
                OnDespawn();
            }
        }

        if (other.CompareTag(Constants.TAG_OBSTACLE))
        {
            OnDespawn();
        }
    }
}
