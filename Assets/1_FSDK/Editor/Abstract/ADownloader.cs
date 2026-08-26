namespace FSDK
{
    using System.IO;
    using System.Threading.Tasks;
    using FSDK.GitDownload;
    using UnityEditor;
    using UnityEngine;

    public abstract class ADownloader : IDownloadable
    {
        protected string   PackageName;
        protected string   GitURL;
        protected string   DownloadedCachePath;
        protected string[] DirectoriesToRemove;
        protected string[] ZipDirectoriesToImport;
        protected string   Extension;

        public bool   IsGettingLatestInfo;
        public string CurrentVersion;
        public string LatestVersion;


        protected string      ImportCachePath;
        protected ReleaseJson ReleaseJson;

        protected void Init()
        {
            this.SetupCachedPaths();
            this.GetDownloadInfo();
        }

        protected void SetupCachedPaths()
        {
            this.ImportCachePath     = Application.dataPath.Replace("Assets", "Temp") + $"/Download/{this.PackageName}";
            this.DownloadedCachePath = this.ImportCachePath + $"/{this.PackageName}{this.Extension}";
        }

        private void GetDownloadInfo()
        {
            this.GetCurrentInfo();
            this.GetLatestInfo();
        }

        protected void GetCurrentInfo()
        {
            this.CurrentVersion = EditorServices.GetPluginVersion(this.PackageName);
        }

        public void GetLatestInfo()
        {
            this.IsGettingLatestInfo = true;
            
            var parser = GitHelper.ParseURL(this.GitURL);
            GitHelper.GetResponse(parser[0], parser[1], this.OnResponse);
        }

        private void OnResponse(string content)
        {
            this.ReleaseJson         = JsonUtility.FromJson<ReleaseJson>(content);
            this.LatestVersion       = this.ReleaseJson?.tag_name;
            this.IsGettingLatestInfo = false;
        }

        public bool IsLatest()
        {
            if (string.IsNullOrEmpty(this.LatestVersion)) return true;
            if (string.IsNullOrEmpty(this.CurrentVersion)) return true;
            
            return this.CurrentVersion.Equals(this.LatestVersion);
        }
        
        public virtual async void RemoveOldArtifacts()
        {
            foreach (var dir in this.DirectoriesToRemove)
            {
                string fullPath = Application.dataPath + "/" + dir;
                if (Directory.Exists(fullPath))
                {
                    AssetDatabase.DeleteAsset($"Assets/{dir}");
                    await Task.Yield();
                }
            }
        }

        public abstract void InstallLatest();
        
        public void Remove()
        {
            EditorServices.DisplayProgressBar("Remove Package", "Removing...", 0f);
            EditorServices.RemovePluginVersion(this.PackageName);
            this.RemoveOldArtifacts();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            EditorServices.DisplayProgressBar("Remove Package", "Removed", 1f);
        }

        protected void UpdateVersion()
        {
            this.CurrentVersion = this.LatestVersion;
            EditorServices.SetPluginVersion(this.PackageName, this.LatestVersion);
        }
    }
}