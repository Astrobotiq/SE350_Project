using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : MonoBehaviour
{
    public int HealthValue;
    public GameObject this_object;

    // Start is called before the first frame update
    void Start()
    {
        HealthValue = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageItem(int damage)
    {
        HealthValue = HealthValue - damage;
        Debug.Log("item took 10 damage");
        if (HealthValue <= 0)
        {
            Debug.Log("must destroy");
            Destroy(gameObject);
        }
        Debug.Log(HealthValue);

    }
    public int getHealthValue()
    {
        return HealthValue;
    }
}
