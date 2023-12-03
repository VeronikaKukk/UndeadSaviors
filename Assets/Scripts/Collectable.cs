using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Collectable : MonoBehaviour
{
    
    public PotionData PotionData;
    [Header("Prefabs")]
    [Space]
    [Tooltip("Needed for showing the potion on UI layer")]
    public GameObject CursorUIObjectPrefab;
    public GameObject CourageEffectPrefab;

    [Header("Sounds")]
    [Space]
    public AudioClipGroup ClickOnPotionAudio;
    public AudioClipGroup ApplyPotionAudio;
    public AudioClipGroup MoneySound;

    [Header("Other")]
    [Space]
    public bool isVisible = false;

    private float aliveTimeLeft; // how long the potion stays on the field
    private bool isPickedUp = false;

    private Vector3 offset;
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
            Events.SetPotionPickedUp(isPickedUp, this);
            cursorUIObject = Instantiate(CursorUIObjectPrefab, canvas.transform);
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            if (IsPointerOverUIButton())// true if potion is over zombietype or when potion is over sell button
            {
                if (zombieType != null && potionBuffs != null)// over zombietype
                {
                    ApplyPotionEffectsToZombies();
                }
                else {//over sell button
                    SellPotion();
                }
                
            }
            else
            {
                spriteRenderer.enabled = true;
            }            

            if (potionToDestroy != null)
            {
                Invoke("PutPotionDown", 0.25f);
            } else
            {
                isPickedUp = false;
                Events.SetPotionPickedUp(isPickedUp, null);
                
            }
            Destroy(cursorUIObject);
        }
    }

    private void PutPotionDown()
    {
        isPickedUp = false;
        Events.SetPotionPickedUp(isPickedUp,null);
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
            //GameController.Instance.SetPotionPickedUp(false);
            isPickedUp = false;
            Events.SetPotionPickedUp(isPickedUp, null);
        }

        if (!entered)
        {
            potionToDestroy = gameObject;
        }
    }

    private void SellPotion() {
        Events.SetMoney(Events.GetMoney() + 20);//get 20 money for selling a potion
        EntityController.Instance.Potions.Remove(this);
        potionToDestroy = gameObject;
        // sell potion effect
        GameObject courageParticle = GameObject.Instantiate(CourageEffectPrefab, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
        courageParticle.GetComponent<ParticleSystem>().Play();
        GameObject courage = GameObject.FindGameObjectWithTag("Courage").gameObject;
        var pos = Camera.main.ScreenToWorldPoint(courage.transform.position);
        TweenCallback tweenCallback = () => { Destroy(courageParticle.gameObject); };
        courageParticle.transform.DOMove(new Vector3(pos.x, pos.y, 10), 1.5f).OnComplete(tweenCallback);
        MoneySound.Play();
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
                print("on shop zombie");

                Shop zombieShop = result.gameObject.GetComponent<Shop>();
                Transform findPanel = result.gameObject.transform.Find("Potions");

                if (findPanel != null && zombieShop != null)
                {
                    zombieType = zombieShop.ShopData.ZombiePrefab.name;
                    potionBuffs = findPanel;
                    return true;
                }
            }
            else if (result.gameObject.CompareTag("SellPotion")) {
                print("on sell potion");
                return true;
            }
        }
        return false;
    }

    public void Update()
    {
        if (isPickedUp && cursorUIObject != null)
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
