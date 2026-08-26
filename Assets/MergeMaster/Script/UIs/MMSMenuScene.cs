using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FSDK.Ads;

public class MMSMenuScene : DTNView
{
    /// <summary>
    /// Khanh
    /// </summary>
    public Animator Animator;
    public MMSGameController GameController;
    public MMSUserBoard UserBoard;
    public MMSDragAndDrop mmsDragAndDrop;
    public AudioListener AudioListener;
    public MMSDailyRewardsTime DailyRewardsTime;
    public Text LevelText;
    public Text CoinText;

    public Button startBtn;
    public Button homeBtn;

    [Header("Buy Warriors")]
    public Button buyWarriorBtn;
    public long coinToBuyWarrior = 5;
    public int buyWarriorBtnCountClick = 0;
    [Header("Buy Archeries")]
    public Button buyArcheryBtn;
    public long coinToBuyArchery = 5;
    public int buyArcheryBtnCountClick = 0;

    public Button SettingButton;
    public Button CardCollectionButton;
    public Button DailyRewardButton;
    public override void InitView()
    {

    }

    public override void Show()
    {
        base.Show();
        Animator.Play("MenuSceneAppear");
        SetUpButtons();
        // Khoa:  online scene ko có LevelText nên phải kiểm tra 
        if (LevelText != null)
            UpdateLevelText();
        //
        SetUpBuyCoin();
        SetUpBuyButton();
        UpdateCoinText();
        ShowLevelState();
        DailyRewardsTime.OfflineReward();
        mmsDragAndDrop.gameObject.SetActive(true);
    }

    public virtual void ShowLevelState()
    {
        DTNViewManagement.Show<MMSLevelState>();
    }

    public void SetUpBuyButton()
    {
        CheckBuyArcheryButton();
        CheckBuyWarriorButton();
    }

    protected void CheckBuyArcheryButton()
    {
        if (CheckCardCount() >= 15)
        {
            buyArcheryBtn.GetComponent<MMSBuyButton>().FullCard(coinToBuyArchery);
            return;
        }

        if (UserBoard.Coin >= coinToBuyArchery)
        {
            buyArcheryBtn.GetComponent<MMSBuyButton>().DefaultUI(coinToBuyArchery);
        }
        else
        {
            buyArcheryBtn.GetComponent<MMSBuyButton>().AdsUI();
        }
    }

    public virtual void UpdateCoinText()
    {
        CoinText.text = DTNNumber.FomatCoin(UserBoard.Coin);
    }

    protected void CheckBuyWarriorButton()
    {
        if (CheckCardCount() >= 15)
        {
            buyWarriorBtn.GetComponent<MMSBuyButton>().FullCard(coinToBuyWarrior);
            return;
        }

        if (UserBoard.Coin >= coinToBuyWarrior)
        {
            buyWarriorBtn.GetComponent<MMSBuyButton>().DefaultUI(coinToBuyWarrior);
        }
        else
        {
            buyWarriorBtn.GetComponent<MMSBuyButton>().AdsUI();
        }
    }
    protected void SetUpBuyCoin()
    {
        coinToBuyArchery = LevelManagement.Instance.LoadCoinOfBuyArchery(coinToBuyArchery);
        coinToBuyWarrior = LevelManagement.Instance.LoadCoinOfBuyWarrior(coinToBuyWarrior);
    }

    public virtual void UpdateLevelText()
    {
        int LevelInfos = GameController.currentLevel + 1;
        LevelText.text = DTNLocalizationSystem.GetText("LEVEL ") + LevelInfos.ToString();
    }

    public void StartBattle()
    {
        DTNSoundManagement.instance.ChangeSoundBackground(2);
        mmsDragAndDrop.gameObject.SetActive(false);
        GameController.StartGame();
        homeBtn.gameObject.SetActive(false);
        Animator.Play("MenuSceneDisappear");
    }

    public void AddCard(string cardName)
    {
        UserBoard.AddCard(cardName);
    }

    protected void StartBtnOnclick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        StartBattle();
    }

    protected int CheckCardCount()
    {
        return UserBoard.CheckCartCount();
    }

    protected void BuyArcheryBtnOnclick()
    {

        if (CheckCardCount() >= 15)
        {
            SetUpBuyButton();
            return;
        }


        if (UserBoard.Coin >= coinToBuyArchery)
        {
            BuyArcheryBtnOnclickCoin();
        }
        else
        {
            BuyArcheryBtnOnclickAds();
        }
        Debug.Log("Archery");
    }

    protected void BuyWarriorBtnOnclick()
    {

        if (CheckCardCount() >= 15)
        {
            SetUpBuyButton();
            return;
        }

        if (UserBoard.Coin >= coinToBuyWarrior)
        {
            BuyWarriorBtnOnclickCoin();
        }
        else
        {
            BuyWarriorBtnOnclickAds();
        }
    }


    protected void BuyWarriorBtnOnclickCoin()
    {
        DTNSoundManagement.instance.Play("cashButtonSound");
        UserBoard.Coin -= coinToBuyWarrior;
        buyWarriorBtnCountClick++;
        PlayerPrefs.SetInt("buyWarriorBtnCountClick", buyWarriorBtnCountClick);


        AddCard("Warrior_1");

        LevelManagement.Instance.SaveUserCoin();

        UpdateCoinText();

        coinToBuyWarrior = UpgradeCoin(coinToBuyWarrior, buyWarriorBtnCountClick);

        LevelManagement.Instance.SaveCoinOfBuyWarrior(coinToBuyWarrior);
        mmsDragAndDrop.SaveUserPos();
        SetUpBuyButton();
    }

    protected void BuyArcheryBtnOnclickCoin()
    {
        DTNSoundManagement.instance.Play("cashButtonSound");

        UserBoard.Coin -= coinToBuyArchery;
        buyArcheryBtnCountClick++;
        PlayerPrefs.SetInt("buyArcheryBtnCountClick", buyArcheryBtnCountClick);
        AddCard("Archery_1");

        LevelManagement.Instance.SaveUserCoin();

        UpdateCoinText();

        coinToBuyArchery = UpgradeCoin(coinToBuyArchery, buyArcheryBtnCountClick);

        LevelManagement.Instance.SaveCoinOfBuyArchery(coinToBuyArchery);
        mmsDragAndDrop.SaveUserPos();
        SetUpBuyButton();
    }

    protected static long UpgradeCoin(long coin, int x)
    {
        coin = (long)(coin * (1f / (0.25f * (x + 1f)) + 1f));
        // Debug.Log("Coin: " + coin);
        return coin;
    }


    public static long BonusCoinChess()
    {
        long _coinToBuyArchery = LevelManagement.Instance.LoadCoinOfBuyArchery(5);

        long _coinToBuyWarrior = LevelManagement.Instance.LoadCoinOfBuyWarrior(5);
        

        long money = Math.Min(_coinToBuyArchery, _coinToBuyWarrior);
        
        return money;

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

    protected void BuyArcheryBtnOnclickAds()
    {
        DTNSoundManagement.instance.Play("buttonSound");
      
            AdsManager.Instance.ShowRewardedAd(() =>
            {
                DTNSoundManagement.instance.Play("buttonSound");
                AddCard("Archery_1");
                AddCard("Archery_1");
                mmsDragAndDrop.SaveUserPos();
                SetUpBuyButton();
            }, () => {
                DTNViewManagement.GetView<MMSNoAdsNotification>().Show();
            }, 0, FSDK.LevelDifficulty.Hard, "");
       
    }
    protected void BuyWarriorBtnOnclickAds()
    {
        DTNSoundManagement.instance.Play("buttonSound");

       
            AdsManager.Instance.ShowRewardedAd(() =>
            {
                DTNSoundManagement.instance.Play("buttonSound");
                AddCard("Warrior_1");
                AddCard("Warrior_1");
                mmsDragAndDrop.SaveUserPos();
                SetUpBuyButton();
            }, () => {
                DTNViewManagement.GetView<MMSNoAdsNotification>().Show();
             }, 0, FSDK.LevelDifficulty.Hard, "");
    }
    public Button ShareButton;
    public void ShareButtonOnClick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        ShareOnSocialMedia.Instance.Share();
    }

    public void SetAudio()
    {
        AudioListener.enabled = !AudioListener.enabled;
    }

    public void OnSettingScene()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        DTNViewManagement.GetView<MMSSettingScene>().Show();
    }

    public void OnCardCollectionScene()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        DTNViewManagement.GetView<MMSCardCollectionScene>().Show();
    }

    protected void SetUpButtons()
    {
        startBtn.onClick.RemoveAllListeners();
        SettingButton.onClick.RemoveAllListeners();
        CardCollectionButton.onClick.RemoveAllListeners();
        buyWarriorBtn.onClick.RemoveAllListeners();
        buyArcheryBtn.onClick.RemoveAllListeners();

        if (PlayerPrefs.HasKey("buyArcheryBtnCountClick"))
            buyArcheryBtnCountClick = PlayerPrefs.GetInt("buyArcheryBtnCountClick");
        else
            buyArcheryBtnCountClick = 0;

        if (PlayerPrefs.HasKey("buyWarriorBtnCountClick"))
            buyWarriorBtnCountClick = PlayerPrefs.GetInt("buyWarriorBtnCountClick");
        else
            buyWarriorBtnCountClick = 0;

        startBtn.onClick.AddListener(() =>
        {
            StartBattle();
        });

        SettingButton.onClick.AddListener(() =>
        {
            OnSettingScene();
        });

        CardCollectionButton.onClick.AddListener(() =>
        {
            OnCardCollectionScene();
        });

        buyArcheryBtn.onClick.AddListener(() =>
        {
            BuyArcheryBtnOnclick();
        }
        );

        buyWarriorBtn.onClick.AddListener(() =>
        {
            BuyWarriorBtnOnclick();
        });


        SetUpPosOfDailyButton();
        SetUpHomeButton();


        ShareButton.onClick.RemoveAllListeners();

        ShareButton.onClick.AddListener(() =>
        {
            ShareButtonOnClick();
        });
    }

    private void SetUpHomeButton()
    {
        if (OptionsChooseGame.Instance.gameType == OptionsChooseGame.GameType.Offline)
        {
            homeBtn.gameObject.SetActive(false);
        }
        else if (OptionsChooseGame.Instance.gameType == OptionsChooseGame.GameType.Normal)
        {
            homeBtn.gameObject.SetActive(true);
            homeBtn.onClick.RemoveAllListeners();
            homeBtn.onClick.AddListener(() =>
            {
                HomeBtnOnclick();
            });
        }
    }
    private void SetUpPosOfDailyButton()
    {
        RectTransform rectDailyButton = DailyRewardButton.GetComponent<RectTransform>();
        RectTransform rectHomeButton = homeBtn.GetComponent<RectTransform>();
        if (OptionsChooseGame.Instance.gameType == OptionsChooseGame.GameType.Offline)
        {
            rectDailyButton.anchorMin = rectHomeButton.anchorMin;
            rectDailyButton.anchorMax = rectHomeButton.anchorMax;
            rectDailyButton.anchoredPosition = rectHomeButton.anchoredPosition;
        }
    }

    public void ShowHome()
    {
        DTNViewManagement.GetView<MMSHomeScene>().Show();

        GameController.BackToHome();

        Animator.Play("MenuSceneDisappear");
    }
    private void HomeBtnOnclick()
    {
        ShowHome();
    }
    public void SwitchCameraView()
    {
        MMSCameraControl.Instance.SwithcView();
    }

}
