using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : GameUnit
{
    [SerializeField] GameObject weaponSprite;
    public const float TIME_ACTIVE = 1.5f;

    public void Throw(Character attacker, Action<Character, Character> onHit)
    {
        Bullet bullet = SimplePool.Spawn<Bullet>(poolType, Tf.position, Quaternion.identity);
        bullet.OnInit(attacker, onHit);
        bullet.DelayDespawnBullet();
        weaponSprite.gameObject.SetActive(false);

        StartCoroutine(DelayActiveWeapon(TIME_ACTIVE));
    }

    private IEnumerator DelayActiveWeapon(float time)
    {
        yield return Cache.GetWFS(time);
        weaponSprite.gameObject.SetActive(true);
    }
}



