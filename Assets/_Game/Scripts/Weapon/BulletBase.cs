using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class BulletBase : GameUnit
{
    [SerializeField] protected float speed = 5f;

    private Transform target;

    public Character character;


    private void Start()
    {
        character = FindObjectOfType<Character>();

    }

    private void Update()
    {
        target = character.GetTarget();
        if (target != null)
        {
            Tf.position = Vector3.MoveTowards(Tf.position, target.position, speed * Time.deltaTime);
            Tf.Rotate(Vector3.forward, 30f * Time.deltaTime);
        }
    }

    public void OnInit()
    {

    }

    public void OnDespawn()
    {
        SimplePool.Despawn(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_CHARACTER))
        {
            Character character = Cache.GetCharacter(other);
            character.OnDead();
            OnDespawn();
        }
    }
}
