namespace FSDK
{
    public interface IPluginChecker
    {
        bool IsInstalled();
        void Check();
    }
}