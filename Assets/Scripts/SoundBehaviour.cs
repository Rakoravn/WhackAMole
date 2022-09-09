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

    private void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySuccessSound() {
        audioSource.PlayOneShot(successSound);
    }

    public void PlayErrorSound() {
        audioSource.PlayOneShot(errorSound);
    }

    public void PlayPushOutSound() {
        audioSource.PlayOneShot(pushOutSound);
    }

    public void PlayPushInSound() {
        audioSource.PlayOneShot(pushInSound);
    }
}
