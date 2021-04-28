using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Inventory : MonoBehaviour
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
    [SerializeField] private GameObject inventoryObject;    //TODO rename this

    [SerializeField] private List<GameObject> inventory = new List<GameObject>();

    [SerializeField] private Camera Camera; //TODO: some sort of global class that handles basic things like the camera?

    private int itemIndex;
    private int previousItemIndex = -1;

    private float nextFire = 0f;

    //debug
    LineRenderer lr;

    void Start()
    {
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

        if (previousItemIndex != -1)
        {
            inventory[previousItemIndex].SetActive(false);
        }

        previousItemIndex = itemIndex;
    }

    void PickUpItem(GameObject obj)
    {
        obj.transform.SetParent(inventoryObject.transform);
        obj.transform.localPosition = Vector3.zero;

        inventory.Add(obj);

        if (AutoSwapNewWeapon)
        {
            EquipItem(inventory.Count - 1);
        }
    }

    void Update()
    {
        CheckSwapWeapon();
        CheckFireWeapon();
        CheckReloadWeapon();
    }

    void CheckSwapWeapon()
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

    void CheckFireWeapon()
    {
        if (Input.GetMouseButton(0))
        {
            PrimaryFire();
        }
        else if (Input.GetMouseButton(1))
        {
            AltFire();
        }
    }

    void CheckReloadWeapon()
    {
        if (Input.GetKeyDown("r"))
        {
            Reload();
        }
    }

    void PrimaryFire()
    {
        if (inventory[itemIndex].GetComponent<Weapon>().currentAmmo > 0)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time;
                nextFire += inventory[itemIndex].GetComponent<Weapon>().rateOfFire;

                //Gross debug raycast stuff start
                ////////////////////////////////
                Debug.Log("Primary fire for " + inventory[itemIndex].GetComponent<Weapon>().weaponName);

                Ray ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                ray.origin = Camera.transform.position;

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.Log("Hit " + hit.collider.gameObject.name);
                    lr.enabled = true;
                    lr.SetPosition(0, ray.origin - new Vector3(0, 0.5f, 0));
                    lr.SetPosition(1, hit.point);
                }
                ////////////////////////////////
                //Gross debug raycast stuff end

                inventory[itemIndex].GetComponent<Weapon>().currentAmmo -= 1;

                inventory[itemIndex].GetComponent<AudioSource>().PlayOneShot(inventory[itemIndex].GetComponent<Weapon>().primaryFireSound, 0.5f);
            }
        }
    }

    void AltFire()
    {

    }

    void Reload()
    {
        if (inventory[itemIndex].GetComponent<Weapon>().currentAmmo != inventory[itemIndex].GetComponent<Weapon>().magSize)
        {
            if (inventory[itemIndex].GetComponent<Weapon>().magSize > inventory[itemIndex].GetComponent<Weapon>().maxAmmo)
            {
                inventory[itemIndex].GetComponent<Weapon>().currentAmmo = inventory[itemIndex].GetComponent<Weapon>().maxAmmo;
                inventory[itemIndex].GetComponent<Weapon>().maxAmmo -= inventory[itemIndex].GetComponent<Weapon>().maxAmmo;
            }
            else
            {
                inventory[itemIndex].GetComponent<Weapon>().maxAmmo -= (inventory[itemIndex].GetComponent<Weapon>().magSize - inventory[itemIndex].GetComponent<Weapon>().currentAmmo);
                inventory[itemIndex].GetComponent<Weapon>().currentAmmo = inventory[itemIndex].GetComponent<Weapon>().magSize;
            }
        }
    }
}
