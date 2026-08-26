namespace FSDK
{
    using System.IO;
    using System.Threading.Tasks;
    using SevenZipExtractor;
    using UnityEngine;

    public static class PackageInstaller
    {
        public static void InstallUnityPackage(string packagePath)
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.ImportPackage(packagePath, false);      
#endif
        }

        public static async Task<bool> UnzipAndInstall(string zipPath, string[] zipDirectoriesToImport)
        {
            var fileInfo     = new FileInfo(zipPath);
            var extractedDir = Directory.CreateDirectory(fileInfo.DirectoryName + @"/Extract-" + fileInfo.Name);

            using (ArchiveFile archiveFile = new ArchiveFile(zipPath))
            {
                archiveFile.Extract(extractedDir.FullName, true);
            }

            var files = extractedDir.GetFiles("*.unitypackage", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                InstallUnityPackage(files[0].FullName);
                return true;
            }

            if (zipDirectoriesToImport.Length > 0)
            {
                var subDirectories = extractedDir.GetDirectories();
                if (subDirectories.Length == 0) return false;

                int counter = 0;
                foreach (var dir in zipDirectoriesToImport)
                {
                    var fullPath = extractedDir.FullName + $"/{subDirectories[0].Name}/{dir}";
                    if (Directory.Exists(fullPath))
                    {
                        var splitDir  = dir.Split('/');
                        var importDir = Application.dataPath + "/" + splitDir[splitDir.Length - 1];
                        Copy(fullPath, importDir);
                        counter++;
                        await Task.Yield();
                    }
                }

                return counter == zipDirectoriesToImport.Length;
            }
                
            return false;
        }
        
        
        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}