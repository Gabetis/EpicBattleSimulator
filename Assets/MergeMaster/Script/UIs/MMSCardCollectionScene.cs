using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MMSCardCollectionScene : DTNView
{
    public Animator Animator;
    public GameObject CardItem;

    public GameObject ArcheryScrollView;
    public GameObject WarriorScrollView;

    public GameObject ArcheryOn;
    public GameObject WarriorOn;

    public GameObject ArcheryContent;
    public GameObject WarriorContent;

    public Button BackButton;
    public Button ArcheryButton;
    public Button WarriorButton;



    public MMSCardInfoSystem MMSCardInfoSystem;
    bool isSpawnItem = false;

    List<MMSCardItem> ArcheryItemList = new List<MMSCardItem>();
    List<MMSCardItem> WarriorItemList = new List<MMSCardItem>();

    public MMSDragAndDrop dragAndDrop;

    public override void InitView()
    {

    }

    public override void Show()
    {
        base.Show();
        Animator.Play("Appear");
        SetUpButtons();

        SpawnItemCards();
        dragAndDrop.OnDrop();
        dragAndDrop.gameObject.SetActive(false);

        CheckUnlockItem();
    }

    public override void Hide()
    {
        dragAndDrop.gameObject.SetActive(true);
        base.Hide();
    }
    public void SpawnItemCards()
    {
        if (!isSpawnItem)
            SpawnItem();

    }

    void SpawnItem()
    {
        isSpawnItem = true;
        for (int i = 0; i < MMSCardInfoSystem.CardInfos.Count; i++)
        {
            GameObject cloneItem = Instantiate(CardItem, CardItem.transform.position, CardItem.transform.rotation);
            MMSCardItem cardItem = cloneItem.GetComponent<MMSCardItem>();
            SetNewCardBoard(cardItem, MMSCardInfoSystem.CardInfos[i].Name);

            if (cardItem.IsWarrior)
            {
                WarriorItemList.Add(cardItem);
                cloneItem.transform.parent = WarriorContent.transform;
            }
            else
            {
                ArcheryItemList.Add(cardItem);
                cloneItem.transform.parent = ArcheryContent.transform;
            }
            cloneItem.transform.localScale = Vector3.one;
        }
    }

    void CheckUnlockItem()
    {
        for (int i = 0; i < ArcheryItemList.Count; i++)
        {
            if (PlayerPrefs.GetInt(ArcheryItemList[i].CardName + "IsUnlock") == 1)
            {
                ArcheryItemList[i].Unlock();
            }
        }

        for (int i = 0; i < WarriorItemList.Count; i++)
        {
            if (PlayerPrefs.GetInt(WarriorItemList[i].CardName + "IsUnlock") == 1)
            {
                WarriorItemList[i].Unlock();
            }
        }
    }

    private void SetNewCardBoard(MMSCardItem cardItem, string CardName)
    {
        if (CardName == null)
            return;

        var cardGameObject = Resources.Load(MMSCardInfoSystem.GetCardAddress(CardName)) as GameObject;

        if (cardGameObject == null)
            return;

        GameObject newCard = cardGameObject;
        MMSCard card = newCard.GetComponent<MMSCard>();
        MMSCardInfo cardInfo = MMSCardInfoSystem.GetCardInfo(CardName);

        cardItem.CardName = CardName;
        cardItem.CardNameText.text = cardInfo.NickName;
        cardItem.CardImage.sprite = cardInfo.Icon;
        cardItem.CardImage2.sprite = cardInfo.Icon;
        cardItem.IsWarrior = cardInfo.IsWarrior;

        cardItem.Health.text = DTNNumber.FomatCoin((long)card.MaxHealth);
        cardItem.Damage.text = DTNNumber.FomatCoin((long)card.Damage);
    }

    public void Back()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        Animator.Play("Disappear");
    }

    public void ActiveWarriorScrollView()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        WarriorOn.SetActive(true);
        ArcheryOn.SetActive(false);
        WarriorScrollView.SetActive(true);
        ArcheryScrollView.SetActive(false);
    }

    public void ActiveArcheryScrollView()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        WarriorOn.SetActive(false);
        ArcheryOn.SetActive(true);
        WarriorScrollView.SetActive(false);
        ArcheryScrollView.SetActive(true);
    }

    public void ShowCard(string cardName)
    {
        DTNSoundManagement.instance.Play("buttonSound");
        DTNViewManagement.GetView<MMSCardInfoScene>().CardName = cardName;
        DTNViewManagement.GetView<MMSCardInfoScene>().Show();
    }

    private void SetUpButtons()
    {
        BackButton.onClick.RemoveAllListeners();
        ArcheryButton.onClick.RemoveAllListeners();
        WarriorButton.onClick.RemoveAllListeners();

        BackButton.onClick.AddListener(() =>
        {
            Back();
        });

        ArcheryButton.onClick.AddListener(() =>
        {
            ActiveArcheryScrollView();
        });

        WarriorButton.onClick.AddListener(() =>
        {
            ActiveWarriorScrollView();
        });
    }
}
