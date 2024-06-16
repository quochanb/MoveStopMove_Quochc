using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private Sound[] sounds;
    public AudioSource audioSource;

    private Dictionary<SoundType, Sound> soundsDict;

    private void Awake()
    {
        soundsDict = new Dictionary<SoundType, Sound>();

        foreach (Sound sound in sounds)
        {
            soundsDict[sound.soundType] = sound;
        }
    }

    public void PlaySound(SoundType soundType)
    {
        if (!soundsDict.ContainsKey(soundType))
        {
            Debug.LogError($"Sound of type {soundType} not found");
        }
        else
        {
            audioSource.clip = soundsDict[soundType].clip;
            audioSource.volume = soundsDict[soundType].volume;
            audioSource.Play();
        }
    }

    public void SoundOff(bool value)
    {
        audioSource.mute = value;
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
