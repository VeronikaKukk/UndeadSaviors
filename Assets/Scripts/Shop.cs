using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public ShopData ShopData;

    public TextMeshProUGUI PriceText;
    public Image IconImage;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(Pressed);
        }

        if (ShopData != null)
        {
            PriceText.text = ShopData.Price.ToString();
            IconImage.sprite = ShopData.Icon;
        }
    }

    public void Pressed()
    {
        Events.SelectZombie(ShopData);
    }
}
