using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Sword : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    private GameObject item;
    public GameObject bulletPrefab;
    private bool isOrb;
    public float range = 0.5f;
    public LayerMask enemyLayer;
    public Transform AttackPoint;
    public GameObject AttackEffect;

    //Sound
    [SerializeField]
    private AudioSource swordAttack;

    [SerializeField] private AudioSource gunAttack;

    // Start is called before the first frame update
    void Start()
    {
        isOrb = true;
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void swordToOrb()
    {
        isOrb = true;
        animator.SetBool("isOrb",true);
    }

    public void orbToSword()
    {
        isOrb = false;
        animator.SetBool("isOrb",false);
    }

    public void attack(Vector2 lookDirection,float angle)
    {
        if (isOrb)
        {
            
            gunAttack.Play();
            GameObject bullet = Instantiate(bulletPrefab, transform.position, new Quaternion(0, 0, angle, 0));
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(lookDirection * 10, ForceMode2D.Impulse);
        }
        else
        {
            animator.SetTrigger("Attack");
        }
        
    }

    public void hitDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint.position, range, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            Debug.Log(enemy.gameObject.name);
            if (enemy.gameObject.tag == "Enemy")
            {
                
                enemy.GetComponent<Enemy_Health>().hit();
            }
            else if (enemy.gameObject.tag == "Placeables")
            {
                enemy.GetComponent<Fridge>().getHit();
            }
            else if (enemy.gameObject.tag == "Radio")
            {
                enemy.GetComponent<radiohead>().takeDamage();
            }

        }
    }

    public Vector3 setPosition(Transform player, Vector2 r )
    {
        transform.position = new Vector3(player.position.x + r.x *2, player.position.y + r.y *2, player.position.z);

        return transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Placeables")
        {
            item = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        item = null;
    }

    public GameObject getItem()
    {
        return item;
    }

    public void invokeBlockAnimation()
    {
        
        
        swordAttack.Play();


        Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint.position, range, enemyLayer);

        foreach (Collider2D enemy in enemies)
        {
            
            if (enemy.gameObject.tag == "Enemy")
            {
                enemy.GetComponent<Enemy_Health>().Block();
            }
            

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(AttackPoint.position, range);
    }
}
