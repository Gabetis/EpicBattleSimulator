using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMSReviewScene : DTNView
{
    public Animator animator;
    public MMSDragAndDrop dragAndDrop;
    public Button SkipButton;
    public string url;

    public Button Rate5StarsBtn;
    public Button Rate4StarsBtn;
    public Button Rate3StarsBtn;
    public Button Rate2StarsBtn;
    public Button Rate1StarsBtn;

    public GameObject[] StarRates;

    public override void InitView()
    {

    }

    public override void Show()
    {
        base.Show();
        dragAndDrop.gameObject.SetActive(false);
        animator.Play("Show");
        SetUpButtons();
    }

    public void Rate5StarsBtnOnclick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        SetStarsRate(5);
        PlayerPrefs.SetInt("RateGame",-1);
        OnClickOpenURL();
        StartCoroutine(EnumTurnOff());
    }

    public void Rate4StarsBtnOnclick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        SetStarsRate(4);
        PlayerPrefs.SetInt("RateGame", -1);
        OnClickOpenURL();
        StartCoroutine(EnumTurnOff());
    }

    public void Rate3StarsBtnOnclick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        SetStarsRate(3);
        StartCoroutine(EnumTurnOff());
    }

    public void Rate2StarsBtnOnclick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        SetStarsRate(2);
        StartCoroutine(EnumTurnOff());
    }

    public void Rate1StarsBtnOnclick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        SetStarsRate(1);
        StartCoroutine(EnumTurnOff());
    }

    void SetStarsRate(int stars)
    {
        for(int i = 0; i < stars; i++)
        {
            StarRates[i].SetActive(true);
        }
    }

    IEnumerator EnumTurnOff()
    {
        yield return new WaitForSeconds(1f);
        animator.Play("Hide2");
    }

    public void OnClickOpenURL()
    {
        Application.OpenURL(url);
    }

    public override void Hide()
    {
        dragAndDrop.gameObject.SetActive(true);
        base.Hide();
    }

    public void SkipOnclick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        animator.Play("Hide1");
    }

    private void SetUpButtons()
    {
        SkipButton.onClick.RemoveAllListeners();
        SkipButton.onClick.AddListener(() =>
        {
            SkipOnclick();
        });

        Rate1StarsBtn.onClick.RemoveAllListeners();
        Rate1StarsBtn.onClick.AddListener(() =>
        {
            Rate1StarsBtnOnclick();
        });

        Rate2StarsBtn.onClick.RemoveAllListeners();
        Rate2StarsBtn.onClick.AddListener(() =>
        {
            Rate2StarsBtnOnclick();
        });

        Rate3StarsBtn.onClick.RemoveAllListeners();
        Rate3StarsBtn.onClick.AddListener(() =>
        {
            Rate3StarsBtnOnclick();
        });

        Rate4StarsBtn.onClick.RemoveAllListeners();
        Rate4StarsBtn.onClick.AddListener(() =>
        {
            Rate4StarsBtnOnclick();
        });

        Rate5StarsBtn.onClick.RemoveAllListeners();
        Rate5StarsBtn.onClick.AddListener(() =>
        {
            Rate5StarsBtnOnclick();
        });
    }
}
