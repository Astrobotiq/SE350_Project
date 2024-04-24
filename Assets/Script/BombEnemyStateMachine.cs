using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.AI;

public class BombEnemyStateMachine : MonoBehaviour
{
    private enum States
    {
        FindSpote,
        GoOut,
    }

    private States _states;
    public GameObject target;
    public GameObject speaker;
    public GameObject HouseExit;
    public NavMeshAgent agent;
    public Animator animator;
    public float dropAndExitRange;
    public LayerMask enemyLayer;
    public int numberOfLocations;
    public List<GameObject> locations;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        _states = States.FindSpote;
    }

    private void Awake()
    {
        HouseExit = GameObject.Find("HouseExit");
        for(int i = 0; i < numberOfLocations; i++) { 
            string temp = (string) "speakerLoc" + (i+1);
            Debug.Log(temp + " name of list game object");
            locations.Add(GameObject.Find(temp));
        }
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

    // Update is called once per frame

    bool first_entry = true;//  ilk kez girdiginde 1 kere taget belirlesin diye
    void FixedUpdate()
    {
        if(target != null)
        {
             Debug.Log(Vector3.Distance(transform.position, target.transform.position));
        }
        
        switch (_states)
        {
            default:
            case States.FindSpote:
                if (first_entry)
                {
                    int randomnumber = Random.Range(1, numberOfLocations);
                    target = locations[randomnumber];
                    agent.SetDestination(target.transform.position);
                    first_entry = false;
                }

                if (Vector3.Distance(transform.position , target.transform.position) < dropAndExitRange)
                {
                    Debug.Log("Ben buraya geldim");
                    StartCoroutine(delay());
                    _states = States.GoOut;
                }
                break;

            case States.GoOut:
                target = HouseExit;
                agent.SetDestination(target.transform.position);
                if (Vector3.Distance(transform.position, target.transform.position) < dropAndExitRange)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    IEnumerator delay()
    {
        Debug.Log("ben buraya geldim");
        agent.speed = 0;
        Instantiate(speaker, target.transform.position, target.transform.rotation);
        agent.speed = 3.5f;
        yield return new WaitForSeconds(3);
    }

   
}

/*
 * public 
 * 
*/
