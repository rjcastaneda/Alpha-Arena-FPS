using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardGun : Gun
{
    AudioSource audioSource;
    Camera playerCamera;

    void Start()
    {
        if (!gameObject.GetComponent<AudioSource>())
        {
            Debug.Log("AudioSource not found for " + itemInfo.itemName + ", creating one now");
            gameObject.AddComponent<AudioSource>();
        }
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void AddCamera(Camera cam)
    {
        playerCamera = cam;
    }

    public override void Fire()
    {
        Debug.Log("Primary fire for " + itemInfo.itemName);
        audioSource.PlayOneShot(((GunInfo)itemInfo).primaryFireSound, 0.5f);
    }

    public override void AltFire()
    {
        Debug.Log("Alternate fire for " + itemInfo.itemName);
        //audioSource.PlayOneShot(((GunInfo)itemInfo).alternateFireSound, 0.5f);
    }
}
