using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_State_2 : MonoBehaviour
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
    public float interludeTýmer;
    public float waveStartTýmer;
    public float waveTýmer;

    public GameObject player;
    public GameObject baby;

    private PlayerHealth playerHealth;
    private BabySoundUpdate babyHealt;

    [SerializeField]
    private AudioSource spawnSound;

    [SerializeField]
    private AudioSource waveBitiþ;

    [SerializeField]
    private AudioSource waveStartSound;
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
        Debug.Log("Random: "+ Random.RandomRange(0,4));

        if (waveState == WaveState.WaveStart)
        {
            Debug.Log("Wave start inside");
            waveStartTýmer += Time.deltaTime;
            if(waveStartTýmer >= 5)
            {
                calculateMoney();
                money = maxMoney;
                waveNum++;
                waveStartTýmer = 0;
                waveState = WaveState.Wave;
            }
        }
        else if (waveState == WaveState.Wave)
        {
            if (money <= 0 && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                Debug.Log("Wave Tamamlandý");
                stateKatsayýsý += 0.1f;
                waveBitiþ.Play();
                playerHealth.heal();
                waveState = WaveState.Interlude;
                

            }
            else
            {
                Debug.Log("Wave inside");
                waveTýmer += Time.deltaTime;

                if(waveTýmer >= spawnInterval)
                {
                    int randomNum = (int)Mathf.Floor(Random.Range(0,3));
                    GameObject enemy = enemies[randomNum];
                    Debug.Log("Enemy name: " + enemy.name);
                    Enemy_Health enemyHealth = enemy.GetComponent<Enemy_Health>();
                    if (enemyHealth.getMoney() <= money)
                    {
                        money -= enemyHealth.getMoney();
                        randomNum = (int)Mathf.Floor(Random.Range(0, 4));
                        Transform transform = spawnPoint[randomNum];
                        spawnSound.Play();
                        Instantiate(enemy, transform.position, transform.rotation);
                    }
                    waveTýmer = 0;
                }
                

            }
        }
        else if (waveState == WaveState.Interlude)
        {
            Debug.Log("Here");
            if (waveNum <= 5)
            {
                interludeTýmer += Time.deltaTime;
                if (interludeTýmer >= 30)
                {
                    waveState = WaveState.WaveStart;
                    interludeTýmer = 0;
                }

            }
            else if (waveNum <= 10)
            {
                interludeTýmer += Time.deltaTime;
                if (interludeTýmer >= 60)
                {
                    waveState = WaveState.WaveStart;
                    interludeTýmer = 0;
                }

            }
            else
            {
                interludeTýmer += Time.deltaTime;
                if (interludeTýmer >= 90)
                {
                    waveState = WaveState.WaveStart;
                    interludeTýmer = 0;
                }

            }
        }

    }

  

    public void calculateMoney()
    {

        if (player != null && baby != null)
        {
            if (playerHealth != null && babyHealt != null)
            {
                var avrgHealth = (playerHealth.getHealth() + babyHealt.getHealth())/2;
                Debug.Log("Average health: "+ avrgHealth);
                Debug.Log("Pow: "+ Mathf.Pow(stateKatsayýsý, 2));


                maxMoney = (int)(maxMoney + ((int)avrgHealth * stateKatsayýsý));
                spawnInterval = spawnInterval + ((50 - avrgHealth) * (Mathf.Pow(stateKatsayýsý, 2)));


                maxMoney = maxMoney - (maxMoney % 5);
            }
        }
    }
}
