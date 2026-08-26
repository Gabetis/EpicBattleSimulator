namespace FSDK.GitDownload
{
    using UnityEngine;
    using System;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using static System.IO.Path;

    public static class GitHelper
    {
        const string GITHUB_URL = "https://api.github.com/repos";


        public static string[] ParseURL(string gitURL)
        {
            if (string.IsNullOrEmpty(gitURL)) return null;
            var      m  = Regex.Matches(gitURL, @"^(https://github.com)/(.*)");
            string[] ss = m[0].Groups[2].Value.Split('/');

            return new string[] { ss[0], GetFileNameWithoutExtension(ss[1]) };
        }

        public static string ParseURLToPopup(string gitURL)
        {
            var ss = ParseURL(gitURL);
            return $"{ss[0]} : {ss[1]}";
        }

        public static void GetResponse(string name, string repoName, Action<string> responseAction = null)
        {
            try
            {
                using (var wc = new WebClient())
                {
                    var url = $"{GITHUB_URL}/{name}/{repoName}/releases/latest";

                    wc.Headers.Add("User-Agent", "Nothing");
                    wc.Credentials = CredentialCache.DefaultCredentials;
                    wc.DownloadStringCompleted += (sender, e) =>
                    {
                        var content = e.Result;
                        if (string.IsNullOrEmpty(content)) return;

                        responseAction?.Invoke(content);
                    };
                    wc.DownloadStringAsync(new Uri(url));
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static async void DownloadFile(string url, string outputDirectory, string name, string extension, Action<bool> OnComplete)
        {
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
                await Task.Yield();
            }
            
            Directory.CreateDirectory(outputDirectory);

            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers.Add("User-Agent", "Nothing");
                    wc.DownloadProgressChanged += (sender, e) =>
                    {
                        
                    };
                    wc.DownloadFileCompleted += (sender, e) =>
                    {
                        OnComplete?.Invoke(true);
                    };
                    wc.DownloadFileAsync(new Uri(url), outputDirectory + $"/{name}{extension}");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                OnComplete?.Invoke(false);
            }
        }
    }
}