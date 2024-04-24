using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack_2 : MonoBehaviour
{
    public GameObject enemy;
    private babyEnemyStateMachineScript ESM;

    private void Awake()
    {
        ESM = enemy.GetComponent<babyEnemyStateMachineScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerTag")
        {
            ESM.canAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerTag")
        {
            ESM.canAttack = false;
        }
    }
}
