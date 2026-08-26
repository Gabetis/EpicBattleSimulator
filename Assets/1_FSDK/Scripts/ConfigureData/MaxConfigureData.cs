namespace FSDK
{
    using System;
    using System.Reflection;
    using UnityEngine;

    public class MaxConfigureData : IConfigureData
    {
        public string         SDKKey               { get; set; }
        public string         BannerAdUnitId       { get; set; }
        public string         InterstitialAdUnitId { get; set; }
        public string         RewardedAdUnitId     { get; set; }
        public string         AndroidAdmobAppID    { get; set; } = "ca-app-pub-8301791769525290~1947988445";
        public string         IOSAdmobAppID        { get; set; } = "ca-app-pub-8301791769525290~1947988445";
        public bool           UseBanner            { get; set; } = true;
        public BannerPosition BannerPosition       { get; set; } = BannerPosition.Bottom;

        public MaxConfigureData()
        {
            this.BindData();
        }
        
        public void BindData()
        {
#if UNITY_EDITOR
            string jsonData = FileServices.ReadContentFromTextFile(FSDKServices.MAX_CONFIG_PATH);
#else
            string jsonData = Resources.Load<TextAsset>("max_conf").text;
#endif
            var data = JsonUtility.FromJson<MaxData>(jsonData);

            if (data != null)
            {
                SDKKey               = data.SDKKey;
                BannerAdUnitId       = data.BannerAdUnitId;
                InterstitialAdUnitId = data.InterstitialAdUnitId;
                RewardedAdUnitId     = data.RewardedAdUnitId;
                AndroidAdmobAppID    = data.AndroidAdmobAppID;
                IOSAdmobAppID        = data.IOSAdmobAppID;
                UseBanner            = data.UseBanner;
                BannerPosition       = data.BannerPosition;
            }
        }
        
        public void StoreData()
        {
#if UNITY_EDITOR
            var data = new MaxData()
            {
                SDKKey               = this.SDKKey,
                BannerAdUnitId       = this.BannerAdUnitId,
                InterstitialAdUnitId = this.InterstitialAdUnitId,
                RewardedAdUnitId     = this.RewardedAdUnitId,
                AndroidAdmobAppID    = this.AndroidAdmobAppID,
                IOSAdmobAppID        = this.IOSAdmobAppID,
                UseBanner            = this.UseBanner,
                BannerPosition       = this.BannerPosition
            };

            var dataAsString = JsonUtility.ToJson(data, true);
            FileServices.WriteContentToTextFile(FSDKServices.MAX_CONFIG_PATH, dataAsString, false);
#endif
        }
        
        public PropertyInfo[] GetProperties()
        {
            return ReflectionUtils.GetProperties<MaxConfigureData>();
        }
        
        public object GetPropertyValue(string propertyName)
        {
            return this.GetPropertyValue<MaxConfigureData>(propertyName);
        }
    }
}