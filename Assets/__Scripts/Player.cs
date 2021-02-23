using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public CharacterController controller;

    //currently public for debugging use
    public Transform cam;
    public float speed; //initializing stat variables
    public int health; 
    public int strength;
    public int skillAvailable;


    //setting text values for inventory
    public Text SpeedText;
    public Text HealthText;
    public Text StrengthText;
    public Text SkillAvailableText; //text for number of available skill points

    void Start()
    {
        speed = 15f; //setting default stat values
        health = 5;
        strength = 5;
        skillAvailable = 0;

        UpdateStatText();
    }

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection * speed * Time.deltaTime);
        }
    }


    void UpdateStatText()
    {
        SpeedText.text = speed.ToString(); //setting text values for stat menu screen
        HealthText.text = health.ToString();
        StrengthText.text = strength.ToString();
        SkillAvailableText.text = skillAvailable.ToString();
    }

    void IncrementStat(int stat)
    {
        if (skillAvailable > 0)
        {
            stat++;
            skillAvailable--;
        }
        
        UpdateStatText();
    }

}
