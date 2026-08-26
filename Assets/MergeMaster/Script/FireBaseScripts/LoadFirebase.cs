using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.RemoteConfig;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LoadFirebase : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject[] mode0;
    //public GameObject[] mode1;

    //public System.Action loadFinishAction;
    //public System.Action loadFailAction;

    //public UnityEvent loadFinish;
    public UnityEvent loadFail;

    public static LoadFirebase use;

    bool hasLoadBefor = false;
    public int TurnOnAdsVar;
    public int modegameVar;
    public int percentAdsVar;
    void Awake()
    {
        use = this;
        DontDestroyOnLoad(gameObject);
        //PlayerPrefs.SetInt("cauhinhdata", 1);
        //if (PlayerPrefs.GetInt("cauhinhdata",0)==1){
        //    panelDownload.SetActive(false);
        //}
    }

    void Start()
    {
        LoadData(); 
        // if (PlayerPrefs.GetInt("cauhinhdata", 0) == 0)
        // {
        //     panelDownload.SetActive(true);
        // }
        // else
        // {
        //     LoadData();
        //     NextScene();
        // }

    }

    public void LoadConfig()
    {
        // #if !UNITY_EDITOR
        FirebaseRemoteConfig.Instance().fetch(Fetched);
        Debug.Log("LoadConfig");
        // #endif
    }

    void Fetched(bool success)
    {
        //if (FirebaseRemoteConfig.Instance().ConfigMode == 0)
        //{

        //}
        //else
        //{

        //}
        if (success == false)
        {
            ShowError();
        }

        //PlayerPrefs.SetInt("ModePoppyfnfOnBottom", FirebaseRemoteConfig.Instance().ConfigActiveModePoppyFNF);
        PlayerPrefs.SetInt("TurnOnAds", FirebaseRemoteConfig.Instance().ConfigAds);
        PlayerPrefs.SetInt("modegame", FirebaseRemoteConfig.Instance().ConfigMode);
        Debug.Log("ConfigMode" + FirebaseRemoteConfig.Instance().ConfigMode);
        PlayerPrefs.SetInt("percentAds", FirebaseRemoteConfig.Instance().ConfigPercent);
        TurnOnAdsVar = FirebaseRemoteConfig.Instance().ConfigAds;
        modegameVar = FirebaseRemoteConfig.Instance().ConfigMode;
        percentAdsVar = FirebaseRemoteConfig.Instance().ConfigPercent;
    }

    //public static bool ModePoppyfnfOnBottom()
    //{
    //    return PlayerPrefs.GetInt("ModePoppyfnfOnBottom", 0) == 1;
    //}
    public bool TurnOnAds()
    {
        return PlayerPrefs.GetInt("TurnOnAds", 0) == 1;
    }
    public int modeGame()
    {
        return PlayerPrefs.GetInt("modegame", 0);
    }

    public int percentAds()
    {
        return PlayerPrefs.GetInt("percentAds", 30);
    }

    public GameObject panelDownload;
    public Image processImage;
    public Text percent;

    public float _percent = 0;

    public bool isError = false;

    // public static LoadFirebaseProcessing use;



    public float tempPercent = 0;
    // Start is called before the first frame update
    // void Awake() {
    //     use = this;
    //     if (PlayerPrefs.GetInt("cauhinhdata",0)==1){
    //         gameObject.SetActive(false);
    //     }
    // }
    // void Start()
    // {
    //     _LoadData();
    // }

    public void LoadData()
    {
        _percent = 0;
        tempPercent = 0;
        isError = false;
        LoadConfig();

        StartCoroutine(fakeLoading());
    }

    void _LoadData()
    {
        _percent = 0;
        tempPercent = 0;
        isError = false;

        StartCoroutine(fakeLoading());
    }

    public void ShowError()
    {
        // _percent = 0;
        // tempPercent = 0;
        isError = true;
        loadFail.Invoke();
        StopAllCoroutines();
    }

    IEnumerator fakeLoading()
    {

        yield return null;

        for (int i = 0; i < 15; i++)
        {

            yield return new WaitForSeconds(UnityEngine.Random.Range(1, 10) / 10f);
            _percent += 1f / 15f;
        }

    }

    void Update()
    {
        if (!isError)
        {
            tempPercent = Mathf.Lerp(tempPercent, _percent, 3f * Time.deltaTime);
            if (percent != null)
                percent.text = Math.Round(tempPercent * 100f, 1) + "%";
            if (processImage != null)
                processImage.fillAmount = tempPercent;
            if (tempPercent + 0.01f >= 1f)
            {
                PlayerPrefs.SetInt("cauhinhdata", 1);
                // panelDownload.SetActive(false);
                NextScene();
            }
        }
        else
        {
            if (percent != null)
                percent.text = "Error! Tap to try again";
        }
    }

    void NextScene()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
