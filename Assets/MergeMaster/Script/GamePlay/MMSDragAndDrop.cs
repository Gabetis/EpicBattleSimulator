using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MMSDragAndDrop : MonoBehaviour
{
    public MMSCard Card;
    public MMSBoard Board;
    public GameObject RaySupport;
    Vector3 worldPoint;
    public Camera MyCamera;
    public GameObject ChooseLine;
    public GameObject[] SuggestLines;
    public GameObject DeleteArea;
    public CanvasGroup DeleteCanvasGroup;

    private void Update()
    {
        Touch();
    }

    public void TurnOnSuggestLine(List<Transform> targetTrans)
    {
        for (int i = 0; i < targetTrans.Count; i++)
        {
            SuggestLines[i].SetActive(true);
            SuggestLines[i].transform.position = targetTrans[i].position;
        }
    }

    public void TurnOffSuggestLine()
    {
        for (int i = 0; i < SuggestLines.Length; i++)
        {
            SuggestLines[i].SetActive(false);
        }
    }

    void Touch()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            DTNTouch touch = new DTNTouch();

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                touch = new DTNInputTouch(0);
            }
            else
            {
                touch = new DTNInputMouse(0);
            }

            worldPoint = GetWorldPoint(touch.position);

            switch (touch.phase)
            {
                case DTNTouchPhase.Began:

                    Card = GetCard(touch.position);

                    if (Card != null && Card.OnGetBoard() != Board)
                        Card = null;

                    if (Card != null)
                        OnBeginDrag();

                    break;

                case DTNTouchPhase.Moved:

                    if (Card != null)
                        OnDrag();
                    break;

                case DTNTouchPhase.Ended:

                    if (Card != null)
                        OnDrop();

                    break;
            }
        }

    }

    MMSCard GetCard(Vector2 data)
    {
        Ray ray = MyCamera.ScreenPointToRay(data);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000))
        {
            return hitData.transform.gameObject.GetComponent<MMSCard>();
        }
        return null;
    }

    Vector3 GetWorldPoint(Vector2 data)
    {
        Ray ray = MyCamera.ScreenPointToRay(data);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000) && hitData.collider != null)
        {
            return hitData.point;
        }
        else
        {
            return worldPoint;
        }
    }

    public void OnBeginDrag()
    {
        //Card.OnGetBoard(Board);
        if (Card == null)
            return;
        DTNSoundManagement.instance.Play("pickSound");
        MMSCameraControl.Instance.CameraOnBeginDrag();
        Card.transform.position = new Vector3(worldPoint.x, 1.5f, worldPoint.z);

        RaySupport.SetActive(true);
        Board.BeginDrag(Card);
    }

    public void OnDrag()
    {
        if (Card == null)
            return;

        Card.transform.position = new Vector3(worldPoint.x, 1.5f, worldPoint.z);
        Card.Animator.Play("Floating");

        Board.Drag(Card);

        if (worldPoint.z < -15f)
        {
            DeleteArea.SetActive(true);
        }
        else if (worldPoint.z < -10f)
        {
            DeleteArea.SetActive(true);
            DeleteCanvasGroup.alpha = Mathf.Clamp(worldPoint.z / -24f, 0f, 1f);
        }
        else
        {
            DeleteArea.SetActive(false);
        }
    }



    public void OnDrop()
    {
        if (Card == null)
            return;

        if (worldPoint.z < -15f)
        {
            DTNViewManagement.GetView<MMSDeleteCardScene>().Show();
        }
        else
        {
            DropCard();
        }
        /*
        if(UnityEngine.Random.Range(0,32) >= 27)
        {
            DTNViewManagement.GetView<MMSLuckyEarningScene>().SetEarnCoin(MMSDailyRewardsTime.MustEarnCoin(2));
            DTNViewManagement.GetView<MMSLuckyEarningScene>().Show();
        }
        */
        DeleteArea.SetActive(false);
        SaveUserPos();
    }

   
    public void DropCard()
    {
        TurnOffSuggestLine();
        Card.Animator.Play("Jumping Down");
        DTNSoundManagement.instance.Play("putSound");
        Board.Drop(Card);
        RaySupport.SetActive(false);
        Card = null;
    }

    public void DeleteCard()
    {
        TurnOffSuggestLine();
        SetChooseLine(false, 0, 0);
        Destroy(Card.gameObject);
        Board.Drop(Card);
        RaySupport.SetActive(false);
        Card = null;
    }

    public void SaveUserPos()
    {
        List<CardLevelUser> cards = Board.GetUserSaveCardLevel();
        DTNViewManagement.GetView<MMSMenuOnlineScene>().SetUpBuyButton();
        DTNViewManagement.GetView<MMSMenuOfflineScene>().SetUpBuyButton();
        SaveLoadUserBoard.Instance.CardsLevelPlayerData = cards;
        SaveLoadUserBoard.Instance.SaveFile();
    }

    public void SetChooseLine(bool isActive, int x, int y)
    {
        ChooseLine.SetActive(isActive);
        ChooseLine.transform.localPosition = new Vector3((y - (float)(Board.n - 1) / 2f) * Board.RangeY, 0, (x - (float)(Board.m - 1) / 2f) * Board.RangeX);
    }
}
