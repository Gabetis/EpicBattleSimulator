using FSDK.Ads;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MMSHomeScene : DTNView
{
    public Animator Animator;
    public MMSGameController GameController;
    public Button OfflineButton;
    public Button OnlineButton;
    public Button UnlockOnlineButton;
    public Button ShareButton;


    public override void InitView()
    {

    }

    public override void Show()
    {
        Animator.Play("HomeSceneAppear");
        base.Show();
        SetUpButtons();

    }


    public void OfflineButtonOnClick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        GameController.OfflineMode();
        Animator.Play("HomeSceneDisappear");
    }

    public void OnlineButtonOnClick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        GameController.OnlineMode();
        Animator.Play("HomeSceneDisappear");
    }
    public void UnlockOnlineButtonOnClick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
            AdsManager.Instance.ShowRewardedAd(() =>
            {
                UnlockOnlineButton.gameObject.SetActive(false);
                OnlineButton.gameObject.SetActive(true);
                PlayerPrefs.SetInt("OnlineUnlocked", 1);

            }, () =>
            {
                UnlockOnlineButton.gameObject.SetActive(false);
                OnlineButton.gameObject.SetActive(true);
                PlayerPrefs.SetInt("OnlineUnlocked", 1);
                DTNViewManagement.GetView<MMSNoAdsNotification>().Show();
            }, 0, FSDK.LevelDifficulty.Hard, "");
    }

    public void ShareButtonOnClick()
    {
        DTNSoundManagement.instance.Play("buttonSound");
        ShareOnSocialMedia.Instance.Share();
    }

    private void SetUpButtons()
    {
        OfflineButton.onClick.RemoveAllListeners();


        OfflineButton.onClick.AddListener(() =>
        {
            OfflineButtonOnClick();
        });

        // SetupOnlineButton();
        OnlineButton.onClick.RemoveAllListeners();
        OnlineButton.onClick.AddListener(() =>
        {
            OnlineButtonOnClick();
        });

        ShareButton.onClick.RemoveAllListeners();

        ShareButton.onClick.AddListener(() =>
        {
            ShareButtonOnClick();
        });
    }
    // void SetupOnlineButton()
    // {
    //     if (PlayerPrefs.HasKey("OnlineUnlocked") || PlayerPrefs.GetInt("OnlineUnlocked") == 1)
    //     {
    //         UnlockOnlineButton.gameObject.SetActive(false);
    //         OnlineButton.gameObject.SetActive(true);
    //     }
    //     else
    //     {
    //         UnlockOnlineButton.gameObject.SetActive(true);
    //         OnlineButton.gameObject.SetActive(false);
    //     }

    //     OnlineButton.onClick.RemoveAllListeners();
    //     OnlineButton.onClick.AddListener(() =>
    //     {
    //         OnlineButtonOnClick();
    //     });
    //     UnlockOnlineButton.onClick.RemoveAllListeners();
    //     UnlockOnlineButton.onClick.AddListener(() =>
    //     {
    //         UnlockOnlineButtonOnClick();
    //     });
    // }

}
