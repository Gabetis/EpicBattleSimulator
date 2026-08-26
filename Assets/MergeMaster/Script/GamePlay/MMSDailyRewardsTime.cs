using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMSDailyRewardsTime : MonoBehaviour
{
    public static MMSDailyRewardsTime Instance;
    [SerializeField] MMSDailyRewardsDatabase dailyRewardsDatabase;
    public LevelManagement LevelManagement;
    [Header("Time")]
    // wait .. seconds to activate the next reward
    [SerializeField] double nextRewardDelay = 20f;
    // check every ... second one time
    [SerializeField] float checkNextRewardDelay = 5f;
    public bool is_reward_claimed;
    public bool IsRewardedReady;
    public int next_reward_index;
    public DailyRewardButton[] dailyRewardButtons;
    public List<ItemDailyRewardUI> listItemDailyRewardUI;

    // private
    bool isStopChecking;
    double ellapsedSeconds;
    DateTime currentDateTime;
    DateTime rewardClampDateTime;
    DateTime offlineClampDateTime;

    public float timeDelay;
    public float hours;
    public float minutes;
    public float seconds;
    private void Awake()
    {
        Instance = this;

    }
    private void Start()
    {
        if (!PlayerPrefs.HasKey("IsDailyRewardedReady"))
        {
            PlayerPrefs.SetFloat("IsDailyRewardedReady", 1);
            IsRewardedReady = true;
        }
        else
            IsRewardedReady = (PlayerPrefs.GetFloat("IsDailyRewardedReady", 0) == 0) ? false : true;
        is_reward_claimed = (PlayerPrefs.GetFloat("IsDailyRewardedClamp", 0) == 0) ? false : true;
        if (PlayerPrefs.HasKey("Next_Reward_Index"))
        {
            next_reward_index = PlayerPrefs.GetInt("Next_Reward_Index");
        }
        else
        {
            next_reward_index = 0;
            PlayerPrefs.SetInt("Next_Reward_Index", next_reward_index);
        }
        StopAllCoroutines();
        StartCoroutine(CheckForRewards());
    }

    public void OfflineReward()
    {
        currentDateTime = DateTime.Now;
        offlineClampDateTime = DateTime.Parse(PlayerPrefs.GetString("Offline_Clamp_DateTime", currentDateTime.ToString()));

        long ellapSecond = (long)(currentDateTime - offlineClampDateTime).TotalSeconds;
        long ratio = ((ellapSecond * 2) / (long)nextRewardDelay);
        if (ratio >= 0.3)
        {
            ratio = (long)Mathf.Clamp((float)ratio, 0.3f, 2.5f);
            long earncoin = ratio * MustEarnCoin(3);
            PlayerPrefs.SetString("Offline_Clamp_DateTime", currentDateTime.ToString());
            DTNViewManagement.GetView<MMSOfflineEarningScene>().SetEarnCoin(earncoin);
            DTNViewManagement.GetView<MMSOfflineEarningScene>().Show();
        }
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

    public IEnumerator CheckForRewards()
    {
        while (true)
        {
            if (!IsRewardedReady)
            {
                currentDateTime = DateTime.Now;
                rewardClampDateTime = DateTime.Parse(PlayerPrefs.GetString("Reward_Clamp_DateTime", currentDateTime.ToString()));

                // get total seconds between two dates
                ellapsedSeconds = (currentDateTime - rewardClampDateTime).TotalSeconds;
                if (ellapsedSeconds > nextRewardDelay)
                {
                    IsRewardedReady = true;
                    PlayerPrefs.SetFloat("IsDailyRewardedReady", 1);
                    // save next reward index
                    next_reward_index++;
                    if (next_reward_index >= dailyRewardsDatabase.DatasRewardCount)
                    {
                        next_reward_index = 0;
                        dailyRewardsDatabase.ChangeDatabase();
                        // Renew json
                    }

                    PlayerPrefs.SetString("Reward_Clamp_DateTime", currentDateTime.ToString());
                    PlayerPrefs.SetInt("Next_Reward_Index", next_reward_index);
                    is_reward_claimed = false;
                    PlayerPrefs.SetFloat("IsDailyRewardedClamp", 0);
                    DTNViewManagement.GetView<DailyRewardScene>().SetUpClampButton();
                    foreach (var item in dailyRewardButtons)
                    {
                        if (item != null)
                            item.ShowNoti(true);
                    }
                    isStopChecking = true;
                    foreach (var item in listItemDailyRewardUI)
                    {
                        if (item != null)
                            item.SetUpCountTime(false);
                    }
                }
                else
                {
                    if (DTNViewManagement.GetView<DailyRewardScene>().isActiveAndEnabled)
                    {
                        ActiveCaculateTimeToHourMinus();
                    }
                    if (!isStopChecking)
                    {
                        IsRewardedReady = false;
                        PlayerPrefs.SetFloat("IsDailyRewardedReady", 0);
                        foreach (var item in dailyRewardButtons)
                        {
                            if (item != null)
                                item.ShowNoti(false);
                        }
                        isStopChecking = true;
                    }

                }
            }
            yield return new WaitForSeconds(checkNextRewardDelay);
        }
    }
    public void ActiveCaculateTimeToHourMinus()
    {
        currentDateTime = DateTime.Now;
        CaculateTimeToHourMinus();
    }
    private void CaculateTimeToHourMinus()
    {
        timeDelay = (float)(nextRewardDelay - ellapsedSeconds);
        hours = Mathf.Floor(timeDelay / 3600);
        float temp = timeDelay - hours * 3600;
        minutes = Mathf.Floor(temp / 60);
        seconds = Mathf.Floor(temp - minutes * 60);
    }
}
