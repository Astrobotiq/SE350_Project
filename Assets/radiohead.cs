using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radiohead : MonoBehaviour
{
    [SerializeField]
    private AudioSource rickRoll;

    GameObject baby;
    BabySoundUpdate babyHealth;
    float timeInterval;

    [SerializeField] private int health;
    

    private void Awake()
    {
        baby = GameObject.FindWithTag("BabyTag");
        if (baby != null)
        {
            babyHealth = baby.GetComponent<BabySoundUpdate>();
        }
        rickRoll.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (babyHealth != null)
        {
            timeInterval += Time.deltaTime;
            if (timeInterval >= 8)
            {
                babyHealth.takeDamage();
                timeInterval = 0;
            }
        }
    }

    public void takeDamage()
    {
        health -= 10;

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
