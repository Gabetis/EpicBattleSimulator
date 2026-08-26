namespace FSDK.Ads
{
    using UnityEngine;

    public class IronSourceAds : IAdvertisement
    {
        private IConfigureData _configureData;
        public IConfigureData ConfigureData
        {
            get
            {
                if (this._configureData == null)
                {
                    this._configureData = new IronSourceConfigureData();
                }

                return this._configureData;
            }
        }

        private bool _useBanner;
        public  bool UseBanner => this._useBanner;
        
        private BannerPosition _position;

        public void Init()
        {
            this._useBanner = (bool) ConfigureData.GetPropertyValue("UseBanner");
            this._position  = (BannerPosition) ConfigureData.GetPropertyValue("BannerPosition");
            
#if EXISTED_IRON_SOURCE
#if UNITY_ANDROID
            IronSource.Agent.init(ConfigureData.GetPropertyValue("AndroidKey") as string);
#elif UNITY_IOS
            IronSource.Agent.init(ConfigureData.GetPropertyValue("IOSKey") as string);
#endif
            
            IronSource.Agent.validateIntegration();
            IronSource.Agent.shouldTrackNetworkState(true);
#endif
        }

        #region Banner
        public void InitBanner()
        {
#if EXISTED_IRON_SOURCE
            switch (_position)
            {
                case BannerPosition.Top:
                    IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.TOP);
                    break;
                case BannerPosition.Bottom:
                    IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
                    break;
            }

            IronSourceEvents.onBannerAdLoadFailedEvent += this.OnLoadBannerFailed;
#endif
        }
        
#if EXISTED_IRON_SOURCE
        private void OnLoadBannerFailed(IronSourceError obj) 
        {
            switch (this._position)
            {
                case BannerPosition.Top:
                    IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.TOP);
                    break;
                case BannerPosition.Bottom:
                    IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
                    break;
            }
        }
#endif
        
        public void ShowBanner()
        {
#if EXISTED_IRON_SOURCE
            IronSource.Agent.displayBanner();
#endif
        }
        
        public void HideBanner()
        {
#if EXISTED_IRON_SOURCE
            IronSource.Agent.hideBanner();
#endif
        }
        #endregion

        
        #region Interstitial
        public void InitInterstitial()
        {
#if EXISTED_IRON_SOURCE
            IronSourceEvents.onInterstitialAdOpenedEvent     += OnInterstitialOpenEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent += OnInterstitialAdLoadFailedEvent;
            IronSourceEvents.onInterstitialAdShowFailedEvent += OnInterstitialAdShowFailedEvent;
            IronSourceEvents.onInterstitialAdClosedEvent     += OnInterstitialClosedEvent;
            IronSourceEvents.onInterstitialAdReadyEvent      += OnInterstitialAdReadyEvent;
            
            IronSource.Agent.loadInterstitial();
#endif
        }
        
        private void OnInterstitialOpenEvent()
        {
            Debug.Log("[IronSource] OnInterstitialAdOpenEvent");
            AdsServices.OnInterstitialAdOpenEvent?.Invoke();
        }
        
#if EXISTED_IRON_SOURCE
        private void OnInterstitialAdLoadFailedEvent(IronSourceError error)
        {
            Debug.Log("[IronSource] OnInterstitialAdLoadFailedEvent");
            this.LoadInterstitial();
        }
        
        private void OnInterstitialAdShowFailedEvent(IronSourceError error)
        {
            Debug.Log("[IronSource] OnInterstitialAdShowFailedEvent");
            this.LoadInterstitial();
        }
#endif
        
        private void OnInterstitialClosedEvent()
        {
            Debug.Log("[IronSource] OnInterstitialAdCloseEvent");
            this.LoadInterstitial();
            
            AdsServices.OnInterstitialAdCloseEvent?.Invoke();
        }
        
        private void OnInterstitialAdReadyEvent()
        {
            Debug.Log("[IronSource] OnInterstitialAdReadyEvent");
            AdsServices.OnInterstitialAdReadyEvent?.Invoke();
        }

        public bool IsInterstitialReady()
        {
#if EXISTED_IRON_SOURCE
            return IronSource.Agent.isInterstitialReady();
#else       
            return true;
#endif
        }
        
        public void LoadInterstitial()
        {
#if EXISTED_IRON_SOURCE
            IronSource.Agent.loadInterstitial();
#endif
        }
        
        public void ShowInterstitial()
        {
#if EXISTED_IRON_SOURCE
            IronSource.Agent.showInterstitial();
#endif
        }
        #endregion

        
        #region RewaredAd
        public void InitRewardedVideo()
        {
#if EXISTED_IRON_SOURCE
            IronSourceEvents.onRewardedVideoAdOpenedEvent            += OnRewardedAdOpenEvent;
            IronSourceEvents.onRewardedVideoAdClosedEvent            += OnRewardedAdCloseEvent;
            IronSourceEvents.onRewardedVideoAdReadyEvent             += OnRewardedAdReadyEvent;
            IronSourceEvents.onRewardedVideoAdLoadFailedEvent        += OnRewardedAdLoadFailedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent          += OnRewardedAdRewardedEvent;
            IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += OnRewardedAdAvailabilityChangedEvent;
#endif
        }
        
        public bool IsRewardedVideoReady()
        {
#if EXISTED_IRON_SOURCE
            return IronSource.Agent.isRewardedVideoAvailable();
#else       
            return true;
#endif
        }
        
        public void LoadRewardedVideo()
        {
            Debug.Log("[IronSource] RewardedAd automatically load");
        }
        
        private void OnRewardedAdOpenEvent()
        {
            Debug.Log("[IronSource] OnRewardedAdOpenEvent");
            AdsServices.OnRewardedAdOpenEvent?.Invoke();
        }

        private void OnRewardedAdCloseEvent()
        {
            Debug.Log("[IronSource] OnRewardedAdCloseEvent");
            AdsServices.OnRewardedAdCloseEvent?.Invoke();
        }
        
        private void OnRewardedAdReadyEvent()
        {
            Debug.Log("[IronSource] OnRewardedAdReadyEvent");
            AdsServices.OnRewardedAdReadyEvent?.Invoke();
        }

#if EXISTED_IRON_SOURCE
        private void OnRewardedAdLoadFailedEvent(IronSourceError error)
        {
            Debug.Log("[IronSource] OnRewardedAdLoadFailedEvent");
            AdsServices.OnRewardedAdLoadFailedEvent?.Invoke();
        }
        
        private void OnRewardedAdRewardedEvent(IronSourcePlacement obj)
        {
            Debug.Log("[IronSource] OnRewardedAdRewardedEvent");
            AdsServices.OnRewardedAdRewardedEvent?.Invoke();
        }
#endif
        
        private void OnRewardedAdAvailabilityChangedEvent(bool available)
        {
            Debug.Log("[IronSource] OnRewardedAdAvailabilityChangedEvent");
            AdsServices.OnRewardedAdAvailabilityChangedEvent?.Invoke(available);
        }
        
        public void ShowRewardedVideo()
        {
#if EXISTED_IRON_SOURCE
            IronSource.Agent.showRewardedVideo();
#endif
        }
        #endregion
        

        public void OnPause(bool pause)
        {
#if EXISTED_IRON_SOURCE
            IronSource.Agent.onApplicationPause(pause);
#endif
        }
        
        public void OnDestroy()
        {
#if EXISTED_IRON_SOURCE
            IronSourceEvents.onBannerAdLoadFailedEvent -= this.OnLoadBannerFailed;
            
            IronSourceEvents.onInterstitialAdOpenedEvent     -= OnInterstitialOpenEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent -= OnInterstitialAdLoadFailedEvent;
            IronSourceEvents.onInterstitialAdShowFailedEvent -= OnInterstitialAdShowFailedEvent;
            IronSourceEvents.onInterstitialAdClosedEvent     -= OnInterstitialClosedEvent;
            IronSourceEvents.onInterstitialAdReadyEvent      -= OnInterstitialAdReadyEvent;
            
            IronSourceEvents.onRewardedVideoAdOpenedEvent            -= OnRewardedAdOpenEvent;
            IronSourceEvents.onRewardedVideoAdClosedEvent            -= OnRewardedAdCloseEvent;
            IronSourceEvents.onRewardedVideoAdReadyEvent             -= OnRewardedAdReadyEvent;
            IronSourceEvents.onRewardedVideoAdLoadFailedEvent        -= OnRewardedAdLoadFailedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent          -= OnRewardedAdRewardedEvent;
            IronSourceEvents.onRewardedVideoAvailabilityChangedEvent -= OnRewardedAdAvailabilityChangedEvent;
#endif
        }
    }
}