using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeWeapon : Weapon
{
    public override void Throw()
    {
        Debug.Log("throw");
        base.Throw();
        BulletBase b = SimplePool.Spawn<BulletBase>(PoolType.KnifeBullet, bulletPoint.position, bulletPoint.rotation);
        //b.Tf.position += Tf.forward * 5f * Time.deltaTime;
    }
}
