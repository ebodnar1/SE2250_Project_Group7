using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    //Necessary GameObjects and tracking fields
    private float cooldown;
    public GameObject boulder;
    public GameObject shockwave;
    public GameObject aura;
    private GameObject activePlayer;

    // Get the current character and set the cooldown to 5s away
    void Start()
    {
        cooldown = 10;

        //Find what player is currently in the game
        GameObject[] possiblePlayers = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < possiblePlayers.Length; i++)
        {
            if (possiblePlayers[i].activeInHierarchy)
            {
                activePlayer = possiblePlayers[i];
            }
        }
    }

    //Increment the cooldown based on time elapsed since start/last special attack
    //Press 'x' to activate special attack (possible every 15s)
    void Update()
    {
        cooldown += Time.deltaTime;

        if (Input.GetKeyDown("x") && cooldown >= 15.0f)
        {
            switch (activePlayer.name)
            {
                case "electricalCharacter":
                    ElectricalSpecial();
                    break;
                case "civilCharacter":
                    CivilSpecial();
                    break;
                case "chemicalCharacter":
                    ChemicalSpecial();
                    break;
            }

            cooldown = 0;
        }
    }

    //Create a rock and set its position to be in front of and above the player
    private void CivilSpecial()
    {
        GameObject civilRock = Instantiate(boulder);

        civilRock.transform.position = activePlayer.transform.position + (activePlayer.transform.forward * 25)
            + new Vector3(0, 15.0f, 0);

        StartCoroutine(DropBoulder(civilRock));
    }

    //Drop the rock after 1.2s and destroy it around when it hits the ground
    IEnumerator DropBoulder(GameObject obj)
    {
        yield return new WaitForSeconds(1.2f);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.velocity -= new Vector3(0, 25, 0);

        Destroy(obj, 0.9f);
    }

    //Enable the MeshRenderer and SphereCollider of the chemical aura
    private void ChemicalSpecial()
    {
        MeshRenderer rend = aura.GetComponent<MeshRenderer>();
        SphereCollider sph = aura.GetComponent<SphereCollider>();
        rend.enabled = true;
        sph.enabled = true;
        StartCoroutine(AuraOff(rend, sph));
    }

    //Disable the MeshRenderer and SphereCollider after 5 seconds
    IEnumerator AuraOff(MeshRenderer mr, SphereCollider sc)
    {
        yield return new WaitForSeconds(5.0f);
        sc.enabled = false;
        mr.enabled = false;
    }

    //Enable the SphereCollider and MeshRenderer of the shockwave
    private void ElectricalSpecial()
    {
        Animator anim = shockwave.GetComponent<Animator>();
        SphereCollider radius = shockwave.GetComponent<SphereCollider>();
        MeshRenderer mr = shockwave.GetComponent<MeshRenderer>();
        mr.enabled = true;
        radius.enabled = true;
        StartCoroutine(Shockwave(anim, radius, mr));
    }

    //Enable the Animator and then after 1s (length of animation), disable all of these properties
    IEnumerator Shockwave(Animator a, SphereCollider c, MeshRenderer m)
    {
        a.enabled = true;
        yield return new WaitForSeconds(1.0f);
        a.enabled = false;
        c.enabled = false;
        m.enabled = false;
    }
}
