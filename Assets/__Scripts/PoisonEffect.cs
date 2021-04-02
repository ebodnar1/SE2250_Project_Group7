using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    public bool poisoned;
    private int poisonCount;
    private Player player;
    private bool poisonEffect;

    //The player has not been poisoned upon startup
    private void Start()
    {
        poisonCount = 0;
        poisoned = false;
        player = GetComponent<Player>();
        poisonEffect = true;
    }

    //If the player is poisoned and has not been damaged more than 10 times, damage them
    private void Update()
    {
        if(poisonCount < 10 && poisoned)
        {
            StartCoroutine(PoisonDamage());
        }
    }

    //If the player collides with a cactus or poison puddle, they are poisoned
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Cactus") || collider.gameObject.name.Equals("PoisonPuddle(Clone)"))
        {
            poisoned = true;
        }
    }

    //Poison damager
    IEnumerator PoisonDamage()
    {
        //If the poison effect is true, decrement the player's health by 2 each second, turning the
        //poison effect to false between damages so that poison effects cannot stack
        if (poisonEffect) {
            player.SetHealth(Player.GetHealth() - 2);
            poisonCount++;
            poisonEffect = false;
            yield return new WaitForSeconds(1);
            poisonEffect = true;
        }
        //If they were damaged 10 times by poison, remove the poison and reset the damage count
        if(poisonCount >= 10)
        {
            poisoned = false;
            poisonCount = 0;
        }
    }
}
