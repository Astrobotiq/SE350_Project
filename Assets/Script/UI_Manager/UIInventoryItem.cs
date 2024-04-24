using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour
{
    public GameObject item;

    public GameObject border;
    private Image borderImage;


    private void Start()
    {
        borderImage = border.GetComponent<Image>();
        borderImage.enabled = false;
    }

    public void setDisactive()
    {
        borderImage.enabled = false;
    }

    public void setActive()
    {
        borderImage.enabled=true;
    }

}
