namespace FSDK.Ads
{
    using System;

    /// <summary>
    /// This is your custom AdsManager class. Use it when you want your own logic in ads flow.
    /// </summary>
    public partial class AdsManager
    {
        public void ShowBannerCustom()
        {
            //Your custom logic goes here. Ex: Check removed ad, if it is, do not show.
            this.ShowBanner();
        }
        
        public void ShowInterstitialAdCustom(Action onReward, Action onFail, int level, LevelDifficulty difficulty = LevelDifficulty.Normal, string where = "")
        {
            //Your custom logic goes here. Ex: Check removed ad, if it is, do not show.
            this.ShowInterstitialAd(onReward, onFail, level, difficulty, where);
        }
    }
}