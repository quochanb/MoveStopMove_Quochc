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

    private Vector3 destination;
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
        Invoke(nameof(OnDespawn), 1.5f);
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
