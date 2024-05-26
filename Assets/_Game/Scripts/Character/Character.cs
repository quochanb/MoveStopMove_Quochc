using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Character : GameUnit
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Animator anim;
    [SerializeField] protected GameObject body, lockTarget;
    [SerializeField] protected Transform bulletSpawnPoint;
    [SerializeField] protected float radius = 5f;
    [SerializeField] protected Character currentTarget;
    [SerializeField] protected WeaponType currentWeaponType;
    [SerializeField] protected LayerMask characterLayer, groundLayer;
    protected bool isDead, isAttack, isMoving;
    private string currentAnim;

    public Weapon weapon;
    //public bool IsDead => isDead = true;
    public Collider[] enemyInAttackRange = new Collider[10];

    private void Awake()
    {
        weapon = FindObjectOfType<Weapon>();
        OnInit();
    }

    protected virtual void Update()
    {
        if (isDead)
        {
            return;
        }
        if (currentTarget == null || IsOutOfAttackRange(currentTarget))
        {
            FindEnemyTarget();
        }
    }

    protected virtual void OnInit()
    {
        isDead = false;
        isMoving = false;
        isAttack = false;
    }

    protected virtual void OnDespawn()
    {
        Destroy(this.gameObject);
    }


    public Vector3 CheckGround(Vector3 nextPoint)
    {
        RaycastHit hit;
        if (Physics.Raycast(nextPoint, Vector3.down, out hit, 2f, groundLayer))
        {
            return hit.point + Vector3.up;
        }
        return Tf.position;
    }

    public virtual void Move()
    {
        isMoving = true;
        ChangeAnim(Constants.ANIM_RUN);
    }

    public virtual void StopMove()
    {
        isMoving = false;

        if (currentTarget == null)
        {
            ChangeAnim(Constants.ANIM_IDLE);
        }
        else
        {
            Attack(currentTarget);
        }
    }

    public virtual void Attack(Character target)
    {
        if (!isAttack && !target.isDead && !IsOutOfAttackRange(currentTarget))
        {
            Tf.LookAt(target.Tf);
            ChangeAnim(Constants.ANIM_ATTACK);
            StartCoroutine(ThrowWeapon(0.3f));
            isAttack = true;
            StartCoroutine(DelayAttack(1.5f));
        }
    }

    public void Throw(Character attacker, Action<Character, Character> onHit)
    {
        if (currentTarget != null)
        {
            Vector3 target = currentTarget.Tf.position;
            Bullet bullet = SimplePool.Spawn<Bullet>(poolType, bulletSpawnPoint.position, Quaternion.identity);
            bullet.OnInit(attacker, onHit, target);
            bullet.DelayDespawnBullet();
            weapon.DeactiveWeapon();
        }
    }

    public void OnHitVictim(Character attacker, Character victim)
    {
        //victim.OnDead();
    }

    public virtual void OnDead()
    {
        if (!isDead)
        {
            isDead = true;
            ChangeAnim(Constants.ANIM_DEAD);
            Invoke(nameof(OnDespawn), 2f);
        }
    }

    public Character GetTarget()
    {
        if (currentTarget != null)
        {
            return currentTarget;
        }
        return null;
    }

    public void ChangeWeapon(WeaponType weaponType)
    {
        currentWeaponType = weaponType;
    }

    public void ChangeHat()
    {

    }

    public void ChangePant()
    {

    }

    public virtual Character FindEnemyTarget()
    {
        int numberOfCharacterInRange = Physics.OverlapSphereNonAlloc(Tf.position, radius, enemyInAttackRange, characterLayer);

        currentTarget = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < numberOfCharacterInRange; i++)
        {
            if (enemyInAttackRange[i] != null && enemyInAttackRange[i].transform != Tf)
            {
                float distanceToEnemy = Vector3.Distance(Tf.position, enemyInAttackRange[i].transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    currentTarget = Cache.GetCharacter(enemyInAttackRange[i]);
                }
            }
        }

        return currentTarget;
    }

    public void UpSize()
    {
        if (body.transform.localScale.x <= 1.5f)
        {
            body.transform.localScale *= 1.03f;
            radius *= 1.03f;
        }
    }

    public void ActiveLockTarget()
    {
        lockTarget.gameObject.SetActive(true);
    }

    public void DeactiveLockTarget()
    {
        lockTarget.gameObject.SetActive(false);
    }

    public bool IsOutOfAttackRange(Character target)
    {
        if (target == null)
        {
            return true;
        }
        float distanceToTarget = Vector3.Distance(Tf.position, target.Tf.position);
        return distanceToTarget > radius;
    }

    public void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            if (currentAnim != null)
            {
                anim.ResetTrigger(currentAnim);
            }
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }

    private IEnumerator DelayAttack(float time)
    {
        yield return Cache.GetWFS(time);
        isAttack = false;
        ChangeAnim(Constants.ANIM_IDLE);
    }

    private IEnumerator ThrowWeapon(float time)
    {
        yield return Cache.GetWFS(time);
        Throw(this, OnHitVictim);
    }

}
