using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Attractor : MonoBehaviourPun
{
    public Vector3 position;

    [Header("Attractor Values")]
    public float radius = 50; //The bounds the attractor can be positioned at
    public float leadSpd = 100;

    private SphereCollider attractCol;
    [SerializeField]private AIController AICont;

    private void Start()
    {
        attractCol = this.gameObject.GetComponent<SphereCollider>();
        AICont = transform.parent.transform.Find("BoidController").GetComponent<AIController>();
        position = this.gameObject.transform.position;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        { return;  }
        rndLocation();
    }

    [PunRPC]
    //Function for the attractor to change to a random position
    public void rndLocation()
    {
        Vector3 tPos = Vector3.zero;
        Vector3 scale = transform.localScale;
        tPos.x = Random.Range(-radius, radius) * scale.x;
        tPos.y = Random.Range(-radius, radius) * scale.y;
        tPos.z = Random.Range(-radius, radius) * scale.z;
        transform.position = tPos;
        position = tPos;
    }
}
