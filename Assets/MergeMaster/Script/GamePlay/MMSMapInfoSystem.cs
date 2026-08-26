using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenuAttribute(fileName = "MapInfoSystem", menuName = "Data/Scriptable/Map Info System")]
public class MMSMapInfoSystem : ScriptableObject
{
    public List<MMSMapInfo> MapInfos;

    public MMSMapInfo GetMapInfo(int index)
    {
        MMSMapInfo info = MapInfos[index];
        return info;
    }
}
