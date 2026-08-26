using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DTNView : MonoBehaviour
{
    [HideInInspector]
    public bool isInit = false;
    public virtual void Initialize()
    {
        InitView();
        isInit = true;
    }

    public void InitIfNeed()
    {
        if (!isInit)
        {
            Initialize();
        }
    }

    public abstract void InitView();

    public virtual void WillHide()
    {

    }
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        // Debug.Log("Show");
        gameObject.SetActive(true);
    }

    public virtual void WillShow()
    {

    }

    public virtual void Destroy()
    {

    }
}
