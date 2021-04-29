using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  Script placed on the HealthBar Object in the Player HUD.
 *    
 */

public class HealthBarHUD : MonoBehaviour
{
    //Health Bar Values
    public float maxHealth;
    public float currentHealth;

    private Slider HealthSlider;
    private PlayerData playerData;

    void Start()
    {
        HealthSlider = this.gameObject.GetComponent<Slider>();
        playerData = transform.parent.transform.parent.GetComponent<PlayerData>();
    }

    private void Update()
    {
        SetHealthBar();
    }

    public void SetHealthBar()
    {
        currentHealth = playerData.health;
        maxHealth = playerData.maxHealth;
        HealthSlider.maxValue = maxHealth;
        HealthSlider.value = currentHealth;
    }
}
