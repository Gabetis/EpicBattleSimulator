namespace FSDK
{
    public class IronSourcePlugin : APlugin
    {
        public IronSourcePlugin()
        {
            Name       = "IronSource SDK";
            Checker    = new IronSourceChecker();
            Downloader = new IronSourceDownloader();
            Drawer     = new IronSourceDrawer(this);
            Data       = new IronSourceConfigureData();
        }
    }
}