using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] GameObject Nofitication;
    bool IsRewardedReady;
    private void Start()
    {
    }

    private void OnEnable()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
        if (PlayerPrefs.GetFloat("IsDailyRewardedReady") == 0)
        {
            ShowNoti(false);
        }
        else
        {
            ShowNoti(true);
        }
    }
    private void OnClick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        DTNViewManagement.GetView<DailyRewardScene>().dailyRewardButton = this;
        DTNViewManagement.GetView<DailyRewardScene>().Show();
    }
    public void ShowNoti(bool isShow)
    {
        Nofitication.SetActive(isShow);
    }


}
