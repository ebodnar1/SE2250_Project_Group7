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
    private Vector3 playerStartPos;

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
        GetPlayer();

        //Get all of the limbs
        animators = gameObject.GetComponentsInChildren<Animator>();

        canMove = true;
        MoveLimbs();
        playerStartPos = GameObject.Find("Ground").transform.position;
    }

    public void GetPlayer()
    {
        //Find which player is active
        GameObject[] possiblePlayers = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < possiblePlayers.Length; i++)
        {
            if (possiblePlayers[i].activeInHierarchy)
            {
                currentPlayer = possiblePlayers[i];
            }
        }
        playerTransform = currentPlayer.transform;
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
        if (name.Equals("IveyEnemy(Clone)") || name.Equals("GooseEnemy(Clone)"))
        {
            foreach (Animator anim in animators)
            {
                anim.enabled = true;
            }
        }
    }

    private void StopLimbs()
    {
        if (name.Equals("IveyEnemy(Clone)") || name.Equals("GooseEnemy(Clone)"))
        {
            foreach (Animator anim in animators)
            {
                anim.enabled = false;
            }
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

        //Do 10 damage if the enemy is hit by an acid raindrop
        if (collider.gameObject.name.Equals("Raindrop(Clone)"))
        {
            health -= 10.0f;
        }

        //If the electrical special hits the enemy, damage it slightly and stun it for 5 seconds
        if (collider.gameObject.name.Equals("Shockwave"))
        {
            canMove = false;
            Invoke("ResumeMovement", 5.0f);
            StopLimbs();
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
            StopLimbs();
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
        MoveLimbs();
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
        xpDrop.transform.position = new Vector3(transform.position.x, playerStartPos.y + 3, transform.position.z);

        int hpDrop = Random.Range(0, 9);
        if (hpDrop == 0)
        {
            GameObject newDrop = Instantiate(hpPrefab);
            newDrop.transform.position = new Vector3(transform.position.x, playerStartPos.y + 3, transform.position.z);
        }
    }

    //Knock an enemy backwards
    private void KnockBack()
    {
        if (canMove)
        {
            Vector3 movement = Vector3.zero;
            if (name.Equals("IveyEnemy(Clone)") || name.Equals("IveyEnemy"))
            {
                movement = transform.forward;
            }
            if (name.Equals("GooseEnemy(Clone)") || name.Equals("GooseEnemy"))
            {
                if ((transform.position - currentPlayer.transform.position).magnitude <= 20.0f)
                {
                    movement = -transform.forward;
                }
                else
                {
                    movement = transform.forward;
                }
            }
            if (name.Equals("Crab(Clone)") || name.Equals("Crab"))
            {
                movement = -transform.forward;
            }

            rigid.MovePosition(rigid.position - movement);
        }
    }

    public void SetMovementStatus(bool move)
    {
        canMove = move;
    }
}
