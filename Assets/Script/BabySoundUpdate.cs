using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BabySoundUpdate : MonoBehaviour
{
    private int health = 100;
    private int maxHealth = 100;
    public GameObject tempGameObject;
    public int damage_can_get = 10;
    private bool canHeal=false;
    private float healTimer;

    [SerializeField] private AudioSource babyCooing;
    [SerializeField] private AudioSource babyCrying;
    public void Update()
    {
        if (canHeal)
        {
            healTimer += Time.deltaTime;
            if (healTimer >= 5)
            {
                Heal();
                healTimer = 0;
            }
        }
    }

    public int getHealth()
    {
        return health;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.gameObject.tag == "Enemy")
        {
            takeDamage();
        }
        else if(other.gameObject.tag == "PlayerTag")
        {
            canHeal = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerTag")
        {
            canHeal=false;
        }
    }

    private void Heal()
    {
        health += 5;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        tempGameObject.GetComponent<BabySoundBar>().Heal(5f);
    }

    public void takeDamage()
    {
        tempGameObject.GetComponent<BabySoundBar>().Damage(damage_can_get);
        babyCooing.Play();
        health -= damage_can_get;

        if (health <= 0)
        {
            babyCrying.Play();
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }
}
