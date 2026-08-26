using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MMSNoFriend : DTNView
{
    public Animator animator;
    public Button BackButton;
    public Button ShareButton;
    Coroutine backCoroutine;
    public override void InitView()
    {

    }

    public override void Show()
    {
        base.Show();
        animator.Play("Show");
        SetUpButtons();
        backCoroutine = StartCoroutine(EnumBack());
    }

    IEnumerator EnumBack()
    {
        yield return new WaitForSeconds(5f);
        Back();
    }

    public void Back()
    {
        if (backCoroutine != null)
            StopCoroutine(backCoroutine);

        animator.Play("Hide");
        DTNSoundManagement.instance.Play("buttonSound");
    }

    public void Share()
    {
        ShareOnSocialMedia.Instance.Share();
        Back();
    }

    private void SetUpButtons()
    {
        BackButton.onClick.RemoveAllListeners();
        BackButton.onClick.AddListener(() =>
        {
            Back();
        });

        ShareButton.onClick.RemoveAllListeners();
        ShareButton.onClick.AddListener(() =>
        {
            Share();
        });
    }
}