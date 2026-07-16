using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    #region --Clips--

    [Header("Variables")] 
    [SerializeField] private int minDistanceRolloff;
    [SerializeField] private int maxDistanceRolloff;
    
    [Header("Audio Scriptable Objects")]
    [SerializeField] AudioClipSO[] musicClips;
    [SerializeField] UIAudioClips uiAudioClips;
    [SerializeField] VFXClipsSO vfxClips;
    
    #endregion

    AudioSource musicAudioSource;

    public static AudioManager instance;

    private int levelOffset = 2;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        musicAudioSource = GetComponent<AudioSource>();
    }
    
    public void PlaySound(AudioClipSO soundSO, Vector3 pos)
    {
        GameObject soundObject = new GameObject("Audio Clip Source");
        soundObject.transform.position = pos;
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        
        audioSource.spatialBlend = 1;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = minDistanceRolloff;
        audioSource.maxDistance = maxDistanceRolloff;

        audioSource.clip = soundSO.clip;
        audioSource.loop = soundSO.loop;
        audioSource.volume = soundSO.volume;
        audioSource.pitch = soundSO.pitch;
        audioSource.outputAudioMixerGroup = soundSO.audioMixer;

        audioSource.Play();
        Destroy(soundObject, soundSO.clip.length);
    }

    public void PlaySound(AudioClipSO soundSO)
    {
        GameObject soundObject = new GameObject("Audio Clip Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        
        audioSource.clip = soundSO.clip;
        audioSource.loop = soundSO.loop;
        audioSource.volume = soundSO.volume;
        audioSource.pitch = soundSO.pitch;
        audioSource.outputAudioMixerGroup = soundSO.audioMixer;

        audioSource.Play();
        Destroy(soundObject, soundSO.clip.length);
    }
    
    public void PlayMusic(AudioClipSO musicClip)
    {
        musicAudioSource.Stop();
        
        musicAudioSource.clip = musicClip.clip;
        musicAudioSource.loop = musicClip.loop;
        musicAudioSource.volume = musicClip.volume;
        musicAudioSource.pitch = musicClip.pitch;
        musicAudioSource.outputAudioMixerGroup = musicClip.audioMixer;
    
        musicAudioSource.Play();
    }

    public void PlayRandomMusic()
    {
        if (musicClips.Length > 0)
        {
            int randomTrack = Random.Range(0, musicClips.Length);
            musicAudioSource.Stop();

            musicAudioSource.clip = musicClips[randomTrack].clip;
            musicAudioSource.loop = musicClips[randomTrack].loop;
            musicAudioSource.volume = musicClips[randomTrack].volume;
            musicAudioSource.pitch = musicClips[randomTrack].pitch;
            musicAudioSource.outputAudioMixerGroup = musicClips[randomTrack].audioMixer;

            musicAudioSource.Play();
        }
    }
    
    #region -- UI SFX --

    public void PlayLevelEndSFX()
    {
        PlaySound(uiAudioClips.endLevelSound);
    }
    
    public void PlayUISelect()
    {
        PlaySound(uiAudioClips.selectSound);
    }

    public void PlayUIClick()
    {
        PlaySound(uiAudioClips.clickSound);
    }
    
    public void PlayUIClose()
    {
       // PlaySound(uiAudioClips.closeSound);
    }
    
    public void PlayUIError()
    {
        PlaySound(uiAudioClips.errorSound);
    }

    public void PlayInsufficientCredits()
    {
        PlaySound(uiAudioClips.insufficientCredits);
    }

    public void PlayLevelCleared()
    {
        // PlaySound(vfxClips.levelCleared);
    }
    
    public void PlayLevelLost()
    {
        if(uiAudioClips.levelLostSound != null)
            PlaySound(uiAudioClips.levelLostSound);
    }

    public void UISpecialWeaponClick()
    {
        if(uiAudioClips.specialWeaponSound != null)
            PlaySound(uiAudioClips.specialWeaponSound);
    }
    
    #endregion
}
