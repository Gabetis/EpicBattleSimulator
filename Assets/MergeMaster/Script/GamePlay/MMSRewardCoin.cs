using FSDK.Ads;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMSRewardCoin : DTNSingletonMB<MMSRewardCoin>
{
    bool isTouch = false;

    private void OnEnable()
    {
        isTouch = false;
    }

    private void OnDisable()
    {
        isTouch = false;
    }

    private void OnMouseDown()
    {
        if (!isTouch)
        {
            isTouch = true;
            DTNViewManagement.GetView<MMSEarnCoinScene>().Show();
            this.gameObject.SetActive(false);

            AdsManager.Instance.ShowInterstitialAd(() =>
            {
            }, () => { }, 0, FSDK.LevelDifficulty.Normal, "");
        }
    }
}
