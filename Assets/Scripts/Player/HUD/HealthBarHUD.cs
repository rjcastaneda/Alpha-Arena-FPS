using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHUD : MonoBehaviour
{
    //Health Bar Values
    public float maxHealth;
    public float currentHealth;

    private Slider HealthSlider;

    void Start()
    {
        HealthSlider = this.gameObject.GetComponent<Slider>();
        //MaxHealth = PlayerData.maxHealth;
        //CurrentHealth = PlayerData.currentHealth;
    }

    private void Update()
    {
        SetHealthBar();
    }

    public void SetHealthBar()
    {
        //currentHealth = PlayerData.currentHealth;
        //MaxHealth = PlayerData.maxHealth;
        HealthSlider.maxValue = maxHealth;
        HealthSlider.value = currentHealth;
    }
}
