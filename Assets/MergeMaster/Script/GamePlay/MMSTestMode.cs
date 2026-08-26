using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMSTestMode : MMSGameController
{
    protected override void Start()
    {
        Input.multiTouchEnabled = false;
        ModeType = Type.Offline;
        Setup();
        LoadTestGame();
        
    }



    public void LoadTestGame()
    {

     //   DTNViewManagement.GetView<MMSMenuOfflineScene>().Show();

        MMSCameraControl.Instance.CameraMoveToPosStart();
        MMSLoadMap.Instance.LoadMap();

        UserBoard.LoadRewardCards();
        OpponentBoard.LoadRewardCards();

        List<CardLevelUser> cards = UserBoard.GetUserSaveCardLevel();
        SaveLoadUserBoard.Instance.CardsLevelPlayerData = cards;
        SaveLoadUserBoard.Instance.SaveFile();

       // DTNViewManagement.GetView<MMSMenuOfflineScene>().SetUpBuyButton();
    }

    protected override void SetStartGameType()
    { 

    }

    public override bool IsFinishTutorial()
    {
        return true;
    }
}
