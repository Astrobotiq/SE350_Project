using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyer : MonoBehaviour
{
    public Animator animator;
    private void Awake()
    {
        animator.SetTrigger("start");
    }

    public void Destroy()
    {
        Destroy(this.gameObject);  
    }
}
