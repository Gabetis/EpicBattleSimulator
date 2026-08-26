using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlaySword : MonoBehaviour
{
    public MMSWarrior Warrior;
    public AudioSource audioSource;
    public AudioClip[] audioClip;

    private void OnEnable()
    {
        audioSource.volume = PlayerPrefs.GetFloat("SoundVolume") * 0.7f;
    }

    public void PlaySound()
    {
        if (Warrior.HitCallBack())
        {
            audioSource.PlayOneShot( audioClip[Random.Range(0, audioClip.Length-1) ]);
        }
    }
}
