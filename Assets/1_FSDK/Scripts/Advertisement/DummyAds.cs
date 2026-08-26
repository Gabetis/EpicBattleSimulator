namespace FSDK.Ads
{
    using UnityEngine;

    public class DummyAds : IAdvertisement
    {
        public IConfigureData ConfigureData { get; }
        public bool           UseBanner     { get; } = true;
        
        public void Init()
        {
            Debug.Log("[Dummy] Init");
        }
        
        public void InitBanner()
        {
            Debug.Log("[Dummy] Init Banner");
        }
        
        public void ShowBanner()
        {
            Debug.Log("[Dummy] Show Banner");
        }
        
        public void HideBanner()
        {
            Debug.Log("[Dummy] Hide Banner");
        }
        
        public void InitInterstitial()
        {
            Debug.Log("[Dummy] Init Interstitial");
        }
        
        public bool IsInterstitialReady()
        {
            return true;
        }
        
        public void LoadInterstitial()
        {
            Debug.Log("[Dummy] Load Interstitial");
        }
        
        public void ShowInterstitial()
        {
            Debug.Log("[Dummy] Show Interstitial");
        }
        
        public void InitRewardedVideo()
        {
            Debug.Log("[Dummy] Init Rewarded Video");
        }
        
        public bool IsRewardedVideoReady()
        {
            return true;
        }
        
        public void LoadRewardedVideo()
        {
            Debug.Log("[Dummy] Load Rewarded Video");
        }
        
        public void ShowRewardedVideo()
        {
            Debug.Log("[Dummy] Show Rewarded Video");
        }
        
        public void OnPause(bool pause)
        {
            Debug.Log("[Dummy] OnPause");
        }
        
        public void OnDestroy()
        {
            Debug.Log("[Dummy] OnDestroy");
        }
    }
}