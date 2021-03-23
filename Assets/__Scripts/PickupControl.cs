using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupControl : MonoBehaviour
{
    //Continuously rotate the pickups
    void Update()
    {
        transform.Rotate(new Vector3(2, 2, 2));
    }
}
