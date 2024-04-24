using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine2 : MonoBehaviour
{
    private enum States
    {
        Chase,
        Attack,
    }


    private States _states;
    private GameObject[] tempArray;
    private Transform baby;
    private List<GameObject> items;
    private GameObject itemholder;
    private ItemsOnScreen holder;
    public NavMeshAgent agent;
    public Animator animator;
    public bool canAttack;
    public Transform AttackPoint;
    public float range = 0.5f;
    public LayerMask enemyLayer;
    private GameObject target;
    
    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        _states = States.Chase;
    }

    private void Awake()
    {
        baby = GameObject.FindGameObjectWithTag("BabyTag").transform;
        itemholder = GameObject.FindGameObjectWithTag("ItemHolder");
        holder = itemholder.GetComponent<ItemsOnScreen>();
        items = holder.ReturnList();
        canAttack = false;
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
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                }
            }
        }

    }

    
    void FixedUpdate()
    {
        switch (_states)
        {
            default:
            case States.Chase:
                NavMeshPath path = new NavMeshPath();
                agent.destination = baby.position;
                agent.CalculatePath(agent.destination,path);
                if (path.status != NavMeshPathStatus.PathComplete)
                {
                    items = holder.ReturnList();
                    var shortestDistance = 1000f;
                    foreach(GameObject item in items)
                    {
                        if(Vector3.Distance(item.transform.position,transform.position) < shortestDistance)
                        {
                            shortestDistance = Vector3.Distance(item.transform.position, transform.position);
                            target = item;
                        }
                    }
                }
                else if (path.status == NavMeshPathStatus.PathComplete)
                {
                    target = GameObject.FindGameObjectWithTag("BabyTag");
                }

                Debug.Log(Vector3.Distance(transform.position, target.transform.position));
                Debug.Log(canAttack);
                float targetRange = 4f;
                if (Vector3.Distance(transform.position, target.transform.position) < targetRange && canAttack)
                {
                    StartCoroutine(delay());
                    _states = States.Attack;
                }
                agent.SetDestination(target.transform.position);
                break;
            case States.Attack:
                _states = States.Chase;
                break;
        }
    }

    IEnumerator delay()
    {
        canAttack = false;
        animator.SetTrigger("Attack");
        Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint.position, range, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            Debug.Log(enemy.gameObject.name);
            if (enemy.gameObject.tag == "PlayerTag")
            {
                enemy.GetComponent<PlayerHealth>().getHit();
            }
            else if (enemy.gameObject.tag == "Placeables")
            {
                enemy.GetComponent<Placeable>().takeDamage();
            }

        }
        yield return new WaitForSeconds(3f);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(AttackPoint.position, range);
    }

}
