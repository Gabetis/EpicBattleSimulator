using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDailyRewardUI : MonoBehaviour
{
    [Header("Reward")]
    [SerializeField] Text rewardName;
    [SerializeField] Text rewardCount;
    [SerializeField] Text dayCountText;
    [SerializeField] Image rewardImage;
    [SerializeField] GameObject NoReward;
    [SerializeField] GameObject CountTime;
    [SerializeField] Text CountTimeText;
    [SerializeField] Button button;


    private void Update()
    {
        if (CountTime.activeSelf)
        {
            SetCountTimeText(MMSDailyRewardsTime.Instance.hours, MMSDailyRewardsTime.Instance.minutes, MMSDailyRewardsTime.Instance.seconds);
        }
    }
    public void SetUpReward(int index, bool isCoin, string name, long count, Sprite sprite)
    {
        rewardImage.sprite = sprite;
        if (isCoin)
        {
            rewardName.text = DTNLocalizationSystem.GetText("Coins");
            rewardCount.text = "+ " + DTNNumber.FomatCoin(count);
        }
        else
        {
            rewardName.text = name;
            rewardCount.text = " x " + count.ToString();
        }
        dayCountText.text = (index + 1).ToString("00");
        button.onClick.AddListener(() =>
        {
            DTNViewManagement.GetView<DailyRewardScene>().claimButton.GetComponent<Animator>().SetTrigger("Forcus");
        });
    }
    public void SetNoReWard()
    {
        NoReward.SetActive(true);
    }
    public void SetReward()
    {
        NoReward.SetActive(false);
    }
    public void SetCountTimeText(float hours, float minutes, float seconds)
    {
        CountTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
    public void SetUpCountTime(bool isShow)
    {
        CountTime.SetActive(isShow);
    }

}
