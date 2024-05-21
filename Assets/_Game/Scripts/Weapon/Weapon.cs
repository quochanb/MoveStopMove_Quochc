using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected BulletBase bulletPrefab;
    [SerializeField] protected Transform bulletPoint;
    
    private Transform tf;
    protected Transform Tf
    {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }

    public WeaponType weaponType;

    public virtual void Throw()
    {
        
    }
}

public enum WeaponType
{
    Knife = 0,
    Axe = 1,
    Boomerang = 2
}
