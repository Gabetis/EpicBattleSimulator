namespace FSDK.Ads
{
    using System;
    using System.Threading.Tasks;
    using UnityEngine;

    public partial class AdsManager : PersistentSingleton<AdsManager>
    {
        private const string BannerAdType       = "banner";
        private const string RewardedAdType     = "rewarded_video";
        private const string InterstitialAdType = "interstitial";

        private IAdvertisement _ads;
        
        private Action OnReward;
        private Action OnFail;

        private bool _isFinishWatchAds;
        
        protected override void Awake()
        {
            this.Setup();
            base.Awake();
        }

        private void Setup()
        {
#if EXISTED_IRON_SOURCE
            this._ads = new IronSourceAds();
#elif EXISTED_MAX_SDK
            this._ads = new ApplovinMaxAds();
#else
            this._ads = new DummyAds();
#endif
        }

        private void Start()
        {
            this._ads.Init();
            
            if (this._ads.UseBanner)
            {
                this._ads.InitBanner();
            }
            this._ads.InitInterstitial();
            this._ads.InitRewardedVideo();
            
            this.InitEvents();
        }

        private void InitEvents()
        {
            AdsServices.OnInterstitialAdOpenEvent  = OnInterstitialAdOpen;
            AdsServices.OnInterstitialAdCloseEvent = OnInterstitialAdClosed;
            AdsServices.OnInterstitialAdReadyEvent = OnInterstitialAdReady;
            
            AdsServices.OnRewardedAdOpenEvent       = OnRewardedAdOpen;
            AdsServices.OnRewardedAdCloseEvent      = OnRewardedAdClosed;
            AdsServices.OnRewardedAdReadyEvent      = OnRewardedAdReady;
            AdsServices.OnRewardedAdLoadFailedEvent = OnRewardedAdLoadFailed;
            AdsServices.OnRewardedAdRewardedEvent   = OnRewardedAdRewarded;
        }

        #region Events
        private void OnInterstitialAdOpen()
        {
            LogAnalytic.LogInterstitialAdDisplayedEvent();
            LogAnalytic.LogAdOpenCustomEvent(InterstitialAdType);
        }
        
        private void OnInterstitialAdClosed()
        {
            this._isFinishWatchAds = true;
            LogAnalytic.LogAdCompleteCustomEvent(InterstitialAdType);
         
            this.CheckReward();
            this._ads.LoadInterstitial();
            Debug.LogWarning($"Watched Interstitial Ad and Load Interstitial Ad");
        }

     

        private void OnInterstitialAdReady()
        {
            LogAnalytic.LogInterstitialAdApiCalledEvent();
        }
        
        private void OnRewardedAdOpen()
        {
            LogAnalytic.LogRewardedAdDisplayedEvent();
            LogAnalytic.LogAdOpenCustomEvent(RewardedAdType);
        }
        
        private void OnRewardedAdClosed()
        {
            this.CheckReward();
        }
        
        private void OnRewardedAdReady()
        {
            LogAnalytic.LogRewardedAdApiCalledEvent();
        }
        
        private void OnRewardedAdLoadFailed()
        {
            this._ads.LoadRewardedVideo();
        }

        private void OnRewardedAdRewarded()
        {
            this._isFinishWatchAds = true;
            LogAnalytic.LogRewardedAdCompleteEvent();
            LogAnalytic.LogAdCompleteCustomEvent(RewardedAdType);
            Debug.LogWarning($"Watched Rewarded Ad and reload it");
          
        }
        #endregion

        private async void CheckReward()
        {
            await Task.Yield();
            
            if (this._isFinishWatchAds)
            {
                this.OnReward?.Invoke();
            }
            else
            {
                this.OnFail?.Invoke();
            }
        }

        public void ShowBanner()
        {
            if (this._ads.UseBanner)
            {
                this._ads.ShowBanner();
            }
        }

        public void HideBanner()
        {
            // if (this._ads.UseBanner)
            // {
            //     this._ads.HideBanner();
            // }
        }
        
        public bool IsInterstitialAdReady()
        {
            return this._ads.IsInterstitialReady();
        }

        public void ShowInterstitialAd(Action onReward, Action onFail, int level, LevelDifficulty difficulty = LevelDifficulty.Normal, string where = "")
        {
            LogAnalytic.LogInterstitialAdCallEvent();
            
            this._isFinishWatchAds = false;
            
            if (IsInterstitialAdReady())
            {
                this.OnReward = onReward;
                this.OnFail   = onFail;
                
                Debug.LogWarning("ShowInterstitialAd");
                this._ads.ShowInterstitial();
                LogAnalytic.LogAdShowMinorCustomEvent(InterstitialAdType, where);
                LogAnalytic.LogAdShowCustomEvent(InterstitialAdType, level, difficulty, where);
            }
            else
            {
                onFail?.Invoke();
            }
                this._ads.LoadInterstitial();
        }
        
        public bool IsRewardedAdReady()
        {
            return this._ads.IsRewardedVideoReady();
        }

        public void ShowRewardedAd(Action onReward, Action onFail, int level, LevelDifficulty difficulty = LevelDifficulty.Hard, string where = "")
        {
            LogAnalytic.LogRewardedAdCallEvent();
            
            if (!this.IsRewardedAdReady())
            {
                Debug.LogWarning($"RewardedAd is not ready, show InterstitialAd instead");
                this._ads.LoadRewardedVideo();
                ShowInterstitialAd(onReward, onFail, level, difficulty, where);
                return;
            }

            this._isFinishWatchAds = false;
            this.OnReward          = onReward;
            this.OnFail            = onFail;
            
            Debug.LogWarning("ShowRewardedAd");
            this._ads.ShowRewardedVideo();
            LogAnalytic.LogAdShowMinorCustomEvent(RewardedAdType, where);
            LogAnalytic.LogAdShowCustomEvent(RewardedAdType, level, difficulty, where);
#if UNITY_EDITOR
            //this.OnReward?.Invoke();
#endif
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            this._ads.OnPause(pauseStatus);
        }

        private void OnDestroy()
        {
            this._ads.OnDestroy();
            this._ads = null;
        }
    }
}