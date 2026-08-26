using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMSKeyControll : MonoBehaviour
{
    public GameObject[] Keys;
    public Text KeyLeftText;

    public void SetKeys(int value)
    {
        if (value <= 0)
        {
            KeyLeftText.gameObject.SetActive(false);
            SetKeyObject(value);
        }
        else
        {
            KeyLeftText.gameObject.SetActive(true);
            KeyLeftText.text = DTNLocalizationSystem.GetText("You have ") + value + DTNLocalizationSystem.GetText(" keys left!");
            SetKeyObject(value);
        }
    }

    void SetKeyObject(int value)
    {
        for (int i = 0; i < Keys.Length; i++)
        {
            if (i < value)
            {
                Keys[i].SetActive(true);
            }
            else
            {
                Keys[i].SetActive(false);
            }
        }
    }
}
