using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject enemy;
    private EnemyStateMachine ESM;

    private void Awake()
    {
        ESM = enemy.GetComponent<EnemyStateMachine>();  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerTag" )
        {
            ESM.canAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerTag")
        {
            ESM.canAttack = false;
        }
    }
}
