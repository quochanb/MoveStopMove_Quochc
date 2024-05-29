using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject weaponSprite;
    [SerializeField] PoolType poolType;

    public void Throw(Character attacker, Action<Character, Character> onHit)
    {
        if (attacker.GetTarget() != null)
        {
            Transform target = attacker.GetTarget().Tf;
            //kiem tra neu huong tan cong khong thay doi thi moi spawn bullet
            if (Vector3.Dot((target.position - attacker.Tf.position).normalized, attacker.Tf.forward) > 0.9f)
            {
                Bullet bullet = SimplePool.Spawn<Bullet>(poolType, attacker.GetSpawnPoint().position, Quaternion.identity);
                bullet.OnInit(attacker, onHit, target.position);
                bullet.DelayDespawnBullet();
                DeactiveWeapon();
            }
        }
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



