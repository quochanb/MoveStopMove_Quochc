using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

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
    public IState currentState;
    public bool IsDestination => Vector3.Distance(Tf.position, destination + (Tf.position.y - destination.y) * Vector3.up) < 0.1f;

    protected override void Update()
    {
        if (GameManager.Instance.currentState == GameState.GamePlay)
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

        int weaponIndex = Random.Range(0, 12);
        int hatIndex = Random.Range(0, 11);
        int panIndex = Random.Range(0, 10);
        int shieldIndex = Random.Range(0, 3);
        int colorIndex = Random.Range(0, 10);

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

    //lay random vi tri
    public Vector3 GetRandomPoint()
    {
        Vector3 point;
        if (NavmeshRandomPoint(Tf.position, 20, out point))
        {
            return point;
        }
        return Tf.position;
    }

    public override void OnDead()
    {
        base.OnDead();
        onDeathEvent?.Invoke();
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

    public Color GetEnemyColor()
    {
        return color;
    }

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
