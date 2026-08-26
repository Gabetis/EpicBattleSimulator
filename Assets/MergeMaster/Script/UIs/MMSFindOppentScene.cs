using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MMSFindOppentScene : DTNView
{
    public Animator Animator;
    public MMSGameController GameController;
    public Text UserNameText;
    public Text OpponentNameText;
    public Text CountDownText;
    public Text MatchingCountingText;

    public Button BackBtn;

    public GameObject UserInfo;
    public GameObject OpponentInfo;

    public GameObject WattingObj;
    public GameObject NotFoundObj;
    Coroutine enumMatching;
    public MMSDragAndDrop dragAndDrop;
    public override void InitView()
    {

    }

    public override void Show()
    {
        base.Show();
        dragAndDrop.OnDrop();
        SetUpButtons();
        Animator.Play("FindSceneAppear");
        DTNViewManagement.GetView<MMSMenuOnlineScene>().Hide();
        CountDownText.text = "VS";
        SetUserName(PlayerPrefs.GetString("UserName"));

        BackBtn.gameObject.SetActive(true);
        OpponentInfo.SetActive(false);
        NotFoundObj.SetActive(false);

        WattingObj.SetActive(true);
        UserInfo.SetActive(true);
        OpponentNameText.text = "";
        dragAndDrop.gameObject.SetActive(false);
        enumMatching = StartCoroutine(EnumMatchingCounting());
    }

    public override void Hide()
    {
        base.Hide();
        // dragAndDrop.gameObject.SetActive(true);
    }

    IEnumerator EnumMatchingCounting()
    {
        for (int i = 30; i >= 0; i--)
        {
            MatchingCountingText.text = DTNLocalizationSystem.GetText("Matching : ") + i + "s";
            yield return new WaitForSeconds(1f);
        }

        NotFoundOpponet();
    }

    public void StopFindingOpponent()
    {
        GameController.StopCoroutinePost();
        WattingObj.SetActive(false);
        BackBtn.gameObject.SetActive(false);
        DTNViewManagement.GetView<MMSMenuOnlineScene>().Show();
        Animator.Play("FindSceneDisappear");
    }

    public void NotFoundOpponet()
    {
        if (gameObject.activeSelf == true)
        {
            DTNViewManagement.GetView<MMSNoFriend>().Show();
            BackBtn.gameObject.SetActive(false);
            WattingObj.SetActive(false);
            NotFoundObj.SetActive(true);
            StartCoroutine(EnumNotFoundOpponet());
        }

    }

    IEnumerator EnumNotFoundOpponet()
    {
        yield return new WaitForSeconds(1f);
        DTNViewManagement.GetView<MMSMenuOnlineScene>().Show();
        DTNViewManagement.GetView<MMSFindOppentScene>().Hide();
        Animator.Play("FindSceneDisappear");
    }

    public void SetCountDownAndFight()
    {
        StopCoroutine(enumMatching);
        MatchingCountingText.text = "";
        StartCoroutine(EnumCountDown());
    }

    IEnumerator EnumCountDown()
    {
        CountDownText.text = "3";
        yield return new WaitForSeconds(1f);
        CountDownText.text = "2";
        yield return new WaitForSeconds(1f);
        CountDownText.text = "1";
        yield return new WaitForSeconds(1f);
        CountDownText.text = DTNLocalizationSystem.GetText("Fight");
        yield return new WaitForSeconds(0.5f);
        GameController.StartFight();
        Animator.Play("FindSceneDisappear");
    }

    public void SetUserName(string name)
    {
        UserNameText.text = name;
    }

    public void SetOpponentName(string name)
    {
        OpponentInfo.SetActive(true);
        OpponentNameText.text = name;
        WattingObj.SetActive(false);
    }

    private void SetUpButtons()
    {
        BackBtn.onClick.RemoveAllListeners();

        BackBtn.onClick.AddListener(() =>
        {
            StopFindingOpponent();
        });
    }
}
