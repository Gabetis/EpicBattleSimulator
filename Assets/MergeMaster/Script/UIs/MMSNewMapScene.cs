using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMSNewMapScene : DTNView
{
    public MMSMapInfoSystem MMSMapInfoSystem;
    public Animator animator;
    public Button BackButton;
    public int MapId = -1;
    public Image MapImage;
    public MMSDragAndDrop dragAndDrop;
    public override void InitView()
    {

    }

    public override void Show()
    {
        base.Show();
        animator.Play("Show");
        dragAndDrop.OnDrop();
        dragAndDrop.gameObject.SetActive(false);
        DTNSoundManagement.instance.Play("newChar");
        SetNewCardBoard();
        SetUpButtons();
    }

    public override void Hide()
    {
        dragAndDrop.gameObject.SetActive(true);
        base.Hide();
    }

    private void SetNewCardBoard()
    {
        if (MapId < 0)
            return;


        PlayerPrefs.SetInt("Map" + MapId + "IsUnlock", 1);
        MMSMapInfo mapInfo = MMSMapInfoSystem.GetMapInfo(MapId);

        MapImage.sprite = mapInfo.PopUpImage;

    }
    public void Back()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        animator.Play("Hide");
    }

    private void SetUpButtons()
    {
        BackButton.onClick.RemoveAllListeners();
        BackButton.onClick.AddListener(() =>
        {
            Back();
        });
    }
}
