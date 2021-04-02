using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeProjectile : MonoBehaviour
{
    public GameObject poisonPuddle;

    //If the snake projectile collides with the terrain, create a poison puddle in the location (on the ground)
    //Then destroy the projectile and destroy the puddle after 7.5 seconds
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Terrain"))
        {
            GameObject result = Instantiate(poisonPuddle);
            result.transform.position = new Vector3(transform.position.x, 0.048f, transform.position.z);
            Destroy(gameObject);
            Destroy(result, 7.5f);
        }
    }
}

