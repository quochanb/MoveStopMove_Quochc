using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyBullet : Bullet
{
    public override void Move()
    {
        base.Move();
        Tf.eulerAngles += new Vector3(0, 600, 0) * Time.deltaTime;
    }
}
