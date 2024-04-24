using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindClosest : GAction
{
    public ItemsOnScreen items;
    private GameObject targetObject;

    private void Start()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("ItemHolder");
        items = temp.GetComponent<ItemsOnScreen>();
    }
    public override bool PrePerform()
    {
        
        return true;
    }

    public override bool PostPerform()
    {
        return true;

    }

    private void Update()
    {
        if (target == null)
        {
            List<GameObject> list = items.ReturnList();
            if (list.Count > 0)
            {
                float distance = 10000;
                foreach (GameObject go in list)
                {
                    Debug.Log(go.gameObject.name);
                    float tempDistance  = Vector2.Distance(this.gameObject.transform.position,go.gameObject.transform.position);

                    if (tempDistance < distance)
                    {
                        targetObject = go;
                    }
                }
                target = targetObject;
                Debug.Log(target.gameObject.name);
            }
        }
        
    }


}
