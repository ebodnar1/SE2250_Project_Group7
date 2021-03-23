using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathHandle : MonoBehaviour
{
    public bool died;
    public GameObject deathScreen;
    private Player player;

    //Player is not dead, and set the death screen to inactive
    private void Start()
    {
        died = false;
        deathScreen.SetActive(false);
        player = GetComponent<Player>();
    }

    //If the player died, made the death screen active
    void Update()
    {
        if (died == true)
        {
            deathScreen.SetActive(true);
        }
        else
        {
            deathScreen.SetActive(false);
        }

        //Check if the player is dead
        CheckDead();
    }

    //If the player's health is 0 or less, they are dead
    private void CheckDead()
    {
        if(Player.GetHealth() <= 0)
        {
            died = true;
        }
    }

    //Load the scene again if the player clicks "Play Again" on the death screen UI
    public void Restart()
    {
        SceneManager.LoadScene("Scene1");
    }
}
