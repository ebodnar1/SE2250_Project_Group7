using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Child of the Enemy class
public class GooseEnemy : Enemy
{
    public GameObject projectile;
    private float projSpeed = 15.0f;
    private bool canShoot;
    public GameObject tail;

    //Attack if the enemy is within 20 points of distance of the player, otherwise keep moving
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

    //Attacking script
    private void Attack(bool proceed)
    {
        //See if the enemy is able to move
        if (canMove)
        {
            //See if the enemy should move (is further than 20 points of distance away)
            if (!proceed)
            {
                //Movement
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(playerTransform.position - transform.position), rotationSpeed * Time.deltaTime * 50f);

                Move();

                rigid.MovePosition(rigid.position + transform.forward * moveSpeed * Time.deltaTime);
            }
            else
            {
                //Look at the player
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(-playerTransform.position + transform.position), rotationSpeed * Time.deltaTime * 50f);

                Move();

                //Enemy shoots (if it can)
                if (!canShoot)
                {
                    StartCoroutine("Shoot");
                }
            }
        }
    }

    //Movement control
    private void Move()
    {
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, eulerRotation.y, 0f);

        transform.position = new Vector3(transform.position.x, 2.55f, transform.position.z);
    }

    //Pause 2s between shots
    IEnumerator Shoot()
    {
        canShoot = true;
        Fire();
        yield return new WaitForSeconds(2.0f);
        canShoot = false;
    }

    //Create a projectile and launch it towards the player, destroying this projectile after 2s
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

