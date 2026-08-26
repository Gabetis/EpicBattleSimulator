namespace FSDK
{
    using UnityEditor;

    [InitializeOnLoad]
    public class FSDKLoader
    {
        private static APluginChecker[] Checkers = new APluginChecker[]
        {
            new InAppPurchasingChecker(),
            new EasyMobileChecker(),
            new AppsFlyerChecker(), 
            new IronSourceChecker(),
            new MaxChecker()
        };
        
        static FSDKLoader()
        {
            CheckPlugins();
            EditorApplication.update += CheckPlugins;
        }

        public static void CheckPlugins()
        {
            foreach (var checker in Checkers)
            {
                checker.Check();
            }
            
            EditorApplication.update -= CheckPlugins;
        }
    }
}