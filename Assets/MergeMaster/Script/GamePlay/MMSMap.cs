using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMSMap : MonoBehaviour
{
    public Material SkyboxMaterial;
    void Start()
    {
        RenderSettings.skybox = SkyboxMaterial;
    }

    void Update()
    {
        //SkyboxMaterial.SetFloat("_Rotation", Time.time * 5f);
    }
}
