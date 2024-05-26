using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : GameUnit
{
    [SerializeField] GameObject weaponSprite;

    public void ActiveWeapon()
    {
        weaponSprite.gameObject.SetActive(true);
    }

    public void DeactiveWeapon()
    {
        weaponSprite.gameObject.SetActive(false);
    }

}



