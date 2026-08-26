using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMSDeleteCardScene : DTNView
{
    public Animator animator;
    public MMSDragAndDrop dragAndDrop;
    public Button YesBtn;
    public Button NoBtn;
    public override void InitView()
    {

    }

    public override void Show()
    {
        base.Show();
        dragAndDrop.gameObject.SetActive(false);
        animator.Play("Show");
        SetUpButtons();
    }


    public override void Hide()
    {
        dragAndDrop.gameObject.SetActive(true);
        base.Hide();
    }

    public void YesOnclick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        dragAndDrop.DeleteCard();
        animator.Play("Hide");
    }
    public void NoOnclick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        dragAndDrop.DropCard();
        animator.Play("Hide");
    }
    private void SetUpButtons()
    {
        YesBtn.onClick.RemoveAllListeners();
        YesBtn.onClick.AddListener(() =>
        {
            YesOnclick();
        });
        NoBtn.onClick.RemoveAllListeners();
        NoBtn.onClick.AddListener(() =>
        {
            NoOnclick();
        });
    }
}
