using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MMSCardInfo
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
    public string NickName;

    [SerializeField]
    public bool IsWarrior = true;

    [SerializeField]
    public string Describe;

    [SerializeField]
    public Sprite Icon;

    [SerializeField]
    public string Address = "Cards/";
}
