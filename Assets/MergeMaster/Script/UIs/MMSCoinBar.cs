using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class MMSCoinBar : MonoBehaviour
{
    public long BaseCoin;
    public long PlusCoin;
    public Animator Animator;
    public Action<long> OnSetPlusCoin;

    private void OnEnable()
    {
        Animator.enabled = true;
    }

    public void OnStart()
    {
        Animator.enabled = true;
    }

    public void SetPlusCoin(int value)
    {
        PlusCoin = BaseCoin * value;
        if (OnSetPlusCoin != null)
        {
            OnSetPlusCoin(PlusCoin);
        }
    }

    public void OnStop()
    {
        Animator.enabled = false;
    }
}
