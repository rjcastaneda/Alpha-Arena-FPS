using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighborhood : MonoBehaviour
{
    public List<Boid> neighbors;
    private SphereCollider coll;
    private AIController AICont;

    // Start is called before the first frame update
    void Start()
    {
        AICont = transform.parent.GetComponent<AIController>();
        neighbors = new List<Boid>();
        coll = GetComponent<SphereCollider>();
        coll.radius = AICont.neighborDist;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(coll.radius != AICont.neighborDist / 2){
            coll.radius = AICont.neighborDist / 2;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Boid b = other.GetComponent<Boid>();
        if(b != null)
        {
            if(neighbors.IndexOf(b) == -1){
                neighbors.Add(b);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Boid b = other.GetComponent<Boid>();
        if(b != null)
        {
            if(neighbors.IndexOf(b) != -1){
                neighbors.Remove(b);
            }
        }
    }

    public Vector3 AvgPosition()
    {
        Vector3 avg = Vector3.zero;
        if(neighbors.Count == 0) { return avg; }
        for(int x = 0; x < neighbors.Count; x++)
        {
            avg += neighbors[x].boidPos;
        }

        avg /= neighbors.Count;
        return avg;
    }

    public Vector3 AvgVel()
    {
        Vector3 avg = Vector3.zero;
        if (neighbors.Count == 0) { return avg; }
        for (int x = 0; x < neighbors.Count; x++)
        {
            avg += neighbors[x].boidRB.velocity;
        }

        avg /= neighbors.Count;
        return avg;
    }

    public Vector3 AvgClosePos()
    {
        Vector3 avg = Vector3.zero;
        Vector3 delta;
        int nearCnt = 0;
        for(int x = 0; x < neighbors.Count; x++)
        {
            delta = neighbors[x].boidPos - transform.position;
            if(delta.magnitude <= AICont.collDist)
            {
                avg += neighbors[x].boidPos;
                nearCnt++;
            }
        }

        if(nearCnt == 0){
            return avg;
        }

        avg /= nearCnt;
        return avg;

    }
}
