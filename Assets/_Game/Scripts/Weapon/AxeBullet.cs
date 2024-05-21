using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeBullet : Bullet
{
    private void Update()
    {
        Move();
    }

    public override void Move()
    {
        base.Move();
        Tf.Translate(Vector3.forward * speed * Time.deltaTime);
        Tf.Rotate(Vector3.forward, 360 * Time.deltaTime);
    }
}
