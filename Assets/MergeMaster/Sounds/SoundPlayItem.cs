using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayItem : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public bool isPlayInSomeWhere;
    private void Awake()
    {
        audioSource.volume = PlayerPrefs.GetFloat("SoundVolume")/5;
        audioSource.clip = audioClip;
        if (!isPlayInSomeWhere)
        {
            PlaySound();
        }
        else return;
    }
    private void PlaySound()
    {
        audioSource.Play();
    }
}
