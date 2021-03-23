using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    //Static stat variables
    protected static float speed; 
    protected static int health; 
    protected static int strength;
    protected int skillAvailable;
    protected int xpPoints;

    //Movement fields
    public float angle;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity = 180;
    private Rigidbody rb;
    
    //for the animations
    private GameObject[] limbs;
    private Animator[] animators;
    private float vSpeed = 0;
    private Vector3 moveVeloc;
    private Animator[] rightArms;
    private Animator attackArm;

    private GameObject activePlayer;
    private bool immune;
    
    private void Awake()
    {
        //Get all of the limbs (via tag)
        limbs = GameObject.FindGameObjectsWithTag("Limb");
        //Arrays for regular walking limbs (legs and left arms) and attacking limbs (right arms)
        animators = new Animator[limbs.Length - 3];
        rightArms = new Animator[3];

        //Initialize the animators array with all of the animations
        int count = 0;
        int j = 0;
        for (int i = 0; i < limbs.Length; i++)
        {
            //If there is a right arm, add it to its own special array
            if (limbs[i].name.Equals("RightArm"))
            {
                rightArms[j] = limbs[i].GetComponent<Animator>();
                rightArms[j].enabled = false;
                j++;
            }
            //Add any other limbs to the animators array
            else
            {
                animators[count] = limbs[i].GetComponent<Animator>();
                count++;
            }
        }

        LimbsMove();

    }
   

    void Start()
    {
        //Find what player is currently in the game
        GameObject[] possiblePlayers = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < possiblePlayers.Length; i++)
        {
            if (possiblePlayers[i].activeInHierarchy)
            {
                activePlayer = possiblePlayers[i];
            }
        }

        ResetStats();

        moveVeloc = Vector3.zero;
        rb = GetComponent<Rigidbody>();

        //Set all the character's animations to on
        LimbsMove();
    }

    void FixedUpdate()
    {
        LimbsMove();
        //Tracks character movements
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //Finds an angle based on character input for rotation, and rotates the character through this angle
        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveVeloc = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * speed;
        }
        //If the character is not moving, stop the animations and set velocity to 0
        else
        {
            moveVeloc = Vector3.zero;
            LimbsStopMove();
        }

        //If the character is on the ground, they can jump
        if (controller.isGrounded)
        {
            vSpeed = 0;
            if (Input.GetKey("space"))
            {
                vSpeed = 28;
            }
        }

        //Gravity simulation
        vSpeed -= 90.8f * Time.deltaTime;
        moveVeloc.y = vSpeed;

        //Move the character via CharacterController
        controller.Move(moveVeloc * Time.deltaTime);
    }

    public void ResetStats()
    {
        //Set the predefined stats of the player
        int[,] playerStats = { { 120, 8, 10 }, { 80, 10, 12 }, { 100, 12, 8 } };
        switch (activePlayer.name)
        {
            case "chemicalCharacter":
                health = playerStats[0, 0];
                strength = playerStats[0, 1];
                speed = playerStats[0, 2];
                break;
            case "electricalCharacter":
                health = playerStats[1, 0];
                strength = playerStats[1, 1];
                speed = playerStats[1, 2];
                break;
            case "civilCharacter":
                health = playerStats[2, 0];
                strength = playerStats[2, 1];
                speed = playerStats[2, 2];
                break;
            default:
                speed = strength = 10;
                health = 100;
                break;
        }

        //Set the player's XP to 0
        xpPoints = 0;
    }

    //Stop the walking animations
    private void LimbsStopMove()
    {
        for(int i = 0; i < animators.Length; i++)
        {
            animators[i].enabled = false;
        }
    }

    //Start the walking animations
    private void LimbsMove()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].enabled = true;
        }
    }

    //Collision/damage handling
    private void OnTriggerEnter(Collider collider)
    {
        //Check if the player is immune currently
        if (!immune)
        {
            //Damage the player for 5 if hit by a goose projectile
            if (collider.gameObject.name.Equals("GooseProjectile(Clone)"))
            {
                Destroy(collider.gameObject);
                health -= 5;
            }
            StartCoroutine(Immunity());
        }
        //Increase the player's XP or HP if they contact a potion
        if (collider.gameObject.CompareTag("Potion")){
            switch (collider.gameObject.name)
            {
                case "XP(Clone)":
                    xpPoints ++;
                    Destroy(collider.gameObject);
                    break;
                case "HealthPotion(Clone)":
                    health += 20;
                    Destroy(collider.gameObject);
                    break;
            }
        }
    }

    //If the player collides with an enemy, damage them by 10
    private void OnCollisionEnter(Collision collision)
    {
        if (!immune)
        {
            if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("EnemyLimb"))
            {
                health -= 10;
            }
            StartCoroutine(Immunity());
        }
    }

    //Make the player immune for 1s after taking damage
    IEnumerator Immunity()
    {
        immune = true;
        yield return new WaitForSeconds(1f);
        immune = false;
    }

    //Setters and getters for strength (attack), speed, health, and XP
    //Allows these to be values to be accessible and editable by the stats UI
    public void SetStrength(int str)
    {
        strength = str;
    }
    public static int GetStrength()
    {
        return strength;
    }

    public void SetSpeed(float spe)
    {
        speed = spe;
    }
    public static float GetSpeed()
    {
        return speed;
    }

    public void SetHealth(int heal)
    {
        health = heal;
    }
    public static int GetHealth()
    {
        return health;
    }

    public void SetXPPoints(int xp)
    {
        xpPoints = xp;
    }
    public int GetXPPoints()
    {
        return xpPoints;
    }
}
