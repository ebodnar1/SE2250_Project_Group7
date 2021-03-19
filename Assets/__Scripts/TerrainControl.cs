using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainControl : MonoBehaviour
{
    //Destroy a projectile if it hits a piece of terrain
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Projectile"))
        {
            Destroy(collider.gameObject, 0.0f);
        }
    }
}
