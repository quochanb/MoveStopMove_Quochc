using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : GameUnit
{
    [SerializeField] GameObject weaponSprite;

    private Vector3 target;

    public void Throw(Character attacker, Action<Character, Character> onHit)
    {
        target = attacker.GetTarget().Tf.position;
        Bullet bullet = SimplePool.Spawn<Bullet>(poolType, Tf.position, Quaternion.identity);
        bullet.OnInit(attacker, onHit, target);
        bullet.DelayDespawnBullet();
        Invoke(nameof(DeactiveWeapon), 0.2f);

        Invoke(nameof(ActiveWeapon), 1.2f);
    }

    public void ActiveWeapon()
    {
        weaponSprite.gameObject.SetActive(true);
    }

    public void DeactiveWeapon()
    {
        weaponSprite.gameObject.SetActive(false);
    }

}



