using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.XR;

public class Input_Manager : MonoBehaviour
{
    //Input for movement
    Input input;
    //Rigidbody for walk and stuff
    public Rigidbody2D rb;

    //Vector2 for left and right stick value
    Vector2 leftRotation;
    Vector2 rightRotation;

    

    //Shooting properity
    public GameObject bulletPrefab;
    Vector2 lookDirection;
    private float angle;

    //Item holder and item rotation
    public GameObject itemHolder;
    private ItemHolder itemHolderScript;
    private ItemsOnScreen itemsOnScreen;
    private GameObject item;
    private Placeable placeable;

    //Grid's location info
    private Vector2 gridInfo;
    public Grid grid;

    //Placement and deplacement
    private bool canPlace = true;

    //Animator Referance
    public Animator animator;

    //UI inventory
    [SerializeField]
    private UIInventoryManager inventoryManager;

    //Sword
    public GameObject sword;
    private Sword swordScript;
    public float x;
    public float y;

    //Particle System
    public ParticleSystem particle;

    //Sound
    [SerializeField]
    private AudioSource walk;

    [SerializeField]
    private AudioSource placementSound;

    [SerializeField]
    private int money;
    public Money moneyUI;

    public 










    // Start is called before the first frame update
    void Start()
    {
        input = new Input();
        input.MainMovement.Enable();
        itemHolderScript = itemHolder.GetComponent<ItemHolder>();
        itemsOnScreen = itemHolder.GetComponent<ItemsOnScreen>();
        swordScript = sword.GetComponent<Sword>();



    }

    // Update is called once per frame
    void Update()
    {
        //Reading input from left and right stick.
        leftRotation = input.MainMovement.Walk.ReadValue<Vector2>();
        rightRotation = input.MainMovement.HandRotation.ReadValue<Vector2>();

        //Character Direction
        if (rightRotation.x > 0)
        {
            transform.rotation = new Quaternion(0,0,0,0);
        }
        else if(rightRotation.x < 0){
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }

        //Setting character walk and speed
        rb.velocity = leftRotation * 3;

        //Character wallking cycle
        if (rb.velocity == Vector2.zero)
        {
            animator.SetBool("isWallking", false);
        }
        else
        {
            animator.SetBool("isWallking", true);
        }

        
        if (canPlace)
        {
            //Setting Character Hands Position
            sword.transform.position = swordScript.setPosition(this.transform, rightRotation);
            Vector3Int cellPos = grid.WorldToCell(sword.transform.position);
            gridInfo = grid.CellToWorld(cellPos);

            //Calculating bullet angle
            lookDirection = sword.transform.position - transform.position;
            angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;

            if (item != null)
            {
                item.transform.position = gridInfo;
            }
        }
        else
        {
            Debug.Log("rotation y: "+ transform.rotation.y);
            if (transform.rotation.y == 0)
            {
                
                sword.transform.position = new Vector2(transform.position.x + 0.8f, transform.position.y + 0.5f );
                sword.transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            }
            else if (transform.rotation.y == 1)
            {
                sword.transform.position = new Vector2(transform.position.x + x, transform.position.y + y);
                sword.transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            }
            
        }
        

        



        

        
    }
    
    //This method where shooting is happening
    public void onShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
                swordScript.attack(lookDirection,angle);

                
            
            
        }
    }

    public void onPlacement(InputAction.CallbackContext context)
    {
        
        if (canPlace)
        {
            if (context.performed && item != null && placeable.isPlaceable && placeable.getCost() <= money)
            {
                money -= placeable.getCost();
                moneyUI.changeText(money);
                placeable.material.color = Color.white;
                Destroy(placeable);
                BoxCollider2D collider = item.GetComponent<BoxCollider2D>();
                collider.isTrigger = false;
                placementSound.Play();
                item = Instantiate(itemHolderScript.getItem());
                placeable = item.GetComponent<Placeable>();
                itemsOnScreen.AddList(item);
            }
            else if (context.performed && item == null)
            {
                placementSound.Play();
                item = swordScript.getItem();
                item.AddComponent<Placeable>();
                placeable = item.GetComponent<Placeable>();
                placeable.material = item.GetComponent<SpriteRenderer>();
                BoxCollider2D collider = item.GetComponent<BoxCollider2D>();
                collider.isTrigger = true;
                canPlace = true;
                itemsOnScreen.DeleteFromList(item);
            }
        }
        
    }

    public void onRotateItem(InputAction.CallbackContext context)
    {
        if (context.performed && item != null) 
        {
            Destroy(item.gameObject);
            item = Instantiate(itemHolderScript.nextItem());
            placeable = item.GetComponent<Placeable>();
            BoxCollider2D collider = item.GetComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }
    }

    public void onSwitch(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            canPlace = !canPlace;
            
            if (!canPlace)
            {
                Destroy(item);
                swordScript.orbToSword();
            }
            else
            {
                swordScript.swordToOrb();
            }
            
        }
    }

    public void onOpenUI(InputAction.CallbackContext context)
    {
        if (context.performed && !inventoryManager.isActiveAndEnabled)
        {
            inventoryManager.show();
        }
        else if (context.performed && inventoryManager.isActiveAndEnabled)
        {
            inventoryManager.hide();
        }
    }

    public void onRight(InputAction.CallbackContext context)
    {
        if (context.performed )
        {
            if (inventoryManager.isActiveAndEnabled)
            {
                if (inventoryManager.getItemIndex()+1 <= inventoryManager.getListSize()-1)
                {
                    inventoryManager.changeItem(inventoryManager.getItemIndex()+1);
                }
                else if (inventoryManager.getItemIndex() ==  inventoryManager.getListSize()-1)
                {
                    inventoryManager.changeItem(0);
                }
            }
        }
    }

    public void onLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryManager.isActiveAndEnabled)
            {
                if (inventoryManager.getItemIndex() - 1 >= 0)
                {
                    inventoryManager.changeItem(inventoryManager.getItemIndex() - 1);
                }
                else if (inventoryManager.getItemIndex() - 1 < 0)
                {
                    inventoryManager.changeItem(inventoryManager.getListSize()-1);
                }
            }
        }
    }

    public void onDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryManager.isActiveAndEnabled)
            {
                if (inventoryManager.getItemIndex() + 4 <= inventoryManager.getListSize() - 1)
                {
                    inventoryManager.changeItem(inventoryManager.getItemIndex() + 4);
                }
                else
                {
                    inventoryManager.changeItem(inventoryManager.getItemIndex() % 4);
                }
            }
        }
    }

    public void onUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryManager.isActiveAndEnabled)
            {
                if (inventoryManager.getItemIndex() - 4 > 0)
                {
                    inventoryManager.changeItem(inventoryManager.getItemIndex() - 4);
                }
                else
                {
                    int possible = inventoryManager.getListSize()-1;
                    while (possible % 4 != inventoryManager.getItemIndex())
                    {
                        possible--;
                    }
                    inventoryManager.changeItem(possible);
                }
            }
        }
    }

    public void onInteraction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryManager.isActiveAndEnabled)
            {
                if (item != null)
                {
                    Destroy(item.gameObject);
                }
                
                item = Instantiate(itemHolderScript.selectedItem(inventoryManager.getItemIndex()));
                placeable = item.GetComponent<Placeable>();
                BoxCollider2D collider = item.GetComponent<BoxCollider2D>();
                collider.isTrigger = true;
            }
        }
    }

    public void playDust()
    {
        walk.Play();
        particle.Play();
    }

    public void addToMoney(int enemyMoney)
    {
        money += enemyMoney;

        moneyUI.changeText(money);
    }

    

    


}
