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
    [SerializeField] private GameObject winRateGamesPage;
    [SerializeField] private GameObject bestHandsPage;
    [SerializeField] private GameObject gamblerBadgePage;
    [SerializeField] private GameObject genelUI;
    [SerializeField] private GameObject HowToPlay;
    [SerializeField] private GameObject getMoney;

    [SerializeField] private GameObject PokerRatings;
    [SerializeField] private GameObject HTP_Hilo;
    [SerializeField] private GameObject HTP_Texas;
    [SerializeField] private GameObject HTP_BJ;
    [SerializeField] private GameObject HTP_ThreeSum;
    [SerializeField] private GameObject HTP_SetPoker;
    [SerializeField] private GameObject HTP_Omaha;
    [SerializeField] private GameObject HTP_VideoPoker;
    [SerializeField] private GameObject HTP_CrazyPoker;


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

    [SerializeField] private List<TextMeshProUGUI> gamesTotalWin = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> gamesTotalLose = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> gamesWinRate = new List<TextMeshProUGUI>();

    [SerializeField] private List<GameObject> TexasBestGO = new List<GameObject>();
    [SerializeField] private List<GameObject> SetPokerBestGO = new List<GameObject>();
    [SerializeField] private List<GameObject> OmahaBestGO = new List<GameObject>();
    [SerializeField] private List<GameObject> VideoPokerBestGO = new List<GameObject>();
    [SerializeField] private List<GameObject> CrazyPokerBestGO = new List<GameObject>();
    [SerializeField] private List<TextMeshProUGUI> BestHandNames = new List<TextMeshProUGUI>();

    [SerializeField] private GameObject dontHaveMoneyText;
    [SerializeField] private GameObject haveMoneyText;
    [SerializeField] private GameObject giveMoneyButton;


    public void ResetSettings()
    {
        mainPage.SetActive(true);

        statisticsPage.SetActive(false);
        winRateGamesPage.SetActive(false);
        bestHandsPage.SetActive(false);
        HowToPlay.SetActive(false);
        getMoney.SetActive(false);
        gamblerBadgePage.SetActive(false);
        GetMoneyChecker();

        PokerRatings.SetActive(false);
        HTP_Hilo.SetActive(false);
        HTP_Texas.SetActive(false);
        HTP_BJ.SetActive(false);
        HTP_ThreeSum.SetActive(false);
        HTP_SetPoker.SetActive(false);
        HTP_Omaha.SetActive(false);
        HTP_VideoPoker.SetActive(false);
        HTP_CrazyPoker.SetActive(false);


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

    private void WinRatesForGames()
    {
        if (PlayerPrefs.HasKey("HiloWin") && PlayerPrefs.HasKey("HiloLose"))
        {
            float win = PlayerPrefs.GetInt("HiloWin");
            float lose = PlayerPrefs.GetInt("HiloLose");
            float rate = (win / (win + lose)) * 100;
            gamesTotalWin[0].text = "Win: " + win.ToString("N0");
            gamesTotalLose[0].text = "Lose: " + lose.ToString("N0");
            gamesWinRate[0].text = "Rate: %" + rate.ToString("N0");
        }

        if (PlayerPrefs.HasKey("TexasWin") && PlayerPrefs.HasKey("TexasLose"))
        {
            float win = PlayerPrefs.GetInt("TexasWin");
            float lose = PlayerPrefs.GetInt("TexasLose");
            float rate = (win / (win + lose)) * 100;
            gamesTotalWin[1].text = "Win: " + win.ToString("N0");
            gamesTotalLose[1].text = "Lose: " + lose.ToString("N0");
            gamesWinRate[1].text = "Rate: %" + rate.ToString("N0");
        }

        if (PlayerPrefs.HasKey("BJWin") && PlayerPrefs.HasKey("BJLose"))
        {
            float win = PlayerPrefs.GetInt("BJWin");
            float lose = PlayerPrefs.GetInt("BJLose");
            float rate = (win / (win + lose)) * 100;
            gamesTotalWin[2].text = "Win: " + win.ToString("N0");
            gamesTotalLose[2].text = "Lose: " + lose.ToString("N0");
            gamesWinRate[2].text = "Rate: %" + rate.ToString("N0");
        }

        if (PlayerPrefs.HasKey("ThreeSumWin") && PlayerPrefs.HasKey("ThreeSumLose"))
        {
            float win = PlayerPrefs.GetInt("ThreeSumWin");
            float lose = PlayerPrefs.GetInt("ThreeSumLose");
            float rate = (win / (win + lose)) * 100;
            gamesTotalWin[3].text = "Win: " + win.ToString("N0");
            gamesTotalLose[3].text = "Lose: " + lose.ToString("N0");
            gamesWinRate[3].text = "Rate: %" + rate.ToString("N0");
        }

        if (PlayerPrefs.HasKey("SetPokerWin") && PlayerPrefs.HasKey("SetPokerLose"))
        {
            float win = PlayerPrefs.GetInt("SetPokerWin");
            float lose = PlayerPrefs.GetInt("SetPokerLose");
            float rate = (win / (win + lose)) * 100;
            gamesTotalWin[4].text = "Win: " + win.ToString("N0");
            gamesTotalLose[4].text = "Lose: " + lose.ToString("N0");
            gamesWinRate[4].text = "Rate: %" + rate.ToString("N0");
        }

        if (PlayerPrefs.HasKey("OmahaWin") && PlayerPrefs.HasKey("OmahaLose"))
        {
            float win = PlayerPrefs.GetInt("OmahaWin");
            float lose = PlayerPrefs.GetInt("OmahaLose");
            float rate = (win / (win + lose)) * 100;
            gamesTotalWin[5].text = "Win: " + win.ToString("N0");
            gamesTotalLose[5].text = "Lose: " + lose.ToString("N0");
            gamesWinRate[5].text = "Rate: %" + rate.ToString("N0");
        }

        if (PlayerPrefs.HasKey("VideoPokerWin") && PlayerPrefs.HasKey("VideoPokerLose"))
        {
            float win = PlayerPrefs.GetInt("VideoPokerWin");
            float lose = PlayerPrefs.GetInt("VideoPokerLose");
            float rate = (win / (win + lose)) * 100;
            gamesTotalWin[6].text = "Win: " + win.ToString("N0");
            gamesTotalLose[6].text = "Lose: " + lose.ToString("N0");
            gamesWinRate[6].text = "Rate: %" + rate.ToString("N0");
        }

        if (PlayerPrefs.HasKey("CrazyPokerWin") && PlayerPrefs.HasKey("CrazyPokerLose"))
        {
            float win = PlayerPrefs.GetInt("CrazyPokerWin");
            float lose = PlayerPrefs.GetInt("CrazyPokerLose");
            float rate = (win / (win + lose)) * 100;
            gamesTotalWin[7].text = "Win: " + win.ToString("N0");
            gamesTotalLose[7].text = "Lose: " + lose.ToString("N0");
            gamesWinRate[7].text = "Rate: %" + rate.ToString("N0");
        }

    }

    private void PrepateBestHands()
    {
        if (PlayerPrefs.HasKey("BestTexasScore"))
        {
            if (PlayerPrefs.GetInt("BestTexasScore") != 0)
            {
                TexasBestGO[0].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandTexas_0")));
                TexasBestGO[1].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandTexas_1")));
                TexasBestGO[2].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandTexas_2")));
                TexasBestGO[3].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandTexas_3")));
                TexasBestGO[4].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandTexas_4")));

                BestHandNames[0].text = ScoreToName(PlayerPrefs.GetInt("BestTexasScore"));
            }
        }

        if (PlayerPrefs.HasKey("BestSetPokerScore"))
        {
            if (PlayerPrefs.GetInt("BestSetPokerScore") != 0)
            {
                SetPokerBestGO[0].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandSetPoker_0")));
                SetPokerBestGO[1].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandSetPoker_1")));
                SetPokerBestGO[2].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandSetPoker_2")));

                BestHandNames[1].text = ScoreToName(PlayerPrefs.GetInt("BestSetPokerScore"));
            }
        }

        if (PlayerPrefs.HasKey("BestOmahaScore"))
        {
            if (PlayerPrefs.GetInt("BestOmahaScore") != 0)
            {
                OmahaBestGO[0].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandOmaha_0")));
                OmahaBestGO[1].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandOmaha_1")));
                OmahaBestGO[2].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandOmaha_2")));
                OmahaBestGO[3].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandOmaha_3")));
                OmahaBestGO[4].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandOmaha_4")));

                BestHandNames[2].text = ScoreToName(PlayerPrefs.GetInt("BestOmahaScore"));
            }
        }

        if (PlayerPrefs.HasKey("BestVideoPokerScore"))
        {
            if (PlayerPrefs.GetInt("BestVideoPokerScore") != 0)
            {
                VideoPokerBestGO[0].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandVideoPoker_0")));
                VideoPokerBestGO[1].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandVideoPoker_1")));
                VideoPokerBestGO[2].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandVideoPoker_2")));
                VideoPokerBestGO[3].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandVideoPoker_3")));
                VideoPokerBestGO[4].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandVideoPoker_4")));

                BestHandNames[3].text = ScoreToName(PlayerPrefs.GetInt("BestVideoPokerScore"));
            }
        }

        if (PlayerPrefs.HasKey("BestCrazyPokerScore"))
        {
            if (PlayerPrefs.GetInt("BestCrazyPokerScore") != 0)
            {
                CrazyPokerBestGO[0].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandCrazyPoker_0")));
                CrazyPokerBestGO[1].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandCrazyPoker_1")));
                CrazyPokerBestGO[2].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandCrazyPoker_2")));
                CrazyPokerBestGO[3].GetComponent<CardScript>().GetCardSprite(CardValueDecoder(PlayerPrefs.GetInt("BestHandCrazyPoker_3")));

                BestHandNames[4].text = ScoreToName(PlayerPrefs.GetInt("BestCrazyPokerScore"));
            }
        }

    }

    private Card CardValueDecoder(int value)
    {
        string cardSuit = "";
        int cardNumber = 0;

        switch (value / 100)
        {
            case 1:
                cardSuit = "Spades";
                break;
            case 2:
                cardSuit = "Hearts";
                break;
            case 3:
                cardSuit = "Clubs";
                break;
            case 4:
                cardSuit = "Diamonds";
                break;
        }

        switch (value % 100)
        {
            case 12:
                cardNumber = 1;
                break;
            case 11:
                cardNumber = 13;
                break;
            case 10:
                cardNumber = 12;
                break;
            case 9:
                cardNumber = 11;
                break;
            case 8:
                cardNumber = 10;
                break;
            case 7:
                cardNumber = 9;
                break;
            case 6:
                cardNumber = 8;
                break;
            case 5:
                cardNumber = 7;
                break;
            case 4:
                cardNumber = 6;
                break;
            case 3:
                cardNumber = 5;
                break;
            case 2:
                cardNumber = 4;
                break;
            case 1:
                cardNumber = 3;
                break;
            case 0:
                cardNumber = 2;
                break;

        }



        Card tmpCard = new Card(cardSuit, cardNumber);



        return tmpCard;

    }

    private string ScoreToName(int score)
    {
        string exitValue = "";

        if (score == 10000)
        {
            exitValue = "Royal Flush";
        }
        else if (score >= 9000 && score < 10000)
        {
            exitValue = "Straight Flush";
        }
        else if (score >= 8000 && score < 9000)
        {
            exitValue = "Four of a Kind";
        }
        else if (score >= 7000 && score < 8000)
        {
            exitValue = "Full House";
        }
        else if (score >= 5000 && score < 7000)
        {
            exitValue = "Flush";
        }
        else if (score >= 4000 && score < 5000)
        {
            exitValue = "Straight";
        }
        else if (score >= 3000 && score < 4000)
        {
            exitValue = "Three of a kind";
        }
        else if (score >= 2000 && score < 3000)
        {
            exitValue = "Two Pair";
        }
        else if (score >= 1000 && score < 2000)
        {
            exitValue = "Pair";
        }
        else
        {
            exitValue = "High Card";
        }

        return exitValue;
    }

    public void Give1000()
    {
        gm.CodeRedeemed(1000);
        getMoney.SetActive(false);
        mainPage.SetActive(true);
        CloseSettings();

    }

    public void GetMoneyChecker()
    {
        if(gm.totalMoney <= 10)
        {
            dontHaveMoneyText.SetActive(true);
            haveMoneyText.SetActive(false);
            giveMoneyButton.SetActive(true);
        }
        else
        {
            dontHaveMoneyText.SetActive(false);
            haveMoneyText.SetActive(true);
            giveMoneyButton.SetActive(false);
        }
    }
    //buttons

    public void CloseSettings()
    {
        gm.Vibrate("soft");
        genelUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void PP()
    {
        Application.OpenURL("https://omermesebuken.com/mobile-games/casino-tower/privacy-policy");
    }
    public void TOU()
    {
        Application.OpenURL("https://omermesebuken.com/mobile-games/casino-tower/terms-of-use");
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
    public void FPS30()
    {
        gm.Vibrate("soft");
        gm.SetFPS(30);
        CheckFPSAndSoundAndVib();
    }
    public void FPS60()
    {
        gm.Vibrate("soft");
        gm.SetFPS(60);
        CheckFPSAndSoundAndVib();
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
    public void OpenGetMoney()
    {
        genelUI.SetActive(false);
        gm.Vibrate("soft");
        mainPage.SetActive(false);
        getMoney.SetActive(true);
    }
    public void CloseGetMoney()
    {
        gm.Vibrate("soft");
        mainPage.SetActive(true);
        getMoney.SetActive(false);
    }

    #region open close buttons

    public void OpenStatistics()
    {
        gm.Vibrate("soft");
        statisticsPage.SetActive(true);
        mainPage.SetActive(false);
        TotalGamesCheck();
    }
    public void CloseStatistics()
    {
        gm.Vibrate("soft");
        statisticsPage.SetActive(false);
        mainPage.SetActive(true);
    }
    public void OpenHowToPlay()
    {
        gm.Vibrate("soft");
        HowToPlay.SetActive(true);
        mainPage.SetActive(false);
    }
    public void CloseHowToPlay()
    {
        gm.Vibrate("soft");
        HowToPlay.SetActive(false);
        mainPage.SetActive(true);
    }
    public void OpenWinRates()
    {
        gm.Vibrate("soft");
        winRateGamesPage.SetActive(true);
        statisticsPage.SetActive(false);
        WinRatesForGames();
    }
    public void CloseWinRates()
    {
        gm.Vibrate("soft");
        winRateGamesPage.SetActive(false);
        statisticsPage.SetActive(true);
    }
    public void OpenBestHands()
    {
        gm.Vibrate("soft");
        bestHandsPage.SetActive(true);
        statisticsPage.SetActive(false);
        PrepateBestHands();
    }
    public void CloseBestHands()
    {
        gm.Vibrate("soft");
        bestHandsPage.SetActive(false);
        statisticsPage.SetActive(true);
    }
    public void OpenGamblerBadge()
    {
        KTGameCenter.SharedCenter().SubmitScore(0, "gambler_badge");
        if (gm.totalMoney > 1000000000)
        {
            gm.Vibrate("soft");
            gamblerBadgePage.SetActive(true);
            statisticsPage.SetActive(false);
        }
    }
    public void CloseGamblerBadge()
    {
        gm.Vibrate("soft");
        gamblerBadgePage.SetActive(false);
        statisticsPage.SetActive(true);

    }
    public void AcceptGamblerBadge()
    {
        gm.Vibrate("soft");
        gm.GamblerBadgeBankAdjust(2500);
        gm.UpdateStatistics("GamblerBadge");
        gamblerBadgePage.SetActive(false);
        mainPage.SetActive(true);
        CloseSettings();
    }



    public void OpenLeaderboard()
    {
        gm.Vibrate("soft");
        //KTGameCenter.SharedCenter().ShowLeaderboard("casino_tower_leaderboards");
    }




    public void Open_PokerRatings()
    {
        gm.Vibrate("soft");
        PokerRatings.SetActive(true);
        HowToPlay.SetActive(false);
    }
    public void Close_PokerRatings()
    {
        gm.Vibrate("soft");
        PokerRatings.SetActive(false);
        HowToPlay.SetActive(true);
    }
    public void Open_HTP_Hilo()
    {
        gm.Vibrate("soft");
        HTP_Hilo.SetActive(true);
        HowToPlay.SetActive(false);
    }
    public void Close_HTP_Hilo()
    {
        gm.Vibrate("soft");
        HTP_Hilo.SetActive(false);
        HowToPlay.SetActive(true);
    }
    public void Open_HTP_Texas()
    {
        gm.Vibrate("soft");
        HTP_Texas.SetActive(true);
        HowToPlay.SetActive(false);
    }
    public void Close_HTP_Texas()
    {
        gm.Vibrate("soft");
        HTP_Texas.SetActive(false);
        HowToPlay.SetActive(true);
    }
    public void Open_HTP_BJ()
    {
        gm.Vibrate("soft");
        HTP_BJ.SetActive(true);
        HowToPlay.SetActive(false);
    }
    public void Close_HTP_BJ()
    {
        gm.Vibrate("soft");
        HTP_BJ.SetActive(false);
        HowToPlay.SetActive(true);
    }
    public void Open_HTP_ThreeSum()
    {
        gm.Vibrate("soft");
        HTP_ThreeSum.SetActive(true);
        HowToPlay.SetActive(false);
    }
    public void Close_HTP_ThreeSum()
    {
        gm.Vibrate("soft");
        HTP_ThreeSum.SetActive(false);
        HowToPlay.SetActive(true);
    }
    public void Open_HTP_SetPoker()
    {
        gm.Vibrate("soft");
        HTP_SetPoker.SetActive(true);
        HowToPlay.SetActive(false);
    }
    public void Close_HTP_SetPoker()
    {
        gm.Vibrate("soft");
        HTP_SetPoker.SetActive(false);
        HowToPlay.SetActive(true);
    }
    public void Open_HTP_Omaha()
    {
        gm.Vibrate("soft");
        HTP_Omaha.SetActive(true);
        HowToPlay.SetActive(false);
    }
    public void Close_HTP_Omaha()
    {
        gm.Vibrate("soft");
        HTP_Omaha.SetActive(false);
        HowToPlay.SetActive(true);
    }
    public void Open_HTP_VideoPoker()
    {
        gm.Vibrate("soft");
        HTP_VideoPoker.SetActive(true);
        HowToPlay.SetActive(false);
    }
    public void Close_HTP_VideoPoker()
    {
        gm.Vibrate("soft");
        HTP_VideoPoker.SetActive(false);
        HowToPlay.SetActive(true);
    }
    public void Open_HTP_CrazyPoker()
    {
        gm.Vibrate("soft");
        HTP_CrazyPoker.SetActive(true);
        HowToPlay.SetActive(false);
    }
    public void Close_HTP_CrazyPoker()
    {
        gm.Vibrate("soft");
        HTP_CrazyPoker.SetActive(false);
        HowToPlay.SetActive(true);
    }

    #endregion


}
