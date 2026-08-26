namespace FSDK
{
    using System.Threading.Tasks;
    using FSDK.GitDownload;

    public class IronSourceDownloader : ADownloader
    {
        public IronSourceDownloader()
        {
            this.PackageName = "IronSource";
            this.GitURL     = @"https://github.com/ironsource-mobile/Unity-sdk.git";
            this.Extension  = @".unitypackage";

            this.DirectoriesToRemove = new[]
            {
                "IronSource",
                "ExternalDependencyManager",
            };
            
            this.Init();
        }

        public override async void InstallLatest()
        {
            EditorServices.DisplayProgressBar($"Install {this.PackageName}", "Removing old artifacts...", 0f);
            this.RemoveOldArtifacts();
            await Task.Yield();
            EditorServices.DisplayProgressBar($"Install {this.PackageName}", "Downloading package...", 0.33f);
            GitHelper.DownloadFile(this.ReleaseJson.assets[0].browser_download_url, this.ImportCachePath, this.PackageName, this.Extension, this.OnComplete);
        }

        public override void RemoveOldArtifacts()
        {
            base.RemoveOldArtifacts();
            EditorServices.RemoveDefine(EditorServices.EXIST_IRON_SOURCE);
        }

        private void OnComplete(bool success)
        {
            if (success)
            {
                EditorServices.DisplayProgressBar($"Install {this.PackageName}", "Installing package...", 0.67f);
                PackageInstaller.InstallUnityPackage(this.DownloadedCachePath);
                EditorServices.DisplayProgressBar($"Install {this.PackageName}", "Package Installed", 1f);
                this.UpdateVersion();
            }
            else
            {
                EditorServices.ClearProgressBarWhenTaskFailed($"{this.PackageName}: Download Failed");
            }
        }
    }
}