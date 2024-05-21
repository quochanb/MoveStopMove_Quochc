using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : GameUnit
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected LayerMask characterLayer, groundLayer;
    [SerializeField] protected float radius = 5f;
    [SerializeField] protected Transform currentTarget;
    [SerializeField] protected WeaponType currentWeaponType;
    [SerializeField] protected CharacterWeapon characterWeapon;

    protected ThrowWeapon throwWeapon;

    private string currentAnim;
    private bool isDead, isAttack, isMoving;

    public Collider[] enemyInAttackRange = new Collider[4];

    private void Start()
    {
        characterWeapon = FindObjectOfType<CharacterWeapon>();
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
        }
    }

    public void StopMove()
    {
        isMoving = false;
        if (currentTarget == null)
        {
            ChangeAnim(Constants.ANIM_IDLE);
            return;
        }
        else
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (!isAttack)
        {
            characterWeapon.gameObject.SetActive(false);
            Tf.LookAt(currentTarget);
            ChangeAnim(Constants.ANIM_ATTACK);
            Debug.Log(this);
            throwWeapon.Throw(this, OnHitVictim);
            isAttack = true;
        }
        else
        {
            characterWeapon.gameObject.SetActive(true);
            isAttack = false;
        }
    }

    public void OnHitVictim(Character attacker, Character victim)
    {
        victim.OnDead();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Tf.position, radius);
    }

    public void FindEnemyTarget()
    {
        int numberOfCharacterInRange = Physics.OverlapSphereNonAlloc(Tf.position, radius, enemyInAttackRange, characterLayer);

        for (int i = 0; i < numberOfCharacterInRange; i++)
        {
            if (enemyInAttackRange[i] != null && enemyInAttackRange[i].transform != Tf)
            {
                currentTarget = enemyInAttackRange[i].transform;
                break;
            }
            else
            {
                currentTarget = null;
            }
        }
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
            Invoke(nameof(OnDespawn), 1f);
            return;
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
}
