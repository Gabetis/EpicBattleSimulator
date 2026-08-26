using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MMSTutorialScene : DTNView
{
    int State = 1;
    public Button StartFightButton;
    public Button ArcheryBuyButton;
    public Button WarriorBuyButton;

    public GameObject HandPoint;
    public GameObject HandDragToMerge;
    public GameObject HandDragDrop;

    public MMSUserBoard UserBoard;
    public MMSDragAndDrop mmsDragAndDrop;
    public MMSGameController GameController;


    public override void InitView()
    {

    }

    public override void Show()
    {
        base.Show();
        SetUpButtons();
        SetState();
    }

    private void SetState()
    {
        switch (State)
        {
            case 0:
                break;
            case 1:
                StartFightButton.gameObject.SetActive(true);
                ArcheryBuyButton.gameObject.SetActive(false);
                WarriorBuyButton.gameObject.SetActive(false);
                mmsDragAndDrop.gameObject.SetActive(false);
                break;

            case 2:
                StartFightButton.gameObject.SetActive(false);
                ArcheryBuyButton.gameObject.SetActive(false);
                WarriorBuyButton.gameObject.SetActive(true);
                break;

            case 3:
                StartFightButton.gameObject.SetActive(false);
                ArcheryBuyButton.gameObject.SetActive(false);
                WarriorBuyButton.gameObject.SetActive(false);
                mmsDragAndDrop.gameObject.SetActive(true);
                // HandPoint.SetActive(true);
                HandPointBtnOnPointDown();
                break;

            case 4:
                HandPoint.SetActive(false);
                mmsDragAndDrop.gameObject.SetActive(true);
                MMSCameraControl.Instance.CameraOnBeginDrag();
                StartFightButton.gameObject.SetActive(false);
                ArcheryBuyButton.gameObject.SetActive(false);
                WarriorBuyButton.gameObject.SetActive(false);
                HandDragToMerge.SetActive(true);
                break;
            case 5:
                HandDragToMerge.SetActive(false);
                MMSCameraControl.Instance.CameraOnBeginDrag();
                mmsDragAndDrop.gameObject.SetActive(true);
                StartFightButton.gameObject.SetActive(false);
                ArcheryBuyButton.gameObject.SetActive(true);
                WarriorBuyButton.gameObject.SetActive(false);

                break;

            case 6:
                HandDragDrop.SetActive(true);
                MMSCameraControl.Instance.CameraOnBeginDrag();
                mmsDragAndDrop.gameObject.SetActive(true);
                StartFightButton.gameObject.SetActive(false);
                ArcheryBuyButton.gameObject.SetActive(false);
                WarriorBuyButton.gameObject.SetActive(false);
                break;

            case 7:
                HandDragDrop.SetActive(false);
                StartFightButton.gameObject.SetActive(true);
                ArcheryBuyButton.gameObject.SetActive(false);
                WarriorBuyButton.gameObject.SetActive(false);
                break;
            case 8:
                StartFightButton.gameObject.SetActive(false);
                ArcheryBuyButton.gameObject.SetActive(false);
                WarriorBuyButton.gameObject.SetActive(false);
                break;
        }
    }

    public void AddCard(string cardName)
    {
        UserBoard.AddCard(cardName);
    }

    public void BuyWarriorBtnOnclickCoin()
    {
        State++;
        SetState();

        DTNSoundManagement.instance.Play("cashButtonSound");
        UserBoard.Coin -= 5;

        AddCard("Warrior_1");

        LevelManagement.Instance.SaveUserCoin();
        LevelManagement.Instance.SaveCoinOfBuyWarrior(5);
        WarriorBuyButton.gameObject.SetActive(false);
    }

    public void HandPointBtnOnPointDown()
    {
        State++;
        SetState();
    }

    public void HandDragToMergeBtnOnPointUp()
    {
        State++;
        SetState();
    }

    public void BuyArcheryBtnOnclickCoin()
    {
        State++;
        SetState();

        DTNSoundManagement.instance.Play("cashButtonSound");
        UserBoard.Coin -= 5;

        AddCard("Archery_1");

        LevelManagement.Instance.SaveUserCoin();
        LevelManagement.Instance.SaveCoinOfBuyArchery(5);
        mmsDragAndDrop.SaveUserPos();
        ArcheryBuyButton.gameObject.SetActive(false);
    }

    public void StartBattle()
    {
        State++;

        if (State >= 4)
            PlayerPrefs.SetInt("FirstPlay", 1);

        DTNSoundManagement.instance.Play("buttonSound");
        GameController.StartOfflineGame();
        DTNViewManagement.GetView<MMSGamePlayScene>().InitIfNeed();
        DTNViewManagement.GetView<MMSGamePlayScene>().WillShow();
        DTNViewManagement.GetView<MMSGamePlayScene>().Show();
        base.Hide();
    }



    private void SetUpButtons()
    {
        StartFightButton.onClick.RemoveAllListeners();
        ArcheryBuyButton.onClick.RemoveAllListeners();
        WarriorBuyButton.onClick.RemoveAllListeners();

        StartFightButton.onClick.AddListener(() =>
        {
            StartBattle();
        });

        ArcheryBuyButton.onClick.AddListener(() =>
        {
            BuyArcheryBtnOnclickCoin();
        });

        WarriorBuyButton.onClick.AddListener(() =>
        {
            BuyWarriorBtnOnclickCoin();
        });
    }
}
