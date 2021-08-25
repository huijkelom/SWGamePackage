using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;

    /// <summary>
    /// [Deprecated] Use Clips instead.
    /// </summary>
    [Tooltip("[Deprecated] Use Clips instead.")]
    public AudioClip Clip;

    public AudioClip[] Clips = new AudioClip[1];
    [Space]
    [Range(0, 1)] public float Volume = 1;
    [Range(0, 3)] public float Pitch = 1;
    [Space]
    public bool Loop = false;
    public bool PlayOnAwake = false;

    [HideInInspector] public AudioSource Source;
    [HideInInspector] public SoundType Type;
    [HideInInspector] public float MaxVolume;

    public void SetClip(AudioClip clip)
    {
        Clips[0] = clip;
        Source.clip = clip;
    }

    public void SetVolume(float value)
    {
        Volume = value;
        Source.volume = value;
    }

    public void ApplyValues()
    {
        Source.volume = Volume;
        Source.pitch = Pitch;

        Source.loop = Loop;
        Source.playOnAwake = PlayOnAwake;
    }
}

public enum SoundType
{
    Sound,
    Music,
    Dialogue
}