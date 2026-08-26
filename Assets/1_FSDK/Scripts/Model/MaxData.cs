namespace FSDK
{
    [System.Serializable]
    public class MaxData
    {
        public string         SDKKey;
        public string         BannerAdUnitId;
        public string         InterstitialAdUnitId;
        public string         RewardedAdUnitId;
        public string         AndroidAdmobAppID;
        public string         IOSAdmobAppID;
        public bool           UseBanner;
        public BannerPosition BannerPosition;
    }
}