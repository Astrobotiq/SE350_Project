using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryManager : MonoBehaviour
{
    [SerializeField]
    private UIInventoryItem itemPrefab;

    [SerializeField]
    private RectTransform contentPanel;

    public List<Sprite> images = new List<Sprite> ();

    [SerializeField]
    private string[] costs = new string[4];

    List<UIInventoryItem> ListOfUIItems = new List<UIInventoryItem>();

    private int inventorySize = 4;
    private int inventoryIndex = 0;

    public void initializeInventoryUI()
    {
        for (int i = 0; i<images.Capacity; i++)
        {
            UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            Image itemImage = uiItem.item.GetComponent<Image>();
            //TextMeshProUGUI text = uiItem.item.GetComponent<TextMeshProUGUI>();
            itemImage.sprite = images[i];
            //text.text = costs[i];
            uiItem.transform.SetParent(contentPanel);
            ListOfUIItems.Add(uiItem);
        }
    }

    private void Start()
    {
        initializeInventoryUI();
        ListOfUIItems[inventoryIndex].setActive();
    }

    public void show()
    {
        gameObject.SetActive(true);
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }

    public int getItemIndex()
    {
        return inventoryIndex;
    }

    public void changeItem(int index)
    {
        ListOfUIItems[inventoryIndex].setDisactive();
        inventoryIndex = index;
        ListOfUIItems[inventoryIndex].setActive();
    }

    public int getListSize()
    {
        return inventorySize;
    }
}
