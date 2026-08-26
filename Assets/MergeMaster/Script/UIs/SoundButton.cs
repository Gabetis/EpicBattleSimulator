using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButton : MonoBehaviour
{
    [SerializeField] GameObject Off;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ShowStatus(bool isActive)
    {
        Off.SetActive(!isActive);
    }
}
