using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    public int health;
    public int maxhealth;
    public bool isPlaceable;
    public int damageCanGet = 5;
    public SpriteRenderer material;

    [SerializeField]
    private int cost;

    

    private void Awake()
    {
        isPlaceable = true;
        material.color = Color.green;
    }

    

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        isPlaceable = false;
        material.color = Color.red;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isPlaceable = true;
        material.color = Color.green;

    }

    public void takeDamage() //gets 5 damage
    {
        
        health = health - damageCanGet;
        Debug.Log("item took 10 damage");
        if (health <= 0)
        {
            Debug.Log("must destroy");
            Destroy(gameObject);
        }
        
    }
    public int getHealthValue()
    {
        return health;
    }

    public void repair()
    {
        if (health + 30 > maxhealth)
        {
            health = maxhealth;
        }
        else
        {
            health += 30;
        }
    }

    public int getCost()
    {
        return cost;
    }
}
