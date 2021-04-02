using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private GameObject currentWeapon;
    public float projSpeed = 20.0f;
    public GameObject projectile;
    public GameObject potion;

    private GameObject[] limbs;
    private Animator[] animators;
    private Animator[] rightArms;
    private Animator attackArm;

    public bool canAttack;

    private void Awake()
    {
        canAttack = true;
        

        //Get all of the limbs (via tag)
        limbs = GameObject.FindGameObjectsWithTag("Limb");
        //Arrays for regular walking limbs (legs and left arms) and attacking limbs (right arms)
        animators = new Animator[limbs.Length - 3];
        rightArms = new Animator[3];

        //Initialize the animators array with all of the animations
        int count = 0;
        int j = 0;
        for (int i = 0; i < limbs.Length; i++)
        {
            //If there is a right arm, add it to its own special array
            if (limbs[i].name.Equals("RightArm"))
            {
                rightArms[j] = limbs[i].GetComponent<Animator>();
                rightArms[j].enabled = false;
                j++;
            }
            //Add any other limbs to the animators array
            else
            {
                animators[count] = limbs[i].GetComponent<Animator>();
                count++;
            }
        }
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

        for (int i = 0; i < rightArms.Length; i++)
        {
            if (rightArms[i].gameObject.activeInHierarchy)
            {
                attackArm = rightArms[i];
                break;
            }
        }
    }

    void LateUpdate()
    {
        //Press 'z' to attack
        if (Input.GetKeyDown("z") && canAttack)
        {
            switch (currentWeapon.name)
            {
                case ("GunTest"):
                    ElectricalAttack();
                    break;
                case ("GirderWeapon"):
                    StartCoroutine(CivilAttack());
                    break;
                case ("Potion"):
                    StartCoroutine(ChemicalAttack());
                    break;
            }
        }
    }

    //Instantiates 3 projectiles at the gun's tip and shoots them in a burst of a small random angle 
    void ElectricalAttack()
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

    //Creates a floating chemical that explodes after 3 seconds
    IEnumerator ChemicalAttack()
    {
        canAttack = false;
        attackArm.enabled = true;
        yield return new WaitForSeconds(5f / 6f);

        GameObject newPotion = Instantiate(potion);
        newPotion.transform.position = currentWeapon.transform.position;
        Rigidbody rig = newPotion.GetComponent<Rigidbody>();

        yield return new WaitForSeconds(0.5f);

        attackArm.enabled = false;
        canAttack = true;

        yield return new WaitForSeconds(3.0f);
        Explode(newPotion);
    }

    /*
     Play the exploding animation and enable the isTrigger collider, destroying the
     'bomb' after 1s (length of animation)
    */
    void Explode(GameObject bomb)
    {
        bomb.GetComponent<SphereCollider>().enabled = true;
        bomb.transform.position = bomb.transform.position;
        Animator explosion = bomb.GetComponentInChildren<Animator>();
        explosion.enabled = true;

        Destroy(bomb, 1.0f);
    }

}
