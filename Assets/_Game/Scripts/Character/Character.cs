using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Character : GameUnit
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Animator anim;
    [SerializeField] protected float radius = 5f;
    [SerializeField] protected GameObject body, lockTarget;
    [SerializeField] protected Transform bulletSpawnPoint;
    [SerializeField] protected Character currentTarget;
    [SerializeField] protected LayerMask characterLayer, groundLayer;

    [SerializeField] protected Transform rightHand, leftHand, head;

    protected bool isDead, isAttack, isMoving;

    public Hat currentHat;
    public Pant currentPant;
    public Shield currentShield;
    public Weapon currentWeapon;

    public HatData hatData;
    public ShieldData shieldData;
    public WeaponData weaponData;
    public bool IsDead => isDead;
    public Collider[] enemyInAttackRange = new Collider[10];

    private string currentAnim;

    private void Awake()
    {
        currentWeapon = FindObjectOfType<Weapon>();
        OnInit();
    }

    protected virtual void Update()
    {

        if (currentTarget == null || IsOutOfAttackRange(currentTarget) || currentTarget.isDead)
        {
            FindEnemyTarget();
        }
    }

    //khoi tao
    public virtual void OnInit()
    {
        isDead = false;
        isMoving = false;
        isAttack = false;
    }

    //goi khi muon huy
    public virtual void OnDespawn()
    {

    }

    //check ground de di chuyen
    public Vector3 CheckGround(Vector3 nextPoint)
    {
        RaycastHit hit;
        if (Physics.Raycast(nextPoint, Vector3.down, out hit, 2f, groundLayer))
        {
            return hit.point + Vector3.up;
        }
        return Tf.position;
    }

    //moving
    public virtual void Move()
    {
        isMoving = true;
        ChangeAnim(Constants.ANIM_RUN);
    }

    //stop moving
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

    //attack
    public virtual void Attack(Character target)
    {
        if (!isAttack && !target.isDead && !IsOutOfAttackRange(currentTarget))
        {
            Tf.LookAt(target.Tf);
            ChangeAnim(Constants.ANIM_ATTACK);
            StartCoroutine(ThrowWeapon(0.24f));
            isAttack = true;
            StartCoroutine(DelayAttack(1f));
        }
    }

    //xu ly khi bullet va cham voi character
    public virtual void OnHitVictim(Character attacker, Character victim)
    {
        victim.OnDead();
    }

    //xu ly khi character die
    public virtual void OnDead()
    {
        isDead = true;
        ChangeAnim(Constants.ANIM_DEAD);
        if (IsDead)
        {
            return;
        }
    }

    //lay ra character muc tieu hien tai
    public Character GetTarget()
    {
        if (currentTarget != null)
        {
            return currentTarget;
        }
        return null;
    }

    //lay vi tri sinh ra bullet
    public Transform GetSpawnPoint()
    {
        return bulletSpawnPoint;
    }

    //change weapon
    public void ChangeWeapon(WeaponType weaponType)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }
        Weapon newWeapon = Instantiate(weaponData.GetWeapon(weaponType), rightHand);
        currentWeapon = newWeapon;

        if(currentWeapon == null)
        {
            Debug.Log("loi xoa");
        }
    }

    //change hat
    public void ChangeHat(HatType hatType)
    {
        if (currentHat != null)
        {
            Destroy(currentHat.gameObject);
            currentHat = null;
        }

        if (hatType != HatType.None)
        {
            Hat newHat = Instantiate(hatData.GetHat(hatType), head);
            currentHat = newHat;
        }
    }

    //change pant
    public void ChangePant(PantType pantType)
    {
        if (currentPant != null)
        {
            if (pantType != PantType.None)
            {
                currentPant.ChangeMaterialPant(pantType);
            }
            else
            {
                currentPant.ResetMaterial();
            }
        }
    }

    //change shield
    public void ChangeShield(ShieldType shieldType)
    {
        if (currentShield != null)
        {
            Destroy(currentShield.gameObject);
            currentShield = null;
        }

        if (shieldType != ShieldType.None)
        {
            Shield newShield = Instantiate(shieldData.GetShield(shieldType), leftHand);
            currentShield = newShield;
        }
    }

    //find enemy gan nhat
    public Character FindEnemyTarget()
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

    //khoa muc tieu
    public void ActiveLockTarget()
    {
        lockTarget.gameObject.SetActive(true);
    }

    //bo khoa muc tieu
    public void DeactiveLockTarget()
    {
        lockTarget.gameObject.SetActive(false);
    }

    //kiem tra character co trong tam danh khong
    public bool IsOutOfAttackRange(Character target)
    {
        if (target == null)
        {
            return true;
        }
        float distanceToTarget = Vector3.Distance(Tf.position, target.Tf.position);
        return distanceToTarget > radius;
    }

    //change anim
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

    //delay den lan tan cong tiep theo
    private IEnumerator DelayAttack(float time)
    {
        yield return Cache.GetWFS(time);
        isAttack = false;
        
    }

    //delay nem vu khi
    private IEnumerator ThrowWeapon(float time)
    {
        yield return Cache.GetWFS(time);
        currentWeapon.Throw(this, OnHitVictim);
        currentWeapon.gameObject.SetActive(false);
    }

    public void ActiveWeapon()
    {
        currentWeapon.gameObject.SetActive(true);
    }
}
