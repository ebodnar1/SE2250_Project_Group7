using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupControl : MonoBehaviour
{
    //Continuously rotate the pickups
    void Update()
    {
        RotateItem();
    }

    private void RotateItem()
    {
        switch (gameObject.name)
        {
            case "XP(Clone)":
                transform.Rotate(new Vector3(1, 1, 1));
                break;
            case "HealthPickup(Clone)":
                transform.Rotate(new Vector3(0, 2, 0));
                break;
        }
    }
}
