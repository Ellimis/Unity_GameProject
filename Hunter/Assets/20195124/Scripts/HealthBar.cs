using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Text text;
    public Gradient gradient;
    public Image fill;


    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        text.text = health.ToString() + " / " + health.ToString();
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        text.text = health.ToString() + " / " + slider.maxValue.ToString();
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
