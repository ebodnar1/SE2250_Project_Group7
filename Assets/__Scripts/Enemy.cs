using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    protected Transform playerTransform;
    public Rigidbody rigid;
    public float rotationSpeed = 300.0f;
    public float moveSpeed = 50.0f;
    public float health = 30.0f;
    protected GameObject currentPlayer;
    protected bool canMove;

    private GameObject[] limbs;
    protected Animator[] animators;

    public GameObject xpPrefab;
    public GameObject hpPrefab;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        //Find which player is active
        GameObject[] possiblePlayers = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < possiblePlayers.Length; i++)
        {
            if (possiblePlayers[i].activeInHierarchy)
            {
                currentPlayer = possiblePlayers[i];
            }
        }
        playerTransform = currentPlayer.transform;

        //Get all of the limbs (via tag)
        limbs = GameObject.FindGameObjectsWithTag("EnemyLimb");
        animators = new Animator[limbs.Length];

        //Initialize the animators array with all of the animations
        for (int i = 0; i < limbs.Length; i++)
        {
            animators[i] = limbs[i].GetComponent<Animator>();
        }

        canMove = true;
        MoveLimbs();
    }

    //Enemy movement script using transforms
    public virtual void LateUpdate()
    {
        if(canMove)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(playerTransform.position - transform.position), rotationSpeed * Time.deltaTime * 50f);

            Vector3 eulerRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0f, eulerRotation.y, 0f);

            transform.position = new Vector3(transform.position.x, 2.55f, transform.position.z);

            rigid.MovePosition(rigid.position + transform.forward * moveSpeed * Time.deltaTime);
        }
    }

    private void MoveLimbs()
    {
        for (int i = 0; i < limbs.Length; i++)
        {
            animators[i].enabled = true;
        }
    }

    private void StopLimbs()
    {
        for (int i = 0; i < limbs.Length; i++)
        {
            animators[i].enabled = false;
        }
    }

    //On collision with a trigger character weapon
    private void OnTriggerEnter(Collider collider)
    {
        CollisionEvents(collider);
    }

    public void CollisionEvents(Collider collider)
    {
        if (collider.gameObject.CompareTag("Weapon") || collider.gameObject.CompareTag("Projectile"))
        {
            if (currentPlayer.name.Equals("chemicalCharacter"))
            {
                health -= 2.5f * Player.GetStrength();
            }
            else
            {
                health -= Player.GetStrength();
            }

            //If a projectile collides with the enemy, destroy the projectile
            if (collider.gameObject.CompareTag("Projectile"))
            {
                Destroy(collider.gameObject);
            }

            //Knock the enemy back and see if it should be destroyed
            KnockBack();
        }
        //If the civil special hits the enemy, destroy it and drop XP/HP
        if (collider.gameObject.name.Equals("CivilSpecial(Clone)"))
        {
            Destroy(gameObject);
            EnemyDrops();
        }

        //If the electrical special hits the enemy, damage it slightly and stun it for 5 seconds
        if (collider.gameObject.name.Equals("Shockwave"))
        {
            canMove = false;
            health -= 10.0f;
            Invoke("ResumeMovement", 5.0f);
        }
        CheckDestroy();
    }

    //If an enemy is in the chemical special aura, continuously damage it for 0.5 damage per frame
    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.name.Equals("SpecialActivator"))
        {
            health -= 0.5f;
            CheckDestroy();
        }
        if (collider.gameObject.name.Equals("Earthquake"))
        {
            canMove = false;
            health -= 0.2f;
            CheckDestroy();
            Invoke("ResumeMovement", 6f);
        }
        if (collider.gameObject.name.Equals("Shield"))
        {
            health -= 20.0f;
            KnockBack();
            CheckDestroy();
        }
    }

    //Allows enemies to move again
    protected void ResumeMovement()
    {
        canMove = true;
    }

    //See if the enemy is out of health
    private void CheckDestroy()
    {
        if(health <= 0.0f)
        {
            Destroy(gameObject);
            EnemyDrops();
        }
    }

    //Drop a random amount of XP between 1 and 5 points
    //There is a 1/10 chance of getting a HP potion drop
    private void EnemyDrops()
    {
        GameObject xpDrop = Instantiate(xpPrefab);
        xpDrop.transform.position = transform.position;

        int hpDrop = Random.Range(0, 9);
        if (hpDrop == 0)
        {
            GameObject newDrop = Instantiate(hpPrefab);
            newDrop.transform.position = transform.position;
        }
    }

    //Knock an enemy backwards
    private void KnockBack()
    {
        if (this.name.Equals("Enemy(Clone)"))
        {
            rigid.MovePosition(rigid.position - transform.forward * 2);
        }
    }
}
