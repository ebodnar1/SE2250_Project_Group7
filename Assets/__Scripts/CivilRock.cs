using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilRock : MonoBehaviour
{
    private Rigidbody rb;
    private MeshCollider mc;

    //Initialize the rigidbody and meshcollider
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mc = GetComponent<MeshCollider>();
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.transform.parent != null)
        {
            //Destroy the rock if it collides with a monolith
            if (collider.gameObject.transform.parent.name.Equals("Monoliths"))
            {
                Destroy(gameObject);
            }
            //Destory a field rock if it collides with a field rock
            if (collider.gameObject.transform.parent.name.Equals("Rocks"))
            {
                Destroy(collider.gameObject);
            }
            //Destory a tree if it collides with a tree
            if (collider.gameObject.transform.parent.name.Equals("MainTrees"))
            {
                Destroy(collider.gameObject, 0.5f);
            }
        }

        //Start a coroutine if it collides with the ground
        if (collider.gameObject.name.Equals("Ground"))
        {
            StartCoroutine(HandleRockfall());
        }
    }

    IEnumerator HandleRockfall()
    {
        //Turn off the gravity for the rigidbody
        rb.useGravity = false;

        //Wait for 4 seconds (for the rock to drop)
        yield return new WaitForSeconds(0.4f);

        //Leave it on the ground and destroy the rigidbody component on the rock, also make it not a trigger anymore
        rb.velocity = Vector3.zero;
        Destroy(rb);
        mc.isTrigger = false;
    }
}
