using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiBlastRainbowSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public float time = 1;
    float timeTemp = 0;
    void OnEnable()
    {
        PlaySound();
        timeTemp = time;
    }

    private void Update()
    {
        if (timeTemp > 0)
        {
            timeTemp -= Time.deltaTime;
        }
        else
        {
            timeTemp = time;
            PlaySound();
        }
    }
    private void PlaySound()
    {
        audioSource.volume = PlayerPrefs.GetFloat("SoundVolume");
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
