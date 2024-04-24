using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_State_Machine : MonoBehaviour
{
    private enum States
    {
        Chase,
        Attack,
        Freeze
    }


    private States _states;
    private GameObject target;
    public NavMeshAgent agent;
    private float attackTime;
    private float freezeTime;
    public Animator animator;
    public Transform AttackPoint;
    public float range = 1;
    public LayerMask enemyLayer;

    [SerializeField]
    private AudioSource furnitureDamageSound;

    [SerializeField]
    private AudioSource playerDamageSound;

    private void Awake()
    {
        _states = States.Chase;
        int randomNum = Random.Range(0,2);
        if (randomNum == 0)
        {
            target = GameObject.FindWithTag("PlayerTag");
            float side = target.transform.position.x - transform.position.x;
            if (side >= 0)
            {
                agent.SetDestination(new Vector3(target.transform.position.x + 3f, target.transform.position.y + 1f, target.transform.position.z));
            }
            else
            {
                agent.SetDestination(new Vector3(target.transform.position.x - 3f, target.transform.position.y + 1f, target.transform.position.z));
            }
        }
        else if (randomNum == 1)
        {
            target = GameObject.FindWithTag("BabyTag");
            float side = target.transform.position.x - transform.position.x;
            if (side >= 0)
            {
                agent.SetDestination(new Vector3(target.transform.position.x + 3f, target.transform.position.y + 1f, target.transform.position.z));
            }
            else
            {
                agent.SetDestination(new Vector3(target.transform.position.x - 3f, target.transform.position.y + 1f, target.transform.position.z));
            };
        }
        agent.SetDestination(target.transform.position);

    }
    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity != Vector3.zero)
        {
            Vector3 moveDirection = new Vector3(agent.velocity.x, 0f, 0f);
            if (moveDirection != Vector3.zero)
            {
                float angel = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg;
                if (angel < 0f)
                {
                    transform.rotation = new Quaternion(0, 180, 0, 0);
                }
                else
                {
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                }
            }
        }



        //Target null ise yeni target seç
        if (target == null) 
        {
            int randomNum = Random.Range(0, 2);
            if (randomNum == 0)
            {
                target = GameObject.FindWithTag("PlayerTag");
                float side = target.transform.position.x - transform.position.x;
                if (side >= 0)
                {
                    agent.SetDestination(new Vector3(target.transform.position.x + 2f, target.transform.position.y + 1f, target.transform.position.z));
                }
                else
                {
                    agent.SetDestination(new Vector3(target.transform.position.x - 2f, target.transform.position.y + 1f, target.transform.position.z));
                }
            }
            else if (randomNum == 1)
            {
                target = GameObject.FindWithTag("BabyTag");
                float side = target.transform.position.x - transform.position.x;
                if (side >= 0)
                {
                    agent.SetDestination(new Vector3(target.transform.position.x + 2f, target.transform.position.y + 1f, target.transform.position.z));
                }
                else
                {
                    agent.SetDestination(new Vector3(target.transform.position.x - 2f, target.transform.position.y + 1f, target.transform.position.z));
                }
            }
        }

        Debug.Log(Vector3.Distance(transform.position,agent.destination));

        switch (_states)
        {
            case States.Chase:
                if (transform.position == agent.destination)
                {
                    agent.velocity = Vector3.zero;
                    agent.speed = 0f;
                    _states = States.Attack;
                }
                break;
            case States.Attack:
                float side = target.transform.position.x - transform.position.x;
                if (side >= 0)
                {
                    transform.rotation = new Quaternion(0, 180, 0, 0);
                }
                else
                {
                    transform.rotation = new Quaternion(0, 180, 0, 0);
                }

                attackTime += Time.deltaTime;

                if (attackTime >= 2)
                {

                    animator.SetTrigger("Attack");
                    attackTime = 0;
                }

                break;
            case States.Freeze:
                animator.SetBool("isFreezing",true);
                freezeTime += Time.deltaTime;

                if(freezeTime >= 3)
                {
                    freezeTime = 0;
                    agent.speed = 2;
                    animator.SetBool("isFreezing", false);
                    _states = States.Chase;
                }
                break;
        }
    }

    public void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint.position, range, enemyLayer);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.gameObject.tag == "PlayerTag")
            {
                playerDamageSound.Play();
                enemy.GetComponent<PlayerHealth>().getHit();
                enemy.GetComponent<PlayerHealth>().Knocback(this.transform);

            }
            else if (enemy.gameObject.tag == "Placeables")
            {
                furnitureDamageSound.Play();
                
                enemy.GetComponent<Placeable>().takeDamage();
                

            }
        }
    }

    public void setFreeze()
    {
        agent.velocity = Vector3.zero;
        agent.speed = 0;

        _states = States.Freeze;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Placeable")
        {
            target = collision.gameObject;
            float side = target.transform.position.x - transform.position.x;
            if (side >= 0)
            {
                agent.SetDestination(new Vector3(target.transform.position.x + 2f, target.transform.position.y + 1.5f, target.transform.position.z));
            }
            else
            {
                agent.SetDestination(new Vector3(target.transform.position.x - 2f, target.transform.position.y + 1.5f, target.transform.position.z));
            }
        }
        else if(collision.gameObject.tag == "PlayerTag")
        {
            target = collision.gameObject;
            float side = target.transform.position.x - transform.position.x;
            if (side >= 0)
            {
                agent.SetDestination(new Vector3(target.transform.position.x + 2f, target.transform.position.y + 1f, target.transform.position.z));
            }
            else
            {
                agent.SetDestination(new Vector3(target.transform.position.x - 2f, target.transform.position.y + 1f, target.transform.position.z));
            }
        }
        else if((collision.gameObject.tag == "BabyTag"))
        {
            target = collision.gameObject;
            float side = target.transform.position.x - transform.position.x;
            if (side >= 0)
            {
                agent.SetDestination(new Vector3(target.transform.position.x + 1.5f, target.transform.position.y , target.transform.position.z));
            }
            else
            {
                agent.SetDestination(new Vector3(target.transform.position.x - 1.5f, target.transform.position.y , target.transform.position.z));
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(agent.destination, range);
    }

}
