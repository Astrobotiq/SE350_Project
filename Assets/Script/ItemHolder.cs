using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public List<List<GameObject>> items = new List<List<GameObject>>();
    public List<GameObject> indexZero;
    public List<GameObject> indexOne;
    public List<GameObject> indexTwo;
    public List<GameObject> indexThree;

    public int itemNum;
    public int indexNum;

    private void Start()
    {
        items.Add(indexZero);
        items.Add(indexOne);
        items.Add(indexTwo);
        items.Add(indexThree);
        itemNum = 0;
    }
    

    public GameObject nextItem()
    {
        if (itemNum+1<items[indexNum].Count)
        {
            return items[indexNum][++itemNum];
        }
        else
        {
            itemNum = 0;
            return items[indexNum][itemNum];
        }
        
    }

    public GameObject getItem()
    {
        return items[indexNum][0];
    }

    public GameObject selectedItem(int num)
    {
        indexNum = num;
        return items[num][0];
    }

}
