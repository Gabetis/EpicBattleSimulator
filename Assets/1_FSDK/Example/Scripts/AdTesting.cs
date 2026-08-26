namespace FSDK.Example
{

    using FSDK;
    using FSDK.Ads;
    using UnityEngine;
    using UnityEngine.UI;

    public class AdTesting : MonoBehaviour
    {
        public Text text;

        public void ShowBanner()
        {
            this.text.text = "Show Banner";
            AdsManager.Instance.ShowBanner();
        }

        public void HideBanner()
        {
            this.text.text = "Hide Banner";
            AdsManager.Instance.HideBanner();
        }

        public void ShowInter()
        {
            this.text.text = "Begin Inter";
            AdsManager.Instance.ShowInterstitialAd(() =>
            {
                this.text.text = "Success Inter";
            }, () =>
            {
                this.text.text = "Failed Inter";
            }, 1, LevelDifficulty.Normal, AdsServices.AdWhereHome);
        }

        public void ShowReward()
        {
            this.text.text = "Begin Reward";
            AdsManager.Instance.ShowRewardedAd(() =>
            {
                this.text.text = "Success Reward";
            }, () =>
            {
                this.text.text = "Failed Reward";
            }, 10, LevelDifficulty.Hard, AdsServices.AdWhereHome);
        }
    }
}
