using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string PotionName;// health speed or damage
    public float BuffAmount;// number that shows how much buff you get
    public float AliveTime; // how long the potion stays on the field
    public bool isVisible = false;

    // here write code for when potion is dragged with mouse onto zombie type in shop??

    // if it is not dragged on zombietype then potion will go back where it was before

    // depending on potionname give the buff to zombie type

    public void OnBecameVisible()
    {
        isVisible = true;
    }
    public void Update()
    {
        if (isVisible && AliveTime > 0) 
        {
            AliveTime -= Time.deltaTime;
        }
        if(AliveTime <= 0)
        {
            AliveTime = 0;
            Destroy(gameObject); // if the alivetime is 0 then remove potion from board
        }
    }
}
