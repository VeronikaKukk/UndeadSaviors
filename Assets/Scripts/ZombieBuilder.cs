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

    private BoxCollider2D startArea;

    private ShopData currentZombieData;
    private bool decreased;

    private void Awake()
    {
        Events.OnZombieSelected += ZombieSelected;
        gameObject.SetActive(false);
    }

    private void Start()
    {
        startArea = GameObject.Find("StartArea").GetComponent<BoxCollider2D>();
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

        if (startArea.OverlapPoint(mousePos))
        {
            TintSprite(AllowColor);
        } else
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
        if (!startArea.OverlapPoint(mousePos)) 
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
