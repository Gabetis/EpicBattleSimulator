using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;


public class SaveLoadUserBoard : MonoBehaviour
{
    public static SaveLoadUserBoard Instance;
    public List<CardLevelUser> CardsLevelPlayerData;
    public MMSJson MMSJson;

    private string _fileKey = "saveUserPos";
    private void Awake()
    {
        Instance = this;
        //check user use BinaryDataStream
        if (PlayerPrefs.GetInt("FirstPlay", 0) == 0)
        {
            // use save json
            PlayerPrefs.SetString("DataSave", "New");
            PlayerPrefs.SetString("UserDataJson", MMSJson.CreateUserDataJsonFromBoard(CardsLevelPlayerData));
        }
        else
        {
            if (PlayerPrefs.GetString("DataSave") == "New")
            {
                // use save json
                GameData gameData = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString("UserDataJson"));
                CardsLevelPlayerData = gameData.CardsLevelPlayerData;
            }
            else
            {
                // use BinaryDataStream
                Debug.Log("Old Data");
                PlayerPrefs.SetString("DataSave", "Old");
                if (BinaryDataStream.Exist(_fileKey))
                {
                    StartCoroutine(ReadDataFile());
                }
                else SaveFile();
            }
        }
    }

    private IEnumerator ReadDataFile()
    {
        CardsLevelPlayerData = BinaryDataStream.Read<List<CardLevelUser>>(_fileKey);
        yield return new WaitForEndOfFrame();
    }
    public void SaveFile()
    {
        if (PlayerPrefs.GetString("DataSave") == "New")
        {
            PlayerPrefs.SetString("UserDataJson", MMSJson.CreateUserDataJsonFromBoard(CardsLevelPlayerData));
        }
        else
        {
            BinaryDataStream.Save<List<CardLevelUser>>(CardsLevelPlayerData, _fileKey);
        }
    }
    [Sirenix.OdinInspector.Button]
    public void DeteleSaveFile()
    {
        BinaryDataStream.Delete(_fileKey);
    }

}