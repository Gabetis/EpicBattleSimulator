using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadFirebaseProcessing : MonoBehaviour
{

    public Image processImage;
    public Text percent;

    public float _percent = 0;

    public bool isError = false;

    public static LoadFirebaseProcessing use;



    public float tempPercent = 0;
    // Start is called before the first frame update
    void Awake() {
        use = this;
        if (PlayerPrefs.GetInt("cauhinhdata",0)==1){
            gameObject.SetActive(false);
        }
    }
    void Start()
    {
        _LoadData();
    }

    public void LoadData(){
        _percent = 0;
        tempPercent = 0;
        isError = false;
        LoadFirebase.use.LoadConfig();
        StartCoroutine(fakeLoading());
    }

    void _LoadData(){
        _percent = 0;
        tempPercent = 0;
        isError = false;
        StartCoroutine(fakeLoading());
    }

    public void ShowError(){
        // _percent = 0;
        // tempPercent = 0;
        isError = true;
        StopAllCoroutines();
    }

    IEnumerator fakeLoading(){

        yield return null;

        for (int i = 0; i < 15; i++)
        {

            yield return new WaitForSeconds(UnityEngine.Random.Range(1,10)/10f);
            _percent+= 1f/15f;
        }

    }

    void Update()
    {
        if (!isError){
            tempPercent = Mathf.Lerp(tempPercent,_percent,3f*Time.deltaTime);
            percent.text = Math.Round(tempPercent*100f,1) + "%";
            processImage.fillAmount = tempPercent;
            if (tempPercent+0.01f >= 1f){
                PlayerPrefs.SetInt("cauhinhdata",1);
                gameObject.SetActive(false);
            }
        }else{
            percent.text = "Error! Tap to try again";
        }
        
    }
}
