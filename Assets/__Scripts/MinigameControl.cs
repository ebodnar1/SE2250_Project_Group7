using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameControl : MonoBehaviour
{
    //Track the minigame parent objects and the player UI
    public GameObject[] minigames;
    public GameObject playerUI;
    public Camera minigameCamera;
    private Camera mainCamera;
    private Player p;

    //Turn off all the minigames
    void Start()
    {
        foreach(GameObject game in minigames)
        {
            game.SetActive(false);
        }

        //Get the cameras
        minigameCamera.enabled = false;
        foreach (Camera cam in Camera.allCameras)
        {
            if (cam.name.Equals("MainCamera"))
            {
                mainCamera = cam;
            }
        }

        p = GetComponent<Player>();
    }

    //Turn on a game by iterating through the list of minigames
    public void ToggleGame(string gameName)
    {
        foreach (GameObject game in minigames)
        {
            if (game.name.Equals(gameName))
            {
                game.SetActive(true);
            }
        }

        //Shift cameras and disable player UI
        mainCamera.enabled = false;
        minigameCamera.enabled = true;
        playerUI.SetActive(false);
        StartCoroutine(ToggleMovements(false, 0.05f));
    }

    //Turn on the player UI and turn off all the games again
    public void ToggleUI()
    {
        playerUI.SetActive(true);
        foreach (GameObject game in minigames)
        {
            game.SetActive(false);
        }

        //Shift cameras back and enable player UI
        mainCamera.enabled = true;
        minigameCamera.enabled = false;
        StartCoroutine(ToggleMovements(true, 0.01f));
    }

    //Toggle the ability of enemies and the player to move, and the ability of spawners to spawn
    private IEnumerator ToggleMovements(bool truth, float delay)
    {
        yield return new WaitForSeconds(delay);

        SpecialAttack.SetPaused(!truth);
        p.canMove = truth;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Enemy enem = enemy.GetComponent<Enemy>();
            enem.SetMovementStatus(truth);
        }

        GameObject spawnerParent = GameObject.Find("Spawners");
        Spawner[] spawners = spawnerParent.GetComponentsInChildren<Spawner>();
        foreach (Spawner spawn in spawners)
        {
            spawn.SetSpawningStatus(truth);
        }
    }
}
