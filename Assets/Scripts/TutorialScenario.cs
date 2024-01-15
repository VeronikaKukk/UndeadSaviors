using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TutorialScenario : MonoBehaviour
{
    public GameObject ShopPanel;
    public Button pauseButton;
    public Button unitInfoButton;
    public AudioClipGroup BookPageTurnAudio;

    // FOR TUTORIAL
    public TextMeshProUGUI[] AllTexts;
    public Image[] AllArrows;
    private int index;
    private int indexArrow;
    private Button shopButton;
    private bool activateShopButton = false;
    private bool isPlantDead = false;
    public GameObject DroppablePotion;
    public GameObject SellPotion;
    private bool allowAudioPlay = true;

    private GameObject collectable;
    //

    public static TutorialScenario Instance;

    void Start()
    {
        Instance = this;

        if (ScenarioController.Instance.currentScene.name == "TutorialScene")
        {
            TutorialStuff();
            AllTexts[0].gameObject.SetActive(true);
            index = 0;
            indexArrow = -1;
            pauseButton.interactable = false;
            unitInfoButton.interactable = false;
            shopButton = ShopPanel.GetComponentInChildren<Button>();
            shopButton.interactable = false;
            SellPotion.SetActive(false);
        }
    }


    void Update()
    {
        if (ScenarioController.Instance.currentScene.name == "TutorialScene")
        {
            if (Input.GetMouseButtonDown(0) && !ScenarioController.Instance.levelPaused)
            {
                Invoke("TutorialClicking", 0.2f);
            }
            if (!activateShopButton)
            {
                shopButton = ShopPanel.GetComponentInChildren<Button>();
                shopButton.interactable = false;
            }
            if (EntityController.Instance.PlantCharacters.Count < 1 && index == 11 && isPlantDead == false)
            {
                isPlantDead = true;
                Invoke("AllPlantsDead", 2f);
            }
        }
    }

    public void AllPlantsDead()
    {
        index += 1;
        AllTexts[11].gameObject.SetActive(false);
        AllTexts[12].gameObject.SetActive(true);
        AllArrows[4].gameObject.SetActive(false);
        TutorialPotionDrop();
        BookPageTurnAudio.Play();
    }

    public void TutorialStuff()
    {
        foreach (TextMeshProUGUI text in AllTexts)
        {
            text.gameObject.SetActive(false);
        }

        foreach (Image arrow in AllArrows)
        {
            arrow.gameObject.SetActive(false);
        }
    }

    // Selle loogikaga tegeles Andre. Kui midagi on katki siis öelge mulle, sellest aru saamine on peavalu kui pole ise teinud :D
    private void TutorialClicking()
    {
        if (index <= 10)
        {
            if (index <= 4 || index >= 6)
            {
                BookPageTurnAudio.Play();
                index += 1;
                if (index == 3 || index == 4 || index == 5 || index == 11)
                {
                    indexArrow += 1;
                }
                //Debug.Log(index);
            }
            else if (index >= 5 && EntityController.Instance.ZombieCharacters.Count > 0)
            {
                BookPageTurnAudio.Play();
                index += 1;
                if (index == 6)
                {
                    indexArrow += 1;
                }
                //Debug.Log(index);
            }

            if (index != 0)
            {
                AllTexts[index - 1].gameObject.SetActive(false);
            }
            if (indexArrow > 0)
            {
                AllArrows[indexArrow - 1].gameObject.SetActive(false);
            }

            if (index == 4) // poest ostmise tutorial message
            {
                activateShopButton = true;
            }

            if (index == 10) // siin on pausi nupu tutorial message
            {
                pauseButton.interactable = true;
            }

            if (index == 11)
            {
                unitInfoButton.interactable = true;
            }

            AllTexts[index].gameObject.SetActive(true);
            if (index == 3 || index == 4 || index == 5 || index == 10 || index == 11)
            {
                AllArrows[indexArrow].gameObject.SetActive(true);
            }
        }

        else if (isPlantDead && index >= 11 && index <= 21) // sõnumid, mis tulevad alles siis kui Plant on surma saanud ja eelnevad messaged ära näidatud
        {
            Image potionSlot = shopButton.GetComponentsInChildren<Image>()[4];
            AllTexts[index - 1].gameObject.SetActive(false); // loogika lihtsuse jaoks, esmakordsel sisenemisel on tarvis
            AllArrows[indexArrow - 1].gameObject.SetActive(false);
            allowAudioPlay = true;

            if (index == 13)
            {
                collectable.GetComponent<BoxCollider2D>().enabled = true;
            }

            if (index == 14 && !GameController.Instance.IsPotionPickedUp) // potion pickup loogika
            {
                index -= 1;
                indexArrow -= 1;
                allowAudioPlay = false;
            }
            else if (index == 15 && !potionSlot.enabled) // kontroll et potion on ikka peale pandud
            {
                index -= 1;
                indexArrow -= 1;
                allowAudioPlay = false;
            }
            else if (index == 18)
            {
                SellPotion.SetActive(true);
            }

            if (allowAudioPlay)
            {
                BookPageTurnAudio.Play();
            }

            index += 1;
            //Debug.Log(index);
            AllTexts[index - 1].gameObject.SetActive(false);
            AllTexts[index].gameObject.SetActive(true);

            if (index == 14 || index == 15 || index == 16 || index == 19 || index == 22)
            {
                indexArrow += 1;
                if (AllArrows.Length - 1 < indexArrow)
                {
                    indexArrow = AllArrows.Length - 1;
                }
                AllArrows[indexArrow].gameObject.SetActive(true);
            }
            else
            {
                AllArrows[indexArrow].gameObject.SetActive(false);
            }
            if (index != 22)
            {
                AllArrows[indexArrow - 1].gameObject.SetActive(false);
            }
        }
        /*
        else if (index == 22) // viimase sõnumi kustutamine
        {
            AllTexts[index].gameObject.SetActive(false);
            AllArrows[indexArrow].gameObject.SetActive(false);
        }
        */
    }

    public void PauseLogic()
    {
        if (index != 11 && allowAudioPlay)
        {
            index -= 1;
            Debug.Log(index);
            Debug.Log("arrow" + indexArrow);
            if (index == 13 || index == 15 || index == 18)
            {
                indexArrow -= 1;
                AllArrows[indexArrow].gameObject.SetActive(false);
            }
            AllTexts[index].gameObject.SetActive(false);
        }
        else if (index == 15 && !allowAudioPlay)
        {
            Debug.Log("enter");
            BookPageTurnAudio.Play();
        }
        else
        {
            Debug.Log(index);
        }
    }

    public GameObject TutorialPotionDrop()
    {
        collectable = GameObject.Instantiate<GameObject>(DroppablePotion, transform.position, Quaternion.identity, null);
        collectable.transform.DOMove(new Vector2((float)-8.2, 0), 1f);
        collectable.GetComponent<BoxCollider2D>().enabled = false;
        return collectable;
    }
}
