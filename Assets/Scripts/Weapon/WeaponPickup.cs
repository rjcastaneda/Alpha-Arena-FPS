using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviourPunCallbacks
{
    [Tooltip("The weapon that this pickup grants.")]
    [SerializeField] GameObject weaponPickup;

    [Tooltip("Should this pickup already be spawned when the match starts?")]
    [SerializeField] bool startSpawned;

    [Tooltip("The time it takes for this pickup to respawn once it is grabbed.")]
    [SerializeField] int spawnTime;

    private GameObject displayPoint;
    private GameObject weapon;
    private string weaponName;

    private Coroutine spawnRoutine = null;

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        displayPoint = transform.Find("DisplayPoint").gameObject;
        weapon = PhotonNetwork.Instantiate(weaponPickup.GetComponent<Weapon>().weaponName, displayPoint.transform.position, Quaternion.identity, 0);
        weaponName = weaponPickup.GetComponent<Weapon>().weaponName;

        weapon.transform.SetParent(displayPoint.transform);
        weapon.transform.localPosition = Vector3.zero;

        if (!startSpawned)
        {
            photonView.RPC("EnableDisableWeaponPickup", RpcTarget.All, false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        if ((spawnRoutine == null) & (!weapon.gameObject.activeSelf))
        {
            spawnRoutine = StartCoroutine(RespawnPickup(spawnTime));
        }
    }

    IEnumerator RespawnPickup(int time)
    {
        yield return new WaitForSeconds(time);
        photonView.RPC("EnableDisableWeaponPickup", RpcTarget.All, true);
        spawnRoutine = null;
    }


    private void OnTriggerStay(Collider other)
    {
        GameObject obj = other.gameObject;
        if (obj.tag == "Player")
        {
            if (weapon.gameObject.activeSelf)
            {
                obj.GetComponent<Player_Inventory>().PickUpItem(Instantiate(weaponPickup));

                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("EnableDisableWeaponPickup", RpcTarget.All, false);
                }
            }
        }
    }

    [PunRPC]
    void EnableDisableWeaponPickup(bool show)
    {
        weapon.SetActive(show);
    }
}
