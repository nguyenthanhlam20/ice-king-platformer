using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingController : MonoBehaviour, IItemCollection
{

    [Header("Scripts Ref:")]
    public GrapplingHook grapplingGunScript;

    [Header("Component Ref:")]
    public GameObject grapplingGun;

    // Start is called before the first frame update
    public void activeItem()
    {
        Debug.Log("Test coollecafafsf");
        grapplingGunScript.activeGrappling = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (grapplingGunScript.canGrappling)
            grapplingGunScript.StopGrappling();
    }

    
}
