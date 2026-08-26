#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class EditorTracking
{
    [MenuItem("KR/Tracking")]
    public static void Tracking()
    {
        GameObject go = new GameObject();
        go.name = "==== Tracking ====";
        go.transform.SetAsLastSibling();
        go.transform.position = Vector3.zero;
        go.AddComponent<AppTracking>();
        EditorUtility.SetDirty(go);
    }

}

#endif
