using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MMSCardInfoScene : DTNView
{
    public string CardName;
    public GameObject CardObjectTrans;
    public GameObject CurrentCard;
    public Button BackBtn;

    public Button WinAniBtn;
    public Button DieAniBtn;
    public Button IdleAniBtn;
    public Button RunAniBtn;
    public Button AttackAniBtn;
    public Button StopRotatingBtn;

    public Text CardNameText;
    public Text HealthText;
    public Text DamageText;
    public MMSCardInfoSystem MMSCardInfoSystem;
    bool isSpawn;
    MMSCard currentCardScript;
    public CharacterRotate characterRotate;
    public override void InitView()
    {

    }

    public override void Show()
    {
        base.Show();
        if (CurrentCard)
        {
            Destroy(CurrentCard);
            isSpawn = false;
        }

        if (!isSpawn)
            SetNewCard();

        SetUpButtons();

    }

    private void SetNewCard()
    {
        if (CardName == null)
            return;

        var cardGameObject = Resources.Load(MMSCardInfoSystem.GetCardAddress(CardName)) as GameObject;

        if (cardGameObject == null)
            return;

        CurrentCard = Instantiate(cardGameObject, CardObjectTrans.transform);
        isSpawn = true;

        currentCardScript = CurrentCard.GetComponent<MMSCard>();
        CurrentCard.GetComponent<MMSCardInfoSetting>().SettingCard();
        CardNameText.text = currentCardScript.FullName;
        HealthText.text = DTNNumber.FomatCoin((long)currentCardScript.MaxHealth);
        DamageText.text = DTNNumber.FomatCoin((long)currentCardScript.Damage);
    }

    public void Back()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        StartCoroutine(HideCartInfoScene());
    }
    public IEnumerator HideCartInfoScene()
    {
        this.GetComponent<Animator>().Play("Hide");
        yield return new WaitForSeconds(0.6f);
        if (CurrentCard)
        {
            Destroy(CurrentCard);
            isSpawn = false;
        }
        Hide();
    }
    public void StopRotatingBtnOnclick()
    {
        characterRotate.isAutoRotating = !characterRotate.isAutoRotating;
        if (characterRotate.isAutoRotating)
        {
            StopRotatingBtn.GetComponentInChildren<Text>().text = DTNLocalizationSystem.GetText("Stop Rotating");
        }
        else
        {
            StopRotatingBtn.GetComponentInChildren<Text>().text = DTNLocalizationSystem.GetText("Auto Rotating");
        }
    }

    public void PlayAniOnclick(string state)
    {
        DTNSoundManagement.instance.Play("buttonSound");
        switch (state)
        {
            case "win":
                currentCardScript.ResetAnimation();
                currentCardScript.WinAnimation();
                break;
            case "die":
                currentCardScript.ResetAnimation();
                currentCardScript.DeadAnimation();
                currentCardScript.enabled = true;
                break;
            case "run":
                currentCardScript.ResetAnimation();
                currentCardScript.MoveAnimation();
                break;
            case "attack":
                currentCardScript.ResetAnimation();
                currentCardScript.AttackAnimation();
                break;
            case "idle":
                currentCardScript.ResetAnimation();
                break;
        }
    }

    private void SetUpButtons()
    {
        BackBtn.onClick.RemoveAllListeners();
        AttackAniBtn.onClick.RemoveAllListeners();
        DieAniBtn.onClick.RemoveAllListeners();
        WinAniBtn.onClick.RemoveAllListeners();
        RunAniBtn.onClick.RemoveAllListeners();
        IdleAniBtn.onClick.RemoveAllListeners();

        BackBtn.onClick.AddListener(() =>
        {
            Back();
        });

        WinAniBtn.onClick.AddListener(() =>
        {
            PlayAniOnclick("win");
        });
        DieAniBtn.onClick.AddListener(() =>
        {
            PlayAniOnclick("die");
        });
        RunAniBtn.onClick.AddListener(() =>
        {
            PlayAniOnclick("run");
        });
        IdleAniBtn.onClick.AddListener(() =>
        {
            PlayAniOnclick("idle");
        });
        AttackAniBtn.onClick.AddListener(() =>
        {
            PlayAniOnclick("attack");
        });

        SetUpRotatingButton();
    }
    void SetUpRotatingButton()
    {
        StopRotatingBtn.onClick.RemoveAllListeners();
        StopRotatingBtn.onClick.AddListener(() =>
        {
            StopRotatingBtnOnclick();
        });
        characterRotate.isAutoRotating = true;
        if (characterRotate.isAutoRotating)
        {
            StopRotatingBtn.GetComponentInChildren<Text>().text = DTNLocalizationSystem.GetText("Stop Rotating");
        }
        else
        {
            StopRotatingBtn.GetComponentInChildren<Text>().text = DTNLocalizationSystem.GetText("Auto Rotating");
        }
    }
}
