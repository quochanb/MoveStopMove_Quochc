using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private MiniPool miniPool;
    [SerializeField] private List<Sound> soundList;

    private List<AudioSource> activeSources = new List<AudioSource>();
    private Dictionary<SoundType, Sound> soundDict;
    private bool isMuted = false;

    private void Awake()
    {
        soundDict = new Dictionary<SoundType, Sound>();
        foreach (var sound in soundList)
        {
            soundDict[sound.soundType] = sound;
        }
    }

    public void PlaySound(SoundType soundType)
    {
        if (soundDict.ContainsKey(soundType))
        {
            AudioSource source = miniPool.Spawn(); //sinh ra tu pool
            source.clip = soundDict[soundType].clip;
            source.volume = isMuted ? 0 : soundDict[soundType].volume;
            source.Play();
            activeSources.Add(source);
            StartCoroutine(DeactivateSound(source));//cho deactive source
        }
        else
        {
            Debug.LogError($"No sound pool for sound type: {soundType}");
        }
    }

    public void SoundOff(bool value)
    {
        isMuted = value;
        foreach (var source in activeSources)
        {
            if (source != null && source.isPlaying)
            {
                source.volume = isMuted ? 0 : soundList.Find(s => s.clip == source.clip).volume;
            }
        }
    }

    private IEnumerator DeactivateSound(AudioSource source)
    {
        yield return Cache.GetWFS(source.clip.length);
        miniPool.Despawn(source);
        activeSources.Remove(source);
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
