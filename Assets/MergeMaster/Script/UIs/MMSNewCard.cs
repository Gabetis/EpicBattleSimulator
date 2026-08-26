using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMSNewCard : DTNView
{
    public MMSCardInfoSystem MMSCardInfoSystem;
    public Animator animator;
    public Button BackButton;
    public string CardName;
    public Text CardNameText;
    public Image CardImage;
    public Text Health;
    public Text Damage;
    public MMSDragAndDrop dragAndDrop;
    public Button Watch3DButton;
    public override void InitView()
    {

    }

    public override void Hide()
    {
        dragAndDrop.gameObject.SetActive(true);
        base.Hide();
    }

    public override void Show()
    {
        base.Show();

        animator.Play("Show");
        dragAndDrop.gameObject.SetActive(false);
        DTNSoundManagement.instance.Play("newChar");
        SetNewCardBoard();
        SetUpButtons();
    }

    IEnumerator ShowCharacter(float time)
    {
        yield return new WaitForSeconds(time);
        if (CardName != null)
        {
            DTNViewManagement.GetView<MMSCardCollectionScene>().ShowCard(CardName);
        }
        // yield return new WaitForSeconds(3f);
        // if (CardName != null && DTNViewManagement.GetView<MMSCardInfoScene>().gameObject.activeSelf)
        // {
        //     DTNViewManagement.GetView<MMSCardInfoScene>().HideCartInfoScene();
        // }
    }

    private void SetNewCardBoard()
    {
        if (CardName == null)
            return;

        var cardGameObject = Resources.Load(MMSCardInfoSystem.GetCardAddress(CardName)) as GameObject;

        if (cardGameObject == null)
            return;

        PlayerPrefs.SetInt(CardName + "IsUnlock", 1);
        GameObject newCard = cardGameObject;
        MMSCard card = newCard.GetComponent<MMSCard>();
        MMSCardInfo cardInfo = MMSCardInfoSystem.GetCardInfo(CardName);

        CardNameText.text = cardInfo.NickName;
        CardImage.sprite = cardInfo.Icon;

        Health.text = DTNNumber.FomatCoin((long)card.MaxHealth);
        Damage.text = DTNNumber.FomatCoin((long)card.Damage);


    }
    public void Back()
    {
        animator.Play("Hide");
        DTNSoundManagement.instance.Play("buttonSound");
    }

    private void SetUpButtons()
    {
        BackButton.onClick.RemoveAllListeners();
        Watch3DButton.onClick.RemoveAllListeners();
        BackButton.onClick.AddListener(() =>
        {
            Back();
        });
        Watch3DButton.onClick.AddListener(() =>
        {
            Watch3DButtonOnclick();

        });
    }

    private void Watch3DButtonOnclick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        StartCoroutine(ShowCharacter(0.25f));
    }

    [Sirenix.OdinInspector.Button("UnlockAll")]
    void UnlockAll()
    {
        for (var i = 1; i < 14; i++)
        {
            string keyArchery = "Archery_" + i;
            PlayerPrefs.SetInt(keyArchery + "IsUnlock", 1);
            string keyWarrior = "Warrior_" + i;
            PlayerPrefs.SetInt(keyWarrior + "IsUnlock", 1);

        }
    }
}
