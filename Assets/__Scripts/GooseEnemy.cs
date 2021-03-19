using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseEnemy : Enemy
{
    public GameObject projectile;
    private float projSpeed = 15.0f;
    private bool canShoot;
    public GameObject tail;

    private void Start()
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

    public override void LateUpdate()
    {
        if ((transform.position - currentPlayer.transform.position).magnitude <= 20.0f)
        {
            Attack(true);
        }
        else
        {
            Attack(false);
        }
    }

    private void Attack(bool proceed)
    {
        if (!proceed)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(playerTransform.position - transform.position), rotationSpeed * Time.deltaTime * 50f);

            Move();

            rigid.MovePosition(rigid.position + transform.forward * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(-playerTransform.position + transform.position), rotationSpeed * Time.deltaTime * 50f);

            Move();

            if (!canShoot)
            {
                StartCoroutine("Shoot");
            }
        }
    }

    private void Move()
    {
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, eulerRotation.y, 0f);

        transform.position = new Vector3(transform.position.x, 2.55f, transform.position.z);
    }

    IEnumerator Shoot()
    {
        canShoot = true;
        Fire();
        yield return new WaitForSeconds(2.0f);
        canShoot = false;
    }

    private void Fire()
    {
        GameObject fired = Instantiate(projectile);
        fired.transform.position = tail.transform.position;
        Rigidbody rigid = fired.GetComponent<Rigidbody>();

        fired.transform.rotation = Quaternion.Slerp(transform.rotation,
               Quaternion.LookRotation(-playerTransform.position + transform.position), rotationSpeed * 50f);
        fired.transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0, transform.rotation.w);

        rigid.velocity -= (fired.transform.forward) * projSpeed;

        Destroy(fired, 2.0f);
    }
}

