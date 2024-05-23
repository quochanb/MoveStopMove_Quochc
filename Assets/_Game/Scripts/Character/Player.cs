using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private float speed = 6f;

    private void Update()
    {
        if (Input.GetMouseButton(0) && Joystick.direction != Vector3.zero)
        {
            Move();
        }
        if (Input.GetMouseButtonUp(0))
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
        FindEnemyTarget();
    }
}
