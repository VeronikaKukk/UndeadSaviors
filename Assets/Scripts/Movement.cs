using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject CombatTextPrefab;
    public float MovementSpeed;

    private void Awake()
    {
        Events.OnAddMovementSpeedValue += AddMovementSpeed;
        // this was in start before
        Health health = GetComponent<Health>();
        MovementSpeed = health.UnitData.MovementSpeed;
    }
    private void Start()
    {
        
    }
    private void OnDestroy()
    {
        Events.OnAddMovementSpeedValue -= AddMovementSpeed;
    }

    void AddMovementSpeed(string unitName, float speed)
    {
        if(gameObject.name.StartsWith(unitName)) { 
            MovementSpeed += speed;
            GameObject combatText = Instantiate(CombatTextPrefab, new Vector3(transform.position.x + UnityEngine.Random.Range(-0.25f, 0.25f), transform.position.y + UnityEngine.Random.Range(-0.25f, 0.25f), transform.position.z), Quaternion.identity);
            combatText.transform.GetChild(0).GetComponent<TextMeshPro>().text = "+" + MovementSpeed;
            combatText.transform.Find("MovementSpeed").gameObject.SetActive(true);
            combatText.transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.green;
            TweenCallback tweenCallback = () => { Destroy(combatText.gameObject); };
            combatText.transform.localScale = combatText.transform.localScale * 0.5f;
            combatText.transform.DOScale(combatText.transform.localScale * 1.5f, 1f).OnComplete(tweenCallback);
        }
    }

    public void Move(Vector2 value) 
    {
        transform.position += (Vector3)value * MovementSpeed * Time.deltaTime;
    }
}
