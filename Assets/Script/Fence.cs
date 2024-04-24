using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    private int health = 100;
    // Start is called before the first frame update
    public void getHit(GameObject e)
    {
        var enemy = e.GetComponent<Enemy_Health>();
        if (health > 0)
        {
            health -= 10;
            
            enemy.hit();
        }
        else if (health <= 0) 
        {
            enemy.hit();
            Destroy(this.gameObject);
        }
    }

    
}
