using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMSCountingChest : MonoBehaviour
{
    public TextMesh[] CountingTexts;

    private void OnEnable()
    {
        StartCoroutine(EnumCounting());
    }

    IEnumerator EnumCounting()
    {
        SetCountingText("" + 5);
        yield return new WaitForSeconds(1f);
        SetCountingText("" + 4);
        yield return new WaitForSeconds(1f);
        SetCountingText("" + 3);
        yield return new WaitForSeconds(1f);
        SetCountingText("" + 2);
        yield return new WaitForSeconds(1f);
        SetCountingText("" + 1);
        yield return new WaitForSeconds(1f);
        SetCountingText(DTNLocalizationSystem.GetText("Time Up"));
    }

    void SetCountingText(string value)
    {
        for (int i = 0; i < CountingTexts.Length; i++)
        {
            CountingTexts[i].text = value;
        }
    }
}
