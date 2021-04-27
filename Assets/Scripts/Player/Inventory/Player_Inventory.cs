using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Inventory : MonoBehaviour
{
    [Tooltip("Automatically swap to a new weapon when picked up.")]
    [SerializeField] private bool AutoSwapNewWeapon = false;

    [Tooltip("Weapon that the player starts with.")]
    [SerializeField] private GameObject StartingWeapon;

    [SerializeField] private List<Item> inventory = new List<Item>();

    private GameObject inventoryObject;

    int itemIndex;
    int previousItemIndex = -1;

    void Start()
    {
        inventoryObject = transform.Find("PlayerCamera").Find("Inventory").gameObject;
        PickUpItem(Instantiate(StartingWeapon));
        EquipItem(0);
    }

    void EquipItem(int index)
    {
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
