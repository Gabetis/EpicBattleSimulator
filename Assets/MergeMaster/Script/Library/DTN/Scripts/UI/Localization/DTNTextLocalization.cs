using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DTNTextLocalization : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Text text = GetComponent<Text>();
        TextMesh textMesh = GetComponent<TextMesh>();
        if (text != null)
        {
            text.text = DTNLocalizationSystem.GetText(text.text);
        }
        if (textMesh != null)
        {
            textMesh.text = DTNLocalizationSystem.GetText(textMesh.text);
        }
    }
}
