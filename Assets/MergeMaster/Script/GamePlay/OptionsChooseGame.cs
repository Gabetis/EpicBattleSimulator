using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsChooseGame : MonoBehaviour
{
    public static OptionsChooseGame Instance;
    public int ChooseIndex;
    public GameType gameType;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        SetupGameType();
    }
    public void SetupGameType()
    {
        if (ChooseIndex == 0)
        {
            gameType = GameType.Normal;
        }
        else if (ChooseIndex == 1)
        {
            gameType = GameType.Offline;
        }
    }

    public enum GameType
    {
        Normal, // Game bình thường
        Offline, // game chỉ có chế độ offline
    }
}
