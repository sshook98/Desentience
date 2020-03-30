using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

//https://www.youtube.com/watch?v=6OT43pvUyfY
[RequireComponent(typeof(DDOL))]
public class AudioManager : MonoBehaviour
{
    public AudioMixerGroup masterAudioMixerGroup;
    public AudioMixerGroup musicAudioMixerGroup;
    public AudioMixerGroup sfxAudioMixerGroup;

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

    // It is not necessary to have all sounds here.
    // I'm still trying to determine exactly why the tutorial I got this from did things this way.
    // What I can think of, is that any concurrent persistent sounds (music, dialog, etc.) should each be in here.
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
        Play("Background_Music", musicAudioMixerGroup);
    }

    public void Play(string name, AudioMixerGroup audioMixerGroup)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("No Sound with name " + name + " was found");
            return;
        }
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.outputAudioMixerGroup = audioMixerGroup;
        s.source.Play();
    }

    // Currently, this is only used for sfx. Can be changed if needed.
    public void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1.0f, float pitch = 1.0f)
    {
        GameObject go = new GameObject("OneShotClipAtPoint");
        go.transform.position = position;
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        //audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.outputAudioMixerGroup = sfxAudioMixerGroup;
        audioSource.PlayOneShot(clip, volume);
        Destroy(go, clip.length);
    }

}
