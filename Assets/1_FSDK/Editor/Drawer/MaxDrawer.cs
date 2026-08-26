namespace FSDK
{
    using System;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class MaxDrawer : ADrawer
    {
        public MaxDrawer(APlugin plugin) : base(plugin)
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
            if (GUILayout.Button("Add SDK Key to Manager"))
            {
                this.AddSDKKey();
            }
            
#if UNITY_2019_1_OR_NEWER
            EditorGUILayout.Space(2);
#else
            EditorGUILayout.Space();
#endif
            if (GUILayout.Button("Add Admob App ID"))
            {
                this.AddAndroidAdmobAppID();
                this.AddToMaxSettings();
            }
            
#if UNITY_2019_1_OR_NEWER
            EditorGUILayout.Space(2);
#else
            EditorGUILayout.Space();
#endif
            if (GUILayout.Button("Open Max Manager"))
            {
                this.OpenManager();
            }
        }
        
        private void AddAndroidAdmobAppID()
        {
#if EXISTED_MAX_SDK
            var maxMediationPath = Application.dataPath + @"/Plugins/Android/MaxMediationGoogle.androidlib/";
            if (!Directory.Exists(maxMediationPath))
            {
                EditorUtility.DisplayDialog("Failed", $"Path: '{maxMediationPath}' is not found", "OK");
                return;
            }

            var files = new DirectoryInfo(maxMediationPath).GetFiles("AndroidManifest.xml", SearchOption.AllDirectories);

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
                EditorUtility.DisplayDialog("Failed", $"There are no AndroidManifest found at {maxMediationPath}", "OK");
            }
#endif
        }

        private void AddToMaxSettings()
        {
#if EXISTED_MAX_SDK
            var maxMediationPath = Application.dataPath + @"/MaxSdk/Resources/";
            if (!Directory.Exists(maxMediationPath))
            {
                EditorUtility.DisplayDialog("Failed", $"Path: '{maxMediationPath}' is not found", "OK");
                return;
            }

            var files = new DirectoryInfo(maxMediationPath).GetFiles("*.asset");
            if (files.Length > 0)
            {
                var asset = AssetDatabase.LoadAssetAtPath<AppLovinSettings>(@"Assets/MaxSdk/Resources/" + files[0].Name);
                if (asset != null)
                {
                    asset.AdMobAndroidAppId = this.Plugin.Data.GetPropertyValue("AndroidAdmobAppID") as string;
                    asset.AdMobIosAppId     = this.Plugin.Data.GetPropertyValue("IOSAdmobAppID") as string;
                    AssetDatabase.SaveAssets();
                    EditorUtility.DisplayDialog("Info", $"Add Admob ID to MaxSettings Succeeded", "OK");
                }
                else
                {
                    EditorUtility.DisplayDialog("Failed", $"Settings asset NULL", "OK");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Failed", $"There are no Settings asset found at {maxMediationPath}", "OK");
            }
#endif
        }

        private void AddSDKKey()
        {
#if EXISTED_MAX_SDK
            var maxMediationPath = Application.dataPath + @"/MaxSdk/Resources/";
            if (!Directory.Exists(maxMediationPath))
            {
                EditorUtility.DisplayDialog("Failed", $"Path: '{maxMediationPath}' is not found", "OK");
                return;
            }

            var files = new DirectoryInfo(maxMediationPath).GetFiles("*.asset");
            if (files.Length > 0)
            {
                var asset = AssetDatabase.LoadAssetAtPath<AppLovinSettings>(@"Assets/MaxSdk/Resources/" + files[0].Name);
                if (asset != null)
                {
                    asset.SdkKey = this.Plugin.Data.GetPropertyValue("SDKKey") as string;
                    AssetDatabase.SaveAssets();
                    EditorUtility.DisplayDialog("Info", $"Add SDKKey Succeeded", "OK");
                }
                else
                {
                    EditorUtility.DisplayDialog("Failed", $"Settings asset NULL", "OK");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Failed", $"There are no Settings asset found at {maxMediationPath}", "OK");
            }
#endif
        }
        
        private void OpenManager()
        {
            EditorApplication.ExecuteMenuItem(@"AppLovin/Integration Manager");
        }
    }
}