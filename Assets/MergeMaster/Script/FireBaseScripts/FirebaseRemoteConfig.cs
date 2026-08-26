using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Firebase;
using Firebase.RemoteConfig;
using System.Threading.Tasks;

public class FirebaseRemoteConfig
{

    // _________________________________________
    #region シングルトン
    private static FirebaseRemoteConfig _Instance = new FirebaseRemoteConfig();
    public static FirebaseRemoteConfig Instance()
    {
        return _Instance;
    }
    private FirebaseRemoteConfig()
    {
    }
    #endregion
    // _________________________________________

    // _________________________________________
    #region 外部取得用Properties
    /// <summary>
    /// RemoteConfigのバージョン
    /// </summary>
    private string _ConfigVersion = "0.0.1";
    public string ConfigVersion
    {
        // set { _ConfigVersion = value; }
        get { return _ConfigVersion; }
    }

    private int _ConfigMode = 0;
    public int ConfigMode
    {
        // set { _ConfigVersion = value; }
        get { return _ConfigMode; }
    }

    //private int _ConfigActiveModePoppyFNF = 0;
    //public int ConfigActiveModePoppyFNF
    //{
    //	// set { _ConfigVersion = value; }
    //	get { return _ConfigActiveModePoppyFNF; }
    //}

    private int _ConfigPercent = 0;
    public int ConfigPercent
    {
        // set { _ConfigVersion = value; }
        get { return _ConfigPercent; }
    }
    private int _ConfigModeGame = 0;
    public int ConfigModeGame
    {
        // set { _ConfigVersion = value; }
        get { return _ConfigModeGame; }
    }

    private int _ConfigAds = 0;
    public int ConfigAds
    {
        // set { _ConfigVersion = value; }
        get { return _ConfigAds; }
    }

    #endregion
    // _________________________________________


    /// <summary>
    /// サーバとの同期を行います
    /// </summary>
    /// <param name="completionHandler">同期完了時のコールバック</param>
    public void fetch(Action<bool> completionHandler)
    {
        // TODO: RELEASE時にここを外す
        // var settings = Firebase.RemoteConfig.FirebaseRemoteConfig.Settings;
        // settings.IsDeveloperMode = true;
        // Firebase.RemoteConfig.FirebaseRemoteConfig.Settings = settings;

        // if ()
        System.Threading.Tasks.Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(new System.TimeSpan(0));

        fetchTask.ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                completionHandler(false);
            }
            else
            {
            }
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
            RefrectProperties();
            completionHandler(true);
        });
    }

    /// <summary>
    /// RemoteConfigからFetchした情報をフィールド反映します
    /// </summary>
    private void RefrectProperties()
    {
        _ConfigVersion = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("version").StringValue;
        Debug.Log("config version = " + _ConfigVersion);
        //
        _ConfigMode = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("mode").DoubleValue;
        OptionsChooseGame.Instance.ChooseIndex = _ConfigMode;
        OptionsChooseGame.Instance.SetupGameType();
        Debug.Log("heatmeter_minus_fan = " + _ConfigMode);
        //
        //_ConfigActiveModePoppyFNF = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("modepoppyfnfonbottom").DoubleValue;
        //Debug.Log("modepoppyfnf = " + _ConfigActiveModePoppyFNF);
        //
        _ConfigPercent = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("percentAds").DoubleValue;
        // Debug.Log("heatmeter_minus_fan = " + _ConfigMode);
        //
        _ConfigAds = (int)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("turnonads").DoubleValue;
        Debug.Log("ads = " + _ConfigAds);
    }
    // Get firebase config



    private void ConfigFetchComplete(Task fetchTask)
    {

    }

}
