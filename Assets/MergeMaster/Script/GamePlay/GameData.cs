using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [SerializeField] public List<CardLevelUser> CardsLevelPlayerData;

    public GameData(List<CardLevelUser> _CardsLevelPlayerData)
    {
        CardsLevelPlayerData = _CardsLevelPlayerData;
    }

}
[System.Serializable]
public class CardLevelUser
{
    [SerializeField] public int row;
    [SerializeField] public int column;
    [SerializeField] public string CardsName;
}
