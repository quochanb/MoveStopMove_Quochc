using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private float speed = 6f;

    protected override void Update()
    {
        base.Update();
        if (currentTarget != null)
        {
            currentTarget.ActiveLockTarget();
            if (currentTarget == null || IsOutOfAttackRange(currentTarget))
            {
                currentTarget.DeactiveLockTarget();
            }
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

    public override void Move()
    {
        base.Move();
        Vector3 nextPoint = Tf.position + Joystick.direction * speed * Time.deltaTime;
        Tf.position = CheckGround(nextPoint);
        Tf.rotation = Quaternion.LookRotation(Joystick.direction);
    }

}
