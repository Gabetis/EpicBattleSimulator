using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DTNSoundManagement : MonoBehaviour
{
    public DTNSound[] sounds;
    public static DTNSoundManagement instance;
    AudioSource backgroundSource;
    AudioSource backgroundMusicFightSource;
    // Start is called before the first frame update
    public virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
        foreach (DTNSound item in sounds)
        {

            item.source = gameObject.AddComponent<AudioSource>();
            item.source.clip = item.clip;
            if (item.name == "backgroundMusicFight")
            {
                backgroundMusicFightSource = item.source;
            }
            if (item.name == "backgroundMusic")
            {
                backgroundSource = item.source;
            }
            item.source.volume = item.volume;
            item.source.loop = item.loop;
            // item.source.awake = item.playAwake;
        }
    }

    public virtual void Start()
    {
        // foreach (DTNSound item in sounds)
        // {

        //     if (item.loop == true) item.source.Play();
        // }
        backgroundSource.volume = PlayerPrefs.GetFloat("SoundVolume") / 5;
        Play("backgroundMusic");

    }

    // Update is called once per frame
    public void Play(string name)
    {
        DTNSound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            // Debug.LogWarning("Sound: " + name + " is not found");
            return;
        }
        sound.source.Play();
        //Debug.Log("Play sound: " + name);
    }

    public void SetVolume()
    {
        foreach (DTNSound item in sounds)
        {
            item.source.volume = PlayerPrefs.GetFloat("SoundVolume");
        }
        backgroundMusicFightSource.volume = PlayerPrefs.GetFloat("SoundVolume") / 10;
        backgroundSource.volume = PlayerPrefs.GetFloat("SoundVolume") / 5;

    }
    public void ChangeSoundBackground(int index)
    {
        if (index == 2)
        {
            if (backgroundSource.isPlaying)
            {
                backgroundSource.Stop();
            }
            backgroundMusicFightSource.Play();
        }
        else if (index == 1)
        {
            backgroundMusicFightSource.Stop();
            backgroundSource.Play();
        }
    }
}
