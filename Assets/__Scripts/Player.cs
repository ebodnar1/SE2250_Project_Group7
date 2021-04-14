using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Singleton Component
    static public Player S;

    public CharacterController controller;
    public Transform cam;
    public HealthBar healthBar;

    protected static int curHealth;
    protected static int maxHealth;

    //Static stat variables
    protected static float speed; 
    protected static int strength;
    protected int skillAvailable;
    protected static int xpPoints;

    //For upgraded special attack
    private static int xpSum;
    private static int specialXP;
    private bool upgradedSpecialAttack;

    //Movement fields
    public float angle;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity = 180;
    public bool canMove = true;
    
    //for the animations
    private GameObject[] limbs;
    private Animator[] animators;
    private float vSpeed = 0;
    private Vector3 moveVeloc;
    private Animator[] rightArms;

    private GameObject activePlayer;
    private bool immune;

    private bool withinMonolithRange;
    private GameObject currentMonolith;
    private MinigameControl minigames;
    public GameObject nearbyMonolith;
    public string monolithName;

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

        //Singleton Awake Component
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Hero.Awake() -  Attempted to Assign second Hero.S");
        }
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

        //If it is the first level, set the player's stats
        if (SceneManager.GetActiveScene().name.Equals("Scene1"))
        {
            ResetStats();
        }

        moveVeloc = Vector3.zero;

        //Set all the character's animations to on
        LimbsMove();

        withinMonolithRange = false;
        currentMonolith = null;
        minigames = GetComponent<MinigameControl>();
    }

    void FixedUpdate()
    {
        LimbsMove();
        //Tracks character movements
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        
        
        //Finds an angle based on character input for rotation, and rotates the character through this angle
        if(direction.magnitude >= 0.1f && canMove)
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

        if (currentMonolith != null)
        {
            if (withinMonolithRange && Input.GetKey("q") && GameObject.Find(currentMonolith.name).activeInHierarchy)
            {
                MonolithInteraction();
            }
        }
    }

    private void MonolithInteraction()
    {
        monolithName = currentMonolith.name;

        //Open a minigame depending on which monolith is interacted with
        switch (monolithName)
        {
            case "PurpleMonolith":
                minigames.ToggleGame("Numbers1");
                break;
            case "RedMonolith":
                minigames.ToggleGame("Maze1");
                break;
            case "GreenMonolith":
                minigames.ToggleGame("Wires1");
                break;
            case "BlueMonolith":
                minigames.ToggleGame("Cross1");
                break;
        }

        //Turn off the "nearby monolith" warning
        nearbyMonolith.SetActive(false);
    }

    //Return to the scene - no longer in monolith range, call ToggleUI from MinigameControl
    public void ReturnToScene()
    {
        withinMonolithRange = false;
        minigames.ToggleUI();
    }

    public void ResetStats()
    {
        //Set the predefined stats of the player
        int[,] playerStats = { { 120, 8, 10 }, { 80, 10, 12 }, { 100, 12, 8 } };
        switch (activePlayer.name)
        {
            case "chemicalCharacter":
                maxHealth = playerStats[0, 0];
                curHealth = maxHealth;
                strength = playerStats[0, 1];
                speed = playerStats[0, 2];
                break;
            case "electricalCharacter":
                maxHealth = playerStats[1, 0];
                curHealth = maxHealth;
                strength = playerStats[1, 1];
                speed = playerStats[1, 2];
                break;
            case "civilCharacter":
                maxHealth = playerStats[2, 0];
                curHealth = maxHealth;
                strength = playerStats[2, 1];
                speed = playerStats[2, 2];
                break;
            default:
                speed = strength = 10;
                maxHealth = 100;
                curHealth = maxHealth;
                break;
        }

        //Set the player's XP to 0
        xpPoints = 0;

        //Initializes special XP fields
        specialXP = 0;
        xpSum = 0;
        upgradedSpecialAttack = false;
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
                DamagePlayer(5);
            }
            //The civil character's special attack can hurt themself
            if (collider.gameObject.name.Equals("CivilSpecial(Clone)"))
            {
                DamagePlayer(15);
            }
            StartCoroutine(Immunity());
        }

        //Turn on the monolith prompt if nearby a monolith
        if (collider.gameObject.name.Equals("MonolithRange"))
        {
            nearbyMonolith.SetActive(true);
        }
    }

    //If within range of a monolith, get the name of the monolith and set withinMonolithRange to true
    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.name.Equals("MonolithRange"))
        {
            withinMonolithRange = true;
            currentMonolith = collider.gameObject.transform.parent.gameObject;
            monolithName = currentMonolith.name;
        }
    }

    //Exiting the range of a monolith causes the prompt to disappear
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name.Equals("MonolithRange"))
        {
            withinMonolithRange = false;
            nearbyMonolith.SetActive(false);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!immune)
        {
            //If the player gets hit by a crab or an explosion
            if (collision.gameObject.name.Equals("Crab(Clone)") || collision.gameObject.name.Equals("CrabBomb"))
            {
                //Crabs deal 5 damage
                DamagePlayer(5);

                //Explosion reaction force for player
                Vector3 explosion = new Vector3(-transform.forward.x, transform.up.y, -transform.forward.z);
                controller.Move(2 * explosion);
            }
            //Otherwise decrease current health by 10
            else if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("EnemyLimb"))
            {
                DamagePlayer(10);
            }
            //Start immunity for 1 second
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

    // Function to damage player health by specified amount, and update the health bar. 
    // -ve damage argument means healing
    public void DamagePlayer(int damage)
    {
        SetHealth(GetHealth() - damage);
        healthBar.SetHealth(curHealth);
    }

    //Accessors and setters for private fields
    public void UpgradedSpecial(bool upgrade)
    {
        upgradedSpecialAttack = upgrade;
    }

    public string GetMonolithName()
    {
        return monolithName;
    }
    public void SetNearbyMonolith(bool truth)
    {
        nearbyMonolith.SetActive(truth);
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
        curHealth = heal;
    }
    public static int GetHealth()
    {
        return curHealth;
    }

    public static int GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(int mh)
    {
        maxHealth = mh;
    }

    public void SetXPPoints(int xp)
    {
        xpPoints = xp;
    }
    public int GetXPPoints()
    {
        return xpPoints;
    }
    public void IncrementXPSum(int xp)
    {
        xpSum += xp;
    }

    public void SetSpecialXP(int specX)
    {
        specialXP = specX;
    }
    public int GetSpecialXP()
    {
        return specialXP;
    }
    public static void IncrementSpecialXP()
    {
        specialXP++;
    }

    public int GetXPSum()
    {
        return xpSum;
    }
    public bool GetUpgradeStatus()
    {
        return upgradedSpecialAttack;
    }
    public GameObject GetActivePlayer()
    {
        return activePlayer;
    }
    public bool GetMonolithProximity()
    {
        return withinMonolithRange;
    }
}
