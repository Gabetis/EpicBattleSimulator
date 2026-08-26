using FSDK.Ads;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class MMSEarnCoinScene : DTNView
{
    public MMSUserBoard UserBoard;
    public MMSOfflineEarningScene OfflineEarningScene;
    public Animator animator;
    public Text EarnCoinText;
    public Button CollectButton;
    public Button BackButton;
    public MMSDragAndDrop dragAndDrop;
    int EarnCoin;

    public override void InitView()
    {

    }
    public override void Show()
    {
        base.Show();
        animator.Play("Show");
        dragAndDrop.OnDrop();
        dragAndDrop.gameObject.SetActive(false);
        SetUpButtons();
        SetEarnCoin();
    }
    public void SetEarnCoin()
    {
        EarnCoin = (int)(MustEarnCoin(4));
        EarnCoinText.text = "" + DTNNumber.FomatCoin(EarnCoin);
    }

    public static long MustEarnCoin(int step)
    {
        long _coinToBuyArchery = LevelManagement.Instance.LoadCoinOfBuyArchery(5);
        int _countArchery = 0;

        long _coinToBuyWarrior = LevelManagement.Instance.LoadCoinOfBuyWarrior(5);
        int _countWarrior = 0;


        if (PlayerPrefs.HasKey("buyArcheryBtnCountClick"))
            _countArchery = PlayerPrefs.GetInt("buyArcheryBtnCountClick");

        if (PlayerPrefs.HasKey("buyWarriorBtnCountClick"))
            _countWarrior = PlayerPrefs.GetInt("buyWarriorBtnCountClick");


        int count = Math.Min(_countArchery, _countWarrior);
        long finalMoney = 0;

        long money = Math.Min(_coinToBuyArchery, _coinToBuyWarrior);
        {
            for (int i = 1; i <= (int)(step / 2f); i++)
            {
                money = UpgradeCoin(money, count + i);
                finalMoney += money;
            }
        }

        money = Math.Min(_coinToBuyArchery, _coinToBuyWarrior);
        {
            for (int i = 1; i <= (int)(step / 2f); i++)
            {
                money = UpgradeCoin(money, count + i);
                finalMoney += money;
            }
        }


        return finalMoney;

        //int nextStep = Mathf.RoundToInt(step*0.3f);
        //for (int i =  1; i<= nextStep; i++)
        //{
        //    _coinToBuyArchery = UpgradeCoin(_coinToBuyArchery, _countArchery + i);
        //}

        //nextStep = Mathf.RoundToInt(step * 0.7f);
        //for (int i = 1; i <= nextStep; i++)
        //{
        //    _coinToBuyWarrior = UpgradeCoin(_coinToBuyWarrior, _countWarrior + i);
        //}
        //return Math.Max(2, _coinToBuyArchery + _coinToBuyWarrior);
    }

    protected static long UpgradeCoin(long coin, int x)
    {
        coin = (long)(coin * (1f / (0.25f * (x + 1f)) + 1f));
        // Debug.Log("Coin: " + coin);
        return coin;
    }


    public void CollectButtonOnclick()
    {

       
            AdsManager.Instance.ShowRewardedAd(() =>
            {
                DTNSoundManagement.instance.Play("cashButtonSound");
                OfflineEarningScene.AddUserCoin(EarnCoin);
                animator.Play("Hide");
            }, () =>
            {
DTNViewManagement.GetView<MMSNoAdsNotification>().Show();
                DTNSoundManagement.instance.Play("cashButtonSound");
                Time.timeScale = 1f;
                animator.Play("Hide");
            }, 0, FSDK.LevelDifficulty.Hard, "");
       
    }

    public override void Hide()
    {
        dragAndDrop.gameObject.SetActive(true);
        base.Hide();
    }

    public void Back()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        animator.Play("Hide");
    }

    private void SetUpButtons()
    {
        BackButton.onClick.RemoveAllListeners();
        BackButton.onClick.AddListener(() =>
        {
            Back();
        });

        CollectButton.onClick.RemoveAllListeners();
        CollectButton.onClick.AddListener(() =>
        {
            CollectButtonOnclick();
        });
    }
}
