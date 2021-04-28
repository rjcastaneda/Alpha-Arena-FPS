using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardGun : Gun
{
    AudioSource audioSource;
    Camera playerCamera;

    float nextFire = 0f;

    //debug
    LineRenderer lr;

    void Start()
    {
        if (!gameObject.GetComponent<AudioSource>())
        {
            Debug.Log("AudioSource not found for " + itemInfo.itemName + ", creating one now");
            gameObject.AddComponent<AudioSource>();
        }
        audioSource = gameObject.GetComponent<AudioSource>();

        //debugging
        lr = gameObject.AddComponent<LineRenderer>();
        lr.useWorldSpace = true;
        lr.enabled = false;
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;
    }

    public void AddCamera(Camera cam)
    {
        playerCamera = cam;
    }

    public override void Fire()
    {
        if (((GunInfo)itemInfo).currentAmmo > 0)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time;
                nextFire += ((GunInfo)itemInfo).rateOfFire;

                //Gross debug raycast stuff start
                ////////////////////////////////
                Debug.Log("Primary fire for " + itemInfo.itemName);

                Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                ray.origin = playerCamera.transform.position;

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.Log("Hit " + hit.collider.gameObject.name);
                    lr.enabled = true;
                    lr.SetPosition(0, ray.origin - new Vector3(0, 0.5f, 0));
                    lr.SetPosition(1, hit.point);
                }
                ////////////////////////////////
                //Gross debug raycast stuff end

                ((GunInfo)itemInfo).currentAmmo -= 1;

                audioSource.PlayOneShot(((GunInfo)itemInfo).primaryFireSound, 0.5f);
            }
        }
    }

    public override void AltFire()
    {
        Debug.Log("Alternate fire for " + itemInfo.itemName);
        //audioSource.PlayOneShot(((GunInfo)itemInfo).alternateFireSound, 0.5f);
    }

}
