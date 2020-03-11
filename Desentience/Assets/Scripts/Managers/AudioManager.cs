using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

//https://www.youtube.com/watch?v=6OT43pvUyfY
[RequireComponent(typeof(DDOL))]
public class AudioManager : MonoBehaviour
{

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }

    [Range(0f, 1f)]
    public float masterVolume = 1f;
    [Range(.1f, 3f)]
    public float masterPitch = 1f;

    // It is not necessary to have all sounds here
    public Sound[] sounds;

    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("Background_Music");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("No Sound with name " + name + " was found");
            return;
        }
        s.source.volume = s.volume * masterVolume;
        s.source.pitch = s.pitch * masterPitch;
        s.source.Play();
    }

    public void PlayOneShot(AudioClip clip)
    {
        GameObject soundObject = new GameObject("OneShotSound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(clip);
    }
}
