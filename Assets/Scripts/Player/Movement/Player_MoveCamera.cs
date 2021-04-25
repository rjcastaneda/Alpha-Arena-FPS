using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_MoveCamera : MonoBehaviour
{
    public Transform player;
    void Update()
    {
        transform.position = player.transform.position;
    }
}
