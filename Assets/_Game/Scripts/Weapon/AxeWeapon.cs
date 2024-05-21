using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeWeapon : Weapon
{
    public override void Throw()
    {
        base.Throw();
        BulletBase b = SimplePool.Spawn<BulletBase>(PoolType.AxeBullet, bulletPoint.position, bulletPoint.rotation);
       
        
    }
}
