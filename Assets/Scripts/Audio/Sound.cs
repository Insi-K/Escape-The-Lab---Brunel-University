using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public enum soundType { SFX, Soundtrack};

    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float maxVolume;

    [Range(0.1f, 3f)]
    public float pitch;

    public soundType type;

    [HideInInspector]
    public AudioSource source;
}
