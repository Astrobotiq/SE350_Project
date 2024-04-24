using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_State : MonoBehaviour
{
    enum WaveState
    {
        WaveStart,
        Wave,
        Interlude,

    }


    public List<Transform> spawnPoint;
    public List<GameObject> enemies;
    public int maxMoney;
    private int money;
    public int waveNum;
    public float stateKatsayýsý;
    public float spawnInterval;
    private WaveState waveState;

    public GameObject player;
    public GameObject baby;

    private PlayerHealth playerHealth;
    private BabySoundUpdate babyHealt;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
        babyHealt = baby.GetComponent<BabySoundUpdate>();
        waveState = WaveState.Interlude;
        maxMoney = 50;
        spawnInterval = 8;
        stateKatsayýsý = 0.1f;
        waveNum = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(waveState);

        if (waveState == WaveState.WaveStart)
        {
            Debug.Log("Wave start inside");
            StartCoroutine(waveStart());
        }
        else if (waveState == WaveState.Wave)
        {
            if (money <= 0 && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                Debug.Log("Wave Tamamlandý");
                stateKatsayýsý += 0.1f;
                waveState = WaveState.Interlude;
              
            }
            else
            {
                Debug.Log("Wave inside");
                StartCoroutine(enemySpawner());
                
            }
        }
        else if(waveState == WaveState.Interlude)
        {
            Debug.Log("Here");
            if (waveNum <= 5)
            {
                StartCoroutine(InterludeWaiter(30f));
                

            }
            else if (waveNum <= 10)
            {
                StartCoroutine(InterludeWaiter(60f));
                
            }
            else
            {
                StartCoroutine(InterludeWaiter(90f));
                
            }
        }
        
    }

    IEnumerator enemySpawner()
    {

        yield return new WaitForSeconds(5f);

        //int randomNum = Random.Range(0,enemies.Count);
        GameObject enemy = enemies[0];
        Debug.Log("Enemy name: "+enemy.name);
        Enemy_Health enemyHealth = enemy.GetComponent<Enemy_Health>();
        if (enemyHealth.getMoney() < money)
        {
            money -= enemyHealth.getMoney();
            //randomNum = Random.Range(0, spawnPoint.Count);
            Transform transform = spawnPoint[0];
            Instantiate(enemy, transform.position,transform.rotation);
        }
        
    }

    IEnumerator InterludeWaiter(float num)
    {
        yield return new WaitForSeconds (num);
        waveState = WaveState.WaveStart;
    }

    IEnumerator waveStart()
    {
        yield return new WaitForSeconds(5f);
        calculateMoney();
        money = maxMoney;
        waveNum++;
        waveState = WaveState.Wave;
    }


    public void calculateMoney()
    {

        if (player != null && baby != null)
        {
            if (playerHealth != null && babyHealt != null)
            {
                var avrgHealth = playerHealth.getHealth() + babyHealt.getHealth();

                maxMoney = (int)(maxMoney + ((int)avrgHealth * stateKatsayýsý));
                spawnInterval = spawnInterval + (50 - avrgHealth * (Mathf.Pow(stateKatsayýsý, 2)));


                maxMoney = maxMoney - (maxMoney % 5);
            }
        }
    }

    
}


