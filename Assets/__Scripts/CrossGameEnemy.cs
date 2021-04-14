using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossGameEnemy : MonoBehaviour
{
    private GameObject mainPlayer;
    private CrossGame cg;

    //Get the player and its CrossGame script
    private void Start()
    {
        mainPlayer = GameObject.Find("Controller");
        cg = mainPlayer.GetComponent<CrossGame>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        //If the car collides with a 'wall' (edge of minigame map), destroy the car
        if (collider.gameObject.name.Equals("Wall"))
        {
            cg.HandleDeath(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //If the car collides with the character, call HandleDeath for the car
        if (collision.collider.gameObject.name.Equals("Controller"))
        {
            cg.HandleDeath(gameObject);
        }
    }
}
