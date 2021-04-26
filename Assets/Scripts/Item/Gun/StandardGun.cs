using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardGun : Gun
{
    public override void Fire()
    {
        Debug.Log("Primary fire");
    }

    public override void AltFire()
    {
        Debug.Log("Alternate fire");
    }
}
