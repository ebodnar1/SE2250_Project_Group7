using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabEnemy : Enemy
{
    private Animator explosion;

    private void Start()
    {
        explosion = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();

        //Find which player is active
        GameObject[] possiblePlayers = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < possiblePlayers.Length; i++)
        {
            if (possiblePlayers[i].activeInHierarchy)
            {
                currentPlayer = possiblePlayers[i];
            }
        }

        canMove = true;
    }

    public override void LateUpdate()
    {
        //Move the crab
        Move();

        //If the crab is within 4m of the player, explode it
        if ((currentPlayer.transform.position - transform.position).magnitude <= 4.0f)
        {
            StartCoroutine(ExplodeAnimation());
        }
    }


    private void Move()
    {
        if (canMove)
        {
            //Move the crab towards the player (and face it forwards)
            Vector3 pos = Vector3.MoveTowards(transform.position, currentPlayer.transform.position, moveSpeed * Time.fixedDeltaTime);
            rigid.MovePosition(pos);
            transform.LookAt(currentPlayer.transform);
            transform.Rotate(0, 180, 0);

            //Make sure the crab does not rotate in the x or z directions
            Vector3 eulerRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0f, eulerRotation.y, 0f);
        }
    }

    //Turn the sphere (exploding) animation on for 1/3 of a second (length of animation)
    IEnumerator ExplodeAnimation()
    {
        explosion.enabled = true;
        yield return new WaitForSeconds(1 / 3f);
        Destroy(gameObject);
    }
}
