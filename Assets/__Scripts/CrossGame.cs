using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrossGame : MonoBehaviour
{
    private Vector3 startPos;
    private float timer;
    public GameObject obstacle;
    public float enemyMaxSpeed;
    public float enemyMinSpeed;
    private List<float> positions;
    private bool canMove;
    public Color[] colours;
    private Player player;
    private bool canSpawn;
    private float multiplier;
    public float scaleFactor = 1;

    void Start()
    {
        //Get the original position of the minigame player
        startPos = transform.position;
        timer = 0.5f;

        //Fill the list with y-positions for the cars to spawn at
        positions = new List<float>();
        float curr = startPos.y + 2.5f;
        for (int i = 0; i <= 26 * scaleFactor; i++)
        {
            positions.Add(curr++);
        }

        //The player can move this frame and cars can spawn
        canMove = true;
        canSpawn = true;

        //Get the player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        //The level 2 game is harder - cars spawn faster
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.Equals("Scene1"))
        {
            multiplier = 1.0f;
        }
        else
        {
            multiplier = 0.65f;
        }
    }

    //Try to move (if possible) and try to spawn an enemy, while incrementing the timer
    void Update()
    {
        StartCoroutine(Move());
        StartObstacle();

        timer += Time.deltaTime;
    }

    private IEnumerator Move()
    {
        //If there is currently a coroutine already running, break from this one
        if (!canMove) yield break;

        //No new Move coroutines can run
        canMove = false;

        //Get player input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        /*
         * 2D retro movement based on forward/back/left/right.
         * Player moves one 'space' on the board each time
         * Player rotates to look where they are moving
        */
        if (vertical > 0)
        {
            transform.position += new Vector3(0, 1f, 0);
            transform.rotation = Quaternion.Euler(-90, 90, -90);
        }
        else if (vertical < 0)
        {
            transform.position += new Vector3(0, -1f, 0);
            transform.rotation = Quaternion.Euler(90, 90, -90);
        }

        if (horizontal > 0)
        {
            transform.position += new Vector3(1f, 0, 0);
            transform.rotation = Quaternion.Euler(0, 90, -90);
        }
        else if (horizontal < 0)
        {
            transform.position += new Vector3(-1f, 0, 0);
            transform.rotation = Quaternion.Euler(180, 90, -90);
        }

        //Can move every 0.2 seconds
        yield return new WaitForSeconds(0.20f);
        canMove = true;
        
    }

    private void StartObstacle()
    {
        //Every 0.5 or 0.5 * multiplier seconds (if canSpawn is true)
        if (timer >= (0.5f * multiplier) && canSpawn)
        {
            //Reset the timer and create a car
            timer = 0;
            GameObject obs = Instantiate(obstacle);

            //Colour the car
            int colourNum = Random.Range(0, 6);
            MeshRenderer mr = obs.GetComponent<MeshRenderer>();
            mr.material.color = colours[colourNum];

            //Determine the location of the car
            DetermineCoordinates(obs);

            //Get the rigidbody
            Rigidbody rb = obs.GetComponent<Rigidbody>();

            //Set the horizontal velocity direction to left, otherwise if the car is on the left side of the minigame map,
            //then set the velocity direction to the right and rotate the car to its front is facing right
            int horizontal = -1;
            if (obs.transform.position.x < 0)
            {
                horizontal = 1;
                obs.transform.rotation = Quaternion.Euler(180, -90, 90);
            }

            //Generate a random speed value between the max and min
            float randomSpeed = Random.Range(enemyMinSpeed, enemyMaxSpeed + 0.01f);

            //Add velocity to the car (on the x axis) corresponding to this speed
            rb.velocity = new Vector3(horizontal, 0, 0) * randomSpeed;
        }
    }

    //Determine the placement of the cars
    private void DetermineCoordinates(GameObject obs)
    {
        //Generate a random index for the positions list and set the value at this index to yPos
        int rand1 = Random.Range(0, positions.Count);
        float yPos = positions[rand1];

        //Remove this index from the list
            //This ensures that a maximum of one car spawns in each 'row' at a time
        positions.RemoveAt(rand1);

        //Set the xPos as 14.5 spaces to the left or right of the player depending on a randomly generated value
        float xPos = startPos.x - 14.5f;
        int rand2 = Random.Range(0, 2);
        if (rand2 != 0)
        {
            xPos = xPos + 29.0f;
        }

        //Move the car to this generated position
        obs.transform.position = new Vector3(xPos, yPos, startPos.z + 0.3f);
    }

    //On death
    public void HandleDeath(GameObject obs)
    {
        //Destroy the car and add the y position back to the list so another car can spawn in this row
        Destroy(obs);
        positions.Add(obs.transform.position.y);
    }

    //If the player collides with an obstacle, set it to the start
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Obstacle"))
        {
            transform.position = startPos;
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        //If a player collides with the 'finish line' trigger
        if (collider.gameObject.name.Equals("Finish"))
        {
            //Set this minigame map to inactive, increment the player's special XP, and resume the scene
            transform.parent.gameObject.SetActive(false);
            Player.IncrementSpecialXP();
            player.ReturnToScene();

            //Destroy the blue monolith
            GameObject monolith = GameObject.Find("BlueMonolith");
            Destroy(monolith);

            //No more cars can spawn
            canSpawn = false;

            //Destroy all the existing cars
            GameObject[] cars = GameObject.FindGameObjectsWithTag("Obstacle");
            foreach(GameObject car in cars)
            {
                Destroy(car);
            }
        }
    }
}
