using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Character : GameUnit
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Animator anim;
    [SerializeField] protected float radius = 5f;
    [SerializeField] protected Character currentTarget;
    [SerializeField] protected Transform bulletSpawnPoint;
    [SerializeField] protected Transform indicatorTf;
    [SerializeField] protected Transform rightHand, leftHand, head;
    [SerializeField] protected GameObject body, lockTarget;
    [SerializeField] protected LayerMask characterLayer, groundLayer;

    [SerializeField] private Hat currentHat;
    [SerializeField] private Pant currentPant;
    [SerializeField] private Shield currentShield;
    [SerializeField] private Weapon currentWeapon;

    [SerializeField] private HatData hatData;
    [SerializeField] private ShieldData shieldData;
    [SerializeField] private WeaponData weaponData;

    protected List<Character> targetList = new List<Character>();
    protected bool isDead, isAttack, isMoving;
    protected float currentSize;
    protected float newSize;

    private string currentAnim;
    private string characterName;
    private int characterScore;

    public bool IsDead => isDead;
    public int Score { get => characterScore; set => characterScore = value; }
    public string Name { get => characterName; set => characterName = value; }
    public float Size { get => currentSize; set => currentSize = value; }
    public Transform IndicatorTf => indicatorTf;
    public Collider[] enemyInAttackRange = new Collider[10];

    private void Awake()
    {
        OnInit();
    }

    protected virtual void Update()
    {
        currentTarget = FindEnemyTarget();
    }

    //khoi tao
    public virtual void OnInit()
    {
        isDead = false;
        isMoving = false;
        isAttack = false;
        this.gameObject.layer = 3; //character layer
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
            return hit.point + new Vector3(0, 0.978f, 0);
        }
        return Tf.position;
    }

    //moving
    public virtual void Move()
    {
        if (isDead) return;

        isMoving = true;
        ChangeAnim(Constants.ANIM_RUN);
    }

    //stop moving
    public virtual void StopMove()
    {
        if (isDead) return;

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
        if (!isAttack && !IsOutOfAttackRange(currentTarget))
        {
            isAttack = true;
            ChangeAnim(Constants.ANIM_ATTACK);
            Tf.LookAt(new Vector3(target.Tf.position.x, Tf.position.y, target.Tf.position.z));
            StartCoroutine(ThrowWeapon(0.21f));
            SoundManager.Instance.PlaySound(SoundType.ThrowWeapon);
            StartCoroutine(ResetAttack());
        }
    }

    //xu ly khi bullet va cham voi character
    public virtual void OnHitVictim(Character attacker, Character victim)
    {
        UpdateScoreOnHit(attacker, victim);
        victim.OnDead();
    }

    //xu ly khi character die
    public virtual void OnDead()
    {
        if (isDead)
        {
            StopAllCoroutines();
            return;
        }
        else
        {
            isDead = true;
            ChangeAnim(Constants.ANIM_DEAD);
            this.gameObject.layer = 0; //doi layer de chuyen current target = null
            SoundManager.Instance.PlaySound(SoundType.Die);
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

    //lay ra level cua character de tinh upsize
    public int CalculateLevel(int score)
    {
        int level = 0; //gia su ban dau level = 0
        while (score > level * (level + 1))
        {
            level++;
        }
        return level;
    }

    //tinh toan score cho charater
    public void UpdateScoreOnHit(Character attacker, Character victim)
    {
        int attackerScore = attacker.Score;
        int victimScore = victim.Score;
        int level = CalculateLevel(attackerScore);

        if (level <= 2)
        {
            if (attackerScore > victimScore)
                attacker.AddScore(1);
            else
                attacker.AddScore(2);
        }
        else if (level <= 4)
        {
            if (attackerScore > victimScore)
                attacker.AddScore(2);
            else
                attacker.AddScore(3);
        }
        else if (level <= 6)
        {
            if (attackerScore > victimScore)
                attacker.AddScore(2);
            else
                attacker.AddScore(4);
        }
        else if (level <= 8)
        {
            if (attackerScore > victimScore)
                attacker.AddScore(3);
            else
                attacker.AddScore(5);
        }
        else
        {
            if (attackerScore > victimScore)
                attacker.AddScore(3);
            else
                attacker.AddScore(6);
        }
    }

    //goi khi hit character
    public virtual void AddScore(int score)
    {
        characterScore += score;
        int currentLevel = CalculateLevel(characterScore);
        if (characterScore >= currentLevel * (currentLevel + 1))
        {
            UpSize(Constants.UP_SIZE);
        }
    }

    //goi khi character dat moc score
    public virtual void UpSize(float value)
    {
        newSize = currentSize + value;
        SetSize(newSize);
        currentSize = newSize;
    }

    //set size
    public void SetSize(float newSize)
    {
        currentSize = Mathf.Clamp(newSize, Constants.MIN_SIZE, Constants.MAX_SIZE);
        body.transform.localScale = currentSize * Vector3.one;
        radius = currentSize / 0.17f;
    }

    //khoa muc tieu
    public void ActiveLockTarget()
    {
        lockTarget.gameObject.SetActive(true);
    }

    //bo khoa muc tieu
    public void DeactiveLockTarget()
    {
        if (lockTarget.gameObject.activeSelf)
        {
            lockTarget.gameObject.SetActive(false);
        }
    }

    //kiem tra character co trong tam danh khong
    public bool IsOutOfAttackRange(Character target)
    {
        if (target == null)
        {
            return true;
        }
        float distanceToTarget = Vector3.Distance(Tf.position, target.Tf.position);
        return distanceToTarget > radius * 0.9f;
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

    //bat weapon
    public void ActiveWeapon()
    {
        currentWeapon.gameObject.SetActive(true);
    }

    //tat weapon
    public void DeactiveWeapon()
    {
        currentWeapon.gameObject.SetActive(false);
    }

    //delay den lan tan cong tiep theo
    private IEnumerator ResetAttack()
    {
        yield return Cache.GetWFS(0.9f);
        isAttack = false;
        ActiveWeapon();
        if (!isDead)
        {
            if (!isMoving)
            {
                ChangeAnim(Constants.ANIM_IDLE);
            }
            if (isMoving)
            {
                ChangeAnim(Constants.ANIM_RUN);
            }
        }
    }

    //delay nem vu khi
    private IEnumerator ThrowWeapon(float time)
    {
        yield return Cache.GetWFS(time);
        currentWeapon.Throw(this, OnHitVictim);
    }
}
