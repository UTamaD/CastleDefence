using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SceneSingleton<SoundManager>
{
    
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip gameOverMusic;
    public List<AudioClip> sfxClips;
    
    private Dictionary<string, AudioClip> sfxDictionary;
    
    
    void Start()
    {
        sfxDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in sfxClips)
        {
            sfxDictionary[clip.name] = clip;
        }

        
        if (backgroundMusic != null)
        {
            PlayMusic(backgroundMusic);
        }
    }
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    
    public void PlaySFX(string clipName)
    {
        if (sfxDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SoundManager: SFX clip not found: " + clipName);
        }
    }

}
