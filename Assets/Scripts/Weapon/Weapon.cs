using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;

    [Header("Weapon Stats")]
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

    [Tooltip("The current amount of ammo that the weapon has in reserve. This should be the same as max reserve ammo when the weapon is initially spawned.")]
    public int currentReserveAmmo;

    [Tooltip("The maximum amount of ammo that the weapon can hold in reserve.")]
    public int maxReserveAmmo;

    [Tooltip("If true, reloading will never deplete ammo from the reserve ammo pool.")]
    public bool infiniteReserveAmmo;

    [Tooltip("Time it takes to finish reloading.")]
    public float reloadTime;

    [Tooltip("Time it takes to finish reloading if the weapon is empty.")]
    public float reloadTimeEmpty;

    [Header("Weapon Sounds")]
    [Tooltip("Sound used for the primary attack.")]
    public AudioClip primaryFireSound;

    [Tooltip("Sound used when reloading.")]
    public AudioClip reloadSound;

    [Tooltip("Sound used when reloading while empty.")]
    public AudioClip emptyReloadSound;

    [Header("Weapon Offset")]
    [Tooltip("Position of the weapon viewmodel relative to the camera.")]
    public Vector3 offsetVector;

    [Tooltip("Used for tracking rate of fire. Do not touch!")]
    public float nextFire;
}
