using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MMSCardItem : MonoBehaviour
{
    public MMSCardCollectionScene CardCollectionScene;
    public GameObject Lock;
    public string CardName;
    public Text CardNameText;
    public Image CardImage;
    public Image CardImage2;
    public Text Health;
    public Text Damage;
    public Button ShowCardButton;
    public bool IsWarrior;

    private void OnEnable()
    {
        ShowCardButton.onClick.AddListener(() => 
        {
            ShowCard();
        }) ;
    }

    public void ShowCard()
    {
        CardCollectionScene.ShowCard(CardName);
    }

    public void Unlock()
    {
        Lock.SetActive(false);
    }
}
