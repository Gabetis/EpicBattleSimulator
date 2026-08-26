namespace FSDK.Editor
{
    using System;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class FSDKUtilsWindow : EditorWindow
    {
        private const string MANIFEST_PATH        = @"/Plugins/Android/AndroidManifest.xml";
        private const string MAIN_GRADLE_PATH     = @"/Plugins/Android/mainTemplate.gradle";
        private const string LAUNCHER_GRADLE_PATH = @"/Plugins/Android/launcherTemplate.gradle";
        
        private const string NETWORK_SECURITY_CONF_PATH = @"/1_FSDK/Editor/Template/network_security_config.xml";
        private const string BUILD_SCRIPT_PATH          = @"/1_FSDK/Editor/Template/buildscript.txt";
        private const string BUILD_SCRIPT_JETIFIER_PATH = @"/1_FSDK/Editor/Template/buildscript_jetifier.txt";
        private const string BUILD_BUNDLE_PATH          = @"/1_FSDK/Editor/Template/buildbundle.txt";

        
        private const string MULTI_DEX_IMPL        = @"    implementation 'androidx.multidex:multidex:2.0.1'";
        private const string MULTI_DEX_ENABLE      = @"        multiDexEnabled true";
        private const string MULTI_DEX_MANIFEST    = @"android:name=""androidx.multidex.MultiDexApplication""";
        private const string NETWORK_SECURITY_CONF = @"android:networkSecurityConfig=""@xml/network_security_config""";
        private const string RESIZEABLE_MANIFEST   = @"android:resizeableActivity=""false""";
        
        [MenuItem("FSDK/Utils", priority = 2)]
        private static void ShowWindow()
        {
            var window = GetWindow<FSDKUtilsWindow>("Falcon Utils", true);
            window.minSize = new Vector2(350, 200);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
#if !UNITY_ANDROID
            GUILayout.Label("Switch your project to Android");
            return;
#endif
            
            EditorGUILayout.LabelField("(i) These fix require NDK version 5.6.4 or higher to work", WindowStyle.BoldTextColorCenterCustomSize(WindowStyle.Blue, 11));
            EditorGUILayout.LabelField("(i) These fix may cause the build fail. You need to fix yourself", WindowStyle.BoldTextColorCenterCustomSize(WindowStyle.Blue, 11));
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Add BuildScript to Main Gradle"))
            {
                this.FixMainTemplateGradle();
            }
            
#if UNITY_2019_1_OR_NEWER
            if (GUILayout.Button("Add BuildScript to Launcher Gradle"))
            {
                this.FixLauncherGradle();
            }
            
            if (GUILayout.Button("Fix Export App Bundle"))
            {
                this.FixBundle();
            }
#endif
            
            if (GUILayout.Button("Enable MultiDex"))
            {
                this.EnableMultiDex();
            }
            
            if (GUILayout.Button("Add Network Security"))
            {
                this.AddNetworkSecurityConfig();
            }
            
            if (GUILayout.Button("Add common setup for AndroidManifest"))
            {
                this.AddCommonStuffToManifest();
            }
        }

        private void FixMainTemplateGradle()
        {
            var path = Application.dataPath + MAIN_GRADLE_PATH;
            if (File.Exists(path))
            {
                string gradle = FileServices.ReadContentFromTextFile(path);
                if (!IsExistTextIn(gradle, "buildscript"))
                {
#if UNITY_2019_1_OR_NEWER
                    string script = FileServices.ReadContentFromTextFile(Application.dataPath + BUILD_SCRIPT_PATH);
#else
                    string script = FileServices.ReadContentFromTextFile(Application.dataPath + BUILD_SCRIPT_JETIFIER_PATH);
#endif
                    string newGradle = script + gradle;
                    FileServices.WriteContentToTextFile(path, newGradle.Trim(), false);
                    EditorUtility.DisplayDialog("Information", $"Build script added to mainTemplate", "OK");
                }
                else
                {
                    EditorUtility.DisplayDialog("Aborted", "Already added", "OK");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Warning", "There are no mainTemplate.gradle, please create first in PlayerSettings", "OK");
            }
        }

        private void FixLauncherGradle()
        {
            var path = Application.dataPath + LAUNCHER_GRADLE_PATH;
            if (File.Exists(path))
            {
                string gradle = FileServices.ReadContentFromTextFile(path);
                if (!IsExistTextIn(gradle, "buildscript"))
                {
                    string script = FileServices.ReadContentFromTextFile(Application.dataPath + BUILD_SCRIPT_PATH);

                    string newGradle = script + gradle;
                    FileServices.WriteContentToTextFile(path, newGradle.Trim(), false);
                    EditorUtility.DisplayDialog("Information", $"Build script added to launcherTemplate", "OK");
                }
                else
                {
                    EditorUtility.DisplayDialog("Aborted", "Already added", "OK");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Warning", "There are no launcherTemplate.gradle, please create first in PlayerSettings", "OK");
            }
        }
        
        private void FixBundle()
        {
            var path = Application.dataPath + LAUNCHER_GRADLE_PATH;
            if (File.Exists(path))
            {
                string gradle = FileServices.ReadContentFromTextFile(path);
                if (!IsExistTextIn(gradle, "tasks.whenTaskAdded"))
                {
                    string script = FileServices.ReadContentFromTextFile(Application.dataPath + BUILD_BUNDLE_PATH);

                    string newGradle = gradle + script;
                    FileServices.WriteContentToTextFile(path, newGradle.Trim(), false);
                    EditorUtility.DisplayDialog("Information", $"Bundle fixed", "OK");
                }
                else
                {
                    EditorUtility.DisplayDialog("Aborted", "Already added", "OK");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Warning", "There are no launcherTemplate.gradle, please create first in PlayerSettings", "OK");
            }
        }

        private void EnableMultiDex()
        {
            this.HandleMultiDexInMainTemplate();
            
#if UNITY_2019_1_OR_NEWER
            this.HandleMultiDexInLauncherTemplate();
#endif
            this.HandleManifest();
            EditorUtility.DisplayDialog("Information", $"Please check mainTemplate, launcherTemplate (if have) and AndroidManifest", "OK");
        }

        private void HandleMultiDexInMainTemplate()
        {
            var path = Application.dataPath + MAIN_GRADLE_PATH;
            if (File.Exists(path))
            {
                string gradle = FileServices.ReadContentFromTextFile(path);
                if (!IsExistTextIn(gradle, MULTI_DEX_IMPL))
                {
                    gradle = NewImplementation(gradle);
                }
                
#if !UNITY_2019_1_OR_NEWER
                if (!IsExistTextIn(gradle, MULTI_DEX_ENABLE))
                {
                    gradle = NewDefaultConfig(gradle);
                }
#endif
                FileServices.WriteContentToTextFile(path, gradle.Trim(), false);
            }
            else
            {
                EditorUtility.DisplayDialog("Warning", "There are no launcherTemplate.gradle, please create first in PlayerSettings", "OK");
            }
        }
        
        private string NewImplementation(string gradle)
        {
            var split = gradle.Split(new[] { @"dependencies {" }, StringSplitOptions.RemoveEmptyEntries);

            int index = 0;
            for (int i = 0; i < split.Length; i++)
            {
                var str = split[i];
                if (str.Trim().StartsWith("implementation"))
                {
                    index = i;
                    break;
                }
            }

            string newItem   = MULTI_DEX_IMPL + split[index];
            string newGradle = string.Empty;
            for (int i = 0; i < split.Length; i++)
            {
                var str = split[i].Trim();
                if (i == index)
                {
                    str = newItem;
                }

                if (i > 0)
                {
                    str = Environment.NewLine + @"dependencies {" + Environment.NewLine + str;
                }

                newGradle += str + Environment.NewLine;
            }

            Debug.Log(newGradle);
            return newGradle;
        }

        
        private void HandleMultiDexInLauncherTemplate()
        {
            var path = Application.dataPath + LAUNCHER_GRADLE_PATH;
            if (File.Exists(path))
            {
                string gradle = FileServices.ReadContentFromTextFile(path);
                
                if (!IsExistTextIn(gradle, MULTI_DEX_ENABLE))
                {
                    gradle = NewDefaultConfig(gradle);
                    FileServices.WriteContentToTextFile(path, gradle.Trim(), false);
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Warning", "There are no launcherTemplate.gradle, please create first in PlayerSettings", "OK");
            }
        }

        private void HandleManifest()
        {
            var path = Application.dataPath + MANIFEST_PATH;
            if (File.Exists(path))
            {
                string manifest = FileServices.ReadContentFromTextFile(path);
                
                if (!IsExistTextIn(manifest, MULTI_DEX_MANIFEST))
                {
                    manifest = this.NewManifest(manifest, MULTI_DEX_MANIFEST);
                    FileServices.WriteContentToTextFile(path, manifest.Trim(), false);
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Warning", "There are no AndroidManifest.xml, please create first in PlayerSettings", "OK");
            }
        }
        
        private string NewDefaultConfig(string gradle)
        {
            var split = gradle.Split(new[] { @"defaultConfig {" }, StringSplitOptions.RemoveEmptyEntries);

            string newItem   = MULTI_DEX_ENABLE + split[1];
            string newGradle = split[0] + Environment.NewLine + @"defaultConfig {" + Environment.NewLine + newItem;

            Debug.Log(newGradle);
            return newGradle;
        }
        
        private string NewManifest(string manifest, string contentToAdd, bool isAppendFirst = true)
        {
            var split = manifest.Split(new[] { @"<application" }, StringSplitOptions.RemoveEmptyEntries);

            string newItem     = string.Empty;
            if (isAppendFirst)
            {
                newItem = contentToAdd + split[1];
            }
            else
            {
                newItem = split[1] + contentToAdd;
            }
            
            string newManifest = split[0] + Environment.NewLine + @"<application" + Environment.NewLine + newItem;

            Debug.Log(newManifest);
            return newManifest;
        }


        private void AddNetworkSecurityConfig()
        {
            this.AddNetworkSecurityConfigFile();
            this.AddNetworkSecurityConfigToManifest();
            
            EditorUtility.DisplayDialog("Information", @"Please check AndroidManifest and folder: /Plugins/Android/res/xml/", "OK");
        }

        private void AddNetworkSecurityConfigFile()
        {
            if (!File.Exists(Application.dataPath + @"/Plugins/Android/res/xml/network_security_config.xml"))
            {
                var path = Application.dataPath + @"/Plugins/Android/res/xml";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var file = new FileInfo(Application.dataPath + NETWORK_SECURITY_CONF_PATH);
                if (file.Exists)
                {
                    file.CopyTo(Path.Combine(path, file.Name), true);
                    AssetDatabase.Refresh();
                }
            }
        }

        private void AddNetworkSecurityConfigToManifest()
        {
            var path = Application.dataPath + MANIFEST_PATH;
            if (File.Exists(path))
            {
                string manifest = FileServices.ReadContentFromTextFile(path);
                
                if (!IsExistTextIn(manifest, NETWORK_SECURITY_CONF))
                {
                    manifest = this.NewManifest(manifest, NETWORK_SECURITY_CONF);
                    FileServices.WriteContentToTextFile(path, manifest.Trim(), false);
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Warning", "There are no AndroidManifest.xml, please create first in PlayerSettings", "OK");
            }
        }
        
        private void AddCommonStuffToManifest()
        {
            this.AddResizableFeature();
        }

        private void AddResizableFeature()
        {
            var path = Application.dataPath + MANIFEST_PATH;
            if (File.Exists(path))
            {
                string manifest = FileServices.ReadContentFromTextFile(path);
                
                if (!IsExistTextIn(manifest, RESIZEABLE_MANIFEST))
                {
                    manifest = this.NewManifest(manifest, RESIZEABLE_MANIFEST);
                    FileServices.WriteContentToTextFile(path, manifest.Trim(), false);
                    
                    EditorUtility.DisplayDialog("Information", "Added Resizable Off feature", "OK");
                }
                else
                {
                    EditorUtility.DisplayDialog("Aborted", "Already added", "OK");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Warning", "There are no AndroidManifest.xml, please create first in PlayerSettings", "OK");
            }
        }

        private bool IsExistTextIn(string content, string textToCheck)
        {
            return content.Contains(textToCheck.Trim());
        }
    }
}