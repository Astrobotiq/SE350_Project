using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleEffect : MonoBehaviour
{

    public ParticleSystem bubble;

    

    public void playBubble()
    {
        bubble.Play();
    }
}
