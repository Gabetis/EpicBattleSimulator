namespace FSDK
{
    using System.Threading.Tasks;
    using FSDK.GitDownload;

    public class AppsFlyerDownloader : ADownloader
    {
        public AppsFlyerDownloader()
        {
            this.PackageName = "AppsFlyer";
            this.GitURL      = @"https://github.com/AppsFlyerSDK/appsflyer-unity-plugin.git";
            this.Extension   = @".zip";

            this.DirectoriesToRemove = new[]
            {
                "AppsFlyer",
                "ExternalDependencyManager",
                "PlayServicesResolver"
            };
            
            this.Init();
        }

        public override async void InstallLatest()
        {
            EditorServices.DisplayProgressBar($"Install {this.PackageName}", "Removing old artifacts...", 0f);
            this.RemoveOldArtifacts();
            await Task.Yield();
            EditorServices.DisplayProgressBar($"Install {this.PackageName}", "Downloading package...", 0.33f);
            GitHelper.DownloadFile(this.ReleaseJson.zipball_url, this.ImportCachePath, this.PackageName, this.Extension, this.OnComplete);
        }
        
        public override void RemoveOldArtifacts()
        {
            base.RemoveOldArtifacts();
            EditorServices.RemoveDefine(EditorServices.EXIST_APPS_FLYER);
        }

        private async void OnComplete(bool success)
        {
            if (success)
            {
                EditorServices.DisplayProgressBar($"Install {this.PackageName}", "Installing package...", 0.67f);
                bool result = await PackageInstaller.UnzipAndInstall(this.DownloadedCachePath, this.ZipDirectoriesToImport);
                if (result)
                {
                    EditorServices.DisplayProgressBar($"Install {this.PackageName}", "Package Installed", 1f);
                    this.UpdateVersion();
                }
                else
                {
                    EditorServices.ClearProgressBarWhenTaskFailed($"{this.PackageName}: Install Failed");
                }
            }
            else
            {
                EditorServices.ClearProgressBarWhenTaskFailed($"{this.PackageName}: Download Failed");
            }
        }
    }
}