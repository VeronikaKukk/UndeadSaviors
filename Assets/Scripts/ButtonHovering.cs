using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonHovering : MonoBehaviour
{
    public AudioSource audioSource;

    public void PointerEnter()
    {
        transform.localScale = new Vector2(1.1f, 1.1f);


    }

    public void PointerExit()
    {
        transform.localScale = new Vector2(1f, 1f);
    }

    public void PlayClickedSound()
    {
        audioSource.Play();
    }

}
