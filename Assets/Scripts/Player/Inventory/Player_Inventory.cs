using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Inventory : MonoBehaviour
{
    [Tooltip("Automatically swap to a new weapon when picked up.")]
    [SerializeField] private bool AutoSwapNewWeapon = false;

    [Tooltip("Primary weapon that the player starts with.")]
    [SerializeField] private GameObject StartingWeaponPrimary;

    [Tooltip("Secondary weapon that the player starts with.")]
    [SerializeField] private GameObject StartingWeaponSecondary;

    [Tooltip("The game object/point that weapons attach to.")]
    [SerializeField] private GameObject inventoryObject;    //TODO rename this

    [SerializeField] private List<Item> inventory = new List<Item>();

    int itemIndex;
    int previousItemIndex = -1;

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
            EquipItem(0);
        }
        else
            Debug.LogError("No primary or secondary starting weapon specified! Fix this!");
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
        obj.transform.SetParent(inventoryObject.transform);
        obj.transform.localPosition = Vector3.zero;

        inventory.Add(obj.GetComponent<StandardGun>());
        if (AutoSwapNewWeapon)
        {
            EquipItem(inventory.Count - 1);
        }
    }

    void Update()
    {
        //Loop to check for weapon swapping
        for (int i = 0; i < inventory.Count; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }
    }
}
