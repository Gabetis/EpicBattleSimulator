namespace FSDK
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    public static class EditorServices
    {
        public const string DOCUMENTATION_URL = "https://docs.google.com/document/d/1lCqQWfPbGww3xBOAwd6GwwyOE0-d_442A8fi31HVaNE/edit?usp=sharing";
        
        public static  string EDITOR_CONFIG_PATH   = Application.dataPath + @"/1_FSDK/Blueprints/editor_conf.json";
        private static string PLUGINS_VERSION_PATH = Application.dataPath + @"/1_FSDK/Blueprints/plugins_version.txt";

        public const string EXIST_IRON_SOURCE = "EXISTED_IRON_SOURCE";
        public const string EXIST_APPS_FLYER  = "EXISTED_APPS_FLYER";
        public const string EXIST_MAX_SDK     = "EXISTED_MAX_SDK";
        
        public const string EXIST_PURCHASING  = "EXISTED_PURCHASING";
        public const string EXIST_EASY_MOBILE = "EXISTED_EASY_MOBILE";
        
        public const string ManifestApplicationTag = 
   @"<application>

        <meta-data 
            android:name=""com.google.android.gms.ads.APPLICATION_ID""
            android:value=""{0}""/>
                
    </application>";
        
        private static readonly BuildTargetGroup[] AllPlatform =
        {
            BuildTargetGroup.Standalone, 
            BuildTargetGroup.Android, 
            BuildTargetGroup.iOS
        };
        
        
        public static void AddDefine(string symbol)
        {
            foreach (var platform in AllPlatform)
            {
                string       symbolStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(platform);
                List<string> symbols   = new List<string>(symbolStr.Split(';'));

                if (!symbols.Contains(symbol))
                {
                    symbols.Add(symbol);
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(platform, string.Join(";", symbols));
                }
            }
        }
        
        public static void RemoveDefine(string symbol)
        {
            foreach (var platform in AllPlatform)
            {
                string       symbolStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(platform);
                List<string> symbols   = new List<string>(symbolStr.Split(';'));

                if (symbols.Contains(symbol))
                {
                    symbols.Remove(symbol);
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(platform, string.Join(";", symbols));
                }
            }
        }

        public static void DisplayProgressBar(string title, string info, float progress)
        {
            EditorUtility.DisplayProgressBar(title, info, progress);
            if (progress >= 1f)
            {
                EditorUtility.ClearProgressBar();
            }
        }

        public static void ClearProgressBarWhenTaskFailed(string content)
        {
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("Task Failed", content,  "OK");
        }

        public static string GetPluginVersion(string pluginName)
        {
            var versions = FileServices.ReadContentFromTextFile(PLUGINS_VERSION_PATH, "|");
            if (string.IsNullOrEmpty(versions)) return string.Empty;

            var split = versions.Split('|');
            foreach (var str in split)
            {
                if (str.StartsWith(pluginName))
                {
                    int index = str.IndexOf(':');
                    return str.Substring(index + 2);
                }
            }

            return string.Empty;
        }

        public static void SetPluginVersion(string pluginName, string version)
        {
            string content = $"{pluginName}: {version}";
            FileServices.WriteContentToTextFile(PLUGINS_VERSION_PATH, content, true);
        }

        public static void RemovePluginVersion(string pluginName)
        {
            var versions = FileServices.ReadContentFromTextFile(PLUGINS_VERSION_PATH, "|");
            if (string.IsNullOrEmpty(versions)) return;
      
            var split = versions.Split('|');
            using (var sw = new StreamWriter(PLUGINS_VERSION_PATH, false))
            {
                foreach (var str in split)
                {
                    if (string.IsNullOrEmpty(str)) continue;
                    if (!str.StartsWith(pluginName))
                    {
                        sw.WriteLine(str);
                    }
                }
                
                sw.Dispose();
                sw.Close();
            }
        }

        public static void DisplayField(object instance, PropertyInfo info)
        {
            if (info.PropertyType == typeof(System.String))
            {
                info.SetValue(instance,EditorGUILayout.TextField(info.Name, (string) info.GetValue(instance)));
            }
            else if (info.PropertyType == typeof(System.Int32))
            {
                info.SetValue(instance,EditorGUILayout.IntField(info.Name, (int) info.GetValue(instance)));
            }
            else if (info.PropertyType == typeof(System.Single))
            {
                info.SetValue(instance,EditorGUILayout.FloatField(info.Name, (float) info.GetValue(instance)));
            }
            else if (info.PropertyType == typeof(System.Boolean))
            {
                info.SetValue(instance,EditorGUILayout.Toggle(info.Name, (bool) info.GetValue(instance)));
            }
            else if (info.PropertyType.IsEnum)
            {
                info.SetValue(instance, EditorGUILayout.EnumPopup(info.Name, (System.Enum) info.GetValue(instance)));
            }
            
        }
    }
}