using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    //Constantly rotate each pickup item
    void LateUpdate()
    {
        transform.Rotate(-1, -1, -1);
    }

    //If the pickup is 'in' the terrain, move it outside
    //Otherwise, if the pickup collides with something that is not an emey (or terrain), destroy it
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Terrain"))
        {
            transform.position += new Vector3(1, 0, 0);
        }
        else if(!collider.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    
}
