using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMSNoAdsNotification : DTNView
{
    public Button CloseButton;
    public Text text;
    public override void InitView()
    {

    }

    public override void Show()
    {
        base.Show();
        // StartCoroutine(EnumDisable());
        text.text = DTNLocalizationSystem.GetText("Ads are not ready.\nPlease wait a moment!");
        CloseButton.onClick.RemoveAllListeners();
        CloseButton.onClick.AddListener(() =>
        {
            this.Hide();
        });
    }

    IEnumerator EnumDisable()
    {
        yield return new WaitForSeconds(1f);
        this.Hide();
    }

}
