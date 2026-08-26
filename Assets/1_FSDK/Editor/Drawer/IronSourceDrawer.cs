namespace FSDK
{
    using System;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class IronSourceDrawer : ADrawer
    {
        public IronSourceDrawer(APlugin plugin) : base(plugin)
        {
            
        }

        public override void DrawConfig()
        {
            EditorGUILayout.BeginVertical("box");
            var props = this.Plugin.Data.GetProperties();
            foreach (var prop in props)
            {
#if UNITY_2019_1_OR_NEWER
                EditorGUILayout.Space(1);
#else
                EditorGUILayout.Space();
#endif
                if (!(bool)this.Plugin.Data.GetPropertyValue("UseBanner"))
                {
                    if(prop.Name.Equals("BannerPosition")) continue;
                }
                EditorServices.DisplayField(this.Plugin.Data, prop);
            }
            EditorGUILayout.EndVertical();

#if UNITY_2019_1_OR_NEWER
            EditorGUILayout.Space(1);
#else
            EditorGUILayout.Space();
#endif
            if (GUILayout.Button("Add Admob App ID to Android Manifest"))
            {
                this.AddAndroidAdmobAppID();
            }
            
#if UNITY_2019_1_OR_NEWER
            EditorGUILayout.Space(2);
#else
            EditorGUILayout.Space();
#endif
            if (GUILayout.Button("Open IronSource Manager"))
            {
                this.OpenIronSourceManager();
            }
        }
        

        private void AddAndroidAdmobAppID()
        {
#if EXISTED_IRON_SOURCE
            var ironSourcePath = Application.dataPath + @"/IronSource/";
            var files       = new DirectoryInfo(ironSourcePath).GetFiles("AndroidManifest.xml", SearchOption.AllDirectories);

            if (files.Length > 0)
            {
                var manifest = files[0];

                string content = FileServices.ReadContentFromTextFile(manifest.FullName);
                var split = content.Split(new [] { "<application>", "</application>" }, StringSplitOptions.RemoveEmptyEntries);
                
                split[1] = string.Format(EditorServices.ManifestApplicationTag, (string)this.Plugin.Data.GetPropertyValue("AndroidAdmobAppID"));

                string newContent = string.Empty;
                foreach (var part in split)
                {
                    newContent += part;
                }
                
                Debug.Log(newContent);
                using (var sw = new StreamWriter(manifest.FullName))
                {
                    sw.Write(newContent);
                    sw.Dispose();
                    sw.Close();
                }

                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("Info", $"AndroidManifest updated", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("Failed", $"There are no AndroidManifest found at {ironSourcePath}", "OK");
            }
#endif
        }

        private void OpenIronSourceManager()
        {
            EditorApplication.ExecuteMenuItem(@"IronSource/Integration Manager");
        }
    }
}