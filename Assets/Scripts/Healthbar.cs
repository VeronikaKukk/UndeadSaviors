using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    Slider healthbar;

    // get maxvalue -> healthbar.value = max
    // get currentvalue -> healthbar.value = current

    private void Start()
    {
        healthbar = GetComponent<Slider>();
    }


}
