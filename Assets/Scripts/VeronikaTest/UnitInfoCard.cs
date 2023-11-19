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

    public Image UnitImage;
    public UnitData data;
    public void SetData(UnitData data)
    {
        this.data = data;
        //UnitImage.sprite = data.UnitPrefab.GetComponent<SpriteRenderer>().sprite;
        SetInfo();
    }

    private void SetInfo() {
        string attack_speed = "";
        string attack_range = "";
        string movement_speed = "";
        string health = "";
        
        if (data.AttackRangeSize <= 1)
        {
            attack_range = "melee";
        }
        else {
            attack_range = "ranged";
        }

        if (data.MovementSpeed <= 1)
        {
            movement_speed = "slow";
            MovementSpeedText.color = Color.red;
        }
        else if (data.MovementSpeed <= 2)
        {
            movement_speed = "medium";
            MovementSpeedText.color = Color.yellow;
        }
        else {
            movement_speed = "fast";
            MovementSpeedText.color = Color.green;
        }

        if (data.AttackSpeed <= 0.5)
        {
            attack_speed = "slow";
            AttackSpeedText.color = Color.red;
        }
        else if (data.AttackSpeed <= 1)
        {
            attack_speed = "medium";
            AttackSpeedText.color = Color.yellow;
        }
        else {
            attack_speed = "fast";
            AttackSpeedText.color = Color.green;
        }

        if (data.MaxHealth <= 10)
        {
            health = "few";
            HealthText.color = Color.red;
        }
        else if (data.MaxHealth <= 20)
        {
            health = "normal";
            HealthText.color = Color.yellow;
        }
        else {
            health = "a lot";
            HealthText.color = Color.green;
        }

        InfoText.text = "Health: \nAttack speed: \nAttack range: \nMovement speed: \n";
        string h = new string(' ',"Health: ".Length*2);
        HealthText.text = h + health;
        h = new string(' ', "Attack speed: ".Length*2);
        AttackSpeedText.text = "\n"+h+ attack_speed;
        h = new string(' ', "Attack range: ".Length * 2);
        AttackRangeText.text = "\n\n"+h+ attack_range;
        h = new string(' ', "Movement speed: ".Length*2);
        MovementSpeedText.text = "\n\n\n"+h+ movement_speed;
    }
}