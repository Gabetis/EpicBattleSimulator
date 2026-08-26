using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagement : MonoBehaviour
{
    public static LevelManagement Instance;
    [SerializeField] private MMSGameController mMSGameController;
    [SerializeField] private MMSBoard userBoard;
    [Header("Show Infomations PlayerPref")]
    public int currentLevelPlayerPref;
    public long currentCoinPlayerPref;
    public long coinToBuyArchery;
    public long coinToBuyWarrior;
    private int currentLevel;
    public MMSDailyRewardsDatabase DailyRewardsDatabase;
    private void Awake()
    {
        Instance = this;
        ChangeCoinFromIntToStringPref();
        FirstPlay();
        LoadLevel();
        LoadUserCoin();
    }
    void FirstPlay()
    {
        if (PlayerPrefs.GetInt("FirstPlay") == 0)
        {
            PlayerPrefs.SetInt("Map" + 0 + "IsUnlock", 1);
            PlayerPrefs.SetInt("Warrior_1" + "IsUnlock", 1);
            PlayerPrefs.SetInt("Archery_1" + "IsUnlock", 1);
            PlayerPrefs.SetString("UserCurrentCoin", "0");
            DailyRewardsDatabase.SetFirstDailyRewardDatabase();
        }
    }

    private void LoadLevel()
    {
        if (PlayerPrefs.GetInt("CurrentLevel") == 0)
        {
            PlayerPrefs.SetInt("CurrentLevel", 0);
            mMSGameController.currentLevel = PlayerPrefs.GetInt("CurrentLevel");

            mMSGameController.currentLevel = currentLevel;
            currentLevelPlayerPref = currentLevel;
        }
        else
        {
            mMSGameController.currentLevel = PlayerPrefs.GetInt("CurrentLevel");
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
            currentLevelPlayerPref = currentLevel;
        }
    }
    public void PlusLevel()
    {
        currentLevel++;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        mMSGameController.currentLevel = currentLevel;
        currentLevelPlayerPref = currentLevel;
    }
    public void LoadUserCoin()
    {
        if (!PlayerPrefs.HasKey("UserCurrentCoin"))
        {
            PlayerPrefs.SetString("UserCurrentCoin", "0");
            userBoard.Coin = long.Parse(PlayerPrefs.GetString("UserCurrentCoin"));
            currentCoinPlayerPref = long.Parse(PlayerPrefs.GetString("UserCurrentCoin"));
        }
        else
        {

            userBoard.Coin = long.Parse(PlayerPrefs.GetString("UserCurrentCoin"));
            currentCoinPlayerPref = long.Parse(PlayerPrefs.GetString("UserCurrentCoin"));
        }
    }
    public void SaveUserCoin()
    {
        PlayerPrefs.SetString("UserCurrentCoin", userBoard.Coin.ToString());
        currentCoinPlayerPref = long.Parse(PlayerPrefs.GetString("UserCurrentCoin"));
    }
    public long LoadCoinOfBuyArchery(long coinOnScene)
    {
        if (!PlayerPrefs.HasKey("CoinToBuyArchery"))
        {
            PlayerPrefs.SetString("CoinToBuyArchery", coinOnScene.ToString());
            coinOnScene = long.Parse(PlayerPrefs.GetString("CoinToBuyArchery"));
            coinToBuyArchery = long.Parse(PlayerPrefs.GetString("CoinToBuyArchery"));
        }
        else
        {

            coinOnScene = long.Parse(PlayerPrefs.GetString("CoinToBuyArchery"));
            coinToBuyArchery = long.Parse(PlayerPrefs.GetString("CoinToBuyArchery"));
        }
        return coinToBuyArchery;
    }
    public void SaveCoinOfBuyArchery(long coinOnScene)
    {
        PlayerPrefs.SetString("CoinToBuyArchery", coinOnScene.ToString());
        coinToBuyArchery = long.Parse(PlayerPrefs.GetString("CoinToBuyArchery"));
    }
    public long LoadCoinOfBuyWarrior(long coinOnScene)
    {
        if (!PlayerPrefs.HasKey("CoinToBuyWarrior"))
        {
            PlayerPrefs.SetString("CoinToBuyWarrior", coinOnScene.ToString());
            coinOnScene = long.Parse(PlayerPrefs.GetString("CoinToBuyWarrior"));
            coinToBuyWarrior = long.Parse(PlayerPrefs.GetString("CoinToBuyWarrior"));
        }
        else
        {

            coinOnScene = long.Parse(PlayerPrefs.GetString("CoinToBuyWarrior"));
            coinToBuyWarrior = long.Parse(PlayerPrefs.GetString("CoinToBuyWarrior"));

        }
        return coinToBuyWarrior;
    }
    public void SaveCoinOfBuyWarrior(long coinOnScene)
    {
        PlayerPrefs.SetString("CoinToBuyWarrior", coinOnScene.ToString());
        coinToBuyWarrior = long.Parse(PlayerPrefs.GetString("CoinToBuyWarrior"));
    }
    private void ChangeCoinFromIntToStringPref()
    {
        if (!PlayerPrefs.HasKey("IsNewCoin"))
        {
            PlayerPrefs.SetFloat("IsNewCoin", 0);
        }
        else
        {
            return;
        }
        bool isNew = PlayerPrefs.GetFloat("IsNewCoin") == 0 ? true : false;
        if (isNew && PlayerPrefs.HasKey("UserCurrentCoin") && PlayerPrefs.HasKey("CoinToBuyWarrior") && PlayerPrefs.HasKey("CoinToBuyArchery"))
        {
            PlayerPrefs.SetFloat("IsNewCoin", 1);
            if (PlayerPrefs.HasKey("UserCurrentCoin"))
            {
                int saveCoin = PlayerPrefs.GetInt("UserCurrentCoin");
                PlayerPrefs.SetString("UserCurrentCoin", saveCoin.ToString());
            }
            if (PlayerPrefs.HasKey("CoinToBuyWarrior"))
            {
                int saveCoinWarrior = PlayerPrefs.GetInt("CoinToBuyWarrior");
                PlayerPrefs.SetString("CoinToBuyWarrior", saveCoinWarrior.ToString());
            }
            if (PlayerPrefs.HasKey("CoinToBuyArchery"))
            {
                int saveCoinArchery = PlayerPrefs.GetInt("CoinToBuyArchery");
                PlayerPrefs.SetString("CoinToBuyArchery", saveCoinArchery.ToString());
            }

        }
    }

    [Sirenix.OdinInspector.Button]
    public void ResetPlayerPref()
    {
        PlayerPrefs.DeleteAll();
    }

    [Sirenix.OdinInspector.Button]
    public void SaveLevelPlayerPref()
    {
        PlayerPrefs.SetInt("CurrentLevel", currentLevelPlayerPref);
        mMSGameController.currentLevel = currentLevelPlayerPref;
        if (currentLevelPlayerPref > 2)
        {
            PlayerPrefs.SetInt("FirstPlay", 2);
        }
        PlayerPrefs.SetString("CoinToBuyWarrior", coinToBuyWarrior.ToString());
        PlayerPrefs.SetString("CoinToBuyArchery", coinToBuyArchery.ToString());
        PlayerPrefs.SetString("UserCurrentCoin", currentCoinPlayerPref.ToString());
    }

    [Sirenix.OdinInspector.Button]
    public void SetPlayerPrefLevel(int level)
    {
        PlayerPrefs.SetInt("CurrentLevel", level);
    }

    // [Sirenix.OdinInspector.Button]
    // public void SaveUserCoinPlayerPref()
    // {
    //     PlayerPrefs.SetInt("CoinToBuyWarrior", (int)coinToBuyWarrior);
    //     PlayerPrefs.SetInt("CoinToBuyArchery", (int)coinToBuyArchery);
    //     PlayerPrefs.SetInt("UserCurrentCoin", (int)currentCoinPlayerPref);
    // }


}
