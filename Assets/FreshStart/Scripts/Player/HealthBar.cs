using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    public void SetHealth(float health)
    {
        healthSlider.value = health;

        fill.color = gradient.Evaluate(healthSlider.normalizedValue);

    }
}
