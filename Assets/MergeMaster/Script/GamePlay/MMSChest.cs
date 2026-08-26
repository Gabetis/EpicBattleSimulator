using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMSChest : DTNSingletonMB<MMSChest>
{
    public MMSGameController GameController;
    public Animator GoldChestAni;
    public ParticleSystem Effect;
    public long EarnCoin;
    public GameObject Counting;
    public bool isTouch = false;

    private void OnEnable()
    {
        isTouch = false;
    }

    private void OnDisable()
    {
        isTouch = false;
    }

    private void OnMouseDown()
    {
        if (!isTouch)
        {
            isTouch = true;
            Counting.SetActive(false);
            DTNViewManagement.GetView<MMSChestScene>().Hide();
            DTNViewManagement.GetView<MMSOpenChestScene>().Show();
        }
    }

    public void OpenChest()
    {
        GoldChestAni.SetBool("OpenChest", true);
        GameController.OnGameWinScene();
    }

    public void OnChestOpen()
    {
        Effect.Play();
        Counting.SetActive(true);
        DTNViewManagement.GetView<MMSChestScene>().Show();
        MMSCameraControl.Instance.ShakeCamera();
    }
}
