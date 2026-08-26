using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMSDeleteTutScene : DTNView
{
    public Animator animator;
    public MMSDragAndDrop dragAndDrop;
    public Button BackBtn;
    public override void InitView()
    {

    }

    public override void Show()
    {
        base.Show();
        PlayerPrefs.SetInt("DeleteTut", PlayerPrefs.GetInt("DeleteTut", 0) + 1);
        dragAndDrop.gameObject.SetActive(false);
        animator.Play("Show");
        SetUpButtons();
    }


    public override void Hide()
    {
        dragAndDrop.gameObject.SetActive(true);
        base.Hide();
    }

    public void BackOnclick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        animator.Play("Hide");
    }
    private void SetUpButtons()
    {
        BackBtn.onClick.RemoveAllListeners();
        BackBtn.onClick.AddListener(() =>
        {
            BackOnclick();
        });
    }
}
