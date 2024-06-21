using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private CombatText combatTextPrefab;

    private Character targetLocking;
    private string killerName;
    private int coin;

    public string KillerName { get { return killerName; } }
    public int Coin { get { return coin; } set { coin = value; } }

    private void Start()
    {
        this.Score = 0;
        SetSize(1f);
    }

    protected override void Update()
    {
        if (GameManager.Instance.IsGameState(GameState.GamePlay))
        {
            //neu co target va target ko nam trong attack range
            if (currentTarget != null && IsOutOfAttackRange(currentTarget))
            {
                ResetTarget();
            }

            base.Update();

            if (IsDead)
            {
                GameManager.Instance.ShowRevivePopup();
            }

            if (Input.GetMouseButtonUp(0))
            {
                ChangeAnim(Constants.ANIM_IDLE);
            }

            if (Input.GetMouseButton(0) && Vector3.Distance(Joystick.direction, Vector3.zero) > 0.1f)
            {
                Move();
            }
            else
            {
                StopMove();
            }

            SetLockTarget(); //hien thi khoa muc tieu
        }
    }

    private void OnEnable()
    {
        UIRevive.reviveEvent += OnRevivePlayer;
        Level.winGameEvent += OnWinGame;
    }

    private void OnDisable()
    {
        UIRevive.reviveEvent -= OnRevivePlayer;
        Level.winGameEvent -= OnWinGame;
    }

    public override void OnInit()
    {
        base.OnInit();
        coin = 0;
        Name = UserDataManager.Instance.GetUserName();
        SetSize(currentSize);
        ChangeCurrentSkin();
    }

    public void ChangeCurrentSkin()
    {
        UserData userData = UserDataManager.Instance.userData;

        int currentWeaponIndex = userData.currentWeaponIndex;
        int currentHatIndex = userData.currentHatIndex;
        int currentPantIndex = userData.currentPantIndex;
        int currentShieldIndex = userData.currentShieldIndex;

        ChangeWeapon((WeaponType)currentWeaponIndex);
        ChangeHat((HatType)currentHatIndex);
        ChangePant((PantType)currentPantIndex);
        ChangeShield((ShieldType)currentShieldIndex);
    }

    //move
    public override void Move()
    {
        base.Move();
        Vector3 nextPoint = Tf.position + Joystick.direction * speed * Time.deltaTime;
        Tf.position = CheckGround(nextPoint);
        Tf.rotation = Quaternion.LookRotation(Joystick.direction);
    }

    public override void UpSize(float size)
    {
        base.UpSize(size);

        SoundManager.Instance.PlaySound(SoundType.UpSize);
    }

    //xu ly hit enemy
    public override void OnHitVictim(Character attacker, Character victim)
    {
        base.OnHitVictim(attacker, victim);
        killerName = victim.Name;
        AddCoins(5);
    }

    public override void AddScore(int score)
    {
        base.AddScore(score);
        Instantiate(combatTextPrefab, Tf).OnInit(score);
    }

    //goi khi hit enemy
    private void AddCoins(int value)
    {
        coin += value;
        int dataCoin = UserDataManager.Instance.GetUserCoin();
        UserDataManager.Instance.UpdateUserCoin(dataCoin += coin);
    }

    //xu ly event revive
    private void OnRevivePlayer()
    {
        OnInit();
    }

    //xu ly event win game
    private void OnWinGame()
    {
        StartCoroutine(DelayChangeAnim());
    }

    //reset khoa muc tieu
    private void ResetTarget()
    {
        currentTarget.DeactiveLockTarget();
        targetLocking = null;
        currentTarget = null;
    }

    //hien thi khoa muc tieu gan nhat
    private void SetLockTarget()
    {
        if (currentTarget != null && !IsOutOfAttackRange(currentTarget))
        {
            //neu dang co target bi khoa va target do khac currentTarget
            if (targetLocking != null && targetLocking != currentTarget)
            {
                targetLocking.DeactiveLockTarget(); //bo target dang bi khoa
            }

            if (targetLocking == currentTarget)
            {
                return;
            }
            currentTarget.ActiveLockTarget(); //khoa target moi

            targetLocking = currentTarget;
        }

        if (currentTarget != null && currentTarget.IsDead)
        {
            ResetTarget();
        }
    }

    //goi khi win game
    IEnumerator DelayChangeAnim()
    {
        yield return Cache.GetWFS(0.8f);
        ChangeAnim(Constants.ANIM_WIN);
    }

    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, radius);
    //}
}
