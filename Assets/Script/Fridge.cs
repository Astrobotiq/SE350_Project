using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : MonoBehaviour
{
    public Animator animator;
    public Placeable placeable;
    private bool canAnimate = true;
    public Transform AttackPoint;
    public Vector2 area = new Vector2(4,2);
    public LayerMask layer;

    [SerializeField] private AudioSource effect;

    public void getHit()
    {
        placeable.takeDamage();
        effect.Play();
        Debug.Log(canAnimate);
        if (canAnimate)
        {
            Debug.Log("Timer dýþýnda");
            StartCoroutine(timer());
            Collider2D[] enemies = Physics2D.OverlapBoxAll(AttackPoint.position, area, this.transform.eulerAngles.z, layer);

            if (enemies.Length > 0)
            {
                foreach (Collider2D c in enemies)
                {
                    c.GetComponent<Enemy_Health>().hit();
                    c.GetComponent<EnemyStateMachine>().Freeze_enemy();
                }
            }

            
            
        }


        
    }



    IEnumerator timer()
    {
        Debug.Log("Timer içinde");
        canAnimate = false;
        animator.SetTrigger("getHit");
        yield return new WaitForSeconds(5f);
        animator.SetBool("hasWaited",true);
        yield return new WaitForSeconds(2);
        animator.SetBool("hasWaited", false);
        canAnimate = true;

        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(AttackPoint.position,area);
    }
}
