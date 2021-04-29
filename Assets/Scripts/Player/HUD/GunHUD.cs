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
}
