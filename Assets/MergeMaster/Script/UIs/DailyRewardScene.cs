using System;
using System.Collections;
using System.Collections.Generic;
using FSDK.Ads;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardScene : DTNView
{

    [SerializeField] ItemDailyRewardUI itemDailyRewardUI;
    private List<ItemDailyRewardUI> listItemDailyRewardUI;
    [SerializeField] MMSDailyRewardsDatabase dailyRewardsDatabase;
    [SerializeField] Sprite coinSprite;
    [SerializeField] Transform container;
    [Header("For Card")]
    [SerializeField] MMSBoard UserBoard;
    [SerializeField] MMSDragAndDrop dragAndDrop;
    [SerializeField] MMSCardInfoSystem cardInfoSystem;

    private Sprite cardSprite;
    private string cardNickName;
    private int saveIndex = -1;


    [Header("Button")]
    [SerializeField] Button backButton;
    [SerializeField] public Button claimButton;
    public DailyRewardButton dailyRewardButton;
    [Header("Success")]
    [SerializeField] GameObject successPanel;
    [SerializeField] Text successAmountText;
    [SerializeField] Image successImage;
    [SerializeField] Button successCloseButton;
    [SerializeField] Button successClampX2Button;
    public bool isTest;

    // private void Start()
    // {
    //     SetUp();
    // }
    public override void InitView()
    {

    }
    public override void Show()
    {
        base.Show();
        SetUp();
        dragAndDrop.gameObject.SetActive(false);
    }
    public override void Hide()
    {
        base.Hide();
    }

    void SetUp()
    {
        if (isTest)
            PlayerPrefs.SetInt("Next_Reward_Index", 0);
        MMSDailyRewardsTime.Instance.next_reward_index = PlayerPrefs.GetInt("Next_Reward_Index", 0);
        if (PlayerPrefs.GetInt("Next_Reward_Index") == 0)
        {
            if (PlayerPrefs.GetFloat("IsDailyRewardedClamp") == 1)
            {
                saveIndex = 0;
                PlayerPrefs.SetInt("Next_Reward_Index_Save", 0);
            }
            else
            {
                saveIndex = -1;
                PlayerPrefs.SetInt("Next_Reward_Index_Save", -1);
            }
        }
        else
        {
            saveIndex = PlayerPrefs.GetInt("Next_Reward_Index_Save", -1);
        }
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("Reward_Clamp_DateTime")))
        {
            PlayerPrefs.SetString("Reward_Clamp_DateTime", System.DateTime.Now.ToString());
        }

        // StopAllCoroutines();
        // StartCoroutine(CheckForRewards());
        SetUpReward();
        SetUpButton();
    }
    void SetUpReward()
    {
        listItemDailyRewardUI = new List<ItemDailyRewardUI>();

        MMSDailyRewardsTime.Instance.listItemDailyRewardUI = new List<ItemDailyRewardUI>();

        for (int i = 0; i < dailyRewardsDatabase.DailyRewardsDatas.Count; i++)
        {
            bool isCoin = false;
            Sprite sprite;
            string name;
            GameObject reward = Instantiate(itemDailyRewardUI.gameObject, container);
            ItemDailyRewardUI item = reward.GetComponent<ItemDailyRewardUI>();
            listItemDailyRewardUI.Add(item);
            MMSDailyRewardsTime.Instance.listItemDailyRewardUI.Add(item);
            if (dailyRewardsDatabase.GetDailyRewardsData(i).rewardType == RewardType.Coin)
            {
                isCoin = true;
                sprite = coinSprite;
                name = "Coin";
            }
            else
            {
                LoadCard(dailyRewardsDatabase.GetDailyRewardsData(i).rewardName);
                name = cardNickName;
                sprite = cardSprite;
            }
            item.SetUpReward(i, isCoin, name, dailyRewardsDatabase.GetDailyRewardsData(i).Amount, sprite);
            item.SetUpCountTime(false);
            if (saveIndex >= 0)
            {
                if (i <= saveIndex)
                {
                    item.SetNoReWard();
                }
            }
            else
            {
                if (i < MMSDailyRewardsTime.Instance.next_reward_index)
                {
                    item.SetNoReWard();
                }
            }
        }

        for (int i = 0; i < listItemDailyRewardUI.Count; i++)
        {
            if (!MMSDailyRewardsTime.Instance.IsRewardedReady && saveIndex >= 0 && i == saveIndex && i + 1 < listItemDailyRewardUI.Count)
            {
                listItemDailyRewardUI[i + 1].SetUpCountTime(true);
            }
        }

    }

    void SetUpButton()
    {
        backButton.onClick.RemoveAllListeners();
        claimButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(BackButtonOnclick);
        if (MMSDailyRewardsTime.Instance.is_reward_claimed)
        {
            claimButton.gameObject.SetActive(false);
        }
        else
            claimButton.onClick.AddListener(ClaimButtonOnclick);
    }
    public void SetUpClampButton()
    {
        claimButton.gameObject.SetActive(true);
        claimButton.onClick.RemoveAllListeners();
        claimButton.onClick.AddListener(ClaimButtonOnclick);
    }
    void BackButtonOnclick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        foreach (var item in listItemDailyRewardUI)
        {
            Destroy(item.gameObject);
        }
        dragAndDrop.gameObject.SetActive(true);

#if UNITY_ANDROID
        //AndroidNotifications.Instance.SendDailyRewardNotification();
#endif

        StartCoroutine(HideDailyRewardScene());
        // gameObject.SetActive(false);
    }
    IEnumerator HideDailyRewardScene()
    {
        GetComponent<Animator>().Play("Hide");
        yield return new WaitForSeconds(0.45f);
        Hide();
    }
    private void TakeReward(int rewardIndex, DailyRewards dailyRewards)
    {

        // check reward type
        if (dailyRewards.rewardType == RewardType.Coin)
        {
            UserBoard.Coin += dailyRewards.Amount;
            if (DTNViewManagement.GetView<MMSMenuOfflineScene>().isActiveAndEnabled)
                DTNViewManagement.GetView<MMSMenuOfflineScene>().UpdateCoinText();
            else if (DTNViewManagement.GetView<MMSMenuOnlineScene>().isActiveAndEnabled)
                DTNViewManagement.GetView<MMSMenuOnlineScene>().UpdateCoinText();

            LevelManagement.Instance.SaveUserCoin();
            successAmountText.text = "+ " + DTNNumber.FomatCoin(dailyRewards.Amount);
            successImage.sprite = coinSprite;
        }
        else if (dailyRewards.rewardType == RewardType.Card)
        {
            for (int i = 0; i < dailyRewards.Amount; i++)
            {
                AddCard(dailyRewards.rewardName);
            }
            foreach (var item in cardInfoSystem.CardInfos)
            {
                if (item.Name == dailyRewards.rewardName)
                {
                    cardSprite = item.Icon;
                    cardNickName = item.NickName;
                    break;
                }
            }
            successAmountText.text = "X " + dailyRewards.Amount.ToString();
            successImage.sprite = cardSprite;
            MMSGameController.Instance.SaveDailyUserBoard();
        }
    }
    void ClaimButtonOnclick()
    {
        int rewardIndex = MMSDailyRewardsTime.Instance.next_reward_index;
        DailyRewards dailyRewards = dailyRewardsDatabase.GetDailyRewardsData(rewardIndex);
        if (MMSDailyRewardsTime.Instance.is_reward_claimed)
        {
            return;
        }

        if (dailyRewards.rewardType == RewardType.Coin)
            DTNSoundManagement.instance.Play("cashButtonSound");
        else if (dailyRewards.rewardType == RewardType.Card)
            DTNSoundManagement.instance.Play("buttonSound");

        TakeReward(rewardIndex, dailyRewards);
        // update UI item was claimed
        for (int i = 0; i < listItemDailyRewardUI.Count; i++)
        {
            listItemDailyRewardUI[i].SetUpCountTime(false);
            if (i <= rewardIndex)
            {
                listItemDailyRewardUI[i].SetNoReWard();
                saveIndex = i;

                PlayerPrefs.SetInt("Next_Reward_Index_Save", i);
            }
        }

        if (saveIndex >= 0 && saveIndex + 1 < listItemDailyRewardUI.Count)
        {

            listItemDailyRewardUI[saveIndex + 1].SetUpCountTime(true);
        }
        else
        {

            listItemDailyRewardUI[saveIndex].SetUpCountTime(false);
        }

        MMSDailyRewardsTime.Instance.is_reward_claimed = true;
        PlayerPrefs.SetFloat("IsDailyRewardedClamp", 1);
        MMSDailyRewardsTime.Instance.IsRewardedReady = false;
        PlayerPrefs.SetFloat("IsDailyRewardedReady", 0);
        // Save datetime of last clamp
        PlayerPrefs.SetString("Reward_Clamp_DateTime", System.DateTime.Now.ToString());
        dailyRewardButton.ShowNoti(false);
        claimButton.gameObject.SetActive(false);
        SetUpSuccess();
#if UNITY_ANDROID 
        //AndroidNotifications.Instance.SendDailyRewardNotification();
#endif
    }


    private void AddCard(string cardName)
    {
        UserBoard.AddCard(cardName);
    }
    private void LoadCard(string cardName)
    {

        foreach (var item in cardInfoSystem.CardInfos)
        {
            if (item.Name == cardName)
            {
                cardSprite = item.Icon;
                cardNickName = item.NickName;
                break;
            }
        }

    }
    void SetUpSuccess()
    {
        successPanel.SetActive(true);
        DTNSoundManagement.instance.Play("mergeSound");
        successCloseButton.onClick.RemoveAllListeners();
        successClampX2Button.onClick.RemoveAllListeners();
        successCloseButton.onClick.AddListener(() =>
        {
            DTNSoundManagement.instance.Play("buttonSound");
            successPanel.SetActive(false);
        });
        successClampX2Button.onClick.AddListener(() =>
        {
            SuccessClampX2ButtonOnclick();

        });
    }


    private void SuccessClampX2ButtonOnclick()
    {
        int rewardIndex = MMSDailyRewardsTime.Instance.next_reward_index;
        DailyRewards dailyRewards = dailyRewardsDatabase.GetDailyRewardsData(rewardIndex);

        if (dailyRewards.rewardType == RewardType.Coin)
            DTNSoundManagement.instance.Play("cashButtonSound");
        else if (dailyRewards.rewardType == RewardType.Card)
            DTNSoundManagement.instance.Play("buttonSound");

       
            AdsManager.Instance.ShowRewardedAd(() =>
            {
                TakeReward(rewardIndex, dailyRewards);
            }, () =>
            {
                DTNViewManagement.GetView<MMSNoAdsNotification>().Show();
            }, 0, FSDK.LevelDifficulty.Hard, "");
       


        successPanel.SetActive(false);
    }
}