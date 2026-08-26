using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Card
{
    public Card(string nameCard,int row, int column)
    {
        this.nameCard = nameCard;
        this.row = row;
        this.column = column;
    }

    public string nameCard;
    public int row;
    public int column;
}

[Serializable]
public class Data
{
    public string name;
    // public string country { get; set; }
    public List<Card> Cards;
}

[Serializable]
public class Root
{
    public Data data;
}

public class MMSJson : MonoBehaviour
{
    public static MMSJson Instance;
    private void Awake()
    {
        Instance = this;
    }

    public string CreateBoardJsonFromData(string userName,string[,] matrix)
    {
        Root root = new Root();
        root.data = new Data();
        root.data.Cards = new List<Card>();
        root.data.name = userName;

        for (int x = 0; x < matrix.GetLength(0); x++)
        {
            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                if (matrix[x,y] != null)
                {
                    root.data.Cards.Add(new Card(matrix[x, y], x, y));
                }
            }
        }

        return JsonUtility.ToJson(root);
    }

    public string CreateUserDataJsonFromBoard(List<CardLevelUser> _CardsLevelPlayerData)
    {
        GameData gameData = new GameData(_CardsLevelPlayerData);
        return JsonUtility.ToJson(gameData);
    }

    public string CreateDailyDataJson(List<DailyRewards> DailyRewardsDatas)
    {
        DailyRewardData data = new DailyRewardData(DailyRewardsDatas);
        return JsonUtility.ToJson(data);
    }

    public List<DailyRewards> DailyRewardsFromJsonFile(string jsonFile)
    {
        DailyRewardData data = JsonUtility.FromJson<DailyRewardData>(jsonFile);
        return data.DailyRewardsDatas;
    }
}
