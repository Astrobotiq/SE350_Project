using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

public class babyEnemyStateMachineScript : MonoBehaviour
{
    private enum States
    {
        Chase,
        Attack,
        Freeze
    }

    private States _states;
    private GameObject baby;
    public NavMeshAgent agent;
    public Animator animator;
    public bool canAttack;
    public Transform AttackPoint;
    public float range = 0.5f;
    public LayerMask enemyLayer;

    [SerializeField] private AudioSource hitBaby;
    [SerializeField] private AudioSource hitFurniture;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        _states = States.Chase;
    }

    private void Awake()
    {
        baby = GameObject.FindGameObjectWithTag("BabyTag");
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
                Debug.Log(angel);
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

        Collider2D[] nearObjects = Physics2D.OverlapCircleAll(AttackPoint.position, range, enemyLayer);
        //Debug.Log(Vector3.Distance(transform.position, player.transform.position));
        switch (_states)
        {
            default:

            case States.Chase:
                if (Time.time > lastFreezeTime + can_freeze_cooldown)//freeze icin bunla alakasi yok
                {
                    can_freeze = true;
                }

                float targetRange = 5f;
                if (Vector3.Distance(transform.position, baby.transform.position) < targetRange && canAttack)
                {
                    Debug.Log("bebeðe saldýrcam");
                    StartCoroutine(delay());
                    _states = States.Attack;
                }
                else if (nearObjects.Length > 0 && nearObjects[0].gameObject.tag == "Placeables")
                {
                    Debug.Log("Objeye saldýrcam");
                    StartCoroutine(delay());
                    _states = States.Attack;
                }
                agent.SetDestination(baby.transform.position);
                break;
            case States.Attack:
                _states = States.Chase;
                break;
            case States Freeze:
                if (first_entry)
                {
                    agent.speed = 0;
                    lastFreezeTime = Time.time;
                    can_freeze = false;
                    first_entry = false;
                }

                if (Time.time > lastFreezeTime + freeze_duration && can_freeze == false)
                {
                    Debug.Log("Dondum duzeldim");
                    agent.speed = 2;
                    _states = States.Chase;
                    lastFreezeTime = Time.time;
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

        Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint.position, range, enemyLayer);
        Debug.Log(enemies.Length);


        foreach (Collider2D enemy in enemies)
        {
            yield return new WaitForSeconds(1.5f);
            Debug.Log(enemy.gameObject.name);
            if (Time.time > lastAttackTime + cooldownTime)
            {
                lastAttackTime = Time.time;
                if (enemy.gameObject.tag == "BabyTag")
                {
                    hitBaby.Play();
                    animator.SetTrigger("Attack"); //burasi enemyStatemachine'den degisik
                    agent.speed = 0;
                    yield return new WaitForSeconds(5f);
                    agent.speed = 2;
                    Debug.Log("baby get hit");
                }
                else if (enemy.gameObject.tag == "Placeables")
                {
                    hitFurniture.Play();
                    animator.SetTrigger("Attack");
                    agent.speed = 0;
                    enemy.GetComponent<Placeable>().takeDamage();
                    yield return new WaitForSeconds(5f);
                    agent.speed = 2;

                }
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


}
