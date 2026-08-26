namespace FSDK
{
    using System;

    [Serializable]
    public class IronSourceData
    {
        public string         AndroidKey;
        public string         IOSKey;
        public string         AndroidAdmobAppID;
        public string         IOSAdmobAppID;
        public bool           UseBanner;
        public BannerPosition BannerPosition;
    }
}