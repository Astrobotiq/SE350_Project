using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WM_script : MonoBehaviour
{
    public Animator animator;
    public Placeable placeable;
    private bool canAnimate = true;
    public Transform AttackPoint;
    public Vector2 area = new Vector2(4, 2);
    public LayerMask layer;

    public void getHit()
    {
        Debug.Log("Hasar almaya hazýrým");
        placeable.takeDamage();
        if (canAnimate)
        {
            Collider2D[] enemies = Physics2D.OverlapBoxAll(AttackPoint.position, area, this.transform.eulerAngles.z, layer);

            foreach (Collider2D c in enemies)
            {
                c.GetComponent<EnemyStateMachine>().Freeze_enemy();
            }
            StartCoroutine(timer());
        }



    }



    IEnumerator timer()
    {
        canAnimate = false;
        animator.SetTrigger("getHit");
        yield return new WaitForSeconds(5f);
        animator.SetBool("hasWaited", true);
        yield return new WaitForSeconds(2);
        animator.SetBool("hasWaited", false);
        canAnimate = true;


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(AttackPoint.position, area);
    }
}
