using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeMove : MonoBehaviour
{
    private Rigidbody rb;
    private float speed;
    private Vector3 startPos;
    public static bool active;
    private Player player;

    //Initialize the attributes
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (SceneManager.GetActiveScene().name.Equals("Scene1"))
        {
            speed = 200.0f;
        }
        else
        {
            speed = 400.0f;
        }

        startPos = transform.position;
        active = false;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    //Move the ball
    void FixedUpdate()
    {
        //If this is an active scene
        if (gameObject.activeInHierarchy)
        {
            //Move the ball using its rigid body
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 move = new Vector3(moveHorizontal, moveVertical, 0.0f);

            rb.AddForce(move * speed * Time.deltaTime);
        }

        //Allow the player to exit
        if (gameObject.activeInHierarchy && Input.GetKeyDown("escape"))
        {
            gameObject.transform.parent.gameObject.SetActive(false);
            transform.position = startPos;
            player.ReturnToScene();
        }
    }

    //If the ball collides with a wall, send it back to the start
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.name.Equals("Wall"))
        {
            transform.position = startPos;
            rb.velocity = Vector3.zero;
        }
    }

    //If the ball collides with a wrench pickup, turn off the minigame, increase the player's special XP, and return to the main game
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Equals("Wrench"))
        {
            GameObject monolith = GameObject.Find("RedMonolith");
            Destroy(monolith);

            collider.gameObject.transform.parent.gameObject.SetActive(false);
            Player.IncrementSpecialXP();
            player.ReturnToScene();
        }
    }
}
