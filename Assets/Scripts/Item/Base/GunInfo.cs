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

    [Tooltip("Amount of rounds currently in the weapon's magazine or clip. This should be the same as mag size when the weapon is spawned.")]
    public int currentMagSize;

    [Tooltip("Index of the ammo pool that the weapon draws from. To be determined.")]
    public int ammoType;

    [Tooltip("Time it takes to finish reloading.")]
    public float reloadTime;

    [Tooltip("Time it takes to finish reloading if the weapon is empty.")]
    public float reloadTimeEmpty;
}
