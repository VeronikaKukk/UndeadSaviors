using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonHovering : MonoBehaviour
{
    public AudioClipGroup ButtonClickAudio;
    public AudioClipGroup ButtonHoverAudio;

    public void PointerEnter()
    {
        transform.localScale = new Vector2(1.1f, 1.1f);
        ButtonHoverAudio.Play();
    }

    public void PointerExit()
    {
        transform.localScale = new Vector2(1f, 1f);
    }

    public void PlayClickedSound()
    {
        ButtonClickAudio.Play();
    }

}
