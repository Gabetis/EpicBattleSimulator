namespace FSDK
{
    public interface ILogAnalytic
    {
        #region Default Events
        void SendLoginEvent();
        void SendCompleteSignUpEvent(string methodSignUp);
        void SendCompleteTutorialEvent(int tutorialID, bool success);

        void SendInterstitialAdCallEvent();
        void SendInterstitialAdApiCalledEvent();
        void SendInterstitialAdDisplayedEvent();
        
        void SendRewardedAdCallEvent();
        void SendRewardedAdApiCalledEvent();
        void SendRewardedAdDisplayedEvent();
        void SendRewardedAdCompleteEvent();
        
#if EXISTED_PURCHASING && EXISTED_EASY_MOBILE
        void SendPurchaseEvent(string sku);
#endif
        
#if EXISTED_PURCHASING && !EXISTED_EASY_MOBILE
        void SendPurchaseEventWithStoreController(UnityEngine.Purchasing.IStoreController storeController, string sku);
#endif
        
        void SendLevelAchievedEvent(int level, int score);
        
        void SendAchievementUnlockedEvent(int contentID, int level);
        void SendBonusClaimedEvent(string bonusType);
        #endregion

        #region Custom Events
        void SendAdShowMinorCustomEvent(string adType, string @where);
        void SendAdShowCustomEvent(string adType, int level, LevelDifficulty difficulty, string @where);
        
        void SendAdOpenCustomEvent(string adType);
        void SendAdCompleteCustomEvent(string adType);
        
#if EXISTED_PURCHASING && EXISTED_EASY_MOBILE
        void SendPurchaseMinorCustomEvent(string sku, string @where);
        void SendPurchaseCustomEvent(string sku, int level, LevelDifficulty difficulty, string @where);
#endif
            
#if EXISTED_PURCHASING && !EXISTED_EASY_MOBILE
        void SendPurchaseMinorCustomEventWithStoreController(UnityEngine.Purchasing.IStoreController storeController, string sku, string @where);
        void SendPurchaseCustomEventWithStoreController(UnityEngine.Purchasing.IStoreController storeController, string sku, int level, LevelDifficulty difficulty, string @where);
#endif   
            
        void SendLevelPassCustomEvent(int level, LevelDifficulty difficulty);
        void SendLevelFailCustomEvent(int level, LevelDifficulty difficulty);
        #endregion
    }
}