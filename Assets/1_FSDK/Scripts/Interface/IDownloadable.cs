namespace FSDK
{
    public interface IDownloadable
    {
        bool IsLatest();
        void RemoveOldArtifacts();
        void InstallLatest();
        void Remove();
    }
}