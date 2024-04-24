using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Animator animator;

    [SerializeField] private AudioSource explodeSound;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log(collision.gameObject.name);
            Enemy_Health enemy = collision.gameObject.GetComponent<Enemy_Health>();
            enemy.hit();
            
        }
        animator.SetTrigger("Explode");

    }

    public void Destroy()
    {
        explodeSound.Play();
        Destroy(this.gameObject);
    }
}
