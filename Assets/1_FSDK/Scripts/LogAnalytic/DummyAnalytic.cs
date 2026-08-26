namespace FSDK
{
    using UnityEngine;

    public class DummyAnalytic : ILogAnalytic
    {
        public void SendLoginEvent()
        {
            Debug.Log("[Dummy] SendLoginEvent");
        }

        public void SendCompleteSignUpEvent(string methodSignUp)
        {
            Debug.Log($"[Dummy] SendCompleteSignUpEvent - SignUpMethod: {methodSignUp}");
        }
        
        public void SendCompleteTutorialEvent(int tutorialID, bool success)
        {
            Debug.Log($"[Dummy] SendCompleteTutorialEvent - TutorialID: {tutorialID}, Success: {success}");
        }
        
        public void SendInterstitialAdCallEvent()
        {
            Debug.Log("[Dummy] SendInterstitialAdCallEvent");
        }

        public void SendInterstitialAdApiCalledEvent()
        {
            Debug.Log("[Dummy] SendInterstitialAdApiCalledEvent");
        }

        public void SendInterstitialAdDisplayedEvent()
        {
            Debug.Log("[Dummy] SendInterstitialAdDisplayedEvent");
        }

        public void SendRewardedAdCallEvent()
        {
            Debug.Log("[Dummy] SendRewardedAdCallEvent");
        }

        public void SendRewardedAdApiCalledEvent()
        {
            Debug.Log("[Dummy] SendRewardedAdApiCalledEvent");
        }

        public void SendRewardedAdDisplayedEvent()
        {
            Debug.Log("[Dummy] SendRewardedAdDisplayedEvent");
        }
        
        public void SendRewardedAdCompleteEvent()
        {
            Debug.Log("[Dummy] SendRewardedAdCompleteEvent");
        }
        
#if EXISTED_PURCHASING && EXISTED_EASY_MOBILE
        public void SendPurchaseEvent(string sku)
        {
            Debug.Log($"[Dummy] SendPurchaseEvent - SKU: {sku}");
        }
#endif
        
#if EXISTED_PURCHASING && !EXISTED_EASY_MOBILE
        public void SendPurchaseEventWithStoreController(UnityEngine.Purchasing.IStoreController storeController, string sku)
        {
            Debug.Log($"[Dummy] SendPurchaseEventWithStoreController - SKU: {sku}");
        }
        
#endif
        public void SendLevelAchievedEvent(int level, int score)
        {
            Debug.Log($"[Dummy] SendLevelAchievedEvent - Level: {level}, Score: {score}");
        }
        
        public void SendAchievementUnlockedEvent(int contentID, int level)
        {
            Debug.Log($"[Dummy] SendAchievementUnlockedEvent - ContentID: {contentID}, Level: {level}");
        }
        
        public void SendBonusClaimedEvent(string bonusType)
        {
            Debug.Log($"[Dummy] SendBonusClaimedEvent - BonusType: {bonusType}");
        }
        
        
        
        public void SendAdShowMinorCustomEvent(string adType, string @where)
        {
            Debug.Log($"[Dummy] SendAdShowMinorCustomEvent - AdType: {adType},  Where: {where}");
        }
        
        public void SendAdShowCustomEvent(string adType, int level, LevelDifficulty difficulty, string @where)
        {
            Debug.Log($"[Dummy] SendAdShowCustomEvent - AdType: {adType}, Level: {level.ToString()}, Difficulty: {difficulty.ToString().ToLower()}, Where: {where}");
        }
        
        public void SendAdOpenCustomEvent(string adType)
        {
            Debug.Log($"[Dummy] SendAdOpenCustomEvent - AdType: {adType}");
        }
        
        public void SendAdCompleteCustomEvent(string adType)
        {
            Debug.Log($"[Dummy] SendAdCompleteCustomEvent - AdType: {adType}");
        }
        
#if EXISTED_PURCHASING && EXISTED_EASY_MOBILE
        public void SendPurchaseMinorCustomEvent(string sku, string @where) 
        {
            Debug.Log($"[Dummy] SendPurchaseMinorCustomEvent - SKU: {sku}, Where: {where}");
        }
        
        public void SendPurchaseCustomEvent(string sku, int level, LevelDifficulty difficulty, string @where) 
        {
            Debug.Log($"[Dummy] SendPurchaseCustomEvent - SKU: {sku}, Level: {level.ToString()}, Difficulty: {difficulty.ToString().ToLower()}, Where: {where}");
        }
#endif
        
#if EXISTED_PURCHASING && !EXISTED_EASY_MOBILE
        public void SendPurchaseMinorCustomEventWithStoreController(UnityEngine.Purchasing.IStoreController storeController, string sku, string @where)
        {
            Debug.Log($"[Dummy] SendPurchaseMinorCustomEventWithStoreController - SKU: {sku}, Where: {where}");
        }
        
        public void SendPurchaseCustomEventWithStoreController(UnityEngine.Purchasing.IStoreController storeController, string sku, int level, LevelDifficulty difficulty, string @where)
        {
            Debug.Log($"[Dummy] SendPurchaseCustomEventWithStoreController - SKU: {sku}, Level: {level.ToString()}, Difficulty: {difficulty.ToString().ToLower()}, Where: {where}");
        }
#endif

        public void SendLevelPassCustomEvent(int level, LevelDifficulty difficulty)
        {
            Debug.Log($"[Dummy] SendLevelPassCustomEvent - Level {level}, Difficulty: {difficulty.ToString().ToLower()}");
        }
        
        public void SendLevelFailCustomEvent(int level, LevelDifficulty difficulty)
        {
            Debug.Log($"[Dummy] SendLevelFailCustomEvent - Level {level}, Difficulty: {difficulty.ToString().ToLower()}");
        }
    }
}