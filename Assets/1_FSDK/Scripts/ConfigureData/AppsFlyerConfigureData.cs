namespace FSDK
{
    using System;
    using System.Reflection;
    using UnityEngine;

    public class AppsFlyerConfigureData : IConfigureData
    {
        public string DevKey        { get; set; } = "UAVHtuYSgwSPXxXQVDGA65";
        public string IOSAppID      { get; set; } = "1234567890";
        public bool   ApplyToPrefab { get; set; } = true;

        public AppsFlyerConfigureData()
        {
            this.BindData();
        }

        public void BindData()
        {
#if UNITY_EDITOR
            string jsonData = FileServices.ReadContentFromTextFile(FSDKServices.APPS_FLYER_CONFIG_PATH);
#else
            string jsonData = Resources.Load<TextAsset>("appsflyer_conf").text;
#endif
            var data = JsonUtility.FromJson<AppsFlyerData>(jsonData);

            if (data != null)
            {
                DevKey        = data.DevKey;
                IOSAppID      = data.IOSAppID;
                ApplyToPrefab = data.ApplyToPrefab;
            }
        }
        
        public void StoreData()
        {
#if UNITY_EDITOR
            var data = new AppsFlyerData()
            {
                DevKey        = this.DevKey,
                IOSAppID      = this.IOSAppID,
                ApplyToPrefab = this.ApplyToPrefab
            };

            var dataAsString = JsonUtility.ToJson(data, true);
            FileServices.WriteContentToTextFile(FSDKServices.APPS_FLYER_CONFIG_PATH, dataAsString, false);
#endif
        }

        public PropertyInfo[] GetProperties()
        {
            return ReflectionUtils.GetProperties<AppsFlyerConfigureData>();
        }
        
        public object GetPropertyValue(string propertyName)
        {
            return this.GetPropertyValue<AppsFlyerConfigureData>(propertyName);
        }
    }
}