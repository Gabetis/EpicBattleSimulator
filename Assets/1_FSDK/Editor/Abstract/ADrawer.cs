namespace FSDK
{
    using UnityEditor;
    using UnityEngine;

    public abstract class ADrawer : IDrawable
    {
        protected APlugin Plugin;

        public ADrawer(APlugin plugin)
        {
            this.Plugin = plugin;
        }

        public virtual void Draw()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Plugin Info", WindowStyle.BoldTextColor(WindowStyle.Green));
            this.DrawInfo();
            EditorGUILayout.EndVertical();

            if (this.Plugin.Checker.IsInstalled())
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("Configure Data", WindowStyle.BoldTextColor(WindowStyle.Blue));
                this.DrawConfig();
                EditorGUILayout.EndVertical();
            }
        }

        public virtual void DrawInfo()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(this.Plugin.Name, WindowStyle.BoldTextColorCenterCustomSize(WindowStyle.Yellow, 15));
            EditorGUILayout.BeginHorizontal("box");
            if (this.Plugin.Checker.IsInstalled())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Delete", GUILayout.Width(70)))
                {
                    this.Delete();
                }
                
                if (GUILayout.Button("Refresh",GUILayout.Width(70)))
                {
                    this.Refresh();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical("box");
            if (this.Plugin.Downloader.IsGettingLatestInfo)
            {
                EditorGUILayout.LabelField("Getting latest version info...");
            }
            else
            {
                string currentVersion = this.Plugin.Downloader.CurrentVersion;
                if (string.IsNullOrEmpty(currentVersion))
                {
                    currentVersion = "---";
                }
                
                EditorGUILayout.LabelField($"Current Version : {currentVersion}");
                EditorGUILayout.LabelField($"Latest Version   : {this.Plugin.Downloader.LatestVersion}");
            }

#if UNITY_2019_1_OR_NEWER
            EditorGUILayout.Space(5);
#else
            EditorGUILayout.Space();
#endif
            if (this.Plugin.Checker.IsInstalled())
            {
                if (!this.Plugin.Downloader.IsLatest())
                {
                    if (GUILayout.Button("Update"))
                    {
                        this.UpdateLatest();
                    }
                }
            }
            else
            {
                if (GUILayout.Button("Install"))
                {
                    this.UpdateLatest();
                }
            }
            EditorGUILayout.EndVertical();
        }

        protected void Delete()
        {
            if (EditorUtility.DisplayDialog("Warning", $"Delete EVERYTHING from {this.Plugin.Name}?\n\nAction cannot be undone!", "Delete"))
            {
                this.Plugin.Downloader.Remove();
            }
        }

        protected void Refresh()
        {
            this.Plugin.Downloader.GetLatestInfo();
        }

        protected void UpdateLatest()
        {
            if (!this.Plugin.Downloader.IsGettingLatestInfo)
            {
                this.Plugin.Downloader.InstallLatest();
            }
            else
            {
                if (EditorUtility.DisplayDialog("Aborted", "Getting latest info, please wait!", "Refresh"))
                {
                    this.Refresh();
                }
            }
        }

        public virtual void DrawConfig()
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
                EditorServices.DisplayField(this.Plugin.Data, prop);
            }
            EditorGUILayout.EndVertical();
        }
    }
}