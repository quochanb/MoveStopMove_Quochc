using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private List<Sound> soundList;
    [SerializeField] private int amount;

    private Dictionary<SoundType, MiniPool<AudioSource>> soundPools = new Dictionary<SoundType, MiniPool<AudioSource>>();
    private bool isMuted = false;

    private void Awake()
    {
        foreach (Sound sound in soundList)
        {
            InitSoundPool(sound);
        }
    }

    private void InitSoundPool(Sound sound)
    {
        GameObject soundObject = new($"Sound_{sound.soundType}");
        soundObject.transform.parent = transform; // set parent

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = sound.clip;
        audioSource.volume = sound.volume;
        audioSource.playOnAwake = false;

        MiniPool<AudioSource> miniPool = new MiniPool<AudioSource>();
        miniPool.OnInit(audioSource, amount, transform);
        soundPools[sound.soundType] = miniPool;
    }

    public void PlaySound(SoundType soundType)
    {
        if (soundPools.ContainsKey(soundType))
        {
            AudioSource source = soundPools[soundType].Spawn(); //sinh ra tu pool
            source.volume = isMuted ? 0 : soundList.Find(s => s.soundType == soundType).volume;
            source.Play();
            StartCoroutine(DeactivateSound(source, soundType));//cho deactive source
        }
        else
        {
            Debug.LogError($"No sound pool for sound type: {soundType}");
        }
    }

    public void SoundOff(bool value)
    {
        isMuted = value;
        foreach (var pool in soundPools.Values)
        {
            foreach (var source in pool.GetActiveObjects())
            {
                source.volume = isMuted ? 0 : soundList.Find(s => s.clip == source.clip).volume;
            }
        }
    }

    private IEnumerator DeactivateSound(AudioSource source, SoundType soundType)
    {
        yield return Cache.GetWFS(source.clip.length);
        soundPools[soundType].Despawn(source);
    }
}

[System.Serializable]
public class Sound
{
    public SoundType soundType;
    public AudioClip clip;
    [Range(0, 1)]
    public float volume;
}
