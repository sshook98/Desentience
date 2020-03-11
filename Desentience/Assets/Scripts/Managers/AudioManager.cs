using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private AudioClip[] playerFiringSounds;
    [SerializeField]
    private AudioSource audioSource;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    public void PlayPlayerFiringSound()
    {
        audioSource.PlayOneShot(playerFiringSounds[Random.Range(0, playerFiringSounds.Length)]);
    }
}
