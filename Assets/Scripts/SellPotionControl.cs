using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellPotionControl : MonoBehaviour
{
    private Image sellPotionImage;
    public Sprite sellPotionRegularSprite;
    public Sprite sellPotionSpecialSprite;
    public TextMeshProUGUI sellPotionText;
    // Start is called before the first frame update
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
       sellPotionImage = GetComponentInChildren<Image>();
       sellPotionImage.sprite = sellPotionRegularSprite;
    }
    void SetPotionPickedUp(bool value) {
        if (value)
        {
            sellPotionImage.sprite = sellPotionSpecialSprite;
            sellPotionText.text = "+20";
            sellPotionText.color = Color.green;
        }
        else {
            sellPotionImage.sprite = sellPotionRegularSprite;
            sellPotionText.text = "";
        }
    }
}
