using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatUI : MonoBehaviour
{
    //Tracking variables
    public bool statScreenEnabled;
    public GameObject statScreen;
    private Player player;
    private static int thresholdS;
    private static int thresholdA;
    private static int thresholdH;

    //setting text values for inventory
    public Text SpeedText;
    public Text HealthText;
    public Text StrengthText;
    public Text SkillAvailableText; //text for number of available skill points
    public GameObject upgradeSpecial;

    public HealthBar healthScript;
    public Text instructions;

    //Get the current player and thresholds for each upgrade
    private void Start()
    {
        player = GetComponent<Player>();
        if (SceneManager.GetActiveScene().name.Equals("Scene1"))
        {
            thresholdA = thresholdH = thresholdS = 3;
        }
        upgradeSpecial.SetActive(false);
    }

    //If the user presses 'e', toggle the inventory on
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            statScreenEnabled = !statScreenEnabled; 
        }

        if (statScreenEnabled == true)
        {
            statScreen.SetActive(true);
        }
        else
        {
            statScreen.SetActive(false);
        }

        //Continuously update the stat text and check for new instructions
        UpdateStatText();
        SetInstructions();
        CheckSpecialAttackCriteria();
    }

    //Different instructions in the inventory for different levels
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
                    "Quick, some more tools! Oh, and watch out for the snakes and crabs in the desert!" +
                    " Press 'E' to close or reopen this menu.";
                break;
        }
    }

    //Set all the stat values to the updated values
    void UpdateStatText()
    {
        SpeedText.text = Player.GetSpeed().ToString(); 
        HealthText.text = Player.GetMaxHealth().ToString();
        StrengthText.text = Player.GetStrength().ToString();
        SkillAvailableText.text = "" + player.GetXPPoints();
    }

    //Increase the player's attack by 1 point
    public void IncrementAttack()
    {
        if (player.GetXPPoints() >= thresholdA)
        {
            player.SetStrength(Player.GetStrength() + 1);
            player.SetXPPoints(player.GetXPPoints() - thresholdA);
            thresholdA++;
        }
    }

    //Increase the player's speed by 1 point
    public void IncrementSpeed()
    {
        if (player.GetXPPoints() >= thresholdS)
        {
            player.SetSpeed(Player.GetSpeed() + 1);
            player.SetXPPoints(player.GetXPPoints() - thresholdS);
            thresholdS++;
        }
    }

    //Increase the player's health by 10 points
    public void IncrementHealth()
    {
        if (player.GetXPPoints() >= thresholdH)
        {
            player.SetMaxHealth(Player.GetMaxHealth() + 10);
            healthScript.SetMaxHealth(Player.GetMaxHealth());

            player.SetHealth(Player.GetHealth() + 10);
            healthScript.SetHealth(Player.GetHealth());

            player.SetXPPoints(player.GetXPPoints() - thresholdH);
            thresholdH++;
        }
    }

    //Player must have accumulated 50 xp total and at least 4 special xp to upgrade their special attack
    private void CheckSpecialAttackCriteria()
    {
        if (player.GetXPSum() >= 50 && player.GetSpecialXP() >= 4)
        {
            upgradeSpecial.SetActive(true);
        }
    }

    //Set the player's special XP to 0, upgrade their special, and set the upgrade prompt to inactive
    public void UpgradeSpecialAttack()
    {
        if(!player.GetUpgradeStatus())
        {
            player.SetSpecialXP(0);
            player.UpgradedSpecial(true);
            upgradeSpecial.SetActive(false);
        }
    }
}
