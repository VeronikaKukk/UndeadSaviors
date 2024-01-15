using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TweeningPanel : MonoBehaviour
// doesn't work right now but also doesn't break anything
// attached to some menu panel panels
{
    public ScalingAnimation OpenAnimation;
    private bool openSubMenu = true; // for triggering menu panel animations

    private void OnEnable()
    {
        gameObject.SetActive(false);
        if (openSubMenu) Open();

    }
    public void Open()
    {
        openSubMenu = false;
        if (gameObject.activeSelf) return;
        gameObject.SetActive(true);
        OpenAnimation.enabled = true;
    }

    }
