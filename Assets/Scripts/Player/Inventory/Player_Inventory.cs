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

    [SerializeField] private List<Item> inventory = new List<Item>();

    [SerializeField] private Camera Camera; //TODO: some sort of global class that handles basic things like the camera?

    private int itemIndex;
    private int previousItemIndex = -1;

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
    }

    void EquipItem(int index)
    {
        if (index == previousItemIndex)
        {
            return;
        }

        itemIndex = index;
        inventory[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1)
        {
            inventory[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;
    }

    void PickUpItem(GameObject obj)
    {
        SetItemParent(obj);

        inventory.Add(obj.GetComponent<StandardGun>());

        if (AutoSwapNewWeapon)
        {
            EquipItem(inventory.Count - 1);
        }
    }

    void SetItemParent(GameObject obj)
    {
        obj.transform.SetParent(inventoryObject.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.GetComponent<StandardGun>().AddCamera(Camera);  //Pass a reference to the player camera to the gun so it knows where to draw raycasts from
    }

    void Update()
    {
        CheckSwapWeapon();
        CheckFireWeapon();
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
            inventory[itemIndex].Fire();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            inventory[itemIndex].AltFire();
        }
    }
}
