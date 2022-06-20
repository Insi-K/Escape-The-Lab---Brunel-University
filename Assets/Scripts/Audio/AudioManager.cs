using UnityEngine.Audio;
using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Variables
    //Convert object into a singleton
    public static AudioManager instance;

    //Soundtracks and SFX Array
    public Sound[] sounds;
    //Main Soundtrack AudioSource.
    public AudioSource sSource;
    //Current soundtrack (non-SFX type)
    private Sound currentTrack;

    private float minVolume = 0f;
    private float maxVolume;

    public float fadeInDuration = 2f;
    public float fadeOutDuration = 2f;
    #endregion

    #region Audio Fade Coroutines
    private IEnumerator fadeIn;
    private IEnumerator fadeOut;
    #endregion

    #region Audio Manager SetUp
    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        sSource.volume = 0f;

        //Genereate SoundSource only if the sound is SFX type. 
        foreach(Sound s in sounds)
        {
            if (s.type == Sound.soundType.SFX)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.maxVolume;
                s.source.pitch = s.pitch;
                s.source.loop = false;
            }
        }
    }
    #endregion

    #region Audio Functions
    public void SetSound()
    {
        sSource.clip = instance.currentTrack.clip;
        maxVolume = instance.currentTrack.maxVolume;
        sSource.pitch = instance.sSource.pitch;
    }

    public void Play(string Track)
    {
        instance.currentTrack = Array.Find(instance.sounds, sound => sound.name == Track);

        if (instance.currentTrack == null)
        {
            Debug.LogWarning("Sound: " + Track + " not found!");
            return;
        }

        if (fadeOut != null) StopCoroutine(fadeOut);
        SetSound();
        instance.sSource.Play();
        fadeIn = FadeIn(sSource, fadeInDuration, maxVolume);
        StartCoroutine(fadeIn);
    }

    public void Stop()
    {
        if (currentTrack == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        fadeOut = FadeOut(sSource, fadeOutDuration, minVolume);
        if(sSource.isPlaying)
        {
            StopCoroutine(fadeIn);
            StartCoroutine(fadeOut);
        }
    }

    public void ChangeTrack(string newTrack)
    {
        Stop();
        Play(newTrack);
    }
    #endregion

    #region SFX Functions
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + " does not exist!");
            return;
        }
        s.source.Play();
    }
    #endregion

    #region Audio Fades (In/Out)
    IEnumerator FadeIn(AudioSource aSource, float duration, float targetVolume)
    {
        float timer = 0f;
        float currentVolume = aSource.volume;
        float targetValue = Mathf.Clamp(targetVolume, minVolume, maxVolume);

        while(timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            var newVolume = Mathf.Lerp(currentVolume, targetValue, timer / duration);
            aSource.volume = newVolume;
            yield return null;
        }
    }

    IEnumerator FadeOut(AudioSource aSource, float duration, float targetVolume)
    {
        float timer = 0f;
        float currentVolume = aSource.volume;
        float targetValue = Mathf.Clamp(targetVolume, minVolume, maxVolume);

        while (aSource.volume > 0)
        {
            timer += Time.unscaledDeltaTime;
            var newVolume = Mathf.Lerp(currentVolume, targetValue, timer / duration);
            aSource.volume = newVolume;
            yield return null;
        }
    }
    #endregion
}
