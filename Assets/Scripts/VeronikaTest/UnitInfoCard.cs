using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoCard : MonoBehaviour
{
    public TextMeshProUGUI InfoText;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI AttackSpeedText;
    public TextMeshProUGUI AttackRangeText;
    public TextMeshProUGUI MovementSpeedText;
    public TextMeshProUGUI AttackDamageText;


    public Color RedColor;
    public Color GreenColor;
    public Color YellowColor;

    public Image UnitImage;
    public UnitData data;
    public void SetData(UnitData data)
    {
        this.data = data;        
        UnitImage.sprite = data.UnitPrefab.GetComponentsInChildren<SpriteRenderer>()[0].sprite;
        SetInfo();
    }

    private void SetInfo() {
        string attack_speed = "";
        string attack_range = "";
        string movement_speed = "";
        string health = "";
        string attack_damage = "";
        if (data.AttackRangeSize <= 1.2)
        {
            attack_range = "    melee";
        }
        else {
            attack_range = "    ranged";
        }

        if (data.MovementSpeed <= 1.5)
        {
            movement_speed = "slow";
            MovementSpeedText.color = RedColor;
        }
        else if (data.MovementSpeed <= 2)
        {
            movement_speed = "medium";
            MovementSpeedText.color = YellowColor;
        }
        else {
            movement_speed = "fast";
            MovementSpeedText.color = GreenColor;
        }

        if (data.TeamName == "Plant") {
            movement_speed = "none";
        }

        if (data.AttackSpeed <= 0.6)
        {
            attack_speed = "    slow";
            AttackSpeedText.color = RedColor;
        }
        else if (data.AttackSpeed <= 1.1)
        {
            attack_speed = "    medium";
            AttackSpeedText.color = YellowColor;
        }
        else {
            attack_speed = "    fast";
            AttackSpeedText.color = GreenColor;
        }

        if (data.MaxHealth <= 10)
        {
            health = "                few";
            HealthText.color = RedColor;
        }
        else if (data.MaxHealth <= 20)
        {
            health = "                normal";
            HealthText.color = YellowColor;
        }
        else {
            health = "                a lot";
            HealthText.color = GreenColor;
        }

        if (data.AttackDamage <= 1)
        {
            attack_damage = "  few";
            AttackDamageText.color = RedColor;
        }
        else if (data.AttackDamage <= 3)
        {
            attack_damage = "  normal";
            AttackDamageText.color = YellowColor;
        }
        else
        {
            attack_damage = "  a lot";
            AttackDamageText.color = GreenColor;
        }

        InfoText.text = "Health: \nAttack speed: \nAttack range: \nMovement speed: \nAttack damage: \n";
        string h = new string(' ',"Health: ".Length*2);
        HealthText.text = h + health;
        h = new string(' ', "Attack speed: ".Length*2);
        AttackSpeedText.text = "\n"+h+ attack_speed;
        h = new string(' ', "Attack range: ".Length * 2);
        AttackRangeText.text = "\n\n"+h+ attack_range;
        h = new string(' ', "Movement speed: ".Length*2);
        MovementSpeedText.text = "\n\n\n"+h+ movement_speed;
        h = new string(' ', "Attack damage: ".Length * 2);
        AttackDamageText.text = "\n\n\n\n" + h + attack_damage;
    }
}