using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject weaponSprite;
    [SerializeField] PoolType poolType;

    public void Throw(Character attacker, Action<Character, Character> onHit)
    {
        Vector3 target = attacker.GetTarget().Tf.position;
        //kiem tra neu huong tan cong khong thay doi thi moi spawn bullet
        if (Vector3.Dot((target - attacker.Tf.position).normalized, attacker.Tf.forward) > 0.9f)
        {
            Bullet bullet = SimplePool.Spawn<Bullet>(poolType, attacker.GetSpawnPoint().position, Quaternion.identity);
            bullet.OnInit(attacker, onHit, target);
            bullet.DelayDespawnBullet();
            DeactiveWeapon();
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



