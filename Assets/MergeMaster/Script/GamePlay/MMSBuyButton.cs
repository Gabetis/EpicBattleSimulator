using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MMSBuyButton : MonoBehaviour
{
    [SerializeField] GameObject FullCardUIObj;
    [SerializeField] GameObject AdsUIObj;
    [SerializeField] GameObject CoinText;

    public void DefaultUI(long coin)
    {
        CoinText.SetActive(true);
        CoinText.GetComponent<Text>().text = DTNNumber.FomatCoin(coin);
        AdsUIObj.SetActive(false);
        FullCardUIObj.SetActive(false);
    }
    public void AdsUI()
    {
        CoinText.SetActive(false);
        AdsUIObj.SetActive(true);
        FullCardUIObj.SetActive(false);
    }

    public void FullCard(long coin)
    {
        CoinText.SetActive(false);
        AdsUIObj.SetActive(false);
        FullCardUIObj.SetActive(true);
        FullCardUIObj.GetComponentInChildren<Text>().text = DTNNumber.FomatCoin(coin);
    }
}
