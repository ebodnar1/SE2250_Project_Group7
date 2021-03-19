using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //Track which enemies will be spawned and how frequently they will be spawned
    public GameObject[] spawnedEnemies;
    public float spawnsPerSecond;
    public GameObject currentPlayer;

    void Awake()
    {
        Invoke("SpawnEnemy", 1f / spawnsPerSecond);
    }

    //Spawn a random enemy at the location of the spawner every 1 / spawnsPerSecond seconds
    private void SpawnEnemy()
    {
        int index = Random.Range(0, spawnedEnemies.Length);
        GameObject spawned = Instantiate(spawnedEnemies[index]);

        spawned.transform.position = transform.position;

        Invoke("SpawnEnemy", 1f / spawnsPerSecond);
    }
}
