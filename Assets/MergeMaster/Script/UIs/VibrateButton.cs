using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrateButton : MonoBehaviour
{
    [SerializeField] GameObject Off;
    private void Start()
    {

        SetUp();
    }
    private void SetUp()
    {
        if (PlayerPrefs.GetInt("Vibrate") == 0)
        {
            PlayerPrefs.SetInt("Vibrate", MMSGameController.Instance.isVibrate ? 1 : 0);
            Off.SetActive(true);
        }

        else
        {
            Off.SetActive(false);
        }
    }
    public void OnClick()
    {
        if (PlayerPrefs.GetInt("Vibrate") == 0)
        {
            PlayerPrefs.SetInt("Vibrate", 1);
            Off.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("Vibrate", 0);
            Off.SetActive(false);
        }
        MMSGameController.Instance.isVibrate = PlayerPrefs.GetInt("Vibrate") == 1 ? true : false;
    }

}
