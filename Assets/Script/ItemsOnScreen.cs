using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsOnScreen : MonoBehaviour
{
     public List<GameObject> list;
    private void Awake()
    {
        list = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GameObject> ReturnList()
    {
        return list;
    }

    public void AddList(GameObject item)
    {
        list.Add(item);
    }

    public void DeleteFromList(GameObject item)
    {
        list.Remove(item);
    }
}
