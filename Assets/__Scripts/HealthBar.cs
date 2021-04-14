using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    private Slider healthBar;
    public Text currentHealth;
    private Image healthBarImage;

    private void Start()
    {
        //Allow player to be initialized first
        Invoke("SetValues", 0.01f);
    }

    //Set the current and maximum health
    private void SetValues()
    {
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = Player.GetMaxHealth();
        healthBar.value = Player.GetMaxHealth();
        SetCurrentHealth();
        healthBarImage = healthBar.GetComponentInChildren<Image>();
    }

    //Change the slider value
    public void SetHealth(int hp)
    {
        healthBar.value = hp;
        SetCurrentHealth();
    }

    //Change the maximu, slider value
    public void SetMaxHealth(int hpMax)
    {
        healthBar.maxValue = hpMax;
    }

    //Change the text value
    public void SetCurrentHealth()
    {
        currentHealth.text = "Health: " + healthBar.value.ToString();
    }


    public void ChangeColour(bool truth)
    {
        //Purple (poisoned)
        if (truth)
        {
            healthBarImage.color = new Color((172f / 255f), (47f / 255f), (210f / 255f));
        }
        //Read (healthy)
        else
        {
            healthBarImage.color = new Color((219f / 255f), (37f / 255f), (37f / 255f));
        }
    }
}
