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
    }

    //Enemy movement script using transforms
    public virtual void LateUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(playerTransform.position - transform.position), rotationSpeed * Time.deltaTime * 50f);

        Vector3 eulerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, eulerRotation.y, 0f);

        transform.position = new Vector3(transform.position.x, 2.55f, transform.position.z);

        rigid.MovePosition(rigid.position + transform.forward * moveSpeed * Time.deltaTime);
    }

    //On collision with a trigger character weapon
    private void OnTriggerEnter(Collider collider)
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

            CheckDestroy();
        }
    }

    private void CheckDestroy()
    {
        if(health <= 0.0f)
        {
            Destroy(gameObject);
            int xpDrop = Random.Range(0, 5);
            for(int i = 0; i < xpDrop; i++)
            {
                GameObject newDrop = Instantiate(xpPrefab);
                newDrop.transform.position = transform.position;
            }

            int hpDrop = Random.Range(0, 9);
            if(hpDrop == 0)
            {
                GameObject newDrop = Instantiate(hpPrefab);
                newDrop.transform.position = transform.position;
            }
        }
    }

    private void KnockBack()
    {
        if (!this.name.Equals("GooseEnemy(Clone)"))
        {
            rigid.MovePosition(rigid.position - transform.forward * 2);
        }
    }
}
