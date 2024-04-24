using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{

    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = "50";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeText(int money)
    {
        text.text = money.ToString();
    }
}
