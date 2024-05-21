using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private AxeBullet axeBulletPrefab;
    [SerializeField] private KnifeBullet knifeBulletPrefab;
    [SerializeField] private BoomerangBullet boomerangBulletPrefab;

    protected WeaponType weaponType;
    
    public void Throw(Character attacker, Action<Character, Character> onHit)
    {
        Bullet bullet = null;
        switch (weaponType)
        {
            case WeaponType.Knife:
                bullet = SimplePool.Spawn<KnifeBullet>(PoolType.KnifeBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                break;
            case WeaponType.Axe:
                bullet = SimplePool.Spawn<AxeBullet>(PoolType.AxeBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                break;
            case WeaponType.Boomerang:
                bullet = SimplePool.Spawn<BoomerangBullet>(PoolType.BoomerangBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                break;
        }

        if(bullet != null)
        {
            bullet.OnInit(attacker, onHit);
        }
    }
}

public enum WeaponType
{
    Knife = 0,
    Axe = 1,
    Boomerang = 2
}

