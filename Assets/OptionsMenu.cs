using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{

    public bool optionsNeedsAttention; // to trigger some options menu methods

    public void ReloadOptions() // doesn't work yet
    {
        if (optionsNeedsAttention)
        {
            optionsNeedsAttention = false;
            AudioManager.Instance.SetSliders();
        }
    }

}
