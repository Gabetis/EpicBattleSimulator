namespace FSDK
{
    using System.IO;
    using System.Linq;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;

    public class AppsFlyerDrawer : ADrawer
    {
        public AppsFlyerDrawer(APlugin plugin) : base(plugin) { }

        public override void DrawConfig()
        {
            base.DrawConfig();

            if ((bool)this.Plugin.Data.GetPropertyValue("ApplyToPrefab"))
            {
                EditorGUILayout.Space();
                if (GUILayout.Button("Apply Prefab"))
                {
                    this.Apply();
                }
            }

#if UNITY_2019_1_OR_NEWER
            EditorGUILayout.Space(1);
#else
            EditorGUILayout.Space();
#endif
            if (GUILayout.Button("Add Prefab to Scene"))
            {
                this.MoveToCurrentScene();
            }
        }

        private void Apply()
        {
#if EXISTED_APPS_FLYER
            var appsFlyerPath = Application.dataPath + @"/AppsFlyer/";
            var prefabs       = new DirectoryInfo(appsFlyerPath).GetFiles("*.prefab", SearchOption.AllDirectories);

            if (prefabs.Length > 0)
            {
                foreach (var prefabFile in prefabs)
                {
                    var assetPath = $"Assets/AppsFlyer/{prefabFile.Name}";
                    var prefab    = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

                    var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                    if(instance == null) continue;

                    var appsFlyer = instance.GetComponent<AppsFlyerObjectScript>();
                    appsFlyer.devKey   = (string)this.Plugin.Data.GetPropertyValue("DevKey");
                    appsFlyer.appID = (string)this.Plugin.Data.GetPropertyValue("IOSAppID");
                    
                    PrefabUtility.ApplyPrefabInstance(appsFlyer.gameObject, InteractionMode.AutomatedAction);
                    Object.DestroyImmediate(appsFlyer.gameObject);
                }

                AssetDatabase.Refresh();
            }
            else
            {
                EditorUtility.DisplayDialog("Failed", $"There are no valid prefab found at {appsFlyerPath}", "OK");
            }
#endif
        }

        private void MoveToCurrentScene()
        {
#if EXISTED_APPS_FLYER
            var appsFlyerPath = Application.dataPath + @"/AppsFlyer/";
            var prefabs       = new DirectoryInfo(appsFlyerPath).GetFiles("*.prefab", SearchOption.AllDirectories);

            if (prefabs.Length > 0)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/AppsFlyer/{prefabs[0].Name}");

                var scene = EditorSceneManager.GetActiveScene();
                if (scene.GetRootGameObjects().Any(_ => _.GetComponent<AppsFlyerObjectScript>() != null))
                {
                    if (EditorUtility.DisplayDialog("Warning", $"{scene.name} already have prefab.\n\nReplace it?", "Replace", "Cancel"))
                    {
                        var allAppsFlyerObjectScripts = scene.GetRootGameObjects().Where(_ => _.GetComponent<AppsFlyerObjectScript>() != null);
                        foreach (var app in allAppsFlyerObjectScripts)
                        {
                            Object.DestroyImmediate(app.gameObject);
                        }
                        
                        PrefabUtility.InstantiatePrefab(prefab, scene);
                        EditorSceneManager.MarkSceneDirty(scene);
                    }
                    return;
                }

                PrefabUtility.InstantiatePrefab(prefab, scene);
                EditorSceneManager.MarkSceneDirty(scene);
            }
            else
            {
                EditorUtility.DisplayDialog("Failed", $"There are no valid prefab found at {appsFlyerPath}", "OK");
            }
#endif
        }
    }
}