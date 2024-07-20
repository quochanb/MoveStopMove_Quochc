using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPool : MonoBehaviour
{
    [SerializeField] private AudioSource prefab;
    [SerializeField] private Transform parent;
    [SerializeField] private int poolSize;

    private Queue<AudioSource> pools = new Queue<AudioSource>();
    private List<AudioSource> listActives = new List<AudioSource>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Despawn(GameObject.Instantiate(prefab, parent));
        }
    }

    public AudioSource Spawn()
    {
        AudioSource source = pools.Count > 0 ? pools.Dequeue() : GameObject.Instantiate(prefab, parent);

        listActives.Add(source);
        source.gameObject.SetActive(true);

        return source;
    }

    public void Despawn(AudioSource source)
    {
        if (source.gameObject.activeSelf)
        {
            source.gameObject.SetActive(false);
            pools.Enqueue(source);
            listActives.Remove(source);
        }
    }

    public void Collect()
    {
        while (listActives.Count > 0)
        {
            Despawn(listActives[0]);
        }
    }

    public void Release()
    {
        Collect();

        while (pools.Count > 0)
        {
            GameObject.Destroy(pools.Dequeue().gameObject);
        }
        pools.Clear();
    }
}
