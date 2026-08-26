namespace FSDK
{
    using System;
    using System.Reflection;
    using UnityEngine;

    public class IronSourceConfigureData : IConfigureData
    {
        public string         AndroidKey        { get; set; } = "12f1ad2f5";
        public string         IOSKey            { get; set; } = "12f1ad2f5";
        public string         AndroidAdmobAppID { get; set; } = "ca-app-pub-8301791769525290~1947988445";
        public string         IOSAdmobAppID     { get; set; } = "ca-app-pub-8301791769525290~1947988445";
        public bool           UseBanner         { get; set; } = true;
        public BannerPosition BannerPosition    { get; set; } = BannerPosition.Bottom;

        public IronSourceConfigureData()
        {
            this.BindData();
        }

        public void BindData()
        {
#if UNITY_EDITOR
            string jsonData = FileServices.ReadContentFromTextFile(FSDKServices.IRON_SOURCE_CONFIG_PATH);
#else
            string jsonData = Resources.Load<TextAsset>("ironsource_conf").text;
#endif
            var data = JsonUtility.FromJson<IronSourceData>(jsonData);

            if (data != null)
            {
                AndroidKey        = data.AndroidKey;
                IOSKey            = data.IOSKey;
                AndroidAdmobAppID = data.AndroidAdmobAppID;
                IOSAdmobAppID     = data.IOSAdmobAppID;
                UseBanner         = data.UseBanner;
                BannerPosition    = data.BannerPosition;
            }
        }
        
        public void StoreData()
        {
#if UNITY_EDITOR
            var data = new IronSourceData()
            {
                AndroidKey        = this.AndroidKey,
                IOSKey            = this.IOSKey,
                AndroidAdmobAppID = this.AndroidAdmobAppID,
                IOSAdmobAppID     = this.IOSAdmobAppID,
                UseBanner         = this.UseBanner,
                BannerPosition    = this.BannerPosition
            };

            var dataAsString = JsonUtility.ToJson(data, true);
            FileServices.WriteContentToTextFile(FSDKServices.IRON_SOURCE_CONFIG_PATH, dataAsString, false);
#endif
        }
        
        public PropertyInfo[] GetProperties()
        {
            return ReflectionUtils.GetProperties<IronSourceConfigureData>();
        }
        
        public object GetPropertyValue(string propertyName)
        {
            return this.GetPropertyValue<IronSourceConfigureData>(propertyName);
        }
    }
}