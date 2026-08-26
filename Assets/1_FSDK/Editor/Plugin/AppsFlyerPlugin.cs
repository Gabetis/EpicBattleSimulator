namespace FSDK
{
    public class AppsFlyerPlugin : APlugin
    {
        public AppsFlyerPlugin()
        {
            Name       = "AppsFlyer SDK";
            Checker    = new AppsFlyerChecker();
            Downloader = new AppsFlyerDownloader();
            Drawer     = new AppsFlyerDrawer(this);
            Data       = new AppsFlyerConfigureData();
        }

        public bool IsAnalytic() { return true; }
    }
}