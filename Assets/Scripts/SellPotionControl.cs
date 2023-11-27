using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellPotionControl : MonoBehaviour
{
    private Image sellPotionImage;
    public Sprite sellPotionRegularSprite; // temp in inspector; TODO
    public Sprite sellPotionSpecialSprite;
    public TextMeshProUGUI sellPotionText;

    private void Awake()
    {
        Events.OnSetPotionPickedUp += SetPotionPickedUp;
    }
    private void OnDestroy()
    {
        Events.OnSetPotionPickedUp -= SetPotionPickedUp;
    }
    void Start()
    {
       sellPotionImage = transform.Find("Image").GetComponentInChildren<Image>();
       sellPotionImage.sprite = sellPotionRegularSprite;
       sellPotionText.text = "";
    }
    void SetPotionPickedUp(bool value, Collectable potion) {
        if (value)
        {
            sellPotionImage.sprite = sellPotionSpecialSprite;
            sellPotionText.text = "sell\n+20";
            sellPotionText.color = Color.green;
        }
        else {
            sellPotionImage.sprite = sellPotionRegularSprite;
            sellPotionText.text = "";
            sellPotionText.color = Color.black;
        }
    }
}
