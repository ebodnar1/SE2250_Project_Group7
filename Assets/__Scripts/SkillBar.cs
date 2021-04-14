using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBar : MonoBehaviour
{
    private Slider xpBar;
    private Image xpBarImage;

    private void Start()
    {
        //Get slider (set max to 15) and initialize it
        xpBar = gameObject.GetComponent<Slider>();
        xpBar.maxValue = 15;
        xpBar.value = SpecialAttack.GetCooldown();
        xpBarImage = xpBar.GetComponentInChildren<Image>();
    }

    private void Update()
    {
        //If the xp bar is full, turn it pink
        if (xpBar.value >= xpBar.maxValue)
        {
            ChangeColour(3);
        }
        //If the xp bar is charging and not upgraded, make it blue
        else if (xpBar.value < xpBar.maxValue && xpBar.maxValue == 15)
        {
            ChangeColour(1);
        }
        //If the xp bar is charging and upgraded, make it greeb
        else if (xpBar.value < xpBar.maxValue && xpBar.maxValue == 20)
        {
            ChangeColour(2);
        }
    }

    //Set the slider value
    public void SetValue(int xp)
    {
        xpBar.value = xp;
    }

    //Set the maximu, slider value
    public void SetMaxValue(int xpMax)
    {
        xpBar.maxValue = xpMax;
    }

    public void ChangeColour(int i)
    {
        switch (i) {
            //Charging bar that is not upgraded
            case 1:
                xpBarImage.color = new Color((37f / 255f), (91f / 255f), (219f / 255f));
                break;
            //Charging bar that is upgraded
            case 2:
                xpBarImage.color = new Color((37f / 255f), (219f / 255f), (116f / 255f));
                break;
            //Charged bar
            case 3:
                xpBarImage.color = new Color((37219f / 255f), (37f / 255f), (194f / 255f));
                break;
        }
        
    }

}
