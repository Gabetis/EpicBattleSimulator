namespace FSDK
{
    using System.Collections.Generic;
    using UnityEngine;

    public class AppsFlyerLogAnalytic : ILogAnalytic
    {
        public void SendLoginEvent()
        {
#if EXISTED_APPS_FLYER
            AppsFlyerSDK.AppsFlyer.sendEvent(AFInAppEvents.LOGIN, null);
#endif
        }

        public void SendCompleteSignUpEvent(string methodSignUp)
        {
#if EXISTED_APPS_FLYER
            Dictionary<string, string> eventValue = new Dictionary<string, string>();
            eventValue.Add(AFInAppEvents.REGSITRATION_METHOD, methodSignUp);
            AppsFlyerSDK.AppsFlyer.sendEvent(AFInAppEvents.LOGIN, eventValue);
#endif
        }

        public void SendCompleteTutorialEvent(int tutorialID, bool success)
        {
#if EXISTED_APPS_FLYER
            Dictionary<string, string> eventValue = new Dictionary<string, string>();
            eventValue.Add("af_tutorial_id", tutorialID.ToString());
            eventValue.Add("af_success", success.ToString());
            AppsFlyerSDK.AppsFlyer.sendEvent(AFInAppEvents.TUTORIAL_COMPLETION, eventValue);
#endif
        }

        public void SendInterstitialAdCallEvent()
        {
#if EXISTED_APPS_FLYER
            AppsFlyerSDK.AppsFlyer.sendEvent("af_inters_ad_eligible", null);
#endif
        }

        public void SendInterstitialAdApiCalledEvent()
        {
#if EXISTED_APPS_FLYER
            AppsFlyerSDK.AppsFlyer.sendEvent("af_inters_api_called", null);
#endif
        }

        public void SendInterstitialAdDisplayedEvent()
        {
#if EXISTED_APPS_FLYER
            AppsFlyerSDK.AppsFlyer.sendEvent("af_inters_displayed", null);
#endif
        }

        public void SendRewardedAdCallEvent()
        {
#if EXISTED_APPS_FLYER
            AppsFlyerSDK.AppsFlyer.sendEvent("af_rewarded_ad_eligible", null);
#endif
        }

        public void SendRewardedAdApiCalledEvent()
        {
#if EXISTED_APPS_FLYER
            AppsFlyerSDK.AppsFlyer.sendEvent("af_rewarded_api_called", null);
#endif
        }

        public void SendRewardedAdDisplayedEvent()
        {
#if EXISTED_APPS_FLYER
            AppsFlyerSDK.AppsFlyer.sendEvent("af_rewarded_displayed", null);
#endif
        }

        public void SendRewardedAdCompleteEvent()
        {
#if EXISTED_APPS_FLYER
            AppsFlyerSDK.AppsFlyer.sendEvent("af_rewarded_ad_completed", null);
#endif
        }

#if EXISTED_PURCHASING && EXISTED_EASY_MOBILE
        /// <summary>
        /// General event heavily in need of EasyMobile to get essential information
        /// </summary>
        public void SendPurchaseEvent(string sku)
        {
#if EXISTED_APPS_FLYER
            Dictionary<string, string> eventValue = new Dictionary<string, string>();
            eventValue.Add(AFInAppEvents.REVENUE, FSDKServices.GetLocalizedPrice(sku).ToString());
            eventValue.Add(AFInAppEvents.CONTENT_ID, sku);
            eventValue.Add(AFInAppEvents.CURRENCY, FSDKServices.GetCurrencyCode(sku));
            eventValue.Add(AFInAppEvents.QUANTITY, "1");
            AppsFlyerSDK.AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, eventValue);
#endif
        }
#endif

#if EXISTED_PURCHASING && !EXISTED_EASY_MOBILE
        /// <summary>
        /// Use it when project does not have EasyMobile
        /// </summary>
        public void SendPurchaseEventWithStoreController(UnityEngine.Purchasing.IStoreController storeController, string sku)
        {
#if EXISTED_APPS_FLYER
            Dictionary<string, string> eventValue = new Dictionary<string, string>();
            eventValue.Add(AFInAppEvents.REVENUE, storeController.GetLocalizedPrice(sku).ToString());
            eventValue.Add(AFInAppEvents.CONTENT_ID, sku);
            eventValue.Add(AFInAppEvents.CURRENCY, storeController.GetCurrencyCode(sku));
            eventValue.Add(AFInAppEvents.QUANTITY, "1");
            AppsFlyerSDK.AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, eventValue);
#endif
        }
#endif

        public void SendLevelAchievedEvent(int level, int score)
        {
#if EXISTED_APPS_FLYER
            Dictionary<string, string> eventValue = new Dictionary<string, string>();
            eventValue.Add("af_level", level.ToString());
            eventValue.Add("af_score", score.ToString());
            AppsFlyerSDK.AppsFlyer.sendEvent(AFInAppEvents.LEVEL_ACHIEVED, eventValue);
#endif
        }

        public void SendAchievementUnlockedEvent(int contentID, int level)
        {
#if EXISTED_APPS_FLYER
            Dictionary<string, string> eventValue = new Dictionary<string, string>();
            eventValue.Add("content_id", contentID.ToString());
            eventValue.Add("af_level", level.ToString());
            AppsFlyerSDK.AppsFlyer.sendEvent(AFInAppEvents.ACHIEVEMENT_UNLOCKED, eventValue);
#endif
        }

        public void SendBonusClaimedEvent(string bonusType)
        {
#if EXISTED_APPS_FLYER
            Dictionary<string, string> eventValue = new Dictionary<string, string>();
            eventValue.Add("bonus_type", bonusType);
            AppsFlyerSDK.AppsFlyer.sendEvent("bonus_claimed", eventValue);
#endif
        }
        
        
        
        public void SendAdShowMinorCustomEvent(string adType, string @where)
        {
#if EXISTED_APPS_FLYER
            Dictionary<string, string> eventValue = new Dictionary<string, string>();
            eventValue.Add("af_type", adType);
            eventValue.Add("af_where", where);
            eventValue.Add("app_version", Application.version);
            AppsFlyerSDK.AppsFlyer.sendEvent( $"{FSDKServices.EVENT_PREFIX}ad_show_{adType}_{where}", eventValue);
#endif
        }
        

        public void SendAdShowCustomEvent(string adType, int level, LevelDifficulty difficulty, string @where)
        {
#if EXISTED_APPS_FLYER
            Dictionary<string, string> eventValue = new Dictionary<string, string>();
            eventValue.Add("af_type", adType);
            eventValue.Add("af_level", level.ToString());
            eventValue.Add("af_difficulty", difficulty.ToString().ToLower());
            eventValue.Add("af_where", where);
            eventValue.Add("app_version", Application.version);
            AppsFlyerSDK.AppsFlyer.sendEvent(FSDKServices.EVENT_PREFIX + "ad_show", eventValue);
#endif
        }

        public void SendAdOpenCustomEvent(string adType)
        {
#if EXISTED_APPS_FLYER
            Dictionary<string, string> adsEvent = new Dictionary<string, string>();
            adsEvent.Add("af_type", adType);
            adsEvent.Add("app_version", Application.version);
            AppsFlyerSDK.AppsFlyer.sendEvent(FSDKServices.EVENT_PREFIX + "ad_opened_" + adType, adsEvent);
#endif
        }

        public void SendAdCompleteCustomEvent(string adType)
        {
#if EXISTED_APPS_FLYER
            Dictionary<string, string> adsEvent = new Dictionary<string, string>();
            adsEvent.Add("af_type", adType);
            adsEvent.Add("app_version", Application.version);
            AppsFlyerSDK.AppsFlyer.sendEvent(FSDKServices.EVENT_PREFIX + "ad_complete_" + adType, adsEvent);
#endif
        }

#if EXISTED_PURCHASING && EXISTED_EASY_MOBILE
        public void SendPurchaseMinorCustomEvent(string sku, string @where) 
        {
#if EXISTED_APPS_FLYER
            Dictionary<string, string> eventValue = new Dictionary<string, string>();
            eventValue.Add(AFInAppEvents.REVENUE, FSDKServices.GetLocalizedPrice(sku).ToString());
            eventValue.Add(AFInAppEvents.CONTENT_ID, sku);
            eventValue.Add(AFInAppEvents.CURRENCY, FSDKServices.GetCurrencyCode(sku));
            eventValue.Add(AFInAppEvents.QUANTITY, "1");
            eventValue.Add("af_where", where);
            eventValue.Add("app_version", Application.version);
            AppsFlyerSDK.AppsFlyer.sendEvent(FSDKServices.EVENT_PREFIX + "purchase_" + where, eventValue);
#endif
        }

        public void SendPurchaseCustomEvent(string sku, int level, LevelDifficulty difficulty, string @where) 
        {
#if EXISTED_APPS_FLYER
            Dictionary<string, string> eventValue = new Dictionary<string, string>();
            eventValue.Add(AFInAppEvents.REVENUE, FSDKServices.GetLocalizedPrice(sku).ToString());
            eventValue.Add(AFInAppEvents.CONTENT_ID, sku);
            eventValue.Add(AFInAppEvents.CURRENCY, FSDKServices.GetCurrencyCode(sku));
            eventValue.Add(AFInAppEvents.QUANTITY, "1");
            eventValue.Add("af_level", level.ToString());
            eventValue.Add("af_difficulty", difficulty.ToString().ToLower());
            eventValue.Add("af_where", where);
            eventValue.Add("app_version", Application.version);
            AppsFlyerSDK.AppsFlyer.sendEvent(FSDKServices.EVENT_PREFIX + "purchase", eventValue);
#endif
        }
#endif

#if EXISTED_PURCHASING && !EXISTED_EASY_MOBILE
        public void SendPurchaseMinorCustomEventWithStoreController(UnityEngine.Purchasing.IStoreController storeController, string sku, string @where)
        {
#if EXISTED_APPS_FLYER
            Dictionary<string, string> eventValue = new Dictionary<string, string>();
            eventValue.Add(AFInAppEvents.REVENUE, storeController.GetLocalizedPrice(sku).ToString());
            eventValue.Add(AFInAppEvents.CONTENT_ID, sku);
            eventValue.Add(AFInAppEvents.CURRENCY, storeController.GetCurrencyCode(sku));
            eventValue.Add(AFInAppEvents.QUANTITY, "1");
            eventValue.Add("af_where", where);
            eventValue.Add("app_version", Application.version);
            AppsFlyerSDK.AppsFlyer.sendEvent(FSDKServices.EVENT_PREFIX + "purchase_" + where, eventValue);
#endif
        }
        
        public void SendPurchaseCustomEventWithStoreController(UnityEngine.Purchasing.IStoreController storeController, string sku, int level, LevelDifficulty difficulty, string @where)
        {
#if EXISTED_APPS_FLYER
            Dictionary<string, string> eventValue = new Dictionary<string, string>();
            eventValue.Add(AFInAppEvents.REVENUE, storeController.GetLocalizedPrice(sku).ToString());
            eventValue.Add(AFInAppEvents.CONTENT_ID, sku);
            eventValue.Add(AFInAppEvents.CURRENCY, storeController.GetCurrencyCode(sku));
            eventValue.Add(AFInAppEvents.QUANTITY, "1");
            eventValue.Add("af_level", level.ToString());
            eventValue.Add("af_difficulty", difficulty.ToString().ToLower());
            eventValue.Add("af_where", where);
            eventValue.Add("app_version", Application.version);
            AppsFlyerSDK.AppsFlyer.sendEvent(FSDKServices.EVENT_PREFIX + "purchase", eventValue);
#endif
        }
#endif

        public void SendLevelPassCustomEvent(int level, LevelDifficulty difficulty)
        {
#if EXISTED_APPS_FLYER
            Dictionary<string, string> eventValue = new Dictionary<string, string>();
            eventValue.Add("af_level", level.ToString());
            eventValue.Add("af_difficulty", difficulty.ToString().ToLower());
            eventValue.Add("app_version", Application.version);
            AppsFlyerSDK.AppsFlyer.sendEvent($"{FSDKServices.EVENT_PREFIX}level_pass_{level.ToString()}_{difficulty.ToString().ToLower()}", eventValue);
#endif
        }

        public void SendLevelFailCustomEvent(int level, LevelDifficulty difficulty)
        {
#if EXISTED_APPS_FLYER
            Dictionary<string, string> eventValue = new Dictionary<string, string>();
            eventValue.Add("af_level", level.ToString());
            eventValue.Add("af_difficulty", difficulty.ToString().ToLower());
            eventValue.Add("app_version", Application.version);
            AppsFlyerSDK.AppsFlyer.sendEvent($"{FSDKServices.EVENT_PREFIX}level_fail_{level.ToString()}_{difficulty.ToString().ToLower()}", eventValue);
#endif
        }
    }
}