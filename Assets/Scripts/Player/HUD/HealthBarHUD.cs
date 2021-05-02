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
    public Image damageGradient;

    private Coroutine gradientRoutine;

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

    public void ShowDamageFeedback()
    {
        if (gradientRoutine != null)
        {
            StopCoroutine(gradientRoutine);
        }
        gradientRoutine = StartCoroutine(FadeDamageGradient(2.0f));
    }

    private IEnumerator FadeDamageGradient(float speed)
    {
        damageGradient.color = new Color(damageGradient.color.r, damageGradient.color.g, damageGradient.color.b, 0.5f);
        while (damageGradient.color.a > 0.0f)
        {
            damageGradient.color = new Color(damageGradient.color.r, damageGradient.color.g, damageGradient.color.b, damageGradient.color.a - (Time.deltaTime * speed));
            yield return null;
        }
    }
}
