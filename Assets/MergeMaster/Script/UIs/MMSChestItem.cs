using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MMSChestItem : MonoBehaviour
{
    public MMSOpenChestScene OpenChestScene;
    public Image chestImage;
    public Button chestButton;
    public bool isCardReward = false;
    public long numberOfCoin = 100;
    public string cardName = "Warrior_1";

    public GameObject CardReward;
    public Image CardImage;

    public GameObject CoinReward;
    public Text CoinText;
    public Animator clampAnim;

    public void RenewChest(string name)
    {
        isCardReward = true;
        cardName = name;
        chestImage.enabled = true;
        chestButton.interactable = true;

        CardImage.sprite = OpenChestScene.CardInfoSystem.GetCardInfo(name).Icon;

        CardReward.SetActive(false);
        CoinReward.SetActive(false);
        chestButton.onClick.RemoveAllListeners();
        chestButton.onClick.AddListener(() =>
        {
            OpenChestOnClick();
        });
    }

    public void RenewChest(long numOfCoin)
    {
        isCardReward = false;
        numberOfCoin = numOfCoin;
        chestImage.enabled = true;
        chestButton.interactable = true;

        CoinText.text = DTNNumber.FomatCoin(numberOfCoin) + "";

        CardReward.SetActive(false);
        CoinReward.SetActive(false);
        chestButton.onClick.RemoveAllListeners();
        chestButton.onClick.AddListener(() =>
        {
            OpenChestOnClick();
        });
    }

    public void OpenChest()
    {
        chestButton.onClick.RemoveAllListeners();
        chestImage.enabled = false;
        chestButton.interactable = false;

        if (isCardReward)
        {
            CardReward.SetActive(true);
        }
        else
        {
            CoinReward.SetActive(true);
        }
    }

    void OpenChestOnClick()
    {
        if (OpenChestScene.ChestOpeningCount > 0)
        {
            chestButton.onClick.RemoveAllListeners();
            chestImage.enabled = false;
            chestButton.interactable = false;

            if (isCardReward)
            {
                CardReward.SetActive(true);
                OpenChestScene.AddCard(cardName);
            }
            else
            {
                CoinReward.SetActive(true);
                OpenChestScene.AddCoin(numberOfCoin);
            }
            DTNViewManagement.GetView<MMSOpenChestScene>().numberOfOpenedChest++;
            int number = DTNViewManagement.GetView<MMSOpenChestScene>().numberOfOpenedChest;
            if (number >= 9)
            {
                DTNViewManagement.GetView<MMSOpenChestScene>().clampBtn.gameObject.SetActive(false);
            }
        }
        else
        {
            clampAnim.SetTrigger("Forcus");
        }
    }
}
