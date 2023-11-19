using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoCard : MonoBehaviour
{
    public TextMeshProUGUI InfoText;
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
            attack_range = "Attack range: melee";
        }
        else {
            attack_range = "Attack range: ranged";
        }


        if (data.MovementSpeed <= 1)
        {
            movement_speed = "Movement Speed: slow";
        }
        else if (data.MovementSpeed <= 2)
        {
            movement_speed = "Movement Speed: medium";
        }
        else {
            movement_speed = "Movement Speed: fast";
        }

        if (data.AttackSpeed <= 0.5)
        {
            attack_speed = "Attack speed: slow";
        }
        else if (data.AttackSpeed <= 1)
        {
            attack_speed = "Attack speed: medium";
        }
        else {
            attack_speed = "Attack speed: fast";
        }

        if (data.MaxHealth <= 10)
        {
            health = "Health: few";
        }
        else if (data.MaxHealth <= 20)
        {
            health = "Health: normal";
        }
        else {
            health = "Health: a lot";
        }

        InfoText.text = health+"\n"+attack_speed+"\n"+ attack_range+"\n"+ movement_speed+"\n";
    }
}