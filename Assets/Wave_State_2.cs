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
    public float stateKatsayısı;
    public float spawnInterval;
    private WaveState waveState;
    public float interludeTımer;
    public float waveStartTımer;
    public float waveTımer;

    public GameObject player;
    public GameObject baby;

    private PlayerHealth playerHealth;
    private BabySoundUpdate babyHealt;

    [SerializeField]
    private AudioSource spawnSound;

    [SerializeField]
    private AudioSource waveBitiş;

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
        stateKatsayısı = 0.1f;
        waveNum = 0;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Random: "+ Random.RandomRange(0,4));

        if (waveState == WaveState.WaveStart)
        {
            Debug.Log("Wave start inside");
            waveStartTımer += Time.deltaTime;
            if(waveStartTımer >= 5)
            {
                calculateMoney();
                money = maxMoney;
                waveNum++;
                waveStartTımer = 0;
                waveState = WaveState.Wave;
            }
        }
        else if (waveState == WaveState.Wave)
        {
            if (money <= 0 && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                Debug.Log("Wave Tamamlandı");
                stateKatsayısı += 0.1f;
                waveBitiş.Play();
                playerHealth.heal();
                waveState = WaveState.Interlude;
                

            }
            else
            {
                Debug.Log("Wave inside");
                waveTımer += Time.deltaTime;

                if(waveTımer >= spawnInterval)
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
                    waveTımer = 0;
                }
                

            }
        }
        else if (waveState == WaveState.Interlude)
        {
            Debug.Log("Here");
            if (waveNum <= 5)
            {
                interludeTımer += Time.deltaTime;
                if (interludeTımer >= 30)
                {
                    waveState = WaveState.WaveStart;
                    interludeTımer = 0;
                }

            }
            else if (waveNum <= 10)
            {
                interludeTımer += Time.deltaTime;
                if (interludeTımer >= 60)
                {
                    waveState = WaveState.WaveStart;
                    interludeTımer = 0;
                }

            }
            else
            {
                interludeTımer += Time.deltaTime;
                if (interludeTımer >= 90)
                {
                    waveState = WaveState.WaveStart;
                    interludeTımer = 0;
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
                Debug.Log("Pow: "+ Mathf.Pow(stateKatsayısı, 2));


                maxMoney = (int)(maxMoney + ((int)avrgHealth * stateKatsayısı));
                spawnInterval = spawnInterval + ((50 - avrgHealth) * (Mathf.Pow(stateKatsayısı, 2)));


                maxMoney = maxMoney - (maxMoney % 5);
            }
        }
    }
}
