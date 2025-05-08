using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip buttonHoverSound;
    
    [Header("Settings")]
    [SerializeField] private float musicVolume = 0.5f;
    [SerializeField] private float sfxVolume = 0.75f;
    [SerializeField] private bool playOnAwake = true;
    [SerializeField] private bool loop = true;
    
    // Singleton pattern for easy access
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }
    
    private void Awake()
    {
        // Singleton setup
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        
        // Optional: Keep this object when loading a new scene
        // DontDestroyOnLoad(gameObject);
        
        // Create audio sources if not assigned
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.volume = musicVolume;
            musicSource.loop = loop;
            musicSource.playOnAwake = false;
        }
        
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.volume = sfxVolume;
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }
    }
    
    private void Start()
    {
        if (playOnAwake && backgroundMusic != null)
        {
            PlayMusic(backgroundMusic);
        }
    }
    
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        
        musicSource.clip = clip;
        musicSource.Play();
    }
    
    public void StopMusic()
    {
        musicSource.Stop();
    }
    
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        
        sfxSource.PlayOneShot(clip);
    }
    
    public void PlayButtonHoverSound()
    {
        if (buttonHoverSound == null) return;
        
        sfxSource.PlayOneShot(buttonHoverSound);
    }
    
    // Volume control methods
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = volume;
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = volume;
    }
}
