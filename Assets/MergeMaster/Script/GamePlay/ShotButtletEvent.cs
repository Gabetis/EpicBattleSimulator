using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotButtletEvent : MonoBehaviour
{
    public MMSArchery archery;
    public void ShowBullet()
    {
        archery.ShotBullet();
    }
}
