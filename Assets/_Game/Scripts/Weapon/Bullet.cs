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
    protected Vector3 direction;
    protected Weapon weapon;
    protected Action<Character, Character> onHit;

    private void Awake()
    {
        weapon = FindObjectOfType<Weapon>();
    }

    private void Update()
    {
        Move();
    }

    public virtual void OnInit(Character attacker, Action<Character, Character> onHit, Vector3 target)
    {
        this.attacker = attacker;
        this.onHit = onHit;
        this.target = target;
        
        direction = (target - Tf.position).normalized;
    }

    public virtual void Move()
    {
        if (target != null)
        {
            Tf.position += direction * speed * Time.deltaTime;
            
        }
    }

    public void OnDespawn()
    {
        SimplePool.Despawn(this);
        weapon.ActiveWeapon();
    }

    public void DelayDespawnBullet()
    {
        Invoke(nameof(OnDespawn), 0.9f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_PLAYER) || other.CompareTag(Constants.TAG_ENEMY))
        {
            Character victim = Cache.GetCharacter(other);
            if (victim != attacker)
            {
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
