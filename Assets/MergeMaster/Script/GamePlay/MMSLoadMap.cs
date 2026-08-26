using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMSLoadMap : MonoBehaviour
{
    public static MMSLoadMap Instance;
    [SerializeField] MMSGameController gameController;
    [SerializeField] GameObject CurrentMap;
    [SerializeField] MMSMapInfoSystem MapInfoSystem;
    public int indexMap;
    private void Awake()
    {
        Instance = this;
    }

    
    public void LoadMap()
    {
        int index = CalculateIndexMap(); 

        if (index >= MapInfoSystem.MapInfos.Count)
        {
            index = index % (MapInfoSystem.MapInfos.Count-1);
        }

        var map = Resources.Load(MapInfoSystem.MapInfos[index].Address) as GameObject;

        gameController.OnUnlockNewMap(index);

        if (CurrentMap != null)
            Destroy(CurrentMap);

        CurrentMap = Instantiate(map, transform);
    }

    [Sirenix.OdinInspector.Button]
    public void LoadMap(int mapId)
    {
        int index = mapId;

        if (index >= MapInfoSystem.MapInfos.Count)
        {
            index = index % (MapInfoSystem.MapInfos.Count - 1);
        }

        var map = Resources.Load(MapInfoSystem.MapInfos[index].Address) as GameObject;

        if (CurrentMap != null)
            DestroyImmediate(CurrentMap);

        CurrentMap = Instantiate(map, transform);
    }

    public void LoadRandomMap()
    {
        int index = Random.Range(0, MapInfoSystem.MapInfos.Count-1);

        var map = Resources.Load(MapInfoSystem.MapInfos[index].Address) as GameObject;

        if (CurrentMap != null)
            Destroy(CurrentMap);

        CurrentMap = Instantiate(map, transform);
    }

    public int CalculateIndexMap()
    {
        indexMap = gameController.currentLevel;
        indexMap /= 10;
        return indexMap;
    }
}
