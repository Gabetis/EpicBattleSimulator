using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMSLevelState : DTNView
{
    public MMSGameController gameController;
    public MMSMapInfoSystem MapInfoSystem;

    public Image CurrentMapImage;
    public Image NextMapImage;

    public Image[] StateImages;

    public Color PassiveColor;
    public Color CurrentColor;
    public Color UnPassiveColor;

    public int levelRangeToChangeMaps;
    public int levelStart = 0;
    public int levelEnd = 0;
    public override void InitView()
    {
       
    }

    public override void Show()
    {
        base.Show();
        LoadLevelState();
    }

    public void LoadLevelState()
    {
        int indexMap = MMSLoadMap.Instance.CalculateIndexMap();
        
        if (indexMap >= MapInfoSystem.MapInfos.Count)
        {
            indexMap = indexMap % (MapInfoSystem.MapInfos.Count - 1);
        }
        CurrentMapImage.sprite = MapInfoSystem.MapInfos[indexMap].Icon;
        indexMap++;
        if (indexMap >= MapInfoSystem.MapInfos.Count)
        {
            indexMap = indexMap % (MapInfoSystem.MapInfos.Count - 1);
        }
        NextMapImage.sprite = MapInfoSystem.MapInfos[indexMap].Icon;

        int indexLevel = CalculateIndexLevel();
        for (int i = 0; i < StateImages.Length; i++)
        {
            if (i == indexLevel)
            {
                StateImages[i].color = CurrentColor;
            }
            else if (i > indexLevel)
            {
                StateImages[i].color = UnPassiveColor;
            }
            else if (i < indexLevel)
            {
                StateImages[i].color = PassiveColor;
            }
        }
    }


    private int CalculateIndexLevel()
    {
        int currentLevel = gameController.currentLevel;
        
        if(currentLevel >= 9)
        {
            currentLevel %= 10;
        }

        return currentLevel;
    }

}
