using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{

    [SerializeField] private GameObject GenelUI;
    [SerializeField] private GameObject GameModeArea;
    [SerializeField] private GameObject TopHud;
    [SerializeField] private GameObject mainCards;
    [SerializeField] private GameObject betting;
    [SerializeField] private GameObject finishButton;
    [SerializeField] private GameObject newCardButton;
    [SerializeField] private GameObject SettingsButton;
    [SerializeField] private GameObject Slider;
    [SerializeField] private GameObject TutorialYesButton;
    [SerializeField] private GameObject TutorialNoButton;
    [SerializeField] private List<GameObject> TutorialTexts = new List<GameObject>();


    
    private int tutorialCounter;

    private float timer;
    [SerializeField] private float timerCooldown;

    private void Start()
    {
        tutorialCounter = 0;
        CloseEveryThingAtStart();
    }
    private void Update()
    {   

        if (PlayerPrefs.HasKey("Tutorial"))
        {
            if (PlayerPrefs.GetInt("Tutorial") == 1)
            {
                TutorialTime();
            }
        }

        increaseTutorial();
    }

    private void CloseEveryThingAtStart()
    {
        GameModeArea.SetActive(false);
        TopHud.SetActive(false);
        mainCards.SetActive(false);
        betting.SetActive(false);
        finishButton.SetActive(false);
        newCardButton.SetActive(false);
        SettingsButton.SetActive(false);
        Slider.SetActive(false);

        foreach (var item in TutorialTexts)
        {
            item.SetActive(false);
        }
        
    }

    private void TutorialTime()
    {

        switch (tutorialCounter)
        {
                case 0:
                TutorialTexts[0].SetActive(true);
                TutorialTexts[1].SetActive(true);
                TutorialTexts[2].SetActive(true);
                TutorialYesButton.SetActive(true);
                TutorialNoButton.SetActive(true);
                break;

                case 1:
                TutorialTexts[0].SetActive(false);
                TutorialTexts[1].SetActive(false);
                TutorialTexts[2].SetActive(false);
                TutorialYesButton.SetActive(false);
                TutorialNoButton.SetActive(false);
                //before after
                mainCards.SetActive(true);
                TutorialTexts[3].SetActive(true);
                TutorialTexts[4].SetActive(true);
                TutorialTexts[5].SetActive(true);
                break;

                case 2:
                mainCards.SetActive(false);
                TutorialTexts[3].SetActive(false);
                TutorialTexts[4].SetActive(false);
                TutorialTexts[5].SetActive(false);
                //before after
                newCardButton.SetActive(true);
                TutorialTexts[6].SetActive(true);
                TutorialTexts[7].SetActive(true);
                break;

                case 3:
                newCardButton.SetActive(false);
                TutorialTexts[6].SetActive(false);
                TutorialTexts[7].SetActive(false);
                //before after
                betting.SetActive(true);
                TutorialTexts[8].SetActive(true);
                TutorialTexts[9].SetActive(true);
                break;

                case 4:
                betting.SetActive(false);
                TutorialTexts[8].SetActive(false);
                TutorialTexts[9].SetActive(false);
                //before after
                GameModeArea.SetActive(true);
                TutorialTexts[10].SetActive(true);
                TutorialTexts[11].SetActive(true);
                break;

                case 5:
                GameModeArea.SetActive(false);
                TutorialTexts[10].SetActive(false);
                TutorialTexts[11].SetActive(false);
                //before after
                finishButton.SetActive(true);
                TutorialTexts[12].SetActive(true);
                TutorialTexts[13].SetActive(true);
                break;

                case 6:
                finishButton.SetActive(false);
                TutorialTexts[12].SetActive(false);
                TutorialTexts[13].SetActive(false);
                //before after
                Slider.SetActive(true);
                TutorialTexts[14].SetActive(true);
                TutorialTexts[15].SetActive(true);
                break;

                case 7:
                Slider.SetActive(false);
                TutorialTexts[14].SetActive(false);
                TutorialTexts[15].SetActive(false);
                //before after
                TopHud.SetActive(true);
                TutorialTexts[16].SetActive(true);
                TutorialTexts[17].SetActive(true);
                break;

                case 8:
                TopHud.SetActive(false);
                TutorialTexts[16].SetActive(false);
                TutorialTexts[17].SetActive(false);
                //before after
                SettingsButton.SetActive(true);
                TutorialTexts[18].SetActive(true);
                TutorialTexts[19].SetActive(true);
                TutorialTexts[20].SetActive(true);
                break;

                case 9:
                SettingsButton.SetActive(false);
                TutorialTexts[18].SetActive(false);
                TutorialTexts[19].SetActive(false);
                TutorialTexts[20].SetActive(false);
                //before after
                TutorialTexts[21].SetActive(true);
                TutorialTexts[22].SetActive(true);
                break;
                case 10:
                TutorialTexts[0].SetActive(false);
                TutorialTexts[1].SetActive(false);
                TutorialTexts[2].SetActive(false);
                TutorialYesButton.SetActive(false);
                TutorialNoButton.SetActive(false);
                TutorialTexts[21].SetActive(false);
                TutorialTexts[22].SetActive(false);
                //before after
                PlayerPrefs.SetInt("Tutorial",0);
                GenelUI.SetActive(true);
                this.gameObject.SetActive(false);
                break;

        }



    }

    private void increaseTutorial()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended && timer > timerCooldown && tutorialCounter != 0)
            {
                tutorialCounter++;
                timer = 0;
            }
        }

        if (Input.GetMouseButtonDown(0) && tutorialCounter != 0)
        {

            if (timer > timerCooldown)
            {
                tutorialCounter++;
                timer = 0;
            }
            
        }





        timer += Time.deltaTime;

    }

    public void YesTutorial()
    {
        tutorialCounter = 1;
    }

    public void NoTutorial()
    {
        tutorialCounter = 10;
    }
}
