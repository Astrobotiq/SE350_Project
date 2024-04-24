using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    private enum States
    {
        Chase,
        Attack,
        Freeze
    }

    private States _states;
    private GameObject player;
    public NavMeshAgent agent;
    public Animator animator;
    public bool canAttack;
    public Transform AttackPoint;
    public float range = 0.5f;
    public LayerMask enemyLayer;
    public ParticleSystem particle;

    [SerializeField]
    private AudioSource furnitureDamageSound;

    [SerializeField]
    private AudioSource playerDamageSound;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        _states = States.Chase;
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PlayerTag");
        canAttack = true;
    }

    private void Update()
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
                    transform.rotation = new Quaternion(0,0,0,0);
                }
            }
        }

        /*Debug.Log(agent.nextPosition.x - transform.position.x);
        if (agent.nextPosition.x - transform.position.x<0)
        {
            
        }
        else
        {
            transform.rotation = new Quaternion(0,0,0,0);
        }*/
    }
    public float cooldownTime = 1.5f;
    public float lastAttackTime = -999999f;

    public float freeze_duration = 5f;
    public float lastFreezeTime = -999999f;
    public float can_freeze_cooldown = 5f;

    public bool can_freeze = true;
    bool first_entry = true;
    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        CircleCollider2D APCollider = AttackPoint.gameObject.GetComponent<CircleCollider2D>();

        if (APCollider != null)
        {
            APCollider.enabled = false;
        }
        else
        {
            Debug.Log("No collider");
        }

        Collider2D[] nearObjects = Physics2D.OverlapCircleAll(AttackPoint.position, range, enemyLayer);
        if(nearObjects.Length > 0)
        {
            Debug.Log("Nearobject name is " + nearObjects[0].gameObject.name);
        }
        
        if (APCollider != null)
        {
            APCollider.enabled = true;
        }
        else
        {
            Debug.Log("No collider");
        }
        */

        Collider2D[] nearObjects = Physics2D.OverlapCircleAll(AttackPoint.position, range, enemyLayer);
        switch (_states)
        {
            default:
           
            case States.Chase:
                if (Time.time > lastFreezeTime + can_freeze_cooldown)//freeze icin bunla alakasi yok
                {
                    can_freeze = true;
                }

                float targetRange =5f;
                if (Vector3.Distance(transform.position, player.transform.position) < targetRange && canAttack)
                {
                    
                    agent.enabled = false;
                    Debug.Log("I am attacking");
                    StartCoroutine(delay());
                    agent.enabled = true;
                    _states = States.Attack;
                }
                else if(nearObjects.Length>0 && nearObjects[0].gameObject.tag == "Placeables")
                {
                    StartCoroutine(delay());
                    _states = States.Attack;
                }
                agent.SetDestination(player.transform.position);
                break;
                /*else if (nearObjects.Length > 0)
                {
                    List<Collider2D> temp_list = new List<Collider2D>();
                    foreach (Collider2D temp_placable in nearObjects)
                    {
                        if (temp_placable.gameObject.tag == "Placeable")
                        {
                            temp_list.Add(temp_placable);
                        }
                    }
                    if(temp_list.Count > 0)
                    {
                        StartCoroutine(delay());
                        _states = States.Attack;
                    }

                    agent.SetDestination(player.transform.position);
                    
                }
                break;*/
            case States.Attack:
                _states = States.Chase;
                break;
            case States Freeze:
                if(first_entry)
                {
                    agent.speed = 0;
                    lastFreezeTime = Time.time;
                    can_freeze = false;
                    first_entry = false;
                }
                
                if(Time.time > lastFreezeTime + freeze_duration && can_freeze==false)
                {
                    
                    agent.speed = 2;
                    _states = States.Chase;
                    lastFreezeTime= Time.time;
                }
                break;
        }
    }

    public void Freeze_enemy()
    {
        if (Time.time > lastFreezeTime + can_freeze_cooldown && can_freeze)
        {
            _states = States.Freeze;
            bool first_entry = true;
        }
        
    }

    IEnumerator delay()
    {

        canAttack = false;

        CircleCollider2D APCollider = AttackPoint.gameObject.GetComponent<CircleCollider2D>();

        if (APCollider != null)
        {
            APCollider.enabled = false;
        }
        else
        {
            Debug.Log("No collider");
        }

        yield return new WaitForSeconds(2f);
        
        Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint.position, range, enemyLayer);
        Debug.Log("Enemies length "+enemies.Length);


        foreach (Collider2D enemy in enemies)
        {
            
            Debug.Log(enemy.gameObject.name);
            if (Time.time > lastAttackTime + cooldownTime) { 
                lastAttackTime = Time.time;
                if (enemy.gameObject.tag == "PlayerTag")
                {
                    playerDamageSound.Play();
                    animator.SetTrigger("Attack");
                    //agent.speed = 0;
                    player.GetComponent<PlayerHealth>().getHit();
                    yield return new WaitForSeconds(1f);
                    //agent.speed = 2;
                    
                }
                else if (enemy.gameObject.tag == "Placeables")
                {
                    furnitureDamageSound.Play();
                    animator.SetTrigger("Attack");
                    //agent.speed = 0;
                    enemy.GetComponent<Placeable>().takeDamage();
                    yield return new WaitForSeconds(1f);
                    //agent.speed = 2;
                
                }
            }
        }

        if (APCollider != null)
        {
            APCollider.enabled = true;
        }
        else
        {
            Debug.Log("No collider");
        }


    }

    public void KnockbackStarter()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint.position, range, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.gameObject.tag == "PlayerTag")
            {
                player.GetComponent<PlayerHealth>().Knocback(this.transform);
            }
        }
    }

    /* void findTarget()
     {
         float targetRange = 50f;

         if (Vector3.Distance(transform.position, player.transform.position) < targetRange)
         {
             _states = States.Chase;
         }
     }*/

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(AttackPoint.position, range);
    }

    public void playDust()
    {
        particle.Play();
    }


}
