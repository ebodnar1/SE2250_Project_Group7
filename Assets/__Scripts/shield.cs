using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield : MonoBehaviour
{
    private BoxCollider bc;

    private void Start()
    {
        bc = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (bc.enabled && collider.gameObject.name.Equals("GooseProjectile(Clone)"))
        {
            Destroy(collider.gameObject);
        }
    }
}
