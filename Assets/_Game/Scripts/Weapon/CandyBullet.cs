using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyBullet : Bullet
{
    private void Update()
    {
        Move();
    }

    public override void Move()
    {
        base.Move();
        //Tf.Rotate(new Vector3(0, 10000, 0) * Time.deltaTime);
    }
}
