using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMSOfflineEarningScene : DTNView
{
    public Animator animator;
    public Button CollectButton;
    public Text CoinText;
    public MMSDragAndDrop dragAndDrop;
    public MMSUserBoard UserBoard;
    public MMSMenuScene[] MenuScenes;
    public GameObject CoinEffect;
    private long EarnCoin;
    public override void InitView()
    {

    }

    public override void Show()
    {
        base.Show();
        animator.Play("Show");
        dragAndDrop.OnDrop();
        CoinEffect.SetActive(false);
        dragAndDrop.gameObject.SetActive(false);
        DTNSoundManagement.instance.Play("newChar");;
        SetUpButtons();
    }

    public void SetEarnCoin(long value)
    {
        EarnCoin = value;
        CoinText.text = "" + DTNNumber.FomatCoin(EarnCoin);
    }

    public void AddUserCoin()
    {
        UserBoard.Coin += EarnCoin;
        LevelManagement.Instance.SaveUserCoin();
        MenuScenes[0].UpdateCoinText();
        MenuScenes[1].UpdateCoinText();
    }

    public void AddUserCoin(int coin)
    {
        UserBoard.Coin += coin;
        LevelManagement.Instance.SaveUserCoin();
        MenuScenes[0].UpdateCoinText();
        MenuScenes[1].UpdateCoinText();
    }

    public override void Hide()
    {
        dragAndDrop.gameObject.SetActive(true);
        base.Hide();
    }

    public void Collect()
    {
        DTNSoundManagement.instance.Play("coincollect");
        AddUserCoin();
        CoinEffect.SetActive(true);
        animator.Play("Hide");
    }

    private void SetUpButtons()
    {
        CollectButton.onClick.RemoveAllListeners();
        CollectButton.onClick.AddListener(() =>
        {
            Collect();
        });
    }
}
