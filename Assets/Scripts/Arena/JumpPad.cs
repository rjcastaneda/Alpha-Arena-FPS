using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    private Player_Movement PlayerTouched;
    private float JumpForce = 120f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Cylinder collider touched jumppad");
        Rigidbody rb = other.attachedRigidbody;
        PlayerTouched = other.gameObject.GetComponent<Player_Movement>();
        PlayerTouched.JumpPad(JumpForce);

    }
    /*private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Player touched jumppad");
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb == null || rb.isKinematic)
        {
            return;
        }

        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }

        Vector3 pushDir = new Vector3(hit.moveDirection.x, Mathf.Abs(hit.moveDirection.y), hit.moveDirection.z);
        Debug.Log("Player touched jumppad");
        PlayerTouched = hit.collider.gameObject.GetComponent<Player_Movement>();
        PlayerTouched.JumpPad(JumpForce);
        rb.velocity = JumpForce * pushDir;
    }*/
}
