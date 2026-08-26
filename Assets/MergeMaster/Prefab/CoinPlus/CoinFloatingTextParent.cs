using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using TMPro;

public class CoinFloatingTextParent : MonoBehaviour
{
    // public float timeToDestroy = 1f;
    [SerializeField] private TextMesh cointext;
    [SerializeField] private TextMesh cointext2;
    // private float timeTemp;
    // private void Start()
    // {
    //     timeTemp = timeToDestroy;
    // }
    // private void Update()
    // {
    //     timeTemp -= Time.deltaTime;
    //     if (timeTemp <= 0)
    //     {
    //         Destroy(gameObject);
    //     }
    // }
    public void UpdateText(int coin)
    {
        cointext.text = coin.ToString();
        cointext2.text = coin.ToString();
    }


}
