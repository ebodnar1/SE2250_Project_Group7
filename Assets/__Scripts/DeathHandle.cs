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


    private void Start()
    {
        died = false;
        deathScreen.SetActive(false);
        player = GetComponent<Player>();
    }

    // Update is called once per frame
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

        CheckDead();
    }

    private void CheckDead()
    {
        if(Player.GetHealth() <= 0)
        {
            died = true;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scene1");
    }
}
