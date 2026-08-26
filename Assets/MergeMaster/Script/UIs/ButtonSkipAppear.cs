using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSkipAppear : MonoBehaviour
{
    [SerializeField] GameObject skipBtn;
    [SerializeField] float timeToAppear = 5f;
    private float timeTemp;
    public void SetUp()
    {
        timeTemp = timeToAppear;
        skipBtn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeTemp > 0)
        {
            timeTemp -= Time.deltaTime;
        }
        else
        {
            Appear();
        }
    }
    private void Appear()
    {
        skipBtn.SetActive(true);
    }
}
