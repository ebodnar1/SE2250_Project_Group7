using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private GameObject currentWeapon;
    private float projSpeed = 20.0f;
    public GameObject projectile;
    public GameObject potion;

    private Animator attackArm;
    private Player player;

    public bool canAttack;

    private void Awake()
    {
        canAttack = true;
    }

    //Find the current weapon and get the attack arm of the character
    void Start()
    {
        GameObject[] possibleWeapons = GameObject.FindGameObjectsWithTag("Weapon");
        for(int i = 0; i < possibleWeapons.Length; i++)
        {
            if (possibleWeapons[i].activeInHierarchy)
            {
                currentWeapon = possibleWeapons[i];
            }
        }

        player = GetComponent<Player>();

        //Enable the limbs but disable any children animators (shockwave, exploder)
        Animator[] limbs = player.GetComponentsInChildren<Animator>();
        foreach (Animator limb in limbs)
        {
            limb.enabled = true;
            if (limb.gameObject.name.Equals("RightArm"))
            {
                attackArm = limb;
                limb.enabled = false;
            }
            if (limb.gameObject.name.Equals("Exploder") || limb.gameObject.name.Equals("Shockwave"))
            {
                limb.enabled = false;
            }
        }
    }

    void LateUpdate()
    {
        //Press 'z' to attack
        if (Input.GetKeyDown("z") && canAttack)
        {
            switch (player.GetActivePlayer().name)
            { 
                case ("electricalCharacter"):
                    ElectricalAttack();
                    break;
                case ("civilCharacter"):
                    StartCoroutine(CivilAttack());
                    break;
                case ("chemicalCharacter"):
                    StartCoroutine(ChemicalAttack());
                    break;
            }
        }
    }

    //Instantiates 3 projectiles at the gun's tip and shoots them in a burst of a small random angle 
    private void ElectricalAttack()
    {
        GameObject proj1 = Instantiate(projectile);

        proj1.transform.position = currentWeapon.transform.position;
        proj1.transform.rotation = currentWeapon.transform.rotation;

        Rigidbody r1 = proj1.GetComponent<Rigidbody>();

        r1.velocity += currentWeapon.transform.forward * projSpeed;

        Destroy(proj1, 3.0f);
    }

    //Activates the arm swining animation for 500ms (length of animation)
    IEnumerator CivilAttack()
    {
        canAttack = false;
        attackArm.enabled = true;
        yield return new WaitForSeconds(5f / 6f);
        attackArm.enabled = false;
        canAttack = true;
    }

    //Creates a chemical vial that explodes after 3 seconds
    IEnumerator ChemicalAttack()
    {
        canAttack = false;
        attackArm.enabled = true;
        yield return new WaitForSeconds(5f / 6f);

        GameObject newPotion = Instantiate(potion);
        newPotion.transform.position = currentWeapon.transform.position;
        Rigidbody rig = newPotion.GetComponent<Rigidbody>();
        rig.velocity = (transform.forward + transform.up).normalized * 10f;

        yield return new WaitForSeconds(0.5f);

        attackArm.enabled = false;
        canAttack = true;

        yield return new WaitForSeconds(2.0f);
        Explode(newPotion);
    }

    /*
     Play the exploding animation and enable the isTrigger collider, destroying the
     'bomb' after 1s (length of animation)
    */
    private void Explode(GameObject bomb)
    {
        SphereCollider col = bomb.GetComponentsInChildren<SphereCollider>()[1];
        col.enabled = true;
        Animator explosion = bomb.GetComponentInChildren<Animator>();
        explosion.enabled = true;

        Destroy(bomb, 1.0f);
    }

}
