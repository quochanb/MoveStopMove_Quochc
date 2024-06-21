using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

public class Enemy : Character
{
    public delegate void OnDeathDelegate();
    public static OnDeathDelegate onDeathEvent;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private ColorData colorData;

    private Vector3 destination;
    private ColorType colorType;
    private Color color;
    private Target target;

    public IState currentState;
    public bool IsDestination => Vector3.Distance(Tf.position, destination + (Tf.position.y - destination.y) * Vector3.up) < 0.1f;
    public bool IsMoving => isMoving;

    protected override void Update()
    {
        if (GameManager.Instance.IsGameState(GameState.GamePlay))
        {
            base.Update();
            if (currentState != null)
            {
                currentState.OnExecute(this);
            }
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
        SetSize(Random.Range(Constants.MIN_SIZE, Constants.MAX_SIZE));
        this.Name = RandomName.GetRandomEnemyName();
        this.Score = 0;

        int weaponIndex = Random.Range(0, Enum.GetValues(typeof(WeaponType)).Length);
        int hatIndex = Random.Range(0, Enum.GetValues(typeof(HatType)).Length);
        int panIndex = Random.Range(0, Enum.GetValues(typeof(PantType)).Length);
        int shieldIndex = Random.Range(0, Enum.GetValues(typeof(ShieldType)).Length);
        int colorIndex = Random.Range(0, Enum.GetValues(typeof(ColorType)).Length);

        ChangeWeapon((WeaponType)weaponIndex);
        ChangeHat((HatType)hatIndex);
        ChangePant((PantType)panIndex);
        ChangeShield((ShieldType)shieldIndex);
        ChangeColor((ColorType)colorIndex);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        SimplePool.Despawn(this);
    }

    //move
    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        agent.SetDestination(destination);
    }

    //lay random vi tri di chuyen
    public Vector3 GetRandomPoint()
    {
        Vector3 point;
        if (NavmeshRandomPoint(Tf.position, 20, out point))
        {
            return point;
        }
        return Tf.position;
    }

    //xu ly khi enemy die
    public override void OnDead()
    {
        if (isDead)
        {
            return;
        }
        base.OnDead();
        
        onDeathEvent?.Invoke(); //phat di su kien
        agent.isStopped = true;
        Invoke(nameof(OnDespawn), 1.2f);
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    //random score
    public void SetScoreEnemy(int score)
    {
        if (score >= 0 && score < 10)
        {
            this.Score = Random.Range(0, score + 3);
        }
        else
        {
            this.Score = Random.Range(score - 5, score + 6);
        }
    }

    //lay color de set cho indicator
    public void UpdateTargetColor()
    {
        if (target == null)
        {
            target = GetComponent<Target>();
        }
        if (target != null)
        {
            target.TargetColor = color;
        }
    }

    //doi mau enemy
    private void ChangeColor(ColorType colorType)
    {
        this.colorType = colorType;
        skinnedMeshRenderer.material = colorData.GetMaterial(colorType);
        color = colorData.GetMaterial(colorType).color;
    }

    private bool NavmeshRandomPoint(Vector3 center, float radius, out Vector3 point)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * radius;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
        {
            point = hit.position;
            return true;
        }
        point = Vector3.zero;
        return false;
    }
}
