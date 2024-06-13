using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] PoolType bulletType;

    public void Throw(Character attacker, Action<Character, Character> onHit)
    {
        if (attacker.GetTarget() != null)
        {
            Transform target = attacker.GetTarget().Tf;
            //kiem tra neu huong tan cong khong thay doi thi moi spawn bullet
            if (Vector3.Dot((target.position - attacker.Tf.position).normalized, attacker.Tf.forward) > 0.9f)
            {
                Bullet bullet = SimplePool.Spawn<Bullet>(bulletType, attacker.GetSpawnPoint().position, Quaternion.identity);
                bullet.OnInit(attacker, onHit, target.position);
                bullet.SetWeaponOnHand(this);
                attacker.DeactiveWeapon();
            }
        }
    }
}



