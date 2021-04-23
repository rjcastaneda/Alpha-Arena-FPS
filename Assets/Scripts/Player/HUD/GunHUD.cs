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
    public string GunName;

    private TextMeshProUGUI AmmoText;
    private TextMeshProUGUI GunText;
    
    // Start is called before the first frame update
    void Start()
    {
        AmmoText = transform.Find("Ammo(TMP)").gameObject.GetComponent<TextMeshProUGUI>();
        GunText = transform.Find("GunName(TMP)").gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrentWeapon();
    }

    public void UpdateCurrentWeapon()
    {
        //Need to integrate weapon data to the Gun Hud; 
        //currentAmmo = Weapon.currentlyHeld.currentAmmo
        //maxAmmo = Weapon.currentlyHeld.clipSize
        AmmoText.text = currentAmmo.ToString() + "/" + maxAmmo.ToString();
        GunText.text = GunName;
    }
}
