using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Collectable : MonoBehaviour
{
    public PotionData PotionData;

    public bool isVisible = false;
    private float aliveTimeLeft; // how long the potion stays on the field
    private bool isPickedUp = false;
    private Vector3 offset;

    public GameObject CursorUIObjectPrefab;

    public AudioClipGroup ClickOnPotionAudio;
    public AudioClipGroup ApplyPotionAudio;


    private GameObject cursorUIObject;
    private SpriteRenderer spriteRenderer;
    private Canvas canvas;
    private Transform potionBuffs;
    private string zombieType;

    private GameObject potionToDestroy;

    public void Awake()
    {
        EntityController.Instance.Potions.Add(this);
    }

    private void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public void OnBecameVisible()
    {
        isVisible = true;
        aliveTimeLeft = PotionData.AliveTime;
    }

    private void OnMouseDown()
    {
        if (!isPickedUp)
        {
            ClickOnPotionAudio.Play();

            spriteRenderer = transform.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
            isPickedUp = true;
            GameController.Instance.SetPotionPickedUp(true);
            cursorUIObject = Instantiate(CursorUIObjectPrefab, canvas.transform);
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            if (IsPointerOverUIButton())
            {
                ApplyPotionEffectsToZombies();
            }
            else
            {
                spriteRenderer.enabled = true;
                GameController.Instance.SetPotionPickedUp(false);
            }

            Destroy(cursorUIObject);
            isPickedUp = false;
            
            if (potionToDestroy != null)
            {
                Invoke("PutPotionDown", 0.1f);
            }
            
        }
    }

    private void PutPotionDown()
    {
        GameController.Instance.SetPotionPickedUp(false);
        Destroy(potionToDestroy);
        potionToDestroy = null;
    }

    private void ApplyPotionEffectsToZombies()
    {
        bool entered = false;
        Image[] images = potionBuffs.GetComponentsInChildren<Image>();
        if (PotionData.PotionName.Equals("Health") && !images[1].enabled)
        {
            ApplyPotionAudio.Play();
            EntityController.Instance.Potions.Remove(this);

            Events.ApplyPotion(zombieType, PotionData);

            images[1].enabled = true;
            Events.AddMaxHealthValue(zombieType, PotionData.BuffAmount);
            //Debug.Log("HealthPotion applied");
        }
        else if (PotionData.PotionName.Equals("Speed") && !images[2].enabled)
        {
            ApplyPotionAudio.Play();
            EntityController.Instance.Potions.Remove(this);

            Events.ApplyPotion(zombieType, PotionData);

            images[2].enabled = true;
            Events.AddAttackSpeedValue(zombieType, PotionData.BuffAmount);
            Events.AddMovementSpeedValue(zombieType, PotionData.BuffAmount);
            //Debug.Log("SpeedPotion applied");
        }
        else if (PotionData.PotionName.Equals("Damage") && !images[3].enabled)
        {
            ApplyPotionAudio.Play();
            EntityController.Instance.Potions.Remove(this);

            Events.ApplyPotion(zombieType, PotionData);

            images[3].enabled = true;
            Events.AddDamageValue(zombieType, PotionData.BuffAmount);
            //Debug.Log("DamagePotion applied");
        }
        else
        {
            entered = true;
            //Debug.Log("No empty slot");
            spriteRenderer.enabled = true;
            GameController.Instance.SetPotionPickedUp(false);
        }

        if (!entered)
        {
            potionToDestroy = gameObject;
        }
    }

    private bool IsPointerOverUIButton()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult result in results)
        {
            // Mouse is over a UI button with the specified tag
            if (result.gameObject.CompareTag("ShopZombie"))
            {
                Shop zombieShop = result.gameObject.GetComponent<Shop>();
                Transform findPanel = result.gameObject.transform.Find("Potions");

                if (findPanel != null && zombieShop != null)
                {
                    zombieType = zombieShop.ShopData.ZombiePrefab.name;
                    potionBuffs = findPanel;
                    return true;
                }
            }
        }
        return false;
    }

    public void Update()
    {
        if (isPickedUp)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

            Vector3 relativePosition = Camera.main.WorldToScreenPoint(transform.position); // Adjust the Z-coordinate as needed.
            cursorUIObject.transform.position = new Vector3(relativePosition.x, relativePosition.y, cursorUIObject.transform.position.z);
        }
        else
        {
            if (isVisible && aliveTimeLeft > 0)
            {
                aliveTimeLeft -= Time.deltaTime;
            }
            if (isVisible && aliveTimeLeft <= 0)
            {
                aliveTimeLeft = 0;
                EntityController.Instance.Potions.Remove(this);
                Destroy(gameObject); // if the aliveTimeLeft is 0 then remove potion from board
            }
        }
    }

}
