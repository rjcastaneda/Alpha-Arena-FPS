using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    public override void Fire()
    {
        Debug.Log("Shotgun primary fire");
    }

    public override void AltFire()
    {
        Debug.Log("Shotgun alternate fire");
    }
}
