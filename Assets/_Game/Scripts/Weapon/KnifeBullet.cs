using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBullet : Bullet
{
    public override void OnInit(Character attacker, Action<Character, Character> onHit, Vector3 target)
    {
        base.OnInit(attacker, onHit, target);
        Tf.LookAt(target); //huong mui ve phia doi thu
        DelayDespawnBullet(); //thoi gian cho huy bullet
    }
}
