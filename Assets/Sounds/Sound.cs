using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public SoundType type;
    public AudioClip clip;
    [Range(0f,1f)] public float volume;
    public bool loop;
    [HideInInspector] public AudioSource source;

    public void Play() {
        source.volume = volume * AudioManager.Instance.masterVolume * AudioManager.Instance.getVolume(type);
        source.Play();
    }

    public void Stop() {
        source.Stop();
    }
}
