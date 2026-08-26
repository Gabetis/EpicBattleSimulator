using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Text;
using FSDK.Ads;

public enum Type
{
    Home = 0,
    Offline = 1,
    Online = 2,
    Tutorial = 3
}

public class MMSGameController : MonoBehaviour
{
    public static MMSGameController Instance;
    public MMSBoard UserBoard;
    public MMSBoard OpponentBoard;
    public MMSLevelManager LevelManager;
    public MMSChest Chest;
    public GameObject RewardHG;
    public int currentLevel;
    public int m = 3, n = 5;
    protected string[,] matrixPlayer;
    protected string[,] matrixCom;
    protected string[,] matrixOpponent;
    public ParticleSystem Confetti;

    protected Type ModeType = Type.Tutorial;
    public bool isVibrate;

    private void Awake()
    {
        Instance = this;
    }

    protected virtual void Start()
    {
        Input.multiTouchEnabled = false;

        AdsManager.Instance.ShowBanner();

        if (PlayerPrefs.GetInt("FirstPlay", 0) == 0)
        {
            DTNViewManagement.GetView<MMSTutorialScene>().Show();
            LoadTutorialGame();
        }
        else
        {
            if (OptionsChooseGame.Instance.gameType == OptionsChooseGame.GameType.Normal)
            {
                DTNViewManagement.GetView<MMSHomeScene>().Show();
            }
            else if (OptionsChooseGame.Instance.gameType == OptionsChooseGame.GameType.Offline)
            {
                LoadOfflineGame();
            }
        }

        Setup();
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetString("Offline_Clamp_DateTime", DateTime.Now.ToString());
    }

    protected void Setup()
    {
        SetDelegate();
        SetUpVibrate();
        SetUpSound();
        SetUpCardCollection();
    }

    void SetUpSound()
    {
        if (!PlayerPrefs.HasKey("SoundVolume"))
        {
            PlayerPrefs.SetFloat("SoundVolume", 1);
        }
        DTNSoundManagement.instance.SetVolume();
    }

    void SetUpCardCollection()
    {
        DTNViewManagement.GetView<MMSCardCollectionScene>().SpawnItemCards();
    }

    void SetUpVibrate()
    {
        Vibration.Init();
        if (PlayerPrefs.HasKey("Vibrate"))
            isVibrate = PlayerPrefs.GetInt("Vibrate") == 1 ? true : false;
        else
        {
            isVibrate = true;
            PlayerPrefs.SetInt("Vibrate", 1);
        }
    }

    void SetDelegate()
    {
        UserBoard.OnFinishAttack = () =>
        {
            if (UserBoard.GetHealth() > 0 && OpponentBoard.GetHealth() <= 0)
                OnFinishAttack(true);
            else
                OnFinishAttack(false);
        };

        OpponentBoard.OnFinishAttack = () =>
        {
            if (OpponentBoard.GetHealth() > 0 && UserBoard.GetHealth() <= 0)
                OnFinishAttack(false);
            else
                OnFinishAttack(true);
        };

        UserBoard.OnSetCoin = (long value) =>
        {
            SetCoin(value);
        };

        UserBoard.OnUnlockNewCard = (string value) =>
        {
            OnUnlockNewCard(value);
        };
    }

    public void OfflineMode()
    {
        ModeType = Type.Offline;
        LoadGame();
    }

    public void OnlineMode()
    {
        ModeType = Type.Online;
        LoadGame();
    }

    public void LoadGame()
    {
        ResetValue();

        if (!IsFinishTutorial())
            return;
        switch (ModeType)
        {
            case Type.Tutorial:
                LoadTutorialGame();
                break;

            case Type.Offline:
                LoadOfflineGame();
                break;

            case Type.Online:
                LoadOnlineGame();
                break;
        }
    }

    public void ResetValue()
    {
        isEndOfLevel = false;
        SetActiveHGReward(isLose);
    }

    public virtual bool IsFinishTutorial()
    {
        if (PlayerPrefs.GetInt("FirstPlay") == 1)
        {
            PlayerPrefs.SetInt("FirstPlay", 2);
            DTNViewManagement.GetView<MMSTutorialScene>().Hide();
            if (OptionsChooseGame.Instance.gameType == OptionsChooseGame.GameType.Normal)
            {
                ModeType = Type.Home;
                DTNViewManagement.GetView<MMSHomeScene>().Show();
            }
            else if (OptionsChooseGame.Instance.gameType == OptionsChooseGame.GameType.Offline)
            {
                ModeType = Type.Offline;
                LoadGame();
            }
            return false;
        }
        return true;
    }

    public void ShowMenuUI()
    {
        switch (ModeType)
        {
            case Type.Home:
                DTNViewManagement.GetView<MMSHomeScene>().Show();
                break;

            case Type.Tutorial:
                DTNViewManagement.GetView<MMSTutorialScene>().Show();
                break;

            case Type.Offline:
                DTNViewManagement.GetView<MMSMenuOfflineScene>().Show();
                break;

            case Type.Online:
                DTNViewManagement.GetView<MMSMenuOnlineScene>().Show();
                break;
        }
    }

    void SetCoin(long value)
    {
        DTNViewManagement.GetView<MMSGamePlayScene>().SetUserCoin(value);
    }

    protected void LoadMatrixPlayer()
    {
        matrixPlayer = new string[m, n];

        foreach (var item in SaveLoadUserBoard.Instance.CardsLevelPlayerData)
        {
            matrixPlayer[item.row, item.column] = item.CardsName;
        }
    }

    protected void LoadMatrixCom()
    {
        matrixCom = new string[m, n];

        if (currentLevel >= LevelManager.LevelInfos.Length)
        {
            currentLevel = LevelManager.LevelInfos.Length - 1;
        }

        foreach (var item in LevelManager.LevelInfos[currentLevel].CardsLevelCom)
        {
            matrixCom[item.column, item.row] = item.Cards;
        }
    }

    protected void SetMatrixOpponent(List<Card> cards)
    {
        matrixOpponent = new string[m, n];

        for (int i = 0; i < cards.Count; i++)
        {
            matrixOpponent[cards[i].row, cards[i].column] = cards[i].nameCard;
        }
    }

    public void LoadTutorialGame()
    {
        DTNViewManagement.GetView<MMSTutorialScene>().Show();

        MMSCameraControl.Instance.CameraMoveToPosStart();
        MMSLoadMap.Instance.LoadMap();

        LoadMatrixPlayer();
        LoadMatrixCom();
        UserBoard.LoadBoard(matrixPlayer, UserBoard);
        OpponentBoard.LoadBoard(matrixCom, OpponentBoard);

        UserBoard.OnBeginGame();
    }
    public void SaveDailyUserBoard()
    {
        List<CardLevelUser> cards = UserBoard.GetUserSaveCardLevel();
        SaveLoadUserBoard.Instance.CardsLevelPlayerData = cards;
        SaveLoadUserBoard.Instance.SaveFile();
    }

    public void LoadOfflineGame()
    {
        DTNViewManagement.GetView<MMSMenuOfflineScene>().Show();

        MMSCameraControl.Instance.CameraMoveToPosStart();
        MMSLoadMap.Instance.LoadMap();

        LoadMatrixPlayer();
        LoadMatrixCom();
        UserBoard.LoadBoard(matrixPlayer, UserBoard);
        OpponentBoard.LoadBoard(matrixCom, OpponentBoard);

        UserBoard.OnBeginGame();
        List<CardLevelUser> cards = UserBoard.GetUserSaveCardLevel();
        SaveLoadUserBoard.Instance.CardsLevelPlayerData = cards;
        SaveLoadUserBoard.Instance.SaveFile();

        DTNViewManagement.GetView<MMSMenuOfflineScene>().SetUpBuyButton();
    }

    public void LoadOnlineGame()
    {
        DTNViewManagement.GetView<MMSMenuOnlineScene>().Show();

        MMSCameraControl.Instance.CameraMoveToPosStart();
        MMSLoadMap.Instance.LoadRandomMap();

        LoadMatrixPlayer();
        OpponentBoard.ResetBoard();

        UserBoard.LoadBoard(matrixPlayer, UserBoard);

        UserBoard.OnBeginGame();

        List<CardLevelUser> cards = UserBoard.GetUserSaveCardLevel();
        SaveLoadUserBoard.Instance.CardsLevelPlayerData = cards;
        SaveLoadUserBoard.Instance.SaveFile();

        DTNViewManagement.GetView<MMSMenuOnlineScene>().SetUpBuyButton();
    }

    public float _timeScale = 1f;

    public void StartGame()
    {
        Time.timeScale = _timeScale;
        SetStartGameType();
        SetActiveHGReward(false);
        switch (ModeType)
        {
            case Type.Offline:
                StartOfflineGame();
                break;

            case Type.Online:
                StartOnlineGame();
                break;
        }
    }
    protected virtual void SetStartGameType()
    {
        if (OptionsChooseGame.Instance.gameType == OptionsChooseGame.GameType.Offline)
        {
            ModeType = Type.Offline;
        }
    }

    public void StartFight()
    {
        DTNViewManagement.GetView<MMSGamePlayScene>().Show();
        MMSCameraControl.Instance.CameraMoveToPosPlay();
        StartCoroutine(EnumAttack(UserBoardAttack));
        StartCoroutine(EnumAttack(OpponentBoardAttack));
    }

    public void StartOfflineGame()
    {
        StartFight();
    }


    Coroutine CoroutinePost;
    public void StartOnlineGame()
    {
        //dang online 
        //1. tim doi thu 

        DTNViewManagement.GetView<MMSFindOppentScene>().Show();

        string url = "https://game.ttdloyalty.com/findopponent/";
        string json = MMSJson.Instance.CreateBoardJsonFromData(PlayerPrefs.GetString("UserName"), matrixPlayer);


        CoroutinePost = StartCoroutine(Post(url, json, (long statusCode, string result) =>
        {
            Debug.Log(result);
            if (statusCode == 200)
            {
                Root root = JsonUtility.FromJson<Root>(result);
                Debug.Log(root.data.name);
                SetMatrixOpponent(root.data.Cards);
                OpponentBoard.LoadBoard(matrixOpponent, OpponentBoard);
                DTNViewManagement.GetView<MMSFindOppentScene>().SetOpponentName(root.data.name);
                DTNViewManagement.GetView<MMSFindOppentScene>().SetCountDownAndFight();
            }
            else
            {
                DTNViewManagement.GetView<MMSFindOppentScene>().NotFoundOpponet();
            }
        }));
    }

    public void StopCoroutinePost()
    {
        StopAllCoroutines();
    }

    IEnumerator Post(string url, string bodyJsonString, System.Action<long, String> finishedAction)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        finishedAction(request.responseCode, request.downloadHandler.text);
    }

    private void OpponentBoardAttack()
    {
        OpponentBoard.Attack(UserBoard);
    }

    private void UserBoardAttack()
    {
        UserBoard.Attack(OpponentBoard);
    }

    IEnumerator EnumAttack(Action attackAction)
    {
        yield return new WaitForFixedUpdate();
        attackAction();
    }

    public void NextLevel()
    {
        if (ModeType != Type.Online) LevelManagement.Instance.PlusLevel();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void OnFinishAttack(bool isWin)
    {
        if (isWin)
        {
            WinLevel();
        }
        else
        {
            LoseLevel();
        }
    }

    public void OnUnlockNewCard(string name)
    {
        if (PlayerPrefs.GetInt(name + "IsUnlock") == 0)
        {
            DTNViewManagement.GetView<MMSNewCard>().CardName = name;

            // get number from card name ex: Warrior_1
            int number = int.Parse(name.Split('_')[1]);
            if (number < 3)
            {
                DTNViewManagement.GetView<MMSNewCard>().Show();
            }
            else
            {
                    AdsManager.Instance.ShowInterstitialAd(() =>
                    {
                        DTNViewManagement.GetView<MMSNewCard>().Show();
                    }, () =>
                    {
                        DTNViewManagement.GetView<MMSNewCard>().Show();
                    }, 0);
            }

        }
    }

    public void OnUnlockNewMap(int Id)
    {
        if (PlayerPrefs.GetInt("Map" + Id + "IsUnlock") == 0)
        {
            DTNViewManagement.GetView<MMSNewMapScene>().MapId = Id;
            DTNViewManagement.GetView<MMSNewMapScene>().Show();
        }
    }

    public void BackToHome()
    {
        UserBoard.ResetBoard();
        OpponentBoard.ResetBoard();
    }

    bool isEndOfLevel = false;
    public void WinLevel()
    {
        if (!isEndOfLevel)
        {
            isEndOfLevel = true;
            Time.timeScale = 1f;
            StartCoroutine(EmumWinLevel());
        }

    }

    public void LoseLevel()
    {
        if (!isEndOfLevel)
        {
            isEndOfLevel = true;
            Time.timeScale = 1f;
            StartCoroutine(EnumLoseLevel());
        }
    }


    IEnumerator EmumWinLevel()
    {
        long RoundCoin = (long)(MMSMenuScene.MustEarnCoin(6) / 2.7f);
        DTNSoundManagement.instance.Play("winSound");
        Confetti.gameObject.SetActive(true);
        Confetti.Play();

        yield return new WaitForSeconds(2f);

        if ((AdsManager.Instance.IsRewardedAdReady() || AdsManager.Instance.IsInterstitialAdReady()) && UnityEngine.Random.Range(0, 100) >= 0 && PlayerPrefs.GetInt("FirstPlay") >= 2)
        {
            Chest.EarnCoin = RoundCoin;
            Chest.gameObject.SetActive(true);
            yield return new WaitForSeconds(4f);
        }

        if (Chest.isTouch) yield break;

        OnGameWinScene();
    }

    public void OnGameWinScene()
    {
        StartCoroutine(EnumOnGameWinScene());
    }

    public void RateGameShow()
    {
        if (currentLevel >= 5 && PlayerPrefs.GetInt("RateGame", 0) >= 7)
        {
            DTNViewManagement.GetView<MMSReviewScene>().Show();
        }

        if (PlayerPrefs.GetInt("RateGame", 0) != -1)
        {
            PlayerPrefs.SetInt("RateGame", PlayerPrefs.GetInt("RateGame", 0) + 1);
        }
    }

    IEnumerator EnumOnGameWinScene()
    {
        yield return new WaitForSeconds(2f);

        if (PlayerPrefs.GetInt("FirstPlay") >= 2)
        {
          
                AdsManager.Instance.ShowInterstitialAd(() =>
                {

                }, () =>
                {
                }, 0, FSDK.LevelDifficulty.Normal, "");
        }

        Confetti.Stop();
        Confetti.gameObject.SetActive(false);
        DTNViewManagement.GetView<MMSGamePlayScene>().Hide();
        long RoundCoin = (long)(MMSMenuScene.MustEarnCoin(6) / 2.7f);
        DTNViewManagement.GetView<MMSGameWinScene>().EarnCoin = (long)RoundCoin;
        DTNViewManagement.GetView<MMSGameWinScene>().Show();

        Chest.gameObject.SetActive(false);

        NextLevel();

        LevelManagement.Instance.SaveUserCoin();
        isLose = false;
    }

    IEnumerator EnumLoseLevel()
    {
        yield return new WaitForSeconds(2f);
        DTNSoundManagement.instance.Play("loseSound");

        long RoundCoin = (long)(MMSMenuScene.MustEarnCoin(4) / 2.7f);

        if (PlayerPrefs.GetInt("FirstPlay") >= 2)
        {
                AdsManager.Instance.ShowInterstitialAd(() =>
                {

                }, () =>
                {
                }, 0, FSDK.LevelDifficulty.Normal, "");
        
        }

        yield return new WaitForSeconds(2f);
        DTNViewManagement.GetView<MMSGamePlayScene>().Hide();

        DTNViewManagement.GetView<MMSGameLoseScene>().EarnCoin = RoundCoin;
        DTNViewManagement.GetView<MMSGameLoseScene>().Show();

        LevelManagement.Instance.SaveUserCoin();
        Debug.Log("Lose !!!!!!!!!!!!!!!!");
        isLose = true;
    }
    bool isLose = false;
    public void SetActiveHGReward(bool value)
    {
        RewardHG.SetActive(value);
    }
}
