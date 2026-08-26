using System;
using System.Collections;
using System.Collections.Generic;
using FSDK.Ads;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MMSLoadScenes : MonoBehaviour

{
    public float LoadingTime = 10f;
    public Slider LoadingSlider;
    public GameObject LoadingText;
    public GameObject LoadingSuccess;
    public String sceneGamePlay = "GamePlay";

    private void Start()
    {
        LoadingToWaitAds();
    }

    private void LoadingToWaitAds()
    {
        SetUpLoaddingSlider();

        StartCoroutine(LoadingToWaitAdsCoroutine());
    }
    private void SetUpLoaddingSlider()
    {
        LoadingSlider.gameObject.SetActive(true);
        LoadingSlider.value = 0;
        LoadingSlider.maxValue = LoadingTime;
    }
    private IEnumerator LoadingToWaitAdsCoroutine()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneGamePlay);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone && LoadingSlider.value < LoadingTime && AdsManager.Instance.IsRewardedAdReady() == false && AdsManager.Instance.IsInterstitialAdReady() == false)
        {
            LoadingSlider.value += Time.deltaTime * 2;
            if (LoadingSlider.value >= LoadingTime)
            {
                LoadingText.SetActive(false);
                LoadingSuccess.SetActive(true);
            }
            yield return new WaitForEndOfFrame();
        }

        asyncLoad.allowSceneActivation = true;
    }
}
