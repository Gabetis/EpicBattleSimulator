namespace FSDK.Ads
{
    using System;

    public class AdsServices
    {
        public static Action OnInterstitialAdOpenEvent;
        public static Action OnInterstitialAdCloseEvent;
        public static Action OnInterstitialAdReadyEvent;
        
        public static Action       OnRewardedAdOpenEvent;
        public static Action       OnRewardedAdCloseEvent;
        public static Action       OnRewardedAdReadyEvent;
        public static Action       OnRewardedAdLoadFailedEvent;
        public static Action       OnRewardedAdRewardedEvent;
        public static Action<bool> OnRewardedAdAvailabilityChangedEvent;

        
        /// Sample AdWhere used by Analytics
        public const string AdWhereHome        = "home";
        public const string AdWhereMenu        = "menu";
        public const string AdWhereShop        = "shop";
        public const string AdWhereGacha       = "gacha";
        public const string AdWhereGift        = "gift";
        public const string AdWhereDailyReward = "daily_reward";
        public const string AdWhereLuckyWheel  = "lucky_wheel";
        public const string AdWhereInGame      = "in_game";
        public const string AdWhereEndGame     = "end_game";
        public const string AdWhereRevive      = "revive";
    }
}