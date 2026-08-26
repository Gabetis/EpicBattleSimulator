
using UnityEngine;

public static class MMSDailyRewardsData
{
    private static int _coin = 0;
    static MMSDailyRewardsData()
    {
        _coin = PlayerPrefs.GetInt("CoinDailyReward", 0);
    }
    public static int CoinDailyReward { get { return _coin; } set { _coin = value; PlayerPrefs.SetInt("CoinDailyReward", _coin); } }


}

