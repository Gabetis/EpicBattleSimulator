namespace FSDK
{
    public class MaxPlugin : APlugin
    {
        public MaxPlugin()
        {
            Name       = "Applovin Max SDK";
            Checker    = new MaxChecker();
            Downloader = new MaxDownloader();
            Drawer     = new MaxDrawer(this);
            Data       = new MaxConfigureData();
        }
    }
}