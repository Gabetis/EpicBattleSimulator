namespace FSDK
{
    public abstract class APlugin : IPlugin
    {
        public string Name;

        public APluginChecker Checker    { get; set; }
        public ADownloader    Downloader { get; set; }
        public ADrawer        Drawer     { get; set; }
        
        public IConfigureData Data       { get; set; }
    }
}