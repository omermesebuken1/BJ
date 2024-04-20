using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

using distriqt.plugins.vibration;
public class GameManager : MonoBehaviour
{
    private FeedbackGenerator _selectGenerator;
    private FeedbackGenerator _impactGenerator;
    [SerializeField] private MainCards maincards;
    [SerializeField] private GameObject GenelUI;

    [SerializeField] private GameObject hiloCanvas;
    [SerializeField] private GameObject THPCanvas;
    [SerializeField] private GameObject BJCanvas;
    [SerializeField] private GameObject SetPokerCanvas;
    [SerializeField] private GameObject ThreeSumCanvas;
    [SerializeField] private GameObject OmahaCanvas;
    [SerializeField] private GameObject CrazyPokerCanvas;
    [SerializeField] private GameObject VideoPokerCanvas;
    [SerializeField] private GameObject SettingsCanvas;
    [SerializeField] private GameObject tutorialCanvas;

    [SerializeField] private List<GameObject> GameModeButtons = new List<GameObject>();

    [SerializeField] private GameObject NewCardButton;
    [SerializeField] private GameObject RedealButton;
    [SerializeField] private GameObject BettingButton;
    [SerializeField] private GameObject BankLowButton;

    [SerializeField] private List<int> betAmount = new List<int>();
    [SerializeField] private TextMeshProUGUI betText;
    private int currentBet;
    private int betCounter;

    [HideInInspector] public double totalMoney;

    [SerializeField] private TextMeshProUGUI bankText;

    [SerializeField] private Slider slider;
    private float sliderValue;

    private float multiplier;

    [SerializeField] private TextMeshProUGUI sliderText;

    private bool betReceived;
    private bool playerLost;
    private double enteredBet;
    private bool bankReady;

    [HideInInspector] public int fps;

    [SerializeField] private AudioClip cardFlipSound;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip modeWin;



    private void Start()
    {
        KTGameCenter.SharedCenter().Authenticate();
        StartPlayerPrefabs();
        bankReady = true;
        totalMoney = System.Convert.ToDouble(PlayerPrefs.GetString("CurrentMoney"));
        if(totalMoney > 1000000000000)
        {
            bankText.text = "Bank: 1T$+";
        }
        else
        {
            bankText.text = "Bank: " + totalMoney.ToString("N0") + "$";
        }
        bankText.text = "Bank: " + totalMoney.ToString("N0") + "$";
        currentBet = betAmount[0];

        GameAreaStatus(true);

        if (PlayerPrefs.GetInt("Tutorial") == 1)
        {
            GenelUI.SetActive(false);
            tutorialCanvas.SetActive(true);
        }
        else
        {
            GenelUI.SetActive(true);
            tutorialCanvas.SetActive(false);
        }

        StartClosingGameModeCanvases();
        ResetBools();
        StartGameModeButtons();
        StartCoroutine(SliderDown());
        StartFPS();
        
    }
    private void Update()
    {

        GameModeManagement();
        SetMainButtonStatus();

    }

    private void StartClosingGameModeCanvases()
    {

        hiloCanvas.SetActive(false);
        THPCanvas.SetActive(false);
        BJCanvas.SetActive(false);
        SetPokerCanvas.SetActive(false);
        OmahaCanvas.SetActive(false);
        CrazyPokerCanvas.SetActive(false);
        ThreeSumCanvas.SetActive(false);
        VideoPokerCanvas.SetActive(false);
        SettingsCanvas.SetActive(false);

    }

    private void betAdjust()
    {

        if (totalMoney < betAmount[0])
        {
            betCounter = 0;
            totalMoney = 0;
            PlayerPrefs.SetString("CurrentMoney", totalMoney.ToString());
            bankText.text = "Bank: 0$";
            currentBet = betAmount[betCounter];
            betText.text = BetAmountShort(currentBet);
            //reklam izle sayfasına yönlendirme
        }
        else
        {
            if (currentBet > totalMoney)
            {

                do
                {
                    betCounter--;
                    currentBet = betAmount[betCounter];
                    betText.text = BetAmountShort(currentBet);

                } while (currentBet > totalMoney);
            }

        }



    }

    private string BetAmountShort(int bet)
    {
        string betName = "";

        switch (bet)
        {
            case 25:
                betName = "25";
                break;

            case 50:
                betName = "50";
                break;

            case 100:
                betName = "100";
                break;

            case 250:
                betName = "250";
                break;

            case 500:
                betName = "500";
                break;

            case 1000:
                betName = "1K";
                break;

            case 2500:
                betName = "2.5K";
                break;

            case 5000:
                betName = "5K";
                break;

            case 10000:
                betName = "10K";
                break;

            case 25000:
                betName = "25K";
                break;

            case 50000:
                betName = "50K";
                break;

            case 100000:
                betName = "100K";
                break;

            case 250000:
                betName = "250K";
                break;

            case 500000:
                betName = "500K";
                break;

            case 1000000:
                betName = "1M";
                break;

            case 2500000:
                betName = "2.5M";
                break;

            case 5000000:
                betName = "5M";
                break;

            case 10000000:
                betName = "10M";
                break;

            case 25000000:
                betName = "25M";
                break;

            case 50000000:
                betName = "50M";
                break;

            case 100000000:
                betName = "100M";
                break;

            case 250000000:
                betName = "250M";
                break;

            case 500000000:
                betName = "500M";
                break;

            case 1000000000:
                betName = "1B";
                break;

        }


        return betName;
    }

    public void betUp()
    {
        Vibrate("soft");
        if (betCounter < betAmount.Count - 1 && betAmount[betCounter + 1] <= totalMoney)
        {
            betCounter++;
            currentBet = betAmount[betCounter];
            betText.text = BetAmountShort(currentBet);

        }

    }
    public void betDown()
    {
        Vibrate("soft");
        if (betCounter > 0)
        {
            betCounter--;
            currentBet = betAmount[betCounter];
            betText.text = BetAmountShort(currentBet);

        }

    }

    private void SetMainButtonStatus()
    {
        if (maincards.mainCardCounter == 0)
        {
            NewCardButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start";
            RedealButton.SetActive(false);

            if (totalMoney < betAmount[0] && !betReceived)
            {
                NewCardButton.SetActive(false);
                BettingButton.SetActive(false);
                BankLowButton.SetActive(true);
            }
            else if (totalMoney >= betAmount[0] && !betReceived)
            {
                NewCardButton.SetActive(true);
                BettingButton.SetActive(true);
                BankLowButton.SetActive(false);
            }

        }
        else
        {
            NewCardButton.GetComponentInChildren<TextMeshProUGUI>().text = "New Card";
            RedealButton.SetActive(true);
            BettingButton.SetActive(false);

        }





    }

    public void GenelRedeal()
    {
        if (bankReady && maincards.cardReady)
        {
            maincards.cardReady = false;
            Vibrate("soft");
            maincards.ReDeal();
            StartCoroutine(ShowEarnedMoney(2));
            StartCoroutine(SliderDown());
            ResetBools();
            playerLost = false;
            betReceived = false;
            betAdjust();

        }
    }

    public void GenelNewCard()
    {
        if (bankReady && maincards.cardReady && maincards.mainCardCounter < 5)
        {
            maincards.cardReady = false;
            Vibrate("soft");
            CardSound();
            maincards.OpenNewMainCard();
            if (!betReceived)
            {
                StartCoroutine(TakeMoneyFromBank());
            }

        }
        if(maincards.mainCardCounter == 5)
        {
            NewCardButton.SetActive(false);
        }



    }

    private void GameAreaStatus(bool status)
    {
        GenelUI.SetActive(status);
    }

    private void StartGameModeButtons()
    {
        foreach (var item in GameModeButtons)
        {
            item.GetComponent<GameMods>().ChangeColor("playable");
        }

    }

    private void GameModeManagement()
    {
        foreach (var item in GameModeButtons)
        {
            if (item.GetComponent<GameMods>().isPlayed)
            {
                item.GetComponent<GameMods>().ChangeColor("played");
            }
            else if (item.GetComponent<GameMods>().buttonLevel == maincards.mainCardCounter && !item.GetComponent<GameMods>().isPlayed)
            {
                item.GetComponent<GameMods>().isLocked = false;
                item.GetComponent<GameMods>().ChangeColor("playable");

            }
            else if (item.GetComponent<GameMods>().buttonLevel != maincards.mainCardCounter && !item.GetComponent<GameMods>().isPlayed)
            {
                item.GetComponent<GameMods>().isLocked = true;
                item.GetComponent<GameMods>().ChangeColor("locked");
            }

        }

    }

    private void ResetBools()
    {
        foreach (var item in GameModeButtons)
        {
            item.GetComponent<GameMods>().isLocked = false;
            item.GetComponent<GameMods>().isPlayed = false;

        }

    }

    private IEnumerator SliderUp()
    {
        sliderValue++;
        multiplier++;
        slider.DOValue(sliderValue, 1);

        sliderText.text = $"x{Mathf.Pow(2, sliderValue)}";

        if (sliderText.text.Length >= 4)
        {
            sliderText.fontSize = 50;
        }
        else
        {
            sliderText.fontSize = 70;
        }

        yield return null;

    }

    private IEnumerator SliderDown()
    {
        yield return new WaitUntil(() => bankReady == true);

        multiplier = 0;
        sliderValue = 0;
        slider.DOValue(sliderValue, 1);

        sliderText.text = "";

        yield return null;

    }

    public IEnumerator TakeMoneyFromBank()
    {
        yield return new WaitUntil(() => bankReady == true);

        bankReady = false;

        enteredBet = betAmount[betCounter];

        double targetNumber = totalMoney - enteredBet;

        DOTween.To(() => totalMoney, x => totalMoney = x, targetNumber, 0.5f)
        .OnUpdate(() =>
        {

        if(totalMoney > 1000000000000)
        {
            bankText.text = "Bank: 1T$+";
        }
        else
        {
            bankText.text = "Bank: " + totalMoney.ToString("N0") + "$";
        }

        });

        betReceived = true;

        yield return new WaitUntil(() => totalMoney == targetNumber);

        if(totalMoney > 1000000000000)
        {

            bankText.text = "Bank: 1T$+";
        }
        else
        {   

            bankText.text = "Bank: " + totalMoney.ToString("N0") + "$";
        }

        PlayerPrefs.SetString("CurrentMoney", totalMoney.ToString());
        bankReady = true;

    }

    public IEnumerator GiveMoneyToBank(double targetNumber)
    {
        CoinSound();
        DOTween.To(() => totalMoney, x => totalMoney = x, targetNumber, 0.5f)

        .OnUpdate(() =>
        {
            Vibrate("soft");
            if(totalMoney > 1000000000000)
        {
            bankText.text = "Bank: 1T$+";
        }
        else
        {
            bankText.text = "Bank: " + totalMoney.ToString("N0") + "$";
        }
        });

        betReceived = false;

        yield return new WaitUntil(() => totalMoney == targetNumber);

        if(totalMoney > 1000000000000)
        {
            bankText.text = "Bank: 1T$+";
        }
        else
        {
            bankText.text = "Bank: " + totalMoney.ToString("N0") + "$";
        }

        PlayerPrefs.SetString("CurrentMoney", totalMoney.ToString());
        UpdateMaxStatistics("maks_bank", (int)(totalMoney));

        enteredBet = 0;
        bankReady = true;

        yield return null;



    }

    public IEnumerator ShowEarnedMoney(float waitTime)
    {

        if (!playerLost)
        {
            yield return new WaitUntil(() => bankReady == true);


            WaitForSeconds wait = new WaitForSeconds(waitTime);

            bankReady = false;

            double moneyToAdd = enteredBet * Mathf.Pow(2, multiplier);

            if (multiplier == 0)
            {
                moneyToAdd = 0;
                enteredBet = 0;
                UpdateStatistics("TotalGamesLose");
            }

            if (moneyToAdd != 0)
            {
                WinSound();
                UpdateMaxStatistics("mask_pot", (int)(moneyToAdd));
                UpdateMaxStatistics("maks_multiplier", (int)(Mathf.Pow(2, multiplier)));
                UpdateStatistics("TotalGamesWin");
                double targetNumber = totalMoney + moneyToAdd;
                bankText.text = "+" + moneyToAdd.ToString("N0") + "$";
                Vector3 originalPos = bankText.transform.localPosition;
                Vector3 startPos = bankText.transform.localPosition + new Vector3(500, 0, 0);
                bankText.transform.localPosition = startPos;
                bankText.transform.DOLocalMove(originalPos + new Vector3(250, 0, 0), 2);

                yield return wait;

                bankText.transform.localPosition = originalPos;

                StartCoroutine(GiveMoneyToBank(targetNumber));

            }
            else
            {
                betAdjust();
                bankReady = true;
                LoseSound();
            }

        }
        else
        {
            enteredBet = 0;
            UpdateStatistics("TotalGamesLose");
            betAdjust();
            LoseSound();
        }


    }

    public void UpdateStatistics(string statistics)
    {
        int tmp = PlayerPrefs.GetInt(statistics);
        tmp++;
        PlayerPrefs.SetInt(statistics, tmp);

        switch (statistics)
        {
            case "TotalGamesWin":
                KTGameCenter.SharedCenter().SubmitScore(tmp, "casino_tower_total_wins");
                break;

            case "GamblerBadge":

                KTGameCenter.SharedCenter().SubmitScore(tmp, "gambler_badge");
                break;

        }

    }

    private void UpdateMaxStatistics(string statistics, int max)
    {

        switch (statistics)
        {
            case "maks_bank":
                KTGameCenter.SharedCenter().SubmitScore(max, statistics);
                break;

            case "mask_pot":
                KTGameCenter.SharedCenter().SubmitScore(max, statistics);
                break;

            case "maks_multiplier":
                KTGameCenter.SharedCenter().SubmitScore(max, statistics);
                break;

        }

    }

    public void CodeRedeemed(float money)
    {
        StartCoroutine(GiveMoneyToBank(totalMoney + money));
    }

    public void GamblerBadgeBankAdjust(float money)
    {
        StartCoroutine(GiveMoneyToBank(money));
    }

    public void OpenSettings()
    {
        Vibrate("soft");
        SettingsCanvas.SetActive(true);
        GenelUI.SetActive(false);
        SettingsCanvas.GetComponent<SettingsScript>().ResetSettings();
    }

    private void StartPlayerPrefabs()
    {
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            PlayerPrefs.SetInt("Tutorial", 1);
        }

        if (!PlayerPrefs.HasKey("CurrentMoney"))
        {
            PlayerPrefs.SetString("CurrentMoney", "2500");
        }

        if (!PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetInt("Sound", 1);
        }

        if (!PlayerPrefs.HasKey("Vibration"))
        {
            PlayerPrefs.SetInt("Vibration", 1);
        }

        if (!PlayerPrefs.HasKey("FPS"))
        {
            PlayerPrefs.SetInt("FPS", 60);
        }


        if (!PlayerPrefs.HasKey("TotalGamesWin"))
        {
            PlayerPrefs.SetInt("TotalGamesWin", 0);
        }

        if (!PlayerPrefs.HasKey("TotalGamesLose"))
        {
            PlayerPrefs.SetInt("TotalGamesLose", 0);
        }

        if (!PlayerPrefs.HasKey("GamblerBadge"))
        {
            PlayerPrefs.SetInt("GamblerBadge", 0);
        }


        if (!PlayerPrefs.HasKey("HiloLose"))
        {
            PlayerPrefs.SetInt("HiloLose", 0);
        }

        if (!PlayerPrefs.HasKey("HiloWin"))
        {
            PlayerPrefs.SetInt("HiloWin", 0);
        }

        if (!PlayerPrefs.HasKey("TexasLose"))
        {
            PlayerPrefs.SetInt("TexasLose", 0);
        }

        if (!PlayerPrefs.HasKey("TexasWin"))
        {
            PlayerPrefs.SetInt("TexasWin", 0);
        }

        if (!PlayerPrefs.HasKey("BJLose"))
        {
            PlayerPrefs.SetInt("BJLose", 0);
        }

        if (!PlayerPrefs.HasKey("BJWin"))
        {
            PlayerPrefs.SetInt("BJWin", 0);
        }

        if (!PlayerPrefs.HasKey("ThreeSumLose"))
        {
            PlayerPrefs.SetInt("ThreeSumLose", 0);
        }

        if (!PlayerPrefs.HasKey("ThreeSumWin"))
        {
            PlayerPrefs.SetInt("ThreeSumWin", 0);
        }

        if (!PlayerPrefs.HasKey("SetPokerLose"))
        {
            PlayerPrefs.SetInt("SetPokerLose", 0);
        }

        if (!PlayerPrefs.HasKey("SetPokerWin"))
        {
            PlayerPrefs.SetInt("SetPokerWin", 0);
        }

        if (!PlayerPrefs.HasKey("OmahaLose"))
        {
            PlayerPrefs.SetInt("OmahaLose", 0);
        }

        if (!PlayerPrefs.HasKey("OmahaWin"))
        {
            PlayerPrefs.SetInt("OmahaWin", 0);
        }

        if (!PlayerPrefs.HasKey("VideoPokerLose"))
        {
            PlayerPrefs.SetInt("VideoPokerLose", 0);
        }

        if (!PlayerPrefs.HasKey("VideoPokerWin"))
        {
            PlayerPrefs.SetInt("VideoPokerWin", 0);
        }

        if (!PlayerPrefs.HasKey("CrazyPokerLose"))
        {
            PlayerPrefs.SetInt("CrazyPokerLose", 0);
        }

        if (!PlayerPrefs.HasKey("CrazyPokerWin"))
        {
            PlayerPrefs.SetInt("CrazyPokerWin", 0);
        }



        if (!PlayerPrefs.HasKey("BestTexasScore"))
        {
            PlayerPrefs.SetInt("BestTexasScore", 0);
        }

        if (!PlayerPrefs.HasKey("BestSetPokerScore"))
        {
            PlayerPrefs.SetInt("BestSetPokerScore", 0);
        }

        if (!PlayerPrefs.HasKey("BestOmahaScore"))
        {
            PlayerPrefs.SetInt("BestOmahaScore", 0);
        }

        if (!PlayerPrefs.HasKey("BestVideoPokerScore"))
        {
            PlayerPrefs.SetInt("BestVideoPokerScore", 0);
        }

        if (!PlayerPrefs.HasKey("BestCrazyPokerScore"))
        {
            PlayerPrefs.SetInt("BestCrazyPokerScore", 0);
        }




        if (!PlayerPrefs.HasKey("BestHandTexas_0"))
        {
            PlayerPrefs.SetInt("BestHandTexas_0", 0);
        }

        if (!PlayerPrefs.HasKey("BestHandTexas_1"))
        {
            PlayerPrefs.SetInt("BestHandTexas_1", 0);
        }

        if (!PlayerPrefs.HasKey("BestHandTexas_2"))
        {
            PlayerPrefs.SetInt("BestHandTexas_2", 0);
        }

        if (!PlayerPrefs.HasKey("BestHandTexas_3"))
        {
            PlayerPrefs.SetInt("BestHandTexas_3", 0);
        }

        if (!PlayerPrefs.HasKey("BestHandTexas_4"))
        {
            PlayerPrefs.SetInt("BestHandTexas_4", 0);
        }



        if (!PlayerPrefs.HasKey("BestHandSetPoker_0"))
        {
            PlayerPrefs.SetInt("BestHandSetPoker_0", 0);
        }

        if (!PlayerPrefs.HasKey("BestHandSetPoker_1"))
        {
            PlayerPrefs.SetInt("BestHandSetPoker_1", 0);
        }

        if (!PlayerPrefs.HasKey("BestHandSetPoker_2"))
        {
            PlayerPrefs.SetInt("BestHandSetPoker_2", 0);
        }



        if (!PlayerPrefs.HasKey("BestHandOmaha_0"))
        {
            PlayerPrefs.SetInt("BestHandOmaha_0", 0);
        }

        if (!PlayerPrefs.HasKey("BestHandOmaha_1"))
        {
            PlayerPrefs.SetInt("BestHandOmaha_1", 0);
        }

        if (!PlayerPrefs.HasKey("BestHandOmaha_2"))
        {
            PlayerPrefs.SetInt("BestHandOmaha_2", 0);
        }

        if (!PlayerPrefs.HasKey("BestHandOmaha_3"))
        {
            PlayerPrefs.SetInt("BestHandOmaha_3", 0);
        }

        if (!PlayerPrefs.HasKey("BestHandOmaha_4"))
        {
            PlayerPrefs.SetInt("BestHandOmaha_4", 0);
        }



        if (!PlayerPrefs.HasKey("BestHandVideoPoker_0"))
        {
            PlayerPrefs.SetInt("BestHandVideoPoker_0", 0);
        }

        if (!PlayerPrefs.HasKey("BestHandVideoPoker_1"))
        {
            PlayerPrefs.SetInt("BestHandVideoPoker_1", 0);
        }

        if (!PlayerPrefs.HasKey("BestHandVideoPoker_2"))
        {
            PlayerPrefs.SetInt("BestHandVideoPoker_2", 0);
        }

        if (!PlayerPrefs.HasKey("BestHandVideoPoker_3"))
        {
            PlayerPrefs.SetInt("BestHandVideoPoker_3", 0);
        }

        if (!PlayerPrefs.HasKey("BestHandVideoPoker_4"))
        {
            PlayerPrefs.SetInt("BestHandVideoPoker_4", 0);
        }



        if (!PlayerPrefs.HasKey("BestHandCrazyPoker_0"))
        {
            PlayerPrefs.SetInt("BestHandCrazyPoker_0", 0);
        }

        if (!PlayerPrefs.HasKey("BestHandCrazyPoker_1"))
        {
            PlayerPrefs.SetInt("BestHandCrazyPoker_1", 0);
        }

        if (!PlayerPrefs.HasKey("BestHandCrazyPoker_2"))
        {
            PlayerPrefs.SetInt("BestHandCrazyPoker_2", 0);
        }

        if (!PlayerPrefs.HasKey("BestHandCrazyPoker_3"))
        {
            PlayerPrefs.SetInt("BestHandCrazyPoker_3", 0);
        }


    }


    #region // Game Buttons

    public void PlayHiLo()
    {
        if (maincards.cardReady && bankReady)
        {
            var hiloMode = GameModeButtons.Find(x => x.gameObject.name == "Hi-Lo");
            if (!hiloMode.GetComponent<GameMods>().isLocked && !hiloMode.GetComponent<GameMods>().isPlayed)
            {
                GameAreaStatus(false);
                hiloCanvas.SetActive(true);
                hiloCanvas.GetComponent<HiLo>().StartHiLo();
            }
        }

    }
    public void RedealHiLo()
    {
        playerLost = true;
        GameAreaStatus(true);
        hiloCanvas.SetActive(false);
        ResetBools();
        GenelRedeal();
        UpdateStatistics("HiloLose");

    }
    public void ContinueHiLo()
    {
        var hiloMode = GameModeButtons.Find(x => x.gameObject.name == "Hi-Lo");
        GameAreaStatus(true);
        hiloCanvas.SetActive(false);
        hiloMode.GetComponent<GameMods>().isPlayed = true;
        StartCoroutine(SliderUp());
        UpdateStatistics("HiloWin");
        ModeWinSound();

    }

    public void PlayTHP()
    {
        if (maincards.cardReady && bankReady)
        {
            var THPMode = GameModeButtons.Find(x => x.gameObject.name == "TexasPoker");
            if (!THPMode.GetComponent<GameMods>().isLocked && !THPMode.GetComponent<GameMods>().isPlayed)
            {
                GameAreaStatus(false);
                THPCanvas.SetActive(true);
                THPCanvas.GetComponent<TexasHoldem>().ResetTHP();
            }
        }

    }
    public void RedealTHP()
    {
        playerLost = true;
        GameAreaStatus(true);
        THPCanvas.SetActive(false);
        ResetBools();
        GenelRedeal();
        UpdateStatistics("TexasLose");

    }
    public void ContinueTHP()
    {
        var texasMode = GameModeButtons.Find(x => x.gameObject.name == "TexasPoker");
        Vibrate("soft");
        GameAreaStatus(true);
        THPCanvas.SetActive(false);
        texasMode.GetComponent<GameMods>().isPlayed = true;
        StartCoroutine(SliderUp());
        UpdateStatistics("TexasWin");
        ModeWinSound();
    }

    public void PlayBJ()
    {
        if (maincards.cardReady && bankReady)
        {
            var BJMode = GameModeButtons.Find(x => x.gameObject.name == "BlackJack");
            if (!BJMode.GetComponent<GameMods>().isLocked && !BJMode.GetComponent<GameMods>().isPlayed)
            {
                GameAreaStatus(false);
                BJCanvas.SetActive(true);
                BJCanvas.GetComponent<BlackJack>().ResetBlackJack();
            }
        }
    }
    public void RedealBJ()
    {
        playerLost = true;
        GameAreaStatus(true);
        BJCanvas.SetActive(false);
        ResetBools();
        GenelRedeal();
        UpdateStatistics("BJLose");
    }
    public void ContinueBJ()
    {
        var BJMode = GameModeButtons.Find(x => x.gameObject.name == "BlackJack");

        GameAreaStatus(true);
        BJCanvas.SetActive(false);
        Vibrate("soft");
        BJMode.GetComponent<GameMods>().isPlayed = true;
        StartCoroutine(SliderUp());
        UpdateStatistics("BJWin");
        ModeWinSound();
    }

    public void PlaySetPoker()
    {
        if (maincards.cardReady && bankReady)
        {
            var SetPokerMode = GameModeButtons.Find(x => x.gameObject.name == "SetPoker");
            if (!SetPokerMode.GetComponent<GameMods>().isLocked && !SetPokerMode.GetComponent<GameMods>().isPlayed)
            {
                GameAreaStatus(false);
                SetPokerCanvas.SetActive(true);
                SetPokerCanvas.GetComponent<SetPoker>().ResetSetPoker();
            }
        }
    }
    public void RedealSetPoker()
    {
        playerLost = true;
        GameAreaStatus(true);
        SetPokerCanvas.SetActive(false);
        ResetBools();
        GenelRedeal();
        UpdateStatistics("SetPokerLose");

    }
    public void ContinueSetPoker()
    {
        var SetPokerMode = GameModeButtons.Find(x => x.gameObject.name == "SetPoker");
        GameAreaStatus(true);
        Vibrate("soft");
        SetPokerCanvas.SetActive(false);
        SetPokerMode.GetComponent<GameMods>().isPlayed = true;
        StartCoroutine(SliderUp());
        UpdateStatistics("SetPokerWin");
        ModeWinSound();
    }

    public void PlayThreeSum()
    {
        if (maincards.cardReady && bankReady)
        {
            var ThreeSumMode = GameModeButtons.Find(x => x.gameObject.name == "ThreeSum");
            if (!ThreeSumMode.GetComponent<GameMods>().isLocked && !ThreeSumMode.GetComponent<GameMods>().isPlayed)
            {
                GameAreaStatus(false);
                ThreeSumCanvas.SetActive(true);
                ThreeSumCanvas.GetComponent<ThreeSum>().ResetThreeSum();
            }
        }
    }
    public void RedealThreeSum()
    {
        playerLost = true;
        GameAreaStatus(true);
        ThreeSumCanvas.SetActive(false);
        ResetBools();
        GenelRedeal();
        UpdateStatistics("ThreeSumLose");

    }
    public void ContinueThreeSum()
    {
        var ThreeSumMode = GameModeButtons.Find(x => x.gameObject.name == "ThreeSum");
        GameAreaStatus(true);
        Vibrate("soft");
        ThreeSumCanvas.SetActive(false);
        ThreeSumMode.GetComponent<GameMods>().isPlayed = true;
        StartCoroutine(SliderUp());
        UpdateStatistics("ThreeSumWin");
        ModeWinSound();
    }

    public void PlayOmaha()
    {
        if (maincards.cardReady && bankReady)
        {
            var OmahaMode = GameModeButtons.Find(x => x.gameObject.name == "Omaha");
            if (!OmahaMode.GetComponent<GameMods>().isLocked && !OmahaMode.GetComponent<GameMods>().isPlayed)
            {
                GameAreaStatus(false);
                OmahaCanvas.SetActive(true);
                OmahaCanvas.GetComponent<OmahaScript>().ResetOmaha();
            }
        }
    }
    public void RedealOmaha()
    {
        playerLost = true;
        GameAreaStatus(true);
        OmahaCanvas.SetActive(false);
        ResetBools();
        GenelRedeal();
        UpdateStatistics("OmahaLose");

    }
    public void ContinueOmaha()
    {
        var OmahaMode = GameModeButtons.Find(x => x.gameObject.name == "Omaha");
        GameAreaStatus(true);
        Vibrate("soft");
        OmahaCanvas.SetActive(false);
        OmahaMode.GetComponent<GameMods>().isPlayed = true;
        StartCoroutine(SliderUp());
        UpdateStatistics("OmahaWin");
        ModeWinSound();
    }

    public void PlayCrazyPoker()
    {
        if (maincards.cardReady && bankReady)
        {
            var CrazyPokerMode = GameModeButtons.Find(x => x.gameObject.name == "Crazy4Poker");
            if (!CrazyPokerMode.GetComponent<GameMods>().isLocked && !CrazyPokerMode.GetComponent<GameMods>().isPlayed)
            {
                GameAreaStatus(false);
                CrazyPokerCanvas.SetActive(true);
                CrazyPokerCanvas.GetComponent<CrazyPoker>().ResetCrazyPoker();
            }
        }
    }
    public void RedealCrazyPoker()
    {
        playerLost = true;
        GameAreaStatus(true);
        CrazyPokerCanvas.SetActive(false);
        ResetBools();
        GenelRedeal();
        UpdateStatistics("CrazyPokerLose");

    }
    public void ContinueCrazyPoker()
    {
        var CrazyPokerMode = GameModeButtons.Find(x => x.gameObject.name == "Crazy4Poker");
        GameAreaStatus(true);
        Vibrate("soft");
        CrazyPokerCanvas.SetActive(false);
        CrazyPokerMode.GetComponent<GameMods>().isPlayed = true;
        StartCoroutine(SliderUp());
        UpdateStatistics("CrazyPokerWin");
        ModeWinSound();
    }

    public void PlayVideoPoker()
    {
        if (maincards.cardReady && bankReady)
        {
            var VideoPokerMode = GameModeButtons.Find(x => x.gameObject.name == "VideoPoker");
            if (!VideoPokerMode.GetComponent<GameMods>().isLocked && !VideoPokerMode.GetComponent<GameMods>().isPlayed)
            {
                GameAreaStatus(false);
                VideoPokerCanvas.SetActive(true);
                VideoPokerCanvas.GetComponent<VideoPoker>().ResetVideoPoker();
            }
        }
    }
    public void RedealVideoPoker()
    {
        playerLost = true;
        GameAreaStatus(true);
        VideoPokerCanvas.SetActive(false);
        ResetBools();
        GenelRedeal();
        UpdateStatistics("VideoPokerLose");

    }
    public void ContinueVideoPoker()
    {
        var VideoPokerMode = GameModeButtons.Find(x => x.gameObject.name == "VideoPoker");
        GameAreaStatus(true);
        Vibrate("soft");
        VideoPokerCanvas.SetActive(false);
        VideoPokerMode.GetComponent<GameMods>().isPlayed = true;
        StartCoroutine(SliderUp());
        UpdateStatistics("VideoPokerWin");
        ModeWinSound();
    }



    #endregion

    public void SetFPS(int tmpFps)
    {
        PlayerPrefs.SetInt("FPS", tmpFps);
        Application.targetFrameRate = tmpFps;
    }

    private void StartFPS()
    {
        if (PlayerPrefs.HasKey("FPS"))
        {
            switch (PlayerPrefs.GetInt("FPS"))
            {
                case 30:
                    SetFPS(30);
                    break;

                case 60:
                    SetFPS(60);
                    break;

            }
        }
    }

    public void Vibrate(string type)
    {
        if (Vibration.isSupported)
        {
            if (PlayerPrefs.HasKey("Vibration"))
            {
                if (PlayerPrefs.GetInt("Vibration") == 1)
                {
                    switch (type)
                    {
                        case "soft":
                            if (_selectGenerator == null)
                            {
                                _selectGenerator = Vibration.Instance.CreateFeedbackGenerator(FeedbackGeneratorType.SELECTION);
                            }
                            _selectGenerator.PerformFeedback();
                            break;

                        case "hard":
                            if (_impactGenerator == null)
                            {
                                _impactGenerator = Vibration.Instance.CreateFeedbackGenerator(FeedbackGeneratorType.IMPACT);
                                _impactGenerator.Prepare();
                            }
                            _impactGenerator.PerformFeedback();
                            break;
                    }
                }
            }
        }

    }

    public void CardSound()
    {

        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 1)
            {

                GetComponent<AudioSource>().PlayOneShot(cardFlipSound, 0.5f);

            }
        }


    }

    public void CoinSound()
    {

        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 1)
            {

                GetComponent<AudioSource>().PlayOneShot(coinSound);

            }
        }


    }

    public void WinSound()
    {

        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 1)
            {

                GetComponent<AudioSource>().PlayOneShot(winSound);

            }
        }


    }

    public void LoseSound()
    {

        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 1)
            {

                GetComponent<AudioSource>().PlayOneShot(loseSound);

            }
        }


    }

    public void ModeWinSound()
    {

        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 1)
            {

                GetComponent<AudioSource>().PlayOneShot(modeWin);

            }
        }


    }


}
