using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : GameUnit
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Animator anim;
    [SerializeField] protected GameObject body;
    [SerializeField] protected float radius = 5f;
    [SerializeField] protected Transform currentTarget;
    [SerializeField] protected WeaponType currentWeaponType;
    [SerializeField] protected LayerMask characterLayer, groundLayer;

    public Weapon weapon;

    private string currentAnim;
    private bool isDead, isAttack, isMoving;

    public Collider[] enemyInAttackRange = new Collider[4];

    private void Awake()
    {
        weapon = FindObjectOfType<Weapon>();
        OnInit();
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
        if (isMoving)
        {
            isAttack = false;
            ChangeAnim(Constants.ANIM_RUN);
        }
    }

    public virtual void StopMove()
    {
        Debug.Log("Stop");
        Attack();
    }

    public void Attack()
    {
        if (currentTarget != null)
        {
            Tf.LookAt(currentTarget);
            ChangeAnim(Constants.ANIM_ATTACK);
            weapon.Throw(this, OnHitVictim);
            isAttack = true;
        }
        if (isAttack)
        {
            StartCoroutine(ChangeAnimAfterDelay(0.5f));
        }
    }

    public void OnHitVictim(Character attacker, Character victim)
    {
        victim.OnDead();
    }

    public Transform GetTarget()
    {
        if (currentTarget != null)
        {
            return currentTarget;
        }
        return null;
    }

    public void OnDead()
    {
        isDead = true;
        if (isDead)
        {
            ChangeAnim(Constants.ANIM_DEAD);
            Invoke(nameof(OnDespawn), 2f);
            return;
        }
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

    public void FindEnemyTarget()
    {
        int numberOfCharacterInRange = Physics.OverlapSphereNonAlloc(Tf.position, radius, enemyInAttackRange, characterLayer);

        currentTarget = null;
        for (int i = 0; i < numberOfCharacterInRange; i++)
        {
            if (enemyInAttackRange[i] != null && enemyInAttackRange[i].transform != Tf)
            {
                currentTarget = enemyInAttackRange[i].transform;
                break;
            }
        }
    }

    public void UpSize()
    {
        if (body.transform.localScale.x <= 1.5f)
        {
            body.transform.localScale *= 1.03f;
        }
    }

    public void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(animName);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }

    private IEnumerator ChangeAnimAfterDelay(float time)
    {
        yield return Cache.GetWFS(time);
        ChangeAnim(Constants.ANIM_IDLE);
        isAttack = false;
    }
}
