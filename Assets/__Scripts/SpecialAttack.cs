using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    //Necessary GameObjects and tracking fields
    private static float cooldown;
    public GameObject boulder;
    public GameObject shockwave;
    public GameObject aura;
    public GameObject terrain;
    public GameObject earthquake;
    private GameObject activePlayer;
    private Player player;
    private bool isUpgraded;
    public GameObject shield;
    public GameObject gun;
    private Weapon weapon;
    public GameObject raindrop;
    private static float limit;
    public SkillBar skillBar;
    private static bool paused;
    private Spawner[] spawners;

    // Get the current character and set the cooldown to 5s away
    void Start()
    {
        cooldown = 10;
        limit = 15;

        //Find what player is currently in the game
        GameObject[] possiblePlayers = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < possiblePlayers.Length; i++)
        {
            if (possiblePlayers[i].activeInHierarchy)
            {
                activePlayer = possiblePlayers[i];
            }
        }

        player = GetComponent<Player>();
        weapon = GetComponent<Weapon>();
        spawners = GameObject.Find("Spawners").GetComponentsInChildren<Spawner>();
    }

    //Increment the cooldown based on time elapsed since start/last special attack
    //Press 'x' to activate special attack (possible every 15s)
    void Update()
    {
        if (!paused)
        {
            cooldown += Time.deltaTime;
        }

        isUpgraded = player.GetUpgradeStatus();
        limit = isUpgraded ? 20.0f : 15.0f;
        skillBar.SetValue((int) cooldown);
        skillBar.SetMaxValue((int) limit);

        if (Input.GetKeyDown("x") && cooldown >= limit)
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

    private void CivilSpecial()
    {
        //Create a rock and set its position to be in front of and above the player
        if (!player.GetUpgradeStatus())
        {
            GameObject civilRock = Instantiate(boulder);

            civilRock.transform.position = activePlayer.transform.position + (activePlayer.transform.forward * 25)
                + new Vector3(0, -10, 0);

            StartCoroutine(DropBoulder(civilRock));
        }
        //Earthquake!
        else
        {
            Animator terrainShake = terrain.GetComponent<Animator>();
            StartCoroutine(Earthquake(terrainShake));
        }
    }


    /*
      Enable the rock's animator, give the rock an upwards velocity to rise out of the ground,
      and then drop the rock (has gravity ON) while disabling the animator and enabling the collider
    */
    IEnumerator DropBoulder(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        Animator anim = obj.GetComponent<Animator>();
        MeshCollider mc = obj.GetComponent<MeshCollider>();

        anim.enabled = true;
        rb.velocity += new Vector3(0, 24, 0);

        yield return new WaitForSeconds(2.95f);

        anim.enabled = false;
        mc.enabled = true;
        rb.velocity = Vector3.zero;
    }

    //Shakes the earth for 5 seconds
    IEnumerator Earthquake(Animator anim)
    {
        ChangeSpawning(false);
        yield return new WaitForSeconds(0.1f);

        anim.enabled = true;
        BoxCollider bc = earthquake.GetComponent<BoxCollider>();
        bc.enabled = true;
        yield return new WaitForSeconds(5f);
        bc.enabled = false;
        anim.enabled = false;

        ChangeSpawning(true);
    }

    //Enemies cannot spawn while the earthquake is on
    private void ChangeSpawning(bool status)
    {
        foreach (Spawner s in spawners)
        {
            s.SetSpawningStatus(status);
        }
    }

    private void ChemicalSpecial()
    {
        //Enable the MeshRenderer and SphereCollider of the chemical aura
        if (!player.GetUpgradeStatus())
        {
            MeshRenderer rend = aura.GetComponent<MeshRenderer>();
            SphereCollider sph = aura.GetComponent<SphereCollider>();
            CapsuleCollider cc = player.GetComponent<CapsuleCollider>();
            rend.enabled = true;
            sph.enabled = true;
            cc.enabled = false;
            StartCoroutine(AuraOff(rend, sph, cc));
        }
        //Acid rain!
        else
        {
            StartCoroutine(AcidRain());
        }
    }

    //Disable the MeshRenderer and SphereCollider after 5 seconds
    IEnumerator AuraOff(MeshRenderer mr, SphereCollider sc, CapsuleCollider cc)
    {
        yield return new WaitForSeconds(3.0f);
        sc.enabled = false;
        mr.enabled = false;
        cc.enabled = true;
    }

    //Creates 1500 raindrops in an area surrounding the player over a short period of time
    IEnumerator AcidRain()
    {
        for(int i = 0; i < 1500; i++)
        {
            GameObject drop = Instantiate(raindrop);
            int rand1 = Random.Range(-30, 31);
            int rand2 = Random.Range(-30, 31);

            drop.transform.position = player.transform.position + new Vector3(rand1, 20, rand2);
            Destroy(drop, 2.5f);

            if (i % 5 == 0)
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private void ElectricalSpecial()
    {
        //Enable the SphereCollider and MeshRenderer of the shockwave
        if (!player.GetUpgradeStatus())
        {
            Animator anim = shockwave.GetComponent<Animator>();
            SphereCollider radius = shockwave.GetComponent<SphereCollider>();
            MeshRenderer mr = shockwave.GetComponent<MeshRenderer>();
            StartCoroutine(Shockwave(anim, radius, mr));
        }
        //Otherwise enable the BoxCollider and MeshRenderer of the shield
        else
        {
            BoxCollider bc = shield.GetComponent<BoxCollider>();
            MeshRenderer mr = shield.GetComponent<MeshRenderer>();
            StartCoroutine(Shield(mr, bc));
        }
    }

    //Enable the Animator and then after 1s (length of animation), disable all of these properties
    IEnumerator Shockwave(Animator a, SphereCollider c, MeshRenderer m)
    {
        m.enabled = true;
        c.enabled = true;
        a.enabled = true;
        yield return new WaitForSeconds(1.0f);
        shockwave.transform.localScale = new Vector3(1, 1, 1);
        a.enabled = false;
        c.enabled = false;
        m.enabled = false;
    }

    //Enable the shield for 7.5s
    IEnumerator Shield(MeshRenderer m, BoxCollider b)
    {
        gun.GetComponent<MeshRenderer>().enabled = false;
        b.enabled = true;
        m.enabled = true;
        weapon.canAttack = false;
        yield return new WaitForSeconds(7.5f);
        weapon.canAttack = true;
        b.enabled = false;
        m.enabled = false;
        gun.GetComponent<MeshRenderer>().enabled = true;
    }

    //Getters and setters for private fields
    public bool GetShieldStatus()
    {
        return shield.GetComponent<MeshRenderer>().enabled;
    }

    public static float GetLimit()
    {
        return limit;
    }
    public static float GetCooldown()
    {
        return cooldown;
    }

    public static void SetPaused(bool pause)
    {
        paused = pause;
    }
}
