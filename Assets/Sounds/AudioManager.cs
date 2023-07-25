using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public Sound[] sounds;

    [Range(0f,1f)] public float masterVolume;
    [SerializeField] [Range(0f,1f)] private float effectsVolume;
    [SerializeField] [Range(0f,1f)] private float musicVolume;

    private void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(this);
            return;
        }

        Instance = this;
        foreach(Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;
        }

        DontDestroyOnLoad(gameObject);
    }

    public float getVolume(SoundType type) {
        switch(type) {
            case SoundType.soundEffect:
                return effectsVolume;
            case SoundType.music:
                return musicVolume;
            default:
                return masterVolume;
        }
    }

    public void Play(string soundName) {
        Sound sound = Array.Find(sounds, s => s.name == soundName);
        sound.Play();
    }

    public void Stop(string soundName) {
        Sound sound = Array.Find(sounds, s => s.name == soundName);
        sound.Stop();
    }
}
