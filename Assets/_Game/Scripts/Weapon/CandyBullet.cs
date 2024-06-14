using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyBullet : Bullet
{
    public override void Move()
    {
        base.Move();
        Tf.eulerAngles += new Vector3(0, 1000, 0) * Time.deltaTime; //xoay bullet
        DelayDespawnBullet(); //thoi gian cho de huy bullet
    }
}
