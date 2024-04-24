using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int maxHealth = 100;
    private int health = 100;
    public GameObject tempObject;
    public Rigidbody2D rb;
    public GameObject AttackEffect;

    [SerializeField] private AudioSource deadSound;
    [SerializeField] private AudioSource damageSound;

public int getHealth()
    {
        return health;
    }


    public void getHit()
    {
        damageSound.Play();
        health -= 10;

        if (health <= 0)
        {
            deadSound.Play();
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
        
        tempObject.GetComponent<PlayerHealthUIScript>().Damage(10);
       
    }

    public void heal()
    {
        health += 50;
        if (health + 50 >= maxHealth)
        {
            health = maxHealth;
        }
        tempObject.GetComponent<PlayerHealthUIScript>().Heal(50f);
    }

    public void Knocback(Transform Enemytransform)
    {
        Vector3 direction = (new Vector3(Enemytransform.position.x - transform.forward.x, Enemytransform.position.y - transform.forward.y-1.2f)).normalized;

        

        if (Enemytransform.rotation.y==0)
        {
            Instantiate(AttackEffect,transform.position,new Quaternion(0,0,0,0));
            rb.AddForce(direction * 50, ForceMode2D.Impulse);
        }
        else if(Enemytransform.rotation.y==1)
        {
            Instantiate(AttackEffect, transform.position, new Quaternion(0, 180, 0, 0));
            rb.AddForce(-direction * 50, ForceMode2D.Impulse);
        }


    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" )
        {
            Debug.Log("player damage yedi player health");
            getHit();
            tempObject.GetComponent<PlayerHealthUIScript>().Damage(10);
        }
    }*/
}
