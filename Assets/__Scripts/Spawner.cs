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
    private bool canSpawn;
    private bool cooldown;
    private bool maximum;

    //Start the spawning, allow enemies to spawn
    void Awake()
    {
        Invoke("SpawnEnemy", 1f / spawnsPerSecond);
        canSpawn = true;
        maximum = false;
    }

    //If enemies can spawn, invoke the spawn function
    private void FixedUpdate()
    {
        if (canSpawn && !maximum)
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

            GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            int enemyCount = activeEnemies.Length;

            foreach(GameObject enem in activeEnemies)
            {
                if(enem.transform.parent != null && enem.transform.parent.name.Equals("Snakes"))
                {
                    enemyCount--;
                }
            }
        }
    }

    //Toggle spawning on or off
    public void SetSpawningStatus(bool truth)
    {
        canSpawn = truth;
    }
}
