using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DailyRewardData
{
    [SerializeField] public List<DailyRewards> DailyRewardsDatas;

    public DailyRewardData(List<DailyRewards> DailyRewardsDatas)
    {
        this.DailyRewardsDatas = DailyRewardsDatas;
    }
}


/*[SerializeField]
    public RewardType rewardType;
    [SerializeField]
    public string rewardName;
    [SerializeField]
    public long Amount;

    public enum RewardType
    {
        Coin,
        Card,
    }*/

[System.Serializable]
[CreateAssetMenuAttribute(fileName = "DailyRewardsDatabase", menuName = "Data/Scriptable/DailyRewards Database")]
public class MMSDailyRewardsDatabase : ScriptableObject
{
    public List<DailyRewards> DailyRewardsDatas;
    public MMSCardInfoSystem CardInfoSystem;
    public int DatasRewardCount
    {
        get { return DailyRewardsDatas.Count; }
    }
    public DailyRewards GetDailyRewardsData(int index)
    {
        DailyRewards returnDailyValue = new DailyRewards();
        returnDailyValue.rewardType = (RewardType)PlayerPrefs.GetInt("DailyReward" + "Type" + "Id" + index, 0);
        returnDailyValue.rewardName = PlayerPrefs.GetString("DailyReward" + "Name" + "Id" + index, "Coin");
        returnDailyValue.Amount = long.Parse(PlayerPrefs.GetString("DailyReward" + "Amount" + "Id" + index, "1"));
        return returnDailyValue;
    }

    public void SetFirstDailyRewardDatabase()
    {
        for(int i =0;i< DailyRewardsDatas.Count; i++)
        {
            SetPlayerPrefsDailyReward(i, (int)DailyRewardsDatas[i].rewardType, DailyRewardsDatas[i].rewardName, (int)DailyRewardsDatas[i].Amount);
        }
        PlayerPrefs.SetInt("DailyReward" + "Type" + "Boss", 2);
    }

    public void ChangeDatabase()
    {
        int strongestArchery = CardInfoSystem.GetStrongestArchery();
        int strongestWarrior = CardInfoSystem.GetStrongestWarrior();
        for (int i = 0; i < DailyRewardsDatas.Count; i++)
        {
            switch (i)
            {
                case 0:
                    SetPlayerPrefsDailyReward(i, (int)RewardType.Coin, "Coin", MMSMenuScene.MustEarnCoin(10) );
                    break;
                case 1:
                    SetPlayerPrefsDailyReward(i, (int)RewardType.Card, "Archery_" + Mathf.Clamp(strongestArchery+2, 1, 11), 1);
                    break;
                case 2:
                    SetPlayerPrefsDailyReward(i, (int)RewardType.Coin, "Coin", MMSMenuScene.MustEarnCoin(10) * 3);
                    break;
                case 3:
                    SetPlayerPrefsDailyReward(i, (int)RewardType.Card, "Warrior_" + Mathf.Clamp(strongestWarrior+2, 1, 13), 1);
                    break;
                case 4:
                    SetPlayerPrefsDailyReward(i, (int)RewardType.Card, "Archery_" + Mathf.Clamp(strongestArchery + 3,1,11), 1);
                    break;
                case 5:
                    SetPlayerPrefsDailyReward(i, (int)RewardType.Card, "Warrior_" + Mathf.Clamp(strongestWarrior+3, 1, 13), 1);
                    break;
                case 6:
                    PlayerPrefs.SetInt("DailyReward" + "Type" + "Boss", PlayerPrefs.GetInt("DailyReward" + "Type" + "Boss") + 1);
                    SetPlayerPrefsDailyReward(i, (int)RewardType.Card, "Boss_" + Mathf.Clamp(PlayerPrefs.GetInt("DailyReward" + "Type" + "Boss"), 1, 5), 1);
                    break;
            }
        }
        PlayerPrefs.SetInt("DailyReward" + "Amount" + "Id" + 2, PlayerPrefs.GetInt("DailyReward" + "Amount" + "Id" + 2) * 4);

        Debug.Log("ChangeDataBase");
    }

    void SetPlayerPrefsDailyReward(int id,int rewardType,string rewardName,long amount)
    {
        PlayerPrefs.SetInt("DailyReward" + "Type" + "Id" + id, rewardType);
        PlayerPrefs.SetString("DailyReward" + "Name" + "Id" + id, rewardName);
        PlayerPrefs.SetString("DailyReward" + "Amount" + "Id" + id, amount.ToString());
    }

    // Jsonnnnn

}
