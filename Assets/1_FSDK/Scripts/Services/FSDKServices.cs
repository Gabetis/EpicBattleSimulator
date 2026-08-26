namespace FSDK
{
    using System.Text.RegularExpressions;
    using UnityEngine;

    public enum LevelDifficulty
    {
        Easy,
        Normal,
        Hard
    }
    
    public static class FSDKServices
    {
        public const  string EVENT_PREFIX            = "falcon_game_";
        public static string APPS_FLYER_CONFIG_PATH  = Application.dataPath + @"/1_FSDK/Resources/appsflyer_conf.json";
        public static string IRON_SOURCE_CONFIG_PATH = Application.dataPath + @"/1_FSDK/Resources/ironsource_conf.json";
        public static string MAX_CONFIG_PATH         = Application.dataPath + @"/1_FSDK/Resources/max_conf.json";
        
#if EXISTED_PURCHASING && EXISTED_EASY_MOBILE
        public static float GetLocalizedPrice(string sku)
        {
            if (EasyMobile.InAppPurchasing.GetProductLocalizedData(sku) != null)
            {
                decimal price = EasyMobile.InAppPurchasing.GetProductLocalizedData(sku).localizedPrice;
                return (float)price * 0.7f;
            }
            return 0.9999f;
        }
        
        public static string GetCurrencyCode(string sku)
        {
            if (EasyMobile.InAppPurchasing.GetProductLocalizedData(sku) != null)
            {
                return EasyMobile.InAppPurchasing.GetProductLocalizedData(sku).isoCurrencyCode;
            }
            return "USD";
        }
#endif
      
#if EXISTED_PURCHASING && !EXISTED_EASY_MOBILE
        public static float GetLocalizedPrice(this UnityEngine.Purchasing.IStoreController storeController, string sku)
        {
            if (storeController != null)
            {
                var price = storeController.products.WithID(sku).metadata.localizedPrice;
                return (float)price * 0.7f;
            }
            return 0;
        }
        
        public static string GetCurrencyCode(this UnityEngine.Purchasing.IStoreController storeController, string sku)
        {
            if (storeController != null)
            {
                return storeController.products.WithID(sku).metadata.isoCurrencyCode;
            }
            return "USD";
        }
#endif

        public static bool IsContainWhiteSpace(this string str)
        {
            return str.Contains(" ");
        }

        public static string ValidateEventName(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                eventName = "not_specified";
            }
            
            eventName = eventName.ToLower().Trim();
            if (eventName.IsContainWhiteSpace())
            {
                eventName = eventName.Replace(" ", "_");
            }
            
            if(Regex.IsMatch(eventName, @"\W")) return "invalid_name";

            return eventName;
        }
        
        public static bool IsTablet()
        {
            float ssw;
            if (Screen.width > Screen.height) { ssw = Screen.width; } else { ssw = Screen.height; }

            if (ssw < 800) return false;

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                float screenWidth  = Screen.width / Screen.dpi;
                float screenHeight = Screen.height / Screen.dpi;
                float size         = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
                if (size >= 6.5f) return true;
            }

            return false;
        }
    }
}