using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FSDK.Ads;

public class MMSGameLoseScene : DTNView
{
    public Animator Animator;
    public MMSGameController GameController;
    public long EarnCoin;
    public long PlusCoin;
    public Button clampBtn;
    public Button skipBtn;
    public Button nextBtn;
    public MMSCoinBar CoinBar;
    public Text EarnCoinText;
    public Text PlusCoinText;
    public MMSUserBoard UserBoard;
    public GameObject coinEffect;
    bool isClamp = false;
    bool isTutorial;
    int skipBtnCountClick = 0;
    public override void InitView()
    {

    }
    public override void Show()
    {
        base.Show();
        isClamp = false;
        isTutorial = PlayerPrefs.GetInt("FirstPlay") == 2 ? false : true;
        skipBtnCountClick = 0;
        DTNSoundManagement.instance.ChangeSoundBackground(1);
        Animator.Play("GameStatusAppear");
        if (isTutorial)
        {
            SetUpTutorialLose();
        }
        else
        {
            this.GetComponent<ButtonSkipAppear>().enabled = true;
            this.GetComponent<ButtonSkipAppear>().SetUp();
            SetUpButtons();
            SetEarnCoin();
            SetUpCoinBar();
            if (DTNViewManagement.GetView<MMSChestScene>().gameObject.activeSelf)
            {
                DTNViewManagement.GetView<MMSChestScene>().Hide();
            }
        }
    }


    private void SetEarnCoin()
    {
        AddUserCoin(EarnCoin);
        EarnCoinText.text = "" + DTNNumber.FomatCoin(EarnCoin);
    }

    private void SetUpCoinBar()
    {
        CoinBar.gameObject.SetActive(true);
        CoinBar.BaseCoin = EarnCoin;
        CoinBar.OnSetPlusCoin = (long value) =>
        {
            OnSetPlusCoin(value);
        };
    }

    public void OnSetPlusCoin(long value)
    {
        PlusCoin = value;
        PlusCoinText.text = "" + DTNNumber.FomatCoin(value);
    }

    private void ClampBtnOnclick()
    {
        
            CoinBar.OnStop();
            AdsManager.Instance.ShowRewardedAd(() =>
            {
                StartCoroutine(ClampBtnOnclickEffect());
            }, () =>
            {
                CoinBar.OnStart();
                StartCoroutine(ShowNoAdsNotification());
                //  DTNViewManagement.GetView<MMSNoAdsNotification>().Show();
            }, 0, FSDK.LevelDifficulty.Hard, "");
       
    }
    private IEnumerator ShowNoAdsNotification()
    {
        DTNViewManagement.GetView<MMSNoAdsNotification>().Show();
        yield return new WaitForSeconds(1.5f);
        DTNViewManagement.GetView<MMSNoAdsNotification>().Hide();
    }
    IEnumerator ClampBtnOnclickEffect()
    {
        EarnCoinText.text = "" + DTNNumber.FomatCoin(PlusCoin);
        isClamp = true;
        DTNSoundManagement.instance.Play("cashButtonSound");
        RecievePlusCoin();
        skipBtn.onClick.RemoveAllListeners();
        coinEffect.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        coinEffect.SetActive(false);
        GameController.ShowMenuUI();
        GameController.LoadGame();
        Animator.Play("GameStatusDisappear");

    }

    void RecievePlusCoin()
    {
        AddUserCoin(PlusCoin);
    }

    public void AddUserCoin(long value)
    {
        UserBoard.Coin += value;
        LevelManagement.Instance.SaveUserCoin();
    }

    private void SkipBtnOnclick()
    {
        if (isClamp)
            return;

        GameController.ShowMenuUI();
        GameController.LoadGame();

        GameController.RateGameShow();
        skipBtnCountClick++;
        if (skipBtnCountClick == 2)
        {

            AdsManager.Instance.ShowInterstitialAd(() =>
            {
                skipBtnCountClick = 0;
            }, () =>
            {
                skipBtnCountClick = 0;
            }, 0, FSDK.LevelDifficulty.Normal, "");
        }
        Animator.Play("GameStatusDisappear");
    }
    private void NextBtnOnclick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        GameController.ShowMenuUI();
        GameController.LoadGame();
        Animator.Play("GameStatusDisappear");
    }
    private void SetUpTutorialLose()
    {
        nextBtn.onClick.RemoveAllListeners();
        nextBtn.onClick.AddListener(NextBtnOnclick);
        clampBtn.gameObject.SetActive(false);
        this.GetComponent<ButtonSkipAppear>().enabled = false;
        skipBtn.gameObject.SetActive(false);
        CoinBar.gameObject.SetActive(false);
        SetEarnCoin();
    }
    private void SetUpButtons()
    {
        clampBtn.gameObject.SetActive(true);
        clampBtn.onClick.RemoveAllListeners();
        skipBtn.onClick.RemoveAllListeners();
        clampBtn.onClick.AddListener(() =>
        {
            ClampBtnOnclick();
        });
        skipBtn.onClick.AddListener(() =>
        {
            SkipBtnOnclick();
        });
        nextBtn.gameObject.SetActive(false);
    }

}
