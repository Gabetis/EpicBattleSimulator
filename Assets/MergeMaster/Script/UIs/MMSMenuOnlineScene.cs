using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MMSMenuOnlineScene : MMSMenuScene
{
    public Text UserName;
    public Button ChangeNameBtn;

    public override void InitView()
    {
    }
    public override void Show()
    {
        SetUpBuyCoin();
        SetUpBuyButton();
        UpdateCoinText();
        SetupNameText();
        SetUpChangeNameButton();
        if (PlayerPrefs.GetInt("FirstTimeOnline") == 0)
        {
            PlayerPrefs.SetInt("FirstTimeOnline", 1);
            DTNViewManagement.GetView<MMSGetNameScene>().Show();
        }
        base.Show();
    }

    public void ChangeNameOnclickBtn()
    {
        DTNViewManagement.GetView<MMSGetNameScene>().Show();
        if (PlayerPrefs.HasKey("UserName"))
            DTNViewManagement.GetView<MMSGetNameScene>().InputField.text = PlayerPrefs.GetString("UserName");
    }

    public void SetupNameText()
    {
        UserName.text = PlayerPrefs.GetString("UserName");
    }

    private void SetUpChangeNameButton()
    {
        ChangeNameBtn.onClick.RemoveAllListeners();
        ChangeNameBtn.onClick.AddListener(() =>
        {
            ChangeNameOnclickBtn();
        });
    }
}
