using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private BoxCollider bc;

    //get the shield's collider
    private void Start()
    {
        bc = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        //If the collider is enabled and a goose projectile collides with it
        if (bc.enabled && collider.gameObject.name.Equals("GooseProjectile(Clone)"))
        {
            //Destroy the goose projectile
            Destroy(collider.gameObject);
        }
    }
}
