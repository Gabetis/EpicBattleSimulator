using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MMSGetNameScene : DTNView
{
    public Button EnterGameButton;
    public Text NameText;
    public InputField InputField;
    public Animator Animator;
    public MMSDragAndDrop dragAndDrop;
    public GameObject ErrorFieldName;
    public override void InitView()
    {

    }

    public override void Show()
    {
        base.Show();
        dragAndDrop.OnDrop();
        Animator.Play("Show");
        SetUpButtons();
    }

    public void GetName()
    {
        if (NameText.text.Length <= 0)
        {
            // PlayerPrefs.SetString("UserName", "Anonymous" + Random.Range(0, 999));
            StartCoroutine(ShowErrorFieldName());
        }
        else
        {
            PlayerPrefs.SetString("UserName", NameText.text);
            DTNViewManagement.GetView<MMSMenuOnlineScene>().SetupNameText();
            Animator.Play("Hide");
        }
    }

    private void SetUpButtons()
    {
        EnterGameButton.onClick.RemoveAllListeners();

        EnterGameButton.onClick.AddListener(() =>
        {
            GetName();
        });
    }
    private IEnumerator ShowErrorFieldName()
    {
        ErrorFieldName.SetActive(true);
        yield return new WaitForSeconds(1);
        ErrorFieldName.SetActive(false);
    }
}
