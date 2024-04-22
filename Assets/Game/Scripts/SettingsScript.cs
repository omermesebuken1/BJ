using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SettingsScript : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private GameObject mainPage;
    [SerializeField] private GameObject statisticsPage;
    [SerializeField] private GameObject genelUI;
    [SerializeField] private GameObject HowToPlay;




    [SerializeField] private Sprite buttonOK;
    [SerializeField] private Sprite buttonNOK;

    [SerializeField] private GameObject fps30Button;
    [SerializeField] private GameObject fps60Button;
    [SerializeField] private GameObject soundButton;
    [SerializeField] private GameObject vibrationButton;

    [SerializeField] private TextMeshProUGUI totalGamesPlayed;
    [SerializeField] private TextMeshProUGUI totalGamesWin;
    [SerializeField] private TextMeshProUGUI totalGamesLose;
    [SerializeField] private TextMeshProUGUI totalWinRateText;

  


    public void ResetSettings()
    {
        mainPage.SetActive(true);

        statisticsPage.SetActive(false);

        HowToPlay.SetActive(false);

        CheckFPSAndSoundAndVib();
    }

    private void TotalGamesCheck()
    {

        if (PlayerPrefs.HasKey("TotalGamesLose") && PlayerPrefs.HasKey("TotalGamesWin"))
        {
            float totalWin = PlayerPrefs.GetInt("TotalGamesWin");
            float totalLose = PlayerPrefs.GetInt("TotalGamesLose");
            float totalWinRate = (totalWin / (totalWin + totalLose)) * 100;
            totalGamesPlayed.text = "Games Played: " + (totalWin + totalLose).ToString("N0");
            totalGamesWin.text = "Total Win: " + totalWin.ToString("N0");
            totalGamesLose.text = "Total Lose: " + totalLose.ToString("N0");
            totalWinRateText.text = "Win Rate: %" + totalWinRate.ToString("N0");
        }

    }

    

    

    

    
    
    //buttons

    public void CloseSettings()
    {
        gm.Vibrate("soft");
        genelUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
    private void CheckFPSAndSoundAndVib()
    {
        if (PlayerPrefs.HasKey("FPS"))
        {
            switch (PlayerPrefs.GetInt("FPS"))
            {
                case 30:
                    fps30Button.GetComponent<Image>().sprite = buttonOK;
                    fps60Button.GetComponent<Image>().sprite = buttonNOK;
                    break;
                case 60:
                    fps30Button.GetComponent<Image>().sprite = buttonNOK;
                    fps60Button.GetComponent<Image>().sprite = buttonOK;
                    break;
            }
        }

        if (PlayerPrefs.HasKey("Sound"))
        {
            switch (PlayerPrefs.GetInt("Sound"))
            {
                case 1:
                    soundButton.GetComponent<Image>().sprite = buttonOK;
                    break;
                case 0:
                    soundButton.GetComponent<Image>().sprite = buttonNOK;
                    break;
            }
        }

        if (PlayerPrefs.HasKey("Vibration"))
        {
            switch (PlayerPrefs.GetInt("Vibration"))
            {
                case 1:
                    vibrationButton.GetComponent<Image>().sprite = buttonOK;
                    break;
                case 0:
                    vibrationButton.GetComponent<Image>().sprite = buttonNOK;
                    break;
            }
        }

    }


    public void ChangeSound()
    {
        gm.Vibrate("soft");
        if (PlayerPrefs.HasKey("Sound"))
        {
            switch (PlayerPrefs.GetInt("Sound"))
            {
                case 1:
                    PlayerPrefs.SetInt("Sound", 0);
                    CheckFPSAndSoundAndVib();
                    break;
                case 0:
                    PlayerPrefs.SetInt("Sound", 1);
                    CheckFPSAndSoundAndVib();
                    break;
            }

        }
    }
    public void ChangeVib()
    {
        gm.Vibrate("soft");
        if (PlayerPrefs.HasKey("Vibration"))
        {
            switch (PlayerPrefs.GetInt("Vibration"))
            {
                case 1:
                    PlayerPrefs.SetInt("Vibration", 0);
                    CheckFPSAndSoundAndVib();
                    break;
                case 0:
                    PlayerPrefs.SetInt("Vibration", 1);
                    CheckFPSAndSoundAndVib();
                    break;
            }

        }
    }
    

   


}
