using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBehaviour : MonoBehaviour
{
    public static SoundBehaviour instance;

    private AudioSource audioSource;

    public AudioClip successSound;
    public AudioClip errorSound;
    public AudioClip pushOutSound;
    public AudioClip pushInSound;

    public AudioClip mainSong;

    private void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = mainSong;
        audioSource.volume = 0.8f;
        audioSource.Play();
    }

    public void PlaySuccessSound() {
        audioSource.loop = false;
        audioSource.clip = successSound;
        audioSource.volume = 1f;
        audioSource.Play();
    }

    public void PlayErrorSound() {
        audioSource.loop = false;
        audioSource.clip = errorSound;
        audioSource.volume = 1f;
        audioSource.Play();
    }

    public void PlayPushOutSound() {
        audioSource.loop = false;
        audioSource.clip = pushOutSound;
        audioSource.volume = 1f;
        audioSource.Play();
    }

    public void PlayPushInSound() {
        audioSource.loop = false;
        audioSource.clip = pushInSound;
        audioSource.volume = 1f;
        audioSource.Play();
    }
}
