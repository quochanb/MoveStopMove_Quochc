using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private float speed = 6f;
    private int coin;

    public int Coin { get { return coin; } set { coin = value; } }

    protected override void Update()
    {
        if (GameManager.Instance.currentState == GameState.GamePlay)
        {
            base.Update();
            if (IsDead)
            {
                GameManager.Instance.OnRevive();
                return;
            }
            if (currentTarget != null)
            {
                currentTarget.ActiveLockTarget();
                if (currentTarget == null || IsOutOfAttackRange(currentTarget) || currentTarget.IsDead)
                {
                    currentTarget.DeactiveLockTarget();
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                ChangeAnim(Constants.ANIM_IDLE);
            }

            if (Input.GetMouseButton(0) && Joystick.direction != Vector3.zero)
            {
                Move();
            }
            else
            {
                StopMove();
            }
        }
    }

    private void OnEnable()
    {
        UIRevive.reviveEvent += OnRevive;
        Level.winGameEvent += OnWinGame;
    }

    private void OnDisable()
    {
        UIRevive.reviveEvent -= OnRevive;
        Level.winGameEvent -= OnWinGame;
    }

    public override void OnInit()
    {
        base.OnInit();
        coin = 0;
        characterLevel = 0;
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

    public override void Move()
    {
        base.Move();
        Vector3 nextPoint = Tf.position + Joystick.direction * speed * Time.deltaTime;
        Tf.position = CheckGround(nextPoint);
        Tf.rotation = Quaternion.LookRotation(Joystick.direction);
    }

    //xu ly hit enemy
    public override void OnHitVictim(Character attacker, Character victim)
    {
        base.OnHitVictim(attacker, victim);
        AddCoins(5);
    }

    //goi khi hit enemy
    private void AddCoins(int value)
    {
        coin += value;
    }

    //xu ly event revive
    private void OnRevive()
    {
        OnInit();
    }

    //xu ly event win game
    private void OnWinGame()
    {
        StartCoroutine(DelayChangeAnim());
    }

    IEnumerator DelayChangeAnim()
    {
        yield return Cache.GetWFS(1f);
        ChangeAnim(Constants.ANIM_WIN);
    }

    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, radius);
    //}
}
