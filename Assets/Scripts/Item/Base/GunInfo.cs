using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Team 8 FPS Project/Create New Gun")]
public class GunInfo : ItemInfo
{
    [Tooltip("Weapon damage.")]
    public int damage;

    [Tooltip("Rate of fire for the gun's primary attack.")]
    public float rateOfFire;

    [Tooltip("Rate of fire for the gun's alternate attack, if applicable.")]
    public float rateOfFireAlt;

    [Tooltip("Amount of rounds that can be stored in the weapon's magazine or clip.")]
    public int magSize;

    [Tooltip("Amount of rounds currently in the weapon's magazine or clip. This should be the same as mag size when the weapon is initially spawned.")]
    public int currentAmmo;

    [Tooltip("The maximum amount of ammo that the weapon can hold in reserve.")]
    public int maxAmmo;

    [Tooltip("Time it takes to finish reloading.")]
    public float reloadTime;

    [Tooltip("Time it takes to finish reloading if the weapon is empty.")]
    public float reloadTimeEmpty;

    [Tooltip("Sound used for the primary attack.")]
    public AudioClip primaryFireSound;

    [Tooltip("Sound used for the alternate attack.")]
    public AudioClip alternateFireSound;
}
