namespace FSDK
{
    public static class LogAnalytic
    {
        private static ILogAnalytic _analytic;
        
        private static void InitAnalyticIfNeeded()
        {
            if (_analytic == null)
            {
#if EXISTED_APPS_FLYER
                _analytic = new AppsFlyerLogAnalytic();
#else
                _analytic = new DummyAnalytic();
#endif
            }
        }

        /// <summary>
        /// Whenever player login to game, call it
        /// </summary>
        public static void LogLoginEvent()
        {
            InitAnalyticIfNeeded();
            _analytic.SendLoginEvent();
        }
        
        /// <summary>
        /// Complete Signup event with method (Facebook or Google for example)
        /// </summary>
        /// <param name="methodSignUp">Your method signup, ex: Facebook, Google, UsernamePassword</param>
        public static void LogCompleteSignUpEvent(string methodSignUp)
        {
            InitAnalyticIfNeeded();
            _analytic.SendCompleteSignUpEvent(methodSignUp);
        }

        /// <summary>
        /// Log tutorial, with tutorial's ID and its success status (true or false)
        /// </summary>
        /// <param name="tutorialID">Id of tutorial</param>
        /// <param name="success">Whether or not this tutorial is success</param>
        public static void LogCompleteTutorialEvent(int tutorialID, bool success)
        {
            InitAnalyticIfNeeded();
            _analytic.SendCompleteTutorialEvent(tutorialID, success);
        }

        /// <summary>
        /// Call it whenever you/user tap to button watch interstitial (ad may not available)
        /// </summary>
        public static void LogInterstitialAdCallEvent()
        {
            InitAnalyticIfNeeded();
            _analytic.SendInterstitialAdCallEvent();
        }
        
        /// <summary>
        /// Call it whenever interstitial is available
        /// </summary>
        public static void LogInterstitialAdApiCalledEvent()
        {
            InitAnalyticIfNeeded();
            _analytic.SendInterstitialAdApiCalledEvent();
        }
        
        /// <summary>
        /// Call it whenever interstitial is displayed, similar to opened
        /// </summary>
        public static void LogInterstitialAdDisplayedEvent()
        {
            InitAnalyticIfNeeded();
            _analytic.SendInterstitialAdDisplayedEvent();
        }
        
        /// <summary>
        /// Call it whenever you/user tap to button watch rewarded ad (ad may not available)
        /// </summary>
        public static void LogRewardedAdCallEvent()
        {
            InitAnalyticIfNeeded();
            _analytic.SendRewardedAdCallEvent();
        }
        
        /// <summary>
        /// Call it whenever rewarded ad is available
        /// </summary>
        public static void LogRewardedAdApiCalledEvent()
        {
            InitAnalyticIfNeeded();
            _analytic.SendRewardedAdApiCalledEvent();
        }
        
        /// <summary>
        /// Call it whenever rewarded ad is displayed, similar to opened
        /// </summary>
        public static void LogRewardedAdDisplayedEvent()
        {
            InitAnalyticIfNeeded();
            _analytic.SendRewardedAdDisplayedEvent();
        }

        /// <summary>
        /// Call it whenever rewarded ad is complete, player should be rewarded
        /// </summary>
        public static void LogRewardedAdCompleteEvent()
        {
            InitAnalyticIfNeeded();
            _analytic.SendRewardedAdCompleteEvent();
        }

#if EXISTED_PURCHASING && EXISTED_EASY_MOBILE
        /// <summary>
        /// Call it whenever a Purchase happened and project is use EasyMobile
        /// </summary>
        /// <param name="sku">IAP Package ID</param>
        public static void LogPurchaseEvent(string sku)
        {
            InitAnalyticIfNeeded();
            _analytic.SendPurchaseEvent(sku);
        }
#endif
        
#if EXISTED_PURCHASING && !EXISTED_EASY_MOBILE
        /// <summary>
        /// Call it whenever a Purchase happened and project is use Unity default IAP mechanism
        /// </summary>
        /// <param name="storeController">Unity Purchasing Controller instance</param>
        /// <param name="sku">IAP Package ID</param>
        public static void LogPurchaseEvent(UnityEngine.Purchasing.IStoreController storeController, string sku)
        {
            InitAnalyticIfNeeded();
            _analytic.SendPurchaseEventWithStoreController(storeController, sku);
        }
#endif
        
        /// <summary>
        /// Call it whenever player is achieved level or conquer it.
        /// If game doesn't have score at all, leave it 0.
        /// </summary>
        /// <param name="level">Level achieved</param>
        /// <param name="score">Score at level that player beat</param>
        public static void LogLevelAchievedEvent(int level, int score)
        {
            InitAnalyticIfNeeded();
            _analytic.SendLevelAchievedEvent(level, score);
        }
        
        /// <summary>
        /// Call it whenever player achieve new achievement (id)
        /// </summary>
        /// <param name="contentID">Achievement ID</param>
        /// <param name="level">Level when achieve</param>
        public static void LogAchievementUnlockedEvent(int contentID, int level)
        {
            InitAnalyticIfNeeded();
            _analytic.SendAchievementUnlockedEvent(contentID, level);
        }
        
        /// <summary>
        /// Call it whenever player claim bonus
        /// </summary>
        /// <param name="bonusType">Type of bonus, ex: Coin or Gem</param>
        public static void LogBonusClaimedEvent(string bonusType)
        {
            InitAnalyticIfNeeded();
            _analytic.SendBonusClaimedEvent(bonusType);
        }
        
        /// <summary>
        /// Use it when you need to log into FalconLogger.
        /// Minor Custom ad show event with extend "where" is ad show?
        /// </summary>
        /// <param name="adType">Type of ad</param>
        /// <param name="where">Ad is showed where</param>
        public static void LogAdShowMinorCustomEvent(string adType, string @where)
        {
            InitAnalyticIfNeeded();

            string validateWhere = FSDKServices.ValidateEventName(where);
            _analytic.SendAdShowMinorCustomEvent(adType, validateWhere);
        }

        /// <summary>
        /// Custom ad show event at level with extend "where" is ad show?
        /// </summary>
        /// <param name="adType">Type of ad</param>
        /// <param name="level">Current level</param>
        /// <param name="difficulty">Difficulty of that level</param>
        /// <param name="where">Ad is showed where</param>
        public static void LogAdShowCustomEvent(string adType, int level, LevelDifficulty difficulty, string @where)
        {
            InitAnalyticIfNeeded();

            string validateWhere = FSDKServices.ValidateEventName(where);
            _analytic.SendAdShowCustomEvent(adType, level, difficulty, validateWhere);
        }
        
        /// <summary>
        /// A custom event for open an ad
        /// </summary>
        /// <param name="adType">Type of ad, ex: reward_video</param>
        public static void LogAdOpenCustomEvent(string adType)
        {
            InitAnalyticIfNeeded();
            _analytic.SendAdOpenCustomEvent(adType);
        }
        
        /// <summary>
        /// A custom event for complete an ad
        /// </summary>
        /// <param name="adType">Type of ad, ex: rewarded_video</param>
        public static void LogAdCompleteCustomEvent(string adType)
        {
            InitAnalyticIfNeeded();
            _analytic.SendAdCompleteCustomEvent(adType);
        }
        
#if EXISTED_PURCHASING && EXISTED_EASY_MOBILE
        /// <summary>
        /// Use it when you need to log into FalconLogger.
        /// Minor custom event for Purchase and project is use EasyMobile with extend "where" is purchase happened?
        /// </summary>
        /// <param name="sku">IAP Package ID</param>
        /// <param name="where">Purchase where</param>
        public static void LogPurchaseMinorCustomEvent(string sku, string @where)
        {
            InitAnalyticIfNeeded();
            
            string validateWhere = FSDKServices.ValidateEventName(where);
            _analytic.SendPurchaseMinorCustomEvent(sku, validateWhere);
        }

        /// <summary>
        /// A custom event for Purchase and project is use EasyMobile at level with extend "where" is purchase happened?
        /// </summary>
        /// <param name="sku">IAP Package ID</param>
        /// <param name="level">Current level</param>
        /// <param name="difficulty">Difficulty of that level</param>
        /// <param name="where">Purchase where</param>
        public static void LogPurchaseCustomEvent(string sku, int level, LevelDifficulty difficulty, string @where)
        {
            InitAnalyticIfNeeded();
            
            string validateWhere = FSDKServices.ValidateEventName(where);
            _analytic.SendPurchaseCustomEvent(sku, level, difficulty, validateWhere);
        }
#endif
        
#if EXISTED_PURCHASING && !EXISTED_EASY_MOBILE
        /// <summary>
        /// Use it when you need to log into FalconLogger.
        /// Minor custom event for Purchase and project is use Unity default IAP mechanism with extend "where" is purchase happened?
        /// </summary>
        /// <param name="storeController">Unity Purchasing Controller instance</param>
        /// <param name="sku">IAP Package ID</param>
        /// <param name="where">Purchase where</param>
        public static void LogPurchaseMinorCustomEventWithStoreController(UnityEngine.Purchasing.IStoreController storeController, string sku, string @where)
        {
            InitAnalyticIfNeeded();
            
            string validateWhere = FSDKServices.ValidateEventName(where);
            _analytic.SendPurchaseMinorCustomEventWithStoreController(storeController, sku, validateWhere);
        }
        
        /// <summary>
        /// A custom event for Purchase and project is use Unity default IAP mechanism at level with extend "where" is purchase happened?
        /// </summary>
        /// <param name="storeController">Unity Purchasing Controller instance</param>
        /// <param name="sku">IAP Package ID</param>
        /// <param name="level">Current level</param>
        /// <param name="difficulty">Difficulty of that level</param>
        /// <param name="where">Purchase where</param>
        public static void LogPurchaseCustomEventWithStoreController(UnityEngine.Purchasing.IStoreController storeController, string sku, int level, LevelDifficulty difficulty, string @where)
        {
            InitAnalyticIfNeeded();
            
            string validateWhere = FSDKServices.ValidateEventName(where);
            _analytic.SendPurchaseCustomEventWithStoreController(storeController, sku, level, difficulty, validateWhere);
        }
#endif

        /// <summary>
        /// A custom event for logging pass level. FalconLogger supported
        /// </summary>
        /// <param name="level">Passed level</param>
        /// <param name="difficulty">Difficulty of this level</param>
        public static void LogLevelPassCustomEvent(int level, LevelDifficulty difficulty)
        {
            InitAnalyticIfNeeded();
            _analytic.SendLevelPassCustomEvent(level, difficulty);
        }
        
        /// <summary>
        /// A custom event for logging fail level. FalconLogger supported
        /// </summary>
        /// <param name="level">Failed level</param>
        /// <param name="difficulty">Difficulty of this level</param>
        public static void LogLevelFailCustomEvent(int level, LevelDifficulty difficulty)
        {
            InitAnalyticIfNeeded();
            _analytic.SendLevelFailCustomEvent(level, difficulty);
        }
    }
}