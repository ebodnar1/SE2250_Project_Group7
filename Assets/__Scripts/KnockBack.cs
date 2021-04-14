using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{

    [SerializeField] private float knockbackStrength;

    private void OnCollisionEnter( Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        
        if(rb != null)
        {
            Vector3 direction =  collision.transform.position - transform.position;
            direction.y = 0;

            rb.AddForce(direction.normalized * knockbackStrength, ForceMode.Impulse);
        }
    }
}
