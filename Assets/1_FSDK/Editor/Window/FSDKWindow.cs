namespace FSDK.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FSDK;
    using UnityEditor;
    using UnityEngine;

    public class FSDKWindow : EditorWindow
    {
        private const string Version = "v1.1";
        
        [MenuItem("FSDK/Settings", priority = 1)]
        private static void ShowWindow()
        {
            var window = GetWindow<FSDKWindow>("Falcon SDK", true);
            window.minSize = new Vector2(460, 1000);
            window.Show();
        }

        private Texture2D _infoIcon;
        private Texture2D _refreshIcon;

        private APlugin _analyticPlugin;
        private APlugin _adsPlugin;

        private int _adsPluginToSelect;
        private int _adsPluginSelected;

        private List<Type> _adsPlugins;
        private string[]   _adsPluginNames;

        private void OnEnable()
        {
            this.InitStyles();
            
            this._analyticPlugin = new AppsFlyerPlugin();
            this.InitAdsPlugin();
        }

        private void InitStyles()
        {
            this._infoIcon    = EditorGUIUtility.Load(@"Assets/1_FSDK/Editor/Icons/info-icon.png") as Texture2D;
            this._refreshIcon = EditorGUIUtility.Load(@"Assets/1_FSDK/Editor/Icons/refresh-icon.png") as Texture2D;
        }

        private void InitAdsPlugin()
        {
            this._adsPlugins = ReflectionUtils.GetAllDerivedTypes<IPlugin>()
                .Where(_ =>
                {
                    var method = _.GetMethod("IsAnalytic");
                    return method == null;
                }).ToList();

            this._adsPluginNames    = this._adsPlugins.Select(_ => _.Name).ToArray();
            this._adsPluginSelected = -1;
            
            string       configAsString = FileServices.ReadContentFromTextFile(EditorServices.EDITOR_CONFIG_PATH);
            EditorConfig config         = JsonUtility.FromJson<EditorConfig>(configAsString);
            if (config != null)
            {
                this._adsPluginSelected = this._adsPluginNames.ToList().IndexOf(config.SelectedPlugin);
            }

            if (this._adsPluginSelected >= 0)
            {
                this._adsPluginToSelect = this._adsPluginSelected;
                SelectAdsPluginByType();
            }
        }

        private void SelectAdsPluginByType()
        {
            var type = this._adsPlugins[this._adsPluginSelected];
            if (type == typeof(IronSourcePlugin))
            {
                this._adsPlugin = new IronSourcePlugin();
            }
            else if (type == typeof(MaxPlugin))
            {
                this._adsPlugin = new MaxPlugin();
            }
        }

        private void OnGUI()
        {
#if UNITY_2019_1_OR_NEWER
            EditorGUILayout.Space(15);
#else
            for (int i = 0; i < 3; i++)
            {
                EditorGUILayout.Space();
            }
#endif
            
            EditorGUILayout.BeginHorizontal("label");
            EditorGUILayout.LabelField("FALCON SDK", WindowStyle.BoldTextColorCustomSize(Color.white, 20));
            
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(this._refreshIcon))
            {
                this.Refresh();
            }
            
            if (GUILayout.Button(this._infoIcon))
            {
                this.OpenDocumentation();
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.LabelField(" " + Version);
            EditorGUILayout.Space();
            
            if (EditorApplication.isCompiling)
            {
#if UNITY_2019_1_OR_NEWER
                EditorGUILayout.Space(25);
#else
                for (int i = 0; i < 5; i++)
                {
                    EditorGUILayout.Space();
                }
#endif
                EditorGUILayout.LabelField("Editor is compiling...", WindowStyle.TextCenter);
                return;
            }
            
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField("Analytic Implementation", WindowStyle.BoldTextColorCustomSize(WindowStyle.Red, 13));
            
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Check", GUILayout.Width(70)))
            {
                this.CheckAnalyticPlugin();
            }
            
            EditorGUILayout.EndHorizontal();
            this._analyticPlugin.Drawer.Draw();

#if UNITY_2019_1_OR_NEWER
            EditorGUILayout.Space(1);
#else
            EditorGUILayout.Space();
#endif
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal("label");
            EditorGUILayout.LabelField("Ads Implementation", WindowStyle.BoldTextColorCustomSize(WindowStyle.Red, 13));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Check", GUILayout.Width(70)))
            {
                this.CheckAdsPlugin();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("(i) Ads Manager will automatically load", WindowStyle.TextColorCustomSize(WindowStyle.Blue, 11));
            
            EditorGUILayout.BeginHorizontal("label");
            this._adsPluginToSelect = EditorGUILayout.Popup("Select Ads Plugin:", this._adsPluginToSelect, this._adsPluginNames);

            if (GUILayout.Button("Select", GUILayout.Width(100)))
            {
                this.SelectAdsPlugin();
            }
            EditorGUILayout.EndHorizontal();

            if (this._adsPlugin != null && this._adsPlugin.Checker.IsInstalled())
            {
                EditorGUILayout.LabelField($"Plugin: {this._adsPlugin.Name} is being used!", WindowStyle.TextColor(WindowStyle.Red));
            }
            
            EditorGUILayout.EndVertical();
            
            if (this._adsPlugin != null)
            {
                this._adsPlugin.Drawer.Draw();
            }
            
#if UNITY_2019_1_OR_NEWER
            if (EditorUtility.IsDirty(this))
            {
                this.StorePluginsData();
            }
#else
            EditorGUILayout.Space();
            if (GUILayout.Button("Store Data"))
            {
                this.StorePluginsData();
            }
#endif
        }
        
        
        private void Refresh()
        {
            FSDKLoader.CheckPlugins();
        }
        
        private void OpenDocumentation()
        {
            Application.OpenURL(EditorServices.DOCUMENTATION_URL);
        }

        private void CheckAnalyticPlugin()
        {
            this._analyticPlugin.Checker.Check();
        }
        
        private void CheckAdsPlugin()
        {
            if (this._adsPlugin != null)
            {
                this._adsPlugin.Checker.Check();
            }
            else
            {
                EditorUtility.DisplayDialog("Warning", "Ads Plugin is not initialized!\n\nPlease select one to initialize and check again!", "OK");
            }
        }

        private void StorePluginsData()
        {
            this._analyticPlugin.Data.StoreData();
            if (this._adsPlugin != null)
            {
                this._adsPlugin.Data.StoreData();
            }
        }

        private void SelectAdsPlugin()
        {
            if (this._adsPluginToSelect != this._adsPluginSelected)
            {
                if (this._adsPlugin != null && this._adsPlugin.Checker.IsInstalled())
                {
                    if (EditorUtility.DisplayDialog("Warning", "Current plugin will be deleted!\n\nAction cannot be undone!", "Change Anyway"))
                    {
                        this.Save();
                        this._adsPlugin.Downloader.Remove();
                        this.SelectAdsPluginByType();    
                    }
                }
                else
                {
                    this.Save();
                    this.SelectAdsPluginByType();
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Aborted", "You are select the same Plugin", "OK");
            }
        }

        private void Save()
        {
            this._adsPluginSelected = this._adsPluginToSelect;
            string asJson = JsonUtility.ToJson(new EditorConfig()
            {
                SelectedPlugin = this._adsPluginNames[this._adsPluginSelected]
            }, true);
            FileServices.WriteContentToTextFile(EditorServices.EDITOR_CONFIG_PATH, asJson, false);
        }
    }
}