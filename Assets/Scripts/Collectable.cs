using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public PotionData PotionData;
    
    public bool isVisible = false;
    private float aliveTimeLeft; // how long the potion stays on the field


    // here write code for when potion is dragged with mouse onto zombie type in shop??

    // if it is not dragged on zombietype then potion will go back where it was before

    // depending on potionname give the buff to zombie type

    public void OnBecameVisible()
    {
        isVisible = true;
        aliveTimeLeft = PotionData.AliveTime;
    }
    public void Update()
    {
        if (isVisible && aliveTimeLeft > 0) 
        {
            aliveTimeLeft -= Time.deltaTime;
        }
        if(isVisible && aliveTimeLeft <= 0)
        {
            aliveTimeLeft = 0;
            Destroy(gameObject); // if the aliveTimeLeft is 0 then remove potion from board
        }
    }
}
