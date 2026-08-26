using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MMSMapInfo
{
    string Title
    {
        get
        {
            return Name;
        }
    }

    [SerializeField]
    public string Name;

    [SerializeField]
    public Sprite Icon;

    [SerializeField]
    public Sprite PopUpImage;

    [SerializeField]
    public string Address = "Maps/";
}
