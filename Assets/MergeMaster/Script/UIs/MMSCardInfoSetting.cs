using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMSCardInfoSetting : MonoBehaviour
{
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;
    [Sirenix.OdinInspector.Button]
    public void SettingCard()
    {
        
        transform.localPosition = Position;
        transform.localEulerAngles = Rotation;
        transform.localScale = Scale;
    }
}
