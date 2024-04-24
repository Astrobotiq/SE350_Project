using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBreakingObject : MonoBehaviour
{
    public GameObject tempGameObject;
    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Damage the item");
        tempGameObject.GetComponent<General>().DamageItem(10);
    }
}
