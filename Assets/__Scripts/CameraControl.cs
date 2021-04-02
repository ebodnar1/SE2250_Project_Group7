using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Allows the camera to remain focused on the ball
public class CameraControl : MonoBehaviour
{
    /*
     * public GameObject to represent the player and private 
     * 3D vector to represent the offset of the camera from the player
    */
    public GameObject player;
    private Vector3 offset;
    public float damping = 5;

    //For pickup tracking and instantiating
    public GameObject pickupPrefab;
    private GameObject[] activePickups;

    //Determines initial offset of the camera from the player
    void Start()
    {
        offset = player.transform.position - transform.position;

        if (SceneManager.GetActiveScene().name.Equals("Scene1"))
        {
            StartLevel1();
        }
        else
        {
            StartLevel2();
        }

        activePickups = GameObject.FindGameObjectsWithTag("Pickup");
    }

    /*
     * Continuously updates the position of the camera as the player moves
     * Uses LateUpdate to update the camera after the player's position
    */
    void LateUpdate()
    {
        activePickups = GameObject.FindGameObjectsWithTag("Pickup");

        float currentAngle = transform.eulerAngles.y;
        float targetAngle = player.transform.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * damping);

        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
        transform.position = player.transform.position - (rotation * offset);

        transform.LookAt(player.transform);

        CheckPickups();
    }

    //Check if there are any active pickups in the scene, and change the scene if there aren't
    private void CheckPickups()
    {
        if (GetRemainingPickups() == 0)
        {
            Invoke("ChangeScene", 2.0f);
        }
    }

    //Go to the next level
    private void ChangeScene()
    {
        int index = PlayerPrefs.GetInt("CharacterSelect");
        PlayerPrefs.SetInt("CharacterSelect", index);

        string currentScene = SceneManager.GetActiveScene().name;
        string nextScene = "Scene2";

        if (currentScene.Equals("Scene2"))
        {
            nextScene = "Scene3";
        }

        SceneManager.LoadScene(nextScene);
    }

    public int GetRemainingPickups()
    {
        return activePickups.Length;
    }

    private void StartLevel1()
    {
        //Instantiate 6 pickup items for progression

        //COMMENTED OUT SO I CAN PASS LEVEL 1 EASIER
        /*
        GameObject newObj1 = Instantiate(pickupPrefab);
        newObj1.transform.position = new Vector3(-95, 3, 95);

        GameObject newObj2 = Instantiate(pickupPrefab);
        newObj2.transform.position = new Vector3(95, 3, 95);

        GameObject newObj3 = Instantiate(pickupPrefab);
        newObj3.transform.position = new Vector3(-95, 3, -95);

        GameObject newObj4 = Instantiate(pickupPrefab);
        newObj4.transform.position = new Vector3(95, 3, -95);

        GameObject newObj5 = Instantiate(pickupPrefab);
        newObj5.transform.position = new Vector3(-6, 15, 60);
        */

        GameObject newObj6 = Instantiate(pickupPrefab);
        newObj6.transform.position = new Vector3(7, 15, -30);
    }

    private void StartLevel2() {
        //Instantiate 6 pickup items for progression
        GameObject newObj1 = Instantiate(pickupPrefab);
        newObj1.transform.position = new Vector3(113, 28, -113);

        GameObject newObj2 = Instantiate(pickupPrefab);
        newObj2.transform.position = new Vector3(88, 3, 88);

        GameObject newObj3 = Instantiate(pickupPrefab);
        newObj3.transform.position = new Vector3(-100, 3, 25);

        GameObject newObj4 = Instantiate(pickupPrefab);
        newObj4.transform.position = new Vector3(100, 3, -50);

        GameObject newObj5 = Instantiate(pickupPrefab);
        newObj5.transform.position = new Vector3(-72, 3, 120);

        GameObject newObj6 = Instantiate(pickupPrefab);
        newObj6.transform.position = new Vector3(-22, 3, 5);
    }

}
