using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetHP(float health)
    {
        slider.value = health;
    }

    public void SetMaxHP(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
}
