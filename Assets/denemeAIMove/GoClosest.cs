using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoClosest : GAction
{
    
    public override bool PrePerform()
    {
        agent.SetDestination(base.target.transform.position);
        return true;
    }

    public override bool PostPerform()
    {

        return true;

    }

}
