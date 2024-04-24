using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : GAction
{
    bool is_target_defined = false;
    public override bool PrePerform()
    {
       
        return true;
    }

    public override bool PostPerform()
    {
        
        return true;
        
    }

    private void FixedUpdate()
    {
        PlayerKovala();
    }

    public void PlayerKovala(){
        if (!is_target_defined)
        {
            this.target = GameObject.FindGameObjectWithTag("PlayerTag").gameObject;
            is_target_defined = true;
        }
        
        agent.SetDestination(base.target.transform.position);
    }
    
}
