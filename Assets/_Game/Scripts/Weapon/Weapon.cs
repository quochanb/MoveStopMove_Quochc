using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : GameUnit
{
    [SerializeField] GameObject weaponSprite;
    public const float TIME_ACTIVE = 1.2f;

    private Vector3 target;

    public void Throw(Character attacker, Action<Character, Character> onHit)
    {
        target = attacker.GetTarget().position;
        Bullet bullet = SimplePool.Spawn<Bullet>(poolType, Tf.position, Quaternion.identity);
        bullet.OnInit(attacker, onHit, target);
        bullet.DelayDespawnBullet();
        DeactiveWeapon();

        StartCoroutine(DelayActiveWeapon(TIME_ACTIVE));
    }

    public void ActiveWeapon()
    {
        weaponSprite.gameObject.SetActive(true);
    }

    public void DeactiveWeapon()
    {
        weaponSprite.gameObject.SetActive(false);
    }

    private IEnumerator DelayActiveWeapon(float time)
    {
        yield return Cache.GetWFS(time);
        ActiveWeapon();
    }
}



