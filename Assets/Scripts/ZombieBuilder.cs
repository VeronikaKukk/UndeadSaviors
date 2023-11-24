using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZombieBuilder : MonoBehaviour
{
    public Color AllowColor;
    public Color DenyColor;
    public GameObject ZombiePrefab;

    public AudioClipGroup ClickOnShopButtonActiveAudio;
    public AudioClipGroup ClickOnShopButtonInactiveAudio;

    public GameObject SpawnParticlePrefab;

    public List<BoxCollider2D> startAreas = new List<BoxCollider2D>();

    private ShopData currentZombieData;

    public static ZombieBuilder Instance;

    private void Awake()
    {
        Events.OnZombieSelected += ZombieSelected;
        gameObject.SetActive(false);

        Instance = this;
    }

    private void Start()
    {
        //startArea = GameObject.Find("StartArea").GetComponent<BoxCollider2D>(); // old solution for single startArea

        BoxCollider2D[] allBoxColliders = GameObject.FindObjectsOfType<BoxCollider2D>();
        foreach (BoxCollider2D area in allBoxColliders)
        {
            if (area.gameObject.name.Contains("StartArea"))
            {
                startAreas.Add(area);
            }
        }
    }

    private void OnDestroy()
    {
        Events.OnZombieSelected -= ZombieSelected;
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;

        bool isOverlapping = false;

        foreach (BoxCollider2D startArea in startAreas)
        {
            if (startArea.OverlapPoint(mousePos))
            {
                isOverlapping = true;
                break;
            }
        }

        if (isOverlapping)
        {
            TintSprite(AllowColor);
        }
        else
        {
            TintSprite(DenyColor);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Build(mousePos);
        }
        if (Input.GetMouseButtonDown(1))
        {
            gameObject.SetActive(false);
        }
    }

    void TintSprite(Color col)
    {
        SpriteRenderer[] renderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer rend in renderers)
        {
            rend.color = col;
        }
    }

    private void ZombieSelected(ShopData data)
    {
        currentZombieData = data;
        if (Events.GetMoney() >= currentZombieData.Price)
        {
            ClickOnShopButtonActiveAudio.Play();
            gameObject.GetComponent<SpriteRenderer>().sprite = currentZombieData.Icon;
            gameObject.SetActive(true);
        }
    }


    void Build(Vector3 mousePos)
    {
        bool isOverlapping = false;

        foreach (BoxCollider2D startArea in startAreas)
        {
            if (startArea.OverlapPoint(mousePos))
            {
                isOverlapping = true;
                break;
            }
        }

        if (!isOverlapping)
        {
            gameObject.SetActive(false);
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Events.SetMoney(Events.GetMoney() - currentZombieData.Price);
        GameObject zombie = GameObject.Instantiate(currentZombieData.ZombiePrefab, transform.position, Quaternion.identity, null);
        // spawnparticles
        GameObject spawnParticle = GameObject.Instantiate(SpawnParticlePrefab, transform.position, Quaternion.identity, null);
        spawnParticle.GetComponent<ParticleSystem>().Play();

        ZombieFactory.Instance.ApplyPotionsOnUnit(zombie, currentZombieData.Name);
        gameObject.SetActive(false);
    }
}
