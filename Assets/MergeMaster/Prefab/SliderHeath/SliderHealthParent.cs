using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderHealthParent : MonoBehaviour
{
    [SerializeField] public Slider sliderHealth;
    public Image FillImage;

    public void SetMaxValue(float value)
    {
        sliderHealth.maxValue = value;
    }
    public void SetValue(float value)
    {
        sliderHealth.value = value;
    }
    public void SetFillImage(Color color)
    {
        FillImage.color = color;
    }
}
