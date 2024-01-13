using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweeningPanel : MonoBehaviour // doesn't work right now but also doesn't break anything
    // attached to pause menu panel
{
    public ScalingAnimation OpenAnimation;
    public ScalingAnimation CloseAnimation;

    public void Open()
    {
        if (gameObject.activeSelf) return;
        gameObject.SetActive(true);
        OpenAnimation.enabled = true;
    }


    public void Close()
    {
        if (!gameObject.activeSelf) return;
        CloseAnimation.enabled = true;

    }


    public void CloseAnimationFinished()
    {
        gameObject.SetActive(false);   
    }
    }
