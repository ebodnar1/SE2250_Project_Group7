using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUI : MonoBehaviour
{

    public bool statScreenEnabled;
    public GameObject statScreen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            statScreenEnabled = !statScreenEnabled; //toggle inventory
        }

        if (statScreenEnabled == true)
        {
            statScreen.SetActive(true);
        }
        else
        {
            statScreen.SetActive(false);
        }
    }
}
