using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Health : MonoBehaviour
{
    public int health;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private bool canBlock = true;
    private bool isBlocking = false;
    private float resetTime = 0;
    public int money;
    public Vector2 forceAmount;
    public GameObject blockEffect;
    public Transform blockPoint;
    public GameObject AttackEffect;

    //This will added to player's money when enemy die
    [SerializeField]
    private int playerMoney;
    private GameObject player;
    private Input_Manager inputManager;

    //Sound
    [SerializeField]
    private AudioSource Hit;
    [SerializeField]
    private AudioSource BlockSound;
    [SerializeField]
    private AudioSource Death;

    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("PlayerTag");
        inputManager = player.GetComponent<Input_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (agent.velocity != Vector3.zero)
        {
            animator.SetBool("isRunning",true);
        }
        else
        {
            animator.SetBool("isRunning", false);

        }

        if (!canBlock )
        {
            resetTime += Time.deltaTime;
            if (resetTime >=5 )
            {
                canBlock = true;
                resetTime = 0;
            }

            
        }

    }

    public int getMoney()
    {
        return money;
    }


    public void Block()
    {
        if (canBlock)
        {
            isBlocking = true;
            canBlock = false;
            StartCoroutine(block());
        }
    }
    

    public void hit()
    {
        if (!isBlocking)
        {
            Hit.Play();
            Instantiate(AttackEffect, transform.position, transform.rotation);
            health = health - 10;
            if (health < 0)
            {
                agent.velocity = Vector3.zero;
                agent.speed = 0;
                Death.Play();
                animator.SetBool("isDead", true);
                inputManager.addToMoney(playerMoney);
                Destroy(this.gameObject, 2f);
            }
        }
    }

    /*public void getFrozen()
    {
        StartCoroutine(waitFor());
        
    }

    IEnumerator waitFor()
    {
        spriteRenderer.material.color = Color.blue;
        yield return new WaitForSeconds(5);
        spriteRenderer.material.color = Color.white;
    }*/

    IEnumerator block()
    {
        BlockSound.Play();
        animator.SetBool("isBlocking",true);
        if (blockEffect!=null)
        {
            GameObject effect = Instantiate(blockEffect,new Vector3(transform.position.x+0.2f,transform.position.y-1.2f),blockPoint.rotation);
            Debug.Log("effect's location " + effect.transform.position);
            Debug.Log("blockPoint location " + blockPoint.position);
           
        }
        
        yield return new WaitForSeconds(1);
        animator.SetBool("isBlocking", false);
        isBlocking = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(blockPoint.position, 0.1f);
    }
}
