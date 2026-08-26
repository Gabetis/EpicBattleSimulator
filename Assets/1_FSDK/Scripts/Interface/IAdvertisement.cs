namespace FSDK
{
    public enum BannerPosition
    {
        Top,
        Bottom
    }
    
    public interface IAdvertisement
    {
        IConfigureData ConfigureData { get; }
        bool           UseBanner     { get; }

        void Init();
        
        void InitBanner();
        void ShowBanner();
        void HideBanner();
        
        
        void InitInterstitial();
        bool IsInterstitialReady();
        void LoadInterstitial();
        void ShowInterstitial();
        
        
        void InitRewardedVideo();
        bool IsRewardedVideoReady();
        void LoadRewardedVideo();
        void ShowRewardedVideo();

        void OnPause(bool pause);
        void OnDestroy();
    }
}