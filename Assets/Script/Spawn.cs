using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int numEnemies;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numEnemies; i++)
        {
            Instantiate(enemyPrefab, this.transform.position, Quaternion.identity);
        }

        Invoke("SpawnEnemy", 5);
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, this.transform.position, Quaternion.identity);
        Invoke("SpawnEnemy", Random.RandomRange(2,10));
        Debug.Log("InSpawnEnemy");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
