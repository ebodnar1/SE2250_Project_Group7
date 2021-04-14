using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnemy : Enemy
{
    public GameObject projectile;
    public GameObject mouth;
    private bool canShoot;
    private bool aboveGround;
    private Transform startPos;
    private bool isMoving;
    
    private void Start()
    {
        base.GetPlayer();

        //The snake starts below ground and can 'move' and shoot
        canShoot = true;
        canMove = true;
        aboveGround = false;

        startPos = transform;
    }

    public override void LateUpdate()
    {
        //Snake looks at the player
        Vector3 target = new Vector3(currentPlayer.transform.position.x, transform.position.y, currentPlayer.transform.position.z);
        transform.LookAt(target);
        transform.Rotate(0, 180, 0);

        //If the snake can shoot (off shoot cooldown) and move (not stunned) and is above ground, shoot a projectile
        if (canShoot && aboveGround && canMove)
        {
            Shoot();
        }

        //Check the movement conditions
        CheckMove();
    }

    private void CheckMove()
    {
        //If the snake can move (not stunned)
        if (canMove)
        {
            //If the player is closer than 50 units of distance, move the snake above ground over 2 seconds
            if ((currentPlayer.transform.position - transform.position).magnitude <= 50.0f && !aboveGround)
            {
                StartCoroutine(Move(new Vector3(startPos.position.x, 0.25f, startPos.position.z), 2));
            }
            //If the player is closer than 3 units of distance, hide the snake below ground over 0.3 seconds
            if ((currentPlayer.transform.position - transform.position).magnitude <= 3.0f && aboveGround)
            {
                StartCoroutine(Move(new Vector3(startPos.position.x, -8, startPos.position.z), 0.3f));
            }
            //If the player is further than 50 units of distance, move the snake below ground over 2 seconds
            else if ((currentPlayer.transform.position - transform.position).magnitude > 50.0f && aboveGround)
            {
                StartCoroutine(Move(new Vector3(startPos.position.x, -8, startPos.position.z), 2));
            }
        }
    }

    //Shooting functionality
    private void Shoot()
    {
        //Instantiate a projectile at the snake's mouth
        GameObject proj = Instantiate(projectile);
        proj.transform.position = mouth.transform.position;

        //Add a forward/upward velocity to the projectile
        Rigidbody projRigid = proj.GetComponent<Rigidbody>();
        projRigid.velocity += (-transform.forward + transform.up) * 10;

        //Start the shooting cooldown
        StartCoroutine(ShootingCooldown());
    }

    //The snake cannot shoot for another 4 seconds
    IEnumerator ShootingCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(4.0f);
        canShoot = true;
    }

    //Handles the snake movements
    private IEnumerator Move(Vector3 targetPosition, float duration)
    {
        //If the snake is already moving then do not start another coroutine
        if (isMoving) yield break;

        isMoving = true;

        var startPosition = transform.position;
        var passedTime = 0f;

        do
        {
            //Move the snake with this speed
            var lerpfactor = passedTime / duration;
            var smoothLerpfactor = Mathf.SmoothStep(0, 1, lerpfactor);

            //Linearly interpolate the snake's upward/downward movement and ensure it is not rotating along the x or z axes
            transform.position = Vector3.Lerp(startPosition, targetPosition, smoothLerpfactor);
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);

            //Increase the tracked passedTime
            passedTime += Time.deltaTime;

            //Return to the main thread
            yield return null;

        } while (passedTime < duration);
        //Repeat for the specified length of time

        //Avoid any slight discrepancies in final location
        transform.position = targetPosition;

        //If the snake ends up above ground, it is aboveGround and enable the collider
        if (targetPosition.y > 0)
        {
            aboveGround = true;
        }
        //If the snake ends up below ground, it is belowGround and disable the collider
        else
        {
            aboveGround = false;
        }

        //Now the coroutine can be started again
        isMoving = false;
    }

}
