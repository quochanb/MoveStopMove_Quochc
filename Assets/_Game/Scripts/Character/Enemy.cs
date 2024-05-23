using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private NavMeshAgent agent;
    private Vector3 destination;
    private IState currentState;


    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
    }

    public override void Move()
    {
        base.Move();
        Vector3 point;
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (NavmeshRandomPoint(Tf.position, 10, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                agent.SetDestination(point);
            }
        }
    }

    public override void StopMove()
    {
        base.StopMove();
        SetDestination(Tf.position);
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        agent.SetDestination(destination);
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
}
