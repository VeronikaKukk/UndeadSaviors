using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Shop : MonoBehaviour
{
    public ShopData ShopData;

    public TextMeshProUGUI PriceText;
    public Image IconImage;

    private Button button;

    private void Awake()
    {
        Events.OnSetMoney += SetMoney;
        Events.OnSetPotionPickedUp += SetPotionPickedUp;
    }

    private void Start()
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
    void SetMoney(int value)
    {
        button = GetComponent<Button>();
        button.interactable = ShopData.Price <= value;
    }
    void SetPotionPickedUp(bool value, Collectable potion) {
        if (value)
        {
            bool show = false;
            Transform potionPanel = transform.Find("Potions");
            if (potion != null) {
                if (potion.PotionData.PotionName.Equals("Health") && potionPanel.Find("HealthPotion").GetComponent<Image>().enabled == false)
                {
                    show = true;
                }
                else if (potion.PotionData.PotionName.Equals("MovementSpeed") && potionPanel.Find("SpeedPotion").GetComponent<Image>().enabled == false)
                {
                    show = true;
                }
                else if (potion.PotionData.PotionName.Equals("Damage") && potionPanel.Find("DamagePotion").GetComponent<Image>().enabled == false) {
                    show = true;
                }
                else if (potion.PotionData.PotionName.Equals("AttackSpeed") && potionPanel.Find("AttackSpeedPotion").GetComponent<Image>().enabled == false)
                {
                    show = true;
                }
                else if (potion.PotionData.PotionName.Equals("AttackRange") && potionPanel.Find("AttackRangePotion").GetComponent<Image>().enabled == false)
                {
                    show = true;
                }
            }

            GameObject highlight = transform.Find("HighlightImage").gameObject;
            if (show && highlight != null)
            {
                highlight.GetComponent<Image>().enabled = true;
            }
        }
        else {
            GameObject highlight = transform.Find("HighlightImage").gameObject;
            if (highlight != null)
            {
                highlight.GetComponent<Image>().enabled = false;
            }
        }
    }

    public void Pressed()
    {
        if (!GameController.Instance.IsPotionPickedUp)
        {
            Events.SelectZombie(ShopData);
        }
    }

    public void OnDestroy()
    {
        Events.OnSetMoney -= SetMoney;
        Events.OnSetPotionPickedUp -= SetPotionPickedUp;
    }
}
