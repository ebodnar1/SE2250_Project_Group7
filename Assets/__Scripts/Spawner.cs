using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //Track which enemies will be spawned and how frequently they will be spawned
    public GameObject[] spawnedEnemies;
    public float spawnsPerSecond;
    public GameObject currentPlayer;

    //Allows for toggling spawners on and off
    public bool canSpawn;
    private bool cooldown;

    //Start the spawning, allow enemies to spawn
    void Awake()
    {
        canSpawn = true;
        Invoke("SpawnEnemy", 1f / spawnsPerSecond);
    }

    //If enemies can spawn, invoke the spawn function
    private void FixedUpdate()
    {
        if (canSpawn)
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    //Spawn a random enemy at the location of the spawner every 1 / spawnsPerSecond seconds
    IEnumerator SpawnEnemy()
    {
        if (!cooldown)
        {
            cooldown = true;
            int index = Random.Range(0, spawnedEnemies.Length);
            GameObject spawned = Instantiate(spawnedEnemies[index]);

            spawned.transform.position = transform.position;
            yield return new WaitForSeconds(1 / spawnsPerSecond);
            cooldown = false;
        }
    }
}
