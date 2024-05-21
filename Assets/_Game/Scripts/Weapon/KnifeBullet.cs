using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBullet : Bullet
{
    private void Update()
    {
        Move();
    }

    public override void Move()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
