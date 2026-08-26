using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DailyRewards
{
    [SerializeField]
    public RewardType rewardType;
    [SerializeField]
    public string rewardName;
    [SerializeField]
    public long Amount;
}

public enum RewardType
{
    Coin = 0,
    Card = 1,
}