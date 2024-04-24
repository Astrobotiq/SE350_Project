using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : GAgent
{
    // Start is called before the first frame update
    private void Start()
    {
        NavMeshAgent agent;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;


        base.Start();
        SubGoal s1 = new SubGoal("GoClosest", 1, true);
        goals.Add(s1, 3);
    }
}
