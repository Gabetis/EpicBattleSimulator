using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class MMSGamePlayScene : DTNView
{
    public Text CoinText;
    public MMSDragAndDrop dragAndDrop;
    public override void InitView()
    {

    }

    public override void Show()
    {
        base.Show();
        dragAndDrop.OnDrop();
        SetUserCoin(long.Parse(PlayerPrefs.GetString("UserCurrentCoin")));
    }


    public void SetUserCoin(long value)
    {
        PlayerPrefs.SetString("UserCurrentCoin", value.ToString());
        CoinText.text = "" + DTNNumber.FomatCoin(value);
    }
}
