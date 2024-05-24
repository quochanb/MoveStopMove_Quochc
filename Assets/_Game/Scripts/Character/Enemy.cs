using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private NavMeshAgent agent;
    private Vector3 destination;
    private IState currentState;

    public bool IsDestination => Vector3.Distance(Tf.position, destination + (Tf.position.y - destination.y) * Vector3.up) < 0.1f;

    private void Update()
    {
        if (isDead)
        {
            return;
        }
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
        ChangeState(new PatrolState());
    }

    public override void Move()
    {
        base.Move();
        agent.isStopped = false;
    }

    public override void StopMove()
    {
        base.StopMove();
        agent.isStopped = true;
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        agent.SetDestination(destination);
    }

    public Vector3 GetNextPoint()
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
        agent.isStopped = true;
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
        if (NavMesh.SamplePosition(randomPoint, out hit, radius, 1))
        {
            point = hit.position;
            return true;
        }
        point = Vector3.zero;
        return false;
    }
}
