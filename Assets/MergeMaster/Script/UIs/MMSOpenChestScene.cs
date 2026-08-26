using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FSDK.Ads;
public class MMSOpenChestScene : DTNView
{
    public MMSChestItem[] ChestItems;
    public MMSBoard Board;
    public MMSCardInfoSystem CardInfoSystem;
    public MMSKeyControll KeyControl;
    public int ChestOpeningCount = 3;
    public GameObject BottomButton;
    int strongestArchery = 1;
    int strongestWarrior = 1;
    List<int> items;
    public Button clampBtn;
    public Button skipBtn;
    bool isClamp = false;
    public Animator m_animator;
    public int numberOfOpenedChest;

    public override void InitView()
    {

    }

    public override void Show()
    {
        numberOfOpenedChest = 0;
        BottomButton.SetActive(false);

        strongestArchery = CardInfoSystem.GetStrongestArchery();
        strongestWarrior = CardInfoSystem.GetStrongestWarrior();

        int numberOfCardOnBoard = Board.CheckCartCount();
        ChestOpeningCount = 3;

        items = new List<int>();
        for (int i = 0; i < ChestItems.Length; i++)
            items.Add(i);

        if (numberOfCardOnBoard == 14)
        {
            SetCardChest(Random.Range(0, items.Count-1));
        }

        if (numberOfCardOnBoard == 13)
        {
            SetCardChest(Random.Range(0, items.Count - 1));
            SetCardChest(Random.Range(0, items.Count - 1));
        }

        if (numberOfCardOnBoard <= 12)
        {
            SetCardChest(Random.Range(0, items.Count - 1));
            SetCardChest(Random.Range(0, items.Count - 1));
            SetCardChest(Random.Range(0, items.Count - 1));
        }
        KeyControl.SetKeys(ChestOpeningCount);
        SetUpButtons();
        SetCoinChests();
        base.Show();
    }

    void SetCardChest(int id)
    {
        if (Random.Range(0, 10) > 5)
        {
            ChestItems[items[id]].RenewChest("Warrior_" + Random.Range(1, strongestWarrior));
        }
        else
        {
            ChestItems[items[id]].RenewChest("Archery_" + Random.Range(1, strongestArchery));
        }

        items.RemoveAt(id);
    }

    void SetCoinChests()
    {
        for (int i = 0; i < items.Count; i++)
        {
            ChestItems[items[i]].RenewChest((long)(MMSMenuScene.BonusCoinChess() * (Random.Range(50, 200) / 150f))+4);
        }
    }

    public void AddCard(string cardName)
    {
        ChestOpeningCount--;
        Board.AddRewardCard(cardName);
        DTNSoundManagement.instance.Play("ChestOpen");
        KeyControl.SetKeys(ChestOpeningCount);
        if (ChestOpeningCount <= 0)
            BottomButton.SetActive(true);
    }

    public void AddCoin(long coin)
    {
        ChestOpeningCount--;
        DTNSoundManagement.instance.Play("ChestOpen");
        Board.Coin += coin;
        PlayerPrefs.SetString("UserCurrentCoin", Board.Coin.ToString());
        KeyControl.SetKeys(ChestOpeningCount);
        if (ChestOpeningCount <= 0)
            BottomButton.SetActive(true);
    }

    private void ClampBtnOnclick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
            AdsManager.Instance.ShowRewardedAd(() =>
            {
                BottomButton.SetActive(false);
                ChestOpeningCount = 3;
                KeyControl.SetKeys(ChestOpeningCount);
            }, () =>
            {
                StartCoroutine(ShowNoAdsNotification());
            }, 0, FSDK.LevelDifficulty.Hard, "");
       
    }
    private IEnumerator ShowNoAdsNotification()
    {
        DTNViewManagement.GetView<MMSNoAdsNotification>().Show();
        yield return new WaitForSeconds(1.5f);
        DTNViewManagement.GetView<MMSNoAdsNotification>().Hide();
    }

    private void SkipBtnOnclick()
    {
        DTNSoundManagement.instance.Play("buttonSound");

        for (int i = 0; i < ChestItems.Length; i++)
        {
            ChestItems[i].OpenChest();
        }

        BottomButton.SetActive(false);

        Time.timeScale = 1f;
        MMSChest.Instance.OpenChest();

        StartCoroutine(HideAnimation());
    }



    IEnumerator HideAnimation()
    {
        if(numberOfOpenedChest < 9)
            yield return new WaitForSeconds(2f);

        m_animator.Play("Hide");
        yield return new WaitForSeconds(0.5f);
        Hide();
    }
    private void SetUpButtons()
    {
        clampBtn.onClick.RemoveAllListeners();
        skipBtn.onClick.RemoveAllListeners();
        clampBtn.gameObject.SetActive(true);
        clampBtn.onClick.AddListener(() =>
        {
            ClampBtnOnclick();
        });
        skipBtn.onClick.AddListener(() =>
        {
            SkipBtnOnclick();
        });
    }
}
