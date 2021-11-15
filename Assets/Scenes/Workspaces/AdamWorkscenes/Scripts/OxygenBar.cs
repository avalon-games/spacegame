using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    public Slider slider;

    public void setOxygenBar(int oxygen) {
        slider.value = oxygen;
    }

    public void setMaxOxygenBar(int maxOxygen) {
        slider.maxValue = maxOxygen;
        slider.value = maxOxygen;
    }

    public void decreaseOxygenBar(float val) {
        slider.value = slider.value - val;
    }
}
