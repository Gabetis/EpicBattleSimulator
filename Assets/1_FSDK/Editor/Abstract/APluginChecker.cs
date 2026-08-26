namespace FSDK
{
    public abstract class APluginChecker : IPluginChecker
    {
        public abstract bool IsInstalled();
        public abstract void Check();
    }
}