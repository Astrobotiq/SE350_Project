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
    public float stateKatsay�s�;
    public float spawnInterval;
    private WaveState waveState;
    public float interludeT�mer;
    public float waveStartT�mer;
    public float waveT�mer;

    public GameObject player;
    public GameObject baby;

    private PlayerHealth playerHealth;
    private BabySoundUpdate babyHealt;

    [SerializeField]
    private AudioSource spawnSound;

    [SerializeField]
    private AudioSource waveBiti�;

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
        stateKatsay�s� = 0.1f;
        waveNum = 0;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Random: "+ Random.RandomRange(0,4));

        if (waveState == WaveState.WaveStart)
        {
            Debug.Log("Wave start inside");
            waveStartT�mer += Time.deltaTime;
            if(waveStartT�mer >= 5)
            {
                calculateMoney();
                money = maxMoney;
                waveNum++;
                waveStartT�mer = 0;
                waveState = WaveState.Wave;
            }
        }
        else if (waveState == WaveState.Wave)
        {
            if (money <= 0 && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                Debug.Log("Wave Tamamland�");
                stateKatsay�s� += 0.1f;
                waveBiti�.Play();
                playerHealth.heal();
                waveState = WaveState.Interlude;
                

            }
            else
            {
                Debug.Log("Wave inside");
                waveT�mer += Time.deltaTime;

                if(waveT�mer >= spawnInterval)
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
                    waveT�mer = 0;
                }
                

            }
        }
        else if (waveState == WaveState.Interlude)
        {
            Debug.Log("Here");
            if (waveNum <= 5)
            {
                interludeT�mer += Time.deltaTime;
                if (interludeT�mer >= 30)
                {
                    waveState = WaveState.WaveStart;
                    interludeT�mer = 0;
                }

            }
            else if (waveNum <= 10)
            {
                interludeT�mer += Time.deltaTime;
                if (interludeT�mer >= 60)
                {
                    waveState = WaveState.WaveStart;
                    interludeT�mer = 0;
                }

            }
            else
            {
                interludeT�mer += Time.deltaTime;
                if (interludeT�mer >= 90)
                {
                    waveState = WaveState.WaveStart;
                    interludeT�mer = 0;
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
                Debug.Log("Pow: "+ Mathf.Pow(stateKatsay�s�, 2));


                maxMoney = (int)(maxMoney + ((int)avrgHealth * stateKatsay�s�));
                spawnInterval = spawnInterval + ((50 - avrgHealth) * (Mathf.Pow(stateKatsay�s�, 2)));


                maxMoney = maxMoney - (maxMoney % 5);
            }
        }
    }
}
