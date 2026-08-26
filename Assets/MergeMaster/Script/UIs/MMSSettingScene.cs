using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MMSSettingScene : DTNView
{
    public Animator Animator;
    public MMSMenuScene MenuScene;
    public MMSDragAndDrop dragAndDrop;
    public Button SoundButton;
    public Button VibrationButton;
    public Button PrivacyButton;
    public Button RestoreButton;
    public Button BackButton;

    public GameObject VibrationOff;
    private bool isSound;

    public override void InitView()
    {

    }

    public override void Show()
    {
        base.Show();
        dragAndDrop.OnDrop();
        Animator.Play("Appear");
        SetUpButtons();
        MenuScene.mmsDragAndDrop.gameObject.SetActive(false);
        isSound = true ? PlayerPrefs.GetFloat("SoundVolume") == 1 : PlayerPrefs.GetFloat("SoundVolume") == 0;
        SoundButton.GetComponent<SoundButton>().ShowStatus(isSound);
    }

    public void SetAudio()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        MenuScene.SetAudio();
    }

    void ChangeSoundStatus()
    {

        // SoundOff.SetActive(!SoundOff.activeInHierarchy);
        // if (SoundOff.activeInHierarchy)
        // {
        //     PlayerPrefs.SetFloat("SoundVolume", 0);
        // }
        // else
        // {
        //     PlayerPrefs.SetFloat("SoundVolume", 1);
        // }
        isSound = !isSound;
        if (isSound)
        {
            PlayerPrefs.SetFloat("SoundVolume", 1);
        }
        else
        {
            PlayerPrefs.SetFloat("SoundVolume", 0);
        }
        DTNSoundManagement.instance.SetVolume();
        SoundButton.GetComponent<SoundButton>().ShowStatus(isSound);

    }

    void VibrationStatus()
    {
        VibrationOff.SetActive(!VibrationOff.activeInHierarchy);
    }

    public void Back()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        Animator.Play("Disappear");
    }

    public override void Hide()
    {
        MenuScene.mmsDragAndDrop.gameObject.SetActive(true);
        base.Hide();
    }

    private void SetUpButtons()
    {
        SoundButton.onClick.RemoveAllListeners();
        VibrationButton.onClick.RemoveAllListeners();
        PrivacyButton.onClick.RemoveAllListeners();
        RestoreButton.onClick.RemoveAllListeners();
        BackButton.onClick.RemoveAllListeners();


        SoundButton.onClick.AddListener(() =>
        {
            // SetAudio();
            ChangeSoundStatus();
        });

        VibrationButton.onClick.AddListener(() =>
        {
            VibrationStatus();
        });

        PrivacyButton.onClick.AddListener(() =>
        {

        });

        RestoreButton.onClick.AddListener(() =>
        {

        });

        BackButton.onClick.AddListener(() =>
        {
            Back();
        });
    }
}
