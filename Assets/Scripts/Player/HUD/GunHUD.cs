using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Script placed on the GunHUD object of the playerHud; 
 */


public class GunHUD : MonoBehaviour
{
    public int currentAmmo;
    public int maxAmmo;

    private TextMeshProUGUI AmmoText;
    private TextMeshProUGUI GunText;
    private Player_Inventory playerInventory;
    public Image hitmarker;

    private Coroutine hitmarkerRoutine;
    // Start is called before the first frame update
    void Start()
    {
        AmmoText = transform.Find("Ammo(TMP)").gameObject.GetComponent<TextMeshProUGUI>();
        GunText = transform.Find("GunName(TMP)").gameObject.GetComponent<TextMeshProUGUI>();
        playerInventory = transform.parent.transform.parent.GetComponent<Player_Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrentWeapon();
    }

    public void UpdateCurrentWeapon()
    {
        Weapon cw = playerInventory.GetCurrentWeapon();
        currentAmmo = cw.currentAmmo;
        maxAmmo = cw.currentReserveAmmo;
        AmmoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        GunText.text = cw.weaponName;
    }

    public void ShowHitmarker()
    {
        if (hitmarkerRoutine != null)
        {
            StopCoroutine(hitmarkerRoutine);
        }
        hitmarkerRoutine = StartCoroutine(FadeHitmarker(2.0f));
    }

    private IEnumerator FadeHitmarker(float speed)
    {
        hitmarker.color = new Color(hitmarker.color.r, hitmarker.color.g, hitmarker.color.b, 1);
        while (hitmarker.color.a > 0.0f)
        {
            hitmarker.color = new Color(hitmarker.color.r, hitmarker.color.g, hitmarker.color.b, hitmarker.color.a - (Time.deltaTime * speed));
            yield return null;
        }
    }
}
