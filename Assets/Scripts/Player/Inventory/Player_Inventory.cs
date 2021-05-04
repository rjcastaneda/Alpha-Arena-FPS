using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Player_Inventory : MonoBehaviourPunCallbacks
{
    [Header("Player Options")]
    [Tooltip("Automatically swap to a new weapon when picked up.")]
    [SerializeField] private bool AutoSwapNewWeapon = false;

    [Header("Inventory")]
    [Tooltip("Primary weapon that the player starts with.")]
    [SerializeField] private GameObject StartingWeaponPrimary;

    [Tooltip("Secondary weapon that the player starts with.")]
    [SerializeField] private GameObject StartingWeaponSecondary;

    [Tooltip("The game object/point that weapons attach to.")]
    [SerializeField] private GameObject inventoryObject;
    private Vector3 inventoryObjectPos;

    [SerializeField] private List<GameObject> inventory = new List<GameObject>();


    [SerializeField] private Camera Camera; //TODO: some sort of global class that handles basic things like the camera?

    private int itemIndex;
    private int previousItemIndex = -1;

    private float nextFire = 0f;
    private float reloadTime;

    private Coroutine reloadCoroutine = null;
    private Coroutine lerpCoroutine;

    private PhotonPlayer photonPlayer;

    private GunHUD gunHUD;

    //debug
    LineRenderer lr;

    void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (StartingWeaponPrimary != null)
        {
            PickUpItem(Instantiate(StartingWeaponPrimary));
        }
        if (StartingWeaponSecondary != null)
        {
            PickUpItem(Instantiate(StartingWeaponSecondary));
        }
        if (inventory.Count > 0)
        {
            EquipItem(inventory.Count - 1);
        }
        else
            Debug.LogError("No primary or secondary starting weapon specified!");

        gunHUD = transform.Find("PlayerHUD").transform.Find("GunHUD").GetComponent<GunHUD>();
        inventoryObjectPos = inventoryObject.transform.localPosition;

        //debugging
        lr = gameObject.AddComponent<LineRenderer>();
        lr.useWorldSpace = true;
        lr.enabled = false;
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;
    }

    void EquipItem(int index)
    {
        if (index == previousItemIndex)
        {
            return;
        }

        itemIndex = index;
        inventory[itemIndex].SetActive(true);
        InterruptReload();

        if (previousItemIndex != -1)
        {
            inventory[previousItemIndex].SetActive(false);
        }

        previousItemIndex = itemIndex;

        if (photonView.IsMine)
        {
            
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!photonView.IsMine && targetPlayer == photonView.Owner)
        {
            //EquipItem((int)changedProps["itemIndex"]);
        }
    }

    private int FindWeaponIndex(string wepName)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].GetComponent<Weapon>().weaponName == wepName)
            {
                return i;
            }
        }
        return -1;
    }

    public Weapon GetCurrentWeapon()
    {
        Weapon cw = inventory[itemIndex].GetComponent<Weapon>();
        return cw;
    }

    public void PickUpItem(GameObject obj)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        int wepIndex = FindWeaponIndex(obj.GetComponent<Weapon>().weaponName);
        if (wepIndex != -1)
        {
            AddAmmoToWeapon(wepIndex, obj.GetComponent<Weapon>().magSize);
            Debug.Log("Picked up ammo for " + obj.GetComponent<Weapon>().weaponName);
            Destroy(obj);
            return;
        }
        obj.transform.SetParent(inventoryObject.transform); //Parent the weapon to our inventory object
        obj.transform.localPosition = obj.GetComponent<Weapon>().offsetVector;  //Offset the viewmodel accordingly
        obj.transform.rotation = inventoryObject.transform.rotation;

        //Disable shadow casting for first person weapons
        if (obj.transform.Find("Object"))
        {
            obj.transform.Find("Object").GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;   
        }
        else
            Debug.Log(obj + " does not have a child named Object!");

        //Enable viewmodel arms for first person
        if (obj.transform.Find("Viewmodel"))
        {
            obj.transform.Find("Viewmodel").gameObject.SetActive(true);
        }
        else
            Debug.Log(obj + " does not have child named Viewmodel!");

        inventory.Add(obj);

        Debug.Log("Picked up " + obj.GetComponent<Weapon>().weaponName);

        obj.SetActive(false);

        if (AutoSwapNewWeapon)
        {
            EquipItem(inventory.Count - 1);
        }
    }

    private void AddAmmoToWeapon(int index, int amount)
    {
        Weapon WC = inventory[index].GetComponent<Weapon>();
        if (WC.currentReserveAmmo + amount > WC.maxReserveAmmo)
        {
            WC.currentReserveAmmo = WC.maxReserveAmmo;
        }
        else
            WC.currentReserveAmmo += amount;
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        CheckSwapWeapon();
        CheckReloadWeapon();

        if (reloadCoroutine == null)
        {
            CheckFireWeapon();
        }
    }

    void CheckSwapWeapon()
    {
        if (Time.time > nextFire)   //Disables weapon swapping if your current weapon isn't ready to fire
        {
            //Number keys
            for (int i = 0; i < inventory.Count; i++)
            {
                if (Input.GetKeyDown((i + 1).ToString()))
                {
                    EquipItem(i);
                    break;
                }
            }

            //Scroll wheel weapon swapping
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
            {
                if (itemIndex >= inventory.Count - 1)
                {
                    EquipItem(0);
                }
                else
                {
                    EquipItem(itemIndex + 1);
                }
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
            {
                if (itemIndex <= 0)
                {
                    EquipItem(inventory.Count - 1);
                }
                else
                {
                    EquipItem(itemIndex - 1);
                }
            }
        }
    }

    void CheckFireWeapon()
    {
        if (Input.GetMouseButton(0))
        {
            PrimaryFire();
        }
    }

    void CheckReloadWeapon()
    {
        Weapon cw = inventory[itemIndex].GetComponent<Weapon>();

        if (Input.GetKeyDown("r"))
        {
            if ( (cw.currentAmmo != cw.magSize) && (cw.currentReserveAmmo != 0) && (reloadCoroutine == null) )
            {
                Debug.Log("Starting reload for " + cw.weaponName);
                inventory[itemIndex].GetComponent<AudioSource>().Stop();
                if (cw.currentAmmo == 0)
                {
                    reloadCoroutine = StartCoroutine(ReloadWeapon(cw.reloadTimeEmpty)); //Empty reload
                    inventory[itemIndex].GetComponent<AudioSource>().PlayOneShot(cw.emptyReloadSound, 0.5f);
                }
                else
                {
                    reloadCoroutine = StartCoroutine(ReloadWeapon(cw.reloadTime));  //Normal reload
                    inventory[itemIndex].GetComponent<AudioSource>().PlayOneShot(cw.reloadSound, 0.5f);
                }
                Vector3 lower = new Vector3(inventoryObject.transform.localPosition.x, inventoryObject.transform.localPosition.y - 5, inventoryObject.transform.localPosition.z);
                lerpCoroutine = StartCoroutine(TransformWithLerp(inventoryObject.transform.localPosition, lower, 0.7f));
            }
        }
    }

    void InterruptReload()
    {
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null; //We use this to track if the player is reloading or not
        }
        if (lerpCoroutine != null)
        {
            StopCoroutine(lerpCoroutine);
            lerpCoroutine = null;
            inventoryObject.transform.localPosition = inventoryObjectPos;
            inventory[itemIndex].GetComponent<AudioSource>().Stop();
        }
    }

    void PrimaryFire()
    {
        Weapon cw = inventory[itemIndex].GetComponent<Weapon>();
        if (cw.currentAmmo > 0)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time;
                nextFire += cw.rateOfFire;

                Ray ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                ray.origin = Camera.transform.position;

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    //Check so you can't shoot yourself
                    if (hit.collider.gameObject.GetComponent<PhotonView>() && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                    {
                        hit.collider.gameObject.GetComponent<PhotonPlayer>()?.TakeDamage(cw.damage);
                        gunHUD.ShowHitmarker();
                    }
                    
                    //DEBUGGING!!!
                    lr.enabled = true;
                    lr.SetPosition(0, ray.origin - new Vector3(0, 0.5f, 0));
                    lr.SetPosition(1, hit.point);
                    //DEBUGGING!!!
                }

                cw.currentAmmo -= 1;

                inventory[itemIndex].GetComponent<AudioSource>().PlayOneShot(cw.primaryFireSound, 0.5f);
                Vector3 recoil = new Vector3(inventoryObject.transform.localPosition.x, inventoryObject.transform.localPosition.y - 0.05f, inventoryObject.transform.localPosition.z - 0.2f);
                lerpCoroutine = StartCoroutine(TransformWithLerp(recoil, inventoryObjectPos, 0.08f));
            }
        }
    }

    

    void Reload()
    {
        Weapon cw = inventory[itemIndex].GetComponent<Weapon>();
        Debug.Log("Reloaded " + cw.weaponName);
        if (cw.currentAmmo != cw.magSize)
        {
            int addedAmmo = Mathf.Clamp(cw.currentReserveAmmo, 0, cw.magSize - cw.currentAmmo);
            cw.currentAmmo += addedAmmo;
            if (!cw.infiniteReserveAmmo)
            {
                cw.currentReserveAmmo -= addedAmmo;
            }
        }
        reloadCoroutine = null;
        lerpCoroutine = StartCoroutine(TransformWithLerp(inventoryObject.transform.localPosition, inventoryObjectPos, 0.3f));
        nextFire = Time.time + 0.3f;
    }

    IEnumerator ReloadWeapon(float time)
    {
        yield return new WaitForSeconds(time);
        Reload();
    }

    IEnumerator TransformWithLerp(Vector3 startPos, Vector3 endPos, float time)
    {
        float elapsed = 0f;
        while (elapsed < time)
        {
            inventoryObject.transform.localPosition = Vector3.Lerp(startPos, endPos, (elapsed / time));
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
