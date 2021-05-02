using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public Vector3 position = Vector3.zero;

    [Header("Attractor Values")]
    public float radius = 50; //The bounds the attractor can be positioned at
    public float leadSpd = 100;

    private SphereCollider attractCol;
    private AIController AICont;

    private void Start()
    {
        attractCol = this.gameObject.GetComponent<SphereCollider>();
        AICont = transform.parent.transform.Find("BoidController").GetComponent<AIController>();
        this.gameObject.transform.position = position;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        { return;  }
        rndLocation();
    }

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
