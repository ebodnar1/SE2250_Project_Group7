using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupControl : MonoBehaviour
{
    //Continuously rotate the pickups
    void Update()
    {
        RotateItem();
    }

    //Constantly rotate the pickups
    private void RotateItem()
    {
        switch (gameObject.name)
        {
            case "XP(Clone)":
                transform.Rotate(new Vector3(1, 1, 1));
                break;
            case "HealthPickup(Clone)":
                transform.Rotate(new Vector3(0, 2, 0));
                break;
        }
    }

    //Handles pickup collisions
    private void OnTriggerEnter(Collider collider)
    {
        //Must collide with the player
        if (collider.gameObject.name.Equals("Player"))
        {
            Player player = collider.GetComponent<Player>();
            switch (gameObject.name)
            {
                //If it is an XP drop, increment the player's XP by a random amount between 1 and 5
                case "XP(Clone)":
                    int xpDrop = Random.Range(1, 6);

                    player.SetXPPoints(player.GetXPPoints() + xpDrop);
                    player.IncrementXPSum(xpDrop);

                    Destroy(gameObject);
                    break;
                /*
                 * Give a 1/10 chance to drop a health potion that heals the player for the minimum of 
                 * 20 HP or enough to bring them to maximum health
                */
                case "HealthPickup(Clone)":
                    if (Player.GetHealth() + 20 <= Player.GetMaxHealth())
                    {
                        player.DamagePlayer(-20);
                    }
                    else
                    {
                        player.DamagePlayer(Player.GetHealth() - Player.GetMaxHealth());
                    }
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
