using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatUI : MonoBehaviour
{

    public bool statScreenEnabled;
    public GameObject statScreen;
    private Player player;
    private int thresholdS;
    private int thresholdA;
    private int thresholdH;

    //setting text values for inventory
    public Text SpeedText;
    public Text HealthText;
    public Text StrengthText;
    public Text SkillAvailableText; //text for number of available skill points
    public Text instructions;

    private void Start()
    {
        player = GetComponent<Player>();
        thresholdA = thresholdH = thresholdS = 3;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            statScreenEnabled = !statScreenEnabled; //toggle inventory
        }

        if (statScreenEnabled == true)
        {
            statScreen.SetActive(true);
        }
        else
        {
            statScreen.SetActive(false);
        }


        UpdateStatText();
        SetInstructions();
    }

    void SetInstructions()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Scene1":
                instructions.text = "You must save the world from the attacking zombies and geese!" +
                    " Collect the six engineering tools scattered around the map to purge this area of zombies and fix your teleporter!" +
                    " Press 'E' to close or reopen this menu.";
                break;
            case "Scene2":
                instructions.text = "Uh oh! Your teleporter moved you to the desert and broke when you arrived! " +
                    "Quick, some more tools! Oh, and watch out for the fearsom desert creatures, NEW ENEMY NAME." +
                    " Press 'E' to close or reopen this menu.";
                break;
        }
    }

    void UpdateStatText()
    {
        SpeedText.text = Player.GetSpeed().ToString(); //setting text values for stat menu screen
        HealthText.text = Player.GetHealth().ToString();
        StrengthText.text = Player.GetStrength().ToString();
        SkillAvailableText.text = "" + player.GetXPPoints();
    }

    public void IncrementAttack()
    {
        if (player.GetXPPoints() >= thresholdA)
        {
            player.SetStrength(Player.GetStrength() + 1);
            player.SetXPPoints(player.GetXPPoints() - thresholdA);
            thresholdA++;
        }
    }

    public void IncrementSpeed()
    {
        if (player.GetXPPoints() >= thresholdS)
        {
            player.SetSpeed(Player.GetSpeed() + 1);
            player.SetXPPoints(player.GetXPPoints() - thresholdS);
            thresholdS++;
        }
    }

    public void IncrementHealth()
    {
        if (player.GetXPPoints() >= thresholdH)
        {
            player.SetHealth(Player.GetHealth() + 10);
            player.SetXPPoints(player.GetXPPoints() - thresholdH);
            thresholdH++;
        }
    }
}
