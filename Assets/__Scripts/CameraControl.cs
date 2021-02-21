using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Determines initial offset of the camera from the player
    void Start()
    {
        offset = player.transform.position - transform.position;
    }

    /*
     * Continuously updates the position of the camera as the player moves
     * Uses LateUpdate to update the camera after the player's position
    */
    void LateUpdate()
    {
        float currentAngle = transform.eulerAngles.y;
        float targetAngle = player.transform.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * damping);

        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
        transform.position = player.transform.position - (rotation * offset);

        transform.LookAt(player.transform);

        /*
        Vector3 moveTo = player.transform.position + offset;
        Vector3 position = Vector3.Lerp(transform.position, moveTo, Time.deltaTime * damping);
        transform.position = position;
        transform.LookAt(player.transform);
        */
    }
}
