using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTNResourceView : DTNView
{

    [SerializeField] string pathResource;

    [HideInInspector]
    public GameObject view;
    public override void InitView(){
        view = Resources.Load(pathResource) as GameObject;
    }
}
