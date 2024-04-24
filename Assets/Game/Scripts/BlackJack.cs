using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Linq;
using System.ComponentModel;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;


public class BlackJack : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private List<GameObject> playerCardsGO = new List<GameObject>();
    [SerializeField] private List<GameObject> dealerCardsGO = new List<GameObject>();

    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI dealerScoreText;
    [SerializeField] private TextMeshProUGUI splitScoreText1;
    [SerializeField] private TextMeshProUGUI splitScoreText2;

    [SerializeField] private TextMeshProUGUI winRateText;
    [SerializeField] private TextMeshProUGUI totalHandText;
    [SerializeField] private TextMeshProUGUI givenText;
    [SerializeField] private TextMeshProUGUI receivedText;
    [SerializeField] private TextMeshProUGUI diffText;

    private float winRate;
    private int totalHand;
    private int winCounter;
    private int loseCounter;
    private int drawCounter;
    private int givenCounter;
    private int receivedCounter;
    private int diffCounter;

    [SerializeField] private Button hitButton;
    [SerializeField] private Button standButton;
    [SerializeField] private Button splitButton;
    [SerializeField] private Button DDButton;
    [SerializeField] private Button continueButton;


    [SerializeField] private GameObject cardPrefab;


    public List<Card> dealerCards = new List<Card>();
    public List<Card> playerCards = new List<Card>();
    public List<Card> splitCards1 = new List<Card>();
    public List<Card> splitCards2 = new List<Card>();


    private Deck deck;
    private Card card;

    public int playerScore;
    public int splitScore1;
    public int splitScore2;
    private int dealerScore;

    [SerializeField] private float gapSizeForPlayer = 240;
    [SerializeField] private float gapSizeForDealer = 240;
    [SerializeField] private float gapSizeForSplit1 = 240;
    [SerializeField] private float gapSizeForSplit2 = 240;

    private bool playerIsStand;

    private bool split2IsStand;
    private bool dealerHitting;

    private bool playerBusted;
    private bool split1Busted;
    private bool split2Busted;
    private bool dealerBusted;
    private bool gameEnd;

    private bool winRateCalculation;

    public Vector3 targetPos1;
    public Vector3 targetPos2;
    public Vector3 targetPos3;
    public Vector3 targetPos4;

    [SerializeField] private GameObject PlayerCardArea;
    [SerializeField] private GameObject SplitCardArea;

    [SerializeField] private List<GameObject> splitCardsGO1 = new List<GameObject>();
    [SerializeField] private List<GameObject> splitCardsGO2 = new List<GameObject>();

    [SerializeField] private HelperImp hp;

    public int phase;

    private bool dealerHitAnimationEnd;

    private bool doubleDowned0;
    private bool doubleDowned1;
    private bool doubleDowned2;

    [SerializeField] private GameObject highScoreArea;
    [SerializeField] private TextMeshProUGUI highScoreHeaderText;
    [SerializeField] private TextMeshProUGUI highScoreHandsText;
    [SerializeField] private TextMeshProUGUI highScoreWinRateText;
    [SerializeField] private TextMeshProUGUI highScoreGivenText;
    [SerializeField] private TextMeshProUGUI highScoreReceivedText;
    [SerializeField] private TextMeshProUGUI highScoreDiffText;

    private int highScoreState;

    private Color32 pinkColor = new Color32(255, 148, 224, 255);
    private Color32 yellowColor = new Color32(234, 187, 70, 255);
    private Color32 cyanColor = new Color32(108, 209, 255, 255);

    private void Start()
    {
        Application.targetFrameRate = 60;
        HighScoreStarter();
        ResetAll();
    }

    public void ResetWinRate()
    {
        winCounter = 0;
        loseCounter = 0;
        drawCounter = 0;
        givenCounter = 0;
        receivedCounter = 0;
        diffCounter = 0;
        winRate = 0;
        totalHand = 0;

    }

    public void ContinueButton()
    {
        float fadeTime = 0.2f;

        hitButton.gameObject.SetActive(true);
        standButton.gameObject.SetActive(true);
        splitButton.gameObject.SetActive(true);
        DDButton.gameObject.SetActive(true);

        continueButton.GetComponentInChildren<TextMeshProUGUI>().DOFade(0f, fadeTime);
        continueButton.image.DOFade(0f, fadeTime).OnComplete(() =>
        {
            continueButton.gameObject.SetActive(false);

            ResetBlackJack();

            hitButton.GetComponentInChildren<TextMeshProUGUI>().DOFade(1f, fadeTime);
            hitButton.image.DOFade(1f, fadeTime);

            standButton.GetComponentInChildren<TextMeshProUGUI>().DOFade(1f, fadeTime);
            standButton.image.DOFade(1f, fadeTime);

            splitButton.GetComponentInChildren<TextMeshProUGUI>().DOFade(1f, fadeTime);
            splitButton.image.DOFade(1f, fadeTime);

            DDButton.GetComponentInChildren<TextMeshProUGUI>().DOFade(1f, fadeTime);
            DDButton.image.DOFade(1f, fadeTime);

        });
    }

    public void ResetAll()
    {

        ResetWinRate();
        ResetBlackJack();
    }

    public void ResetBlackJack()
    {
        highScoreState = 0;
        highScoreArea.SetActive(false);

        winRateCounter();

        phase = 0;

        hp.ResetColors();
        hp.isHit = false;
        hp.isHit1 = false;
        hp.isHit2 = false;
        hp.isSplitted = false;
        hp.helperDone = true;

        doubleDowned0 = false;
        doubleDowned1 = false;
        doubleDowned2 = false;

        PlayerCardArea.SetActive(true);
        SplitCardArea.SetActive(false);

        winRateCalculation = false;



        FadeInButtons();


        hitButton.interactable = false;
        standButton.interactable = false;
        splitButton.interactable = false;
        DDButton.interactable = false;

        playerScoreText.color = new Color32(255, 255, 255, 255); //white
        splitScoreText1.color = new Color32(255, 255, 255, 255); //white
        splitScoreText2.color = new Color32(255, 255, 255, 255); //white

        dealerScoreText.text = "";
        playerScoreText.text = "";
        splitScoreText1.text = "";
        splitScoreText2.text = "";

        dealerHitAnimationEnd = true;

        prepareCards();

    }

    public void prepareCards()
    {
        ResetCards();

        //creating a new deck
        deck = new Deck();

        //adding players cards
        card = deck.DrawCard();
        playerCards.Add(card);

        card = deck.DrawCard();
        playerCards.Add(card);

        //adding dealer cards
        card = deck.DrawCard();
        dealerCards.Add(card);

        card = deck.DrawCard();
        dealerCards.Add(card);

        StartShowCards();

    }

    public void StartShowCards()
    {
        targetPos1 = dealerCardsGO[0].transform.localPosition;
        targetPos2 = dealerCardsGO[1].transform.localPosition;
        targetPos3 = playerCardsGO[0].transform.localPosition;
        targetPos4 = playerCardsGO[1].transform.localPosition;

        MoveCards();

    }

    public void DD()
    {
        hitButton.interactable = false;
        standButton.interactable = false;
        splitButton.interactable = false;
        DDButton.interactable = false;
        hp.ResetColors();

        givenCounter++;
        winRateCounter();

        switch (phase)
        {
            case 0:
                doubleDowned0 = true;
                DD_0();
                break;

            case 1:
                doubleDowned1 = true;
                DD_1();
                break;

            case 2:
                doubleDowned2 = true;
                DD_2();
                break;
        }

    }

    public void DD_0()
    {
        //bir unit daha verilcek

        if (gapSizeForPlayer > 80)
        {
            gapSizeForPlayer -= 40;
        }

        card = deck.DrawCard();
        playerCards.Add(card);

        var newCard = Instantiate(cardPrefab);
        playerCardsGO.Add(newCard);

        newCard.GetComponent<CardScript>().GetCardBack();

        newCard.transform.parent = playerCardsGO[0].transform.parent;
        newCard.transform.position = playerCardsGO[0].transform.position;
        newCard.transform.localScale = playerCardsGO[0].transform.localScale;
        newCard.GetComponent<SpriteRenderer>().sortingOrder = playerCardsGO[playerCardsGO.Count - 2].GetComponent<SpriteRenderer>().sortingOrder + 1;
        ArrangeObjects(playerCardsGO, gapSizeForPlayer);
        Vector3 targetPos = playerCardsGO[playerCardsGO.Count - 1].transform.localPosition;
        newCard.transform.localPosition = new Vector3(1000, 0, 0);
        newCard.transform.DOLocalMove(targetPos, 0.2f);

        DOTween.Sequence().AppendInterval(0.3f).OnComplete(() =>
        {
            StartCoroutine(FlipCard(newCard, 0.1f, card));
            playerScore = Calculator(playerCards);
            playerScoreText.text = playerScore.ToString();
            Stand();

        });

    }

    public void DD_1()
    {
        //bir unit daha verilcek

        hitButton.interactable = false;
        standButton.interactable = false;
        splitButton.interactable = false;
        DDButton.interactable = false;

        if (gapSizeForSplit1 > 80)
        {
            gapSizeForSplit1 -= 40;
        }

        card = deck.DrawCard();
        splitCards1.Add(card);

        var newCard = Instantiate(cardPrefab);
        splitCardsGO1.Add(newCard);

        newCard.GetComponent<CardScript>().GetCardBack();

        newCard.transform.parent = splitCardsGO1[0].transform.parent;
        newCard.transform.position = splitCardsGO1[0].transform.position;
        newCard.transform.localScale = splitCardsGO1[0].transform.localScale;
        newCard.GetComponent<SpriteRenderer>().sortingOrder = splitCardsGO1[splitCardsGO1.Count - 2].GetComponent<SpriteRenderer>().sortingOrder + 1;
        ArrangeObjects(splitCardsGO1, gapSizeForSplit1);
        Vector3 targetPos = splitCardsGO1[splitCardsGO1.Count - 1].transform.localPosition;
        newCard.transform.localPosition = new Vector3(1000, 0, 0);
        newCard.transform.DOLocalMove(targetPos, 0.2f);

        DOTween.Sequence().AppendInterval(0.3f).OnComplete(() =>
        {
            StartCoroutine(FlipCard(newCard, 0.1f, card));
            splitScore1 = Calculator(splitCards1);
            splitScoreText1.text = splitScore1.ToString();
            Stand();

        });

    }

    public void DD_2()
    {
        //bir unit daha verilcek

        hitButton.interactable = false;
        standButton.interactable = false;
        splitButton.interactable = false;
        DDButton.interactable = false;


        if (gapSizeForSplit2 > 80)
        {
            gapSizeForSplit2 -= 40;
        }

        card = deck.DrawCard();
        splitCards2.Add(card);

        var newCard = Instantiate(cardPrefab);
        splitCardsGO2.Add(newCard);

        newCard.GetComponent<CardScript>().GetCardBack();

        newCard.transform.parent = splitCardsGO2[0].transform.parent;
        newCard.transform.position = splitCardsGO2[0].transform.position;
        newCard.transform.localScale = splitCardsGO2[0].transform.localScale;
        newCard.GetComponent<SpriteRenderer>().sortingOrder = splitCardsGO2[splitCardsGO2.Count - 2].GetComponent<SpriteRenderer>().sortingOrder + 1;
        ArrangeObjects(splitCardsGO2, gapSizeForSplit2);
        Vector3 targetPos = splitCardsGO2[splitCardsGO2.Count - 1].transform.localPosition;
        newCard.transform.localPosition = new Vector3(1000, 0, 0);
        newCard.transform.DOLocalMove(targetPos, 0.2f);

        DOTween.Sequence().AppendInterval(0.3f).OnComplete(() =>
        {
            StartCoroutine(FlipCard(newCard, 0.1f, card));
            splitScore2 = Calculator(splitCards2);
            splitScoreText2.text = splitScore2.ToString();
            Stand();

        });
    }

    public void Hit()
    {
        switch (phase)
        {
            case 0:
                HitPlayer();
                break;

            case 1:
                HitSplit1();
                break;

            case 2:
                HitSplit2();
                break;
        }
    }

    private void MoveCards()
    {
        dealerCardsGO[0].GetComponent<CardScript>().GetCardBack();
        dealerCardsGO[1].GetComponent<CardScript>().GetCardBack();
        playerCardsGO[0].GetComponent<CardScript>().GetCardBack();
        playerCardsGO[1].GetComponent<CardScript>().GetCardBack();

        dealerCardsGO[0].transform.localPosition = new Vector3(1500, 0, 0);
        dealerCardsGO[1].transform.localPosition = new Vector3(1500, 0, 0);
        playerCardsGO[0].transform.localPosition = new Vector3(1500, 0, 0);
        playerCardsGO[1].transform.localPosition = new Vector3(1500, 0, 0);

        playerCardsGO[0].transform.DOLocalMove(targetPos3, 0.2f);
        DOTween.Sequence().AppendInterval(0.3f).OnComplete(() =>
        {
            StartCoroutine(FlipCard(playerCardsGO[0], 0.1f, playerCards[0]));
            DealerMoveCard();
        });
    }
    public void DealerMoveCard()
    {
        dealerCardsGO[0].transform.DOLocalMove(targetPos1, 0.2f);
        DOTween.Sequence().AppendInterval(0.3f).OnComplete(() =>
        {
            StartCoroutine(FlipCard(dealerCardsGO[0], 0.1f, dealerCards[0]));
            PlayerMoveCard_1();
        });
    }
    private void PlayerMoveCard_1()
    {
        playerCardsGO[1].transform.DOLocalMove(targetPos4, 0.2f);
        DOTween.Sequence().AppendInterval(0.3f).OnComplete(() =>
        {
            StartCoroutine(FlipCard(playerCardsGO[1], 0.1f, playerCards[1]));
            playerScore = Calculator(playerCards);
            playerScoreText.text = playerScore.ToString();
            DealerMoveCard_1();
        });

    }
    private void DealerMoveCard_1()
    {
        dealerCardsGO[1].transform.DOLocalMove(targetPos2, 0.2f);
        DOTween.Sequence().AppendInterval(0.3f).OnComplete(() =>
        {
            hitButton.interactable = true;
            standButton.interactable = true;
            DDButton.interactable = true;
            givenCounter++;

            if (playerCards[0].number == playerCards[1].number)
            {
                splitButton.interactable = true;
            }

            if (playerScore == 21)
            {
                Stand();
            }
            else
            {
                hp.getHelp();
            }



        });
    }

    private IEnumerator FlipCard(GameObject cardGO, float waitTime, Card card)
    {

        WaitForSeconds wait = new WaitForSeconds(waitTime);

        cardGO.transform.DOLocalRotate(new Vector3(0, 90, 0), 0.1f);
        yield return wait;

        cardGO.GetComponent<CardScript>().GetCardSprite(card);

        cardGO.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f);
        yield return wait;

        yield return new WaitUntil(() => cardGO.transform.rotation == Quaternion.Euler(0, 0, 0));

    }

    public void HitPlayer()
    {
        hitButton.interactable = false;
        standButton.interactable = false;
        splitButton.interactable = false;
        DDButton.interactable = false;

        hp.ResetColors();
        hp.isHit = true;


        if (gapSizeForPlayer > 80)
        {
            gapSizeForPlayer -= 40;
        }

        card = deck.DrawCard();
        playerCards.Add(card);

        var newCard = Instantiate(cardPrefab);
        playerCardsGO.Add(newCard);

        newCard.GetComponent<CardScript>().GetCardBack();

        newCard.transform.parent = playerCardsGO[0].transform.parent;
        newCard.transform.position = playerCardsGO[0].transform.position;
        newCard.transform.localScale = playerCardsGO[0].transform.localScale;
        newCard.GetComponent<SpriteRenderer>().sortingOrder = playerCardsGO[playerCardsGO.Count - 2].GetComponent<SpriteRenderer>().sortingOrder + 1;
        ArrangeObjects(playerCardsGO, gapSizeForPlayer);
        Vector3 targetPos = playerCardsGO[playerCardsGO.Count - 1].transform.localPosition;
        newCard.transform.localPosition = new Vector3(1000, 0, 0);
        newCard.transform.DOLocalMove(targetPos, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            StartCoroutine(FlipCard(newCard, 0.1f, card));
            playerScore = Calculator(playerCards);
            playerScoreText.text = playerScore.ToString();

            hitButton.interactable = true;
            standButton.interactable = true;

            if (playerScore == 21)
            {
                Stand();
            }
            else
            {
                hp.getHelp();
            }
        });




    }

    private void HitDealer()
    {

        if (dealerHitAnimationEnd)
        {

            dealerHitAnimationEnd = false;

            if (gapSizeForDealer > 80)
            {
                gapSizeForDealer -= 40;
            }

            card = deck.DrawCard();
            dealerCards.Add(card);

            var newCard = Instantiate(cardPrefab);
            dealerCardsGO.Add(newCard);

            newCard.GetComponent<CardScript>().GetCardBack();

            newCard.transform.parent = dealerCardsGO[0].transform.parent;
            newCard.transform.position = dealerCardsGO[0].transform.position;
            newCard.transform.localScale = dealerCardsGO[0].transform.localScale;
            newCard.GetComponent<SpriteRenderer>().sortingOrder = dealerCardsGO[dealerCardsGO.Count - 2].GetComponent<SpriteRenderer>().sortingOrder + 1;
            ArrangeObjects(dealerCardsGO, gapSizeForDealer);
            Vector3 targetPos = dealerCardsGO[dealerCardsGO.Count - 1].transform.localPosition;
            newCard.transform.localPosition = new Vector3(1000, 0, 0);
            newCard.transform.DOLocalMove(targetPos, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                StartCoroutine(FlipCard(newCard, 0.1f, card));
                dealerScore = Calculator(dealerCards);
                dealerScoreText.text = dealerScore.ToString();
                dealerHitAnimationEnd = true;

            });



        }

    }

    private void ResetCards()
    {
        playerScore = 0;
        dealerScore = 0;
        splitScore1 = 0;
        splitScore2 = 0;

        playerIsStand = false;
        split2IsStand = false;

        dealerHitting = false;
        playerBusted = false;
        dealerBusted = false;
        split1Busted = false;
        split2Busted = false;
        gameEnd = false;

        gapSizeForPlayer = 240;
        gapSizeForDealer = 240;
        gapSizeForSplit1 = 240;
        gapSizeForSplit2 = 240;

        playerCards.Clear();
        dealerCards.Clear();
        splitCards1.Clear();
        splitCards2.Clear();

        do
            if (playerCardsGO.Count >= 3)
            {
                var objectToRemove = playerCardsGO[playerCardsGO.Count - 1];
                playerCardsGO.Remove(objectToRemove);
                Destroy(objectToRemove.gameObject);

            }

        while (playerCardsGO.Count >= 3);



        do
            if (dealerCardsGO.Count >= 3)
            {
                var objectToRemove = dealerCardsGO[dealerCardsGO.Count - 1];
                dealerCardsGO.Remove(objectToRemove);
                Destroy(objectToRemove.gameObject);

            }

        while (dealerCardsGO.Count >= 3);

        do
            if (splitCardsGO1.Count >= 3)
            {
                var objectToRemove = splitCardsGO1[splitCardsGO1.Count - 1];
                splitCardsGO1.Remove(objectToRemove);
                Destroy(objectToRemove.gameObject);

            }

        while (splitCardsGO1.Count >= 3);

        do
            if (splitCardsGO2.Count >= 3)
            {
                var objectToRemove = splitCardsGO2[splitCardsGO2.Count - 1];
                splitCardsGO2.Remove(objectToRemove);
                Destroy(objectToRemove.gameObject);

            }

        while (splitCardsGO2.Count >= 3);

        ArrangeObjects(playerCardsGO, gapSizeForPlayer);
        ArrangeObjects(dealerCardsGO, gapSizeForDealer);
        ArrangeObjects(splitCardsGO1, gapSizeForSplit1);
        ArrangeObjects(splitCardsGO2, gapSizeForSplit2);

    }

    private void ArrangeObjects(List<GameObject> objectsToArrange, float gapSize)
    {
        if (objectsToArrange.Count == 0)
        {
            Debug.LogWarning("No objects to arrange.");
            return;
        }

        float totalWidth = (objectsToArrange.Count - 1) * gapSize;
        Vector3 startPosition = transform.position - new Vector3(totalWidth / 2f, 0f, 0f);

        for (int i = 0; i < objectsToArrange.Count; i++)
        {
            Vector3 newPosition = startPosition + new Vector3(i * gapSize, 0f, 0f);
            newPosition = new Vector3(newPosition.x, 0, 0);
            objectsToArrange[i].transform.localPosition = newPosition;
        }
    }

    private int Calculator(List<Card> tmpList)
    {

        tmpList = tmpList.OrderByDescending(x => x.number).ToList();

        int asCount = 0;

        //as as sorunu çözülcek

        foreach (var item in tmpList)
        {
            if (item.number == 1)
            {
                asCount++;
            }

        }

        int toplam = 0;

        foreach (var item in tmpList)
        {
            if (item.number > 1 && item.number < 10)
            {
                toplam += item.number;
            }
            else if (item.number >= 10)
            {
                toplam += 10;
            }


        }


        switch (asCount)
        {
            case 1:

                if (toplam + 11 <= 21)
                {
                    toplam += 11;
                }
                else
                {
                    toplam += 1;
                }

                break;

            case 2:

                if (toplam + 12 <= 21)
                {
                    toplam += 12;
                }
                else
                {
                    toplam += 2;
                }

                break;

            case 3:

                if (toplam + 13 <= 21)
                {
                    toplam += 13;
                }
                else
                {
                    toplam += 3;
                }

                break;

            case 4:

                if (toplam + 14 <= 21)
                {
                    toplam += 14;
                }
                else
                {
                    toplam += 4;
                }

                break;

        }


        return toplam;

    }

    public void Stand()
    {

        switch (phase)
        {
            case 0:
                standPlayer();
                break;

            case 1:
                standSplit1();
                break;

            case 2:
                standSplit2();
                break;
        }

    }

    private void standPlayer()
    {

        if (!playerBusted)
        {
            dealerScore = Calculator(dealerCards);
            dealerScoreText.text = dealerScore.ToString();
            playerIsStand = true;
            dealerHitting = true;
            hitButton.interactable = false;
            standButton.interactable = false;
            splitButton.interactable = false;
            DDButton.interactable = false;
        }

    }

    private void standSplit1()
    {
        hp.ResetColors();
        hitButton.interactable = true;
        standButton.interactable = true;
        splitButton.interactable = false;
        DDButton.interactable = true;
        phase = 2;
        PhaseColors();
        hp.getHelp();
    }

    private void standSplit2()
    {
        if (!split2Busted)
        {
            dealerScore = Calculator(dealerCards);
            dealerScoreText.text = dealerScore.ToString();
            split2IsStand = true;
            dealerHitting = true;
            hitButton.interactable = false;
            standButton.interactable = false;
            splitButton.interactable = false;
            DDButton.interactable = false;
            hp.ResetColors();
        }


    }

    private void Update()
    {

        if (phase == 0)
        {
            GameStatus();
        }
        else if (phase == 1)
        {
            GameStatus1();
        }
        else if (phase == 2)
        {
            GameStatus2();
        }


    }

    private void GameStatus()
    {

        if (playerScore > 21)
        {
            playerBusted = true;
            gameEnd = true;
        }

        if (dealerScore > 21)
        {
            dealerBusted = true;
            gameEnd = true;
        }

        if (playerIsStand && dealerHitting && !gameEnd)
        {
            StartCoroutine(DealerHit(0.4f));
            dealerHitting = false;
        }

        if (gameEnd && !winRateCalculation)
        {
            hitButton.interactable = false;
            standButton.interactable = false;
            splitButton.interactable = false;
            DDButton.interactable = false;

            if (playerBusted)
            {
                playerScoreText.color = new Color32(255, 119, 94, 255); //red
                loseCounter++;
            }
            else if (!playerBusted && dealerBusted)
            {
                playerScoreText.color = new Color32(93, 255, 151, 255); //green
                winCounter++;
                receivedCounter += doubleDowned0 ? 4 : 2;
            }
            else if (!playerBusted && !dealerBusted)
            {
                if (playerScore > dealerScore)
                {
                    playerScoreText.color = new Color32(93, 255, 151, 255); //green
                    winCounter++;
                    receivedCounter += doubleDowned0 ? 4 : 2;
                }
                else if (dealerScore > playerScore)
                {
                    playerScoreText.color = new Color32(255, 119, 94, 255); //red
                    loseCounter++;
                }
                else if (dealerScore == playerScore)
                {
                    drawCounter++;
                    receivedCounter += doubleDowned0 ? 2 : 1;
                }
            }

            winRateCalculation = true;
            FadeOutButtons();
        }



    }

    private void GameStatus1()
    {
        if (splitScore1 > 21)
        {
            split1Busted = true;
            splitScoreText1.color = new Color32(255, 119, 94, 255); //red
            standSplit1();
        }
    }

    private void GameStatus2()
    {
        if (splitScore1 > 21)
        {
            split1Busted = true;
            splitScoreText1.color = new Color32(255, 119, 94, 255); //red
        }

        if (splitScore2 == 21)
        {
            Stand();
        }

        if (splitScore2 > 21)
        {
            split2Busted = true;
            splitScoreText2.color = new Color32(255, 119, 94, 255); //red

            if (splitScore1 > 21 && splitScore2 > 21)
            {
                dealerHitting = false;
                gameEnd = true;
            }
            else
            {
                dealerScore = Calculator(dealerCards);
                dealerScoreText.text = dealerScore.ToString();
                split2IsStand = true;
                dealerHitting = true;
                hitButton.interactable = false;
                standButton.interactable = false;
                splitButton.interactable = false;
                DDButton.interactable = false;
            }

        }

        if (dealerScore > 21)
        {
            dealerBusted = true;
            gameEnd = true;
        }

        if (split2IsStand && dealerHitting && !gameEnd)
        {
            if (dealerHitAnimationEnd)
            {
                StartCoroutine(DealerHit(0.4f));
                dealerHitting = false;
            }

        }

        if (gameEnd && !winRateCalculation)
        {
            phase = 3;
            PhaseColors();

            hitButton.interactable = false;
            standButton.interactable = false;
            splitButton.interactable = false;
            DDButton.interactable = false;

            if (split2Busted)
            {
                splitScoreText2.color = new Color32(255, 119, 94, 255); //red
                loseCounter++;
            }
            else if (!split2Busted && dealerBusted)
            {
                splitScoreText2.color = new Color32(93, 255, 151, 255); //green
                winCounter++;
                receivedCounter += doubleDowned2 ? 4 : 2;
            }
            else if (!split2Busted && !dealerBusted)
            {
                if (splitScore2 > dealerScore)
                {
                    splitScoreText2.color = new Color32(93, 255, 151, 255); //green
                    winCounter++;
                    receivedCounter += doubleDowned2 ? 4 : 2;
                }
                else if (dealerScore > splitScore2)
                {
                    splitScoreText2.color = new Color32(255, 119, 94, 255); //red
                    loseCounter++;
                }
                else if (dealerScore == splitScore2)
                {
                    splitScoreText2.color = new Color32(255, 255, 255, 255); //white
                    drawCounter++;
                    receivedCounter += doubleDowned2 ? 2 : 1;
                }
            }


            if (split1Busted)
            {
                splitScoreText1.color = new Color32(255, 119, 94, 255); //red
                loseCounter++;
            }
            else if (!split1Busted && dealerBusted)
            {
                splitScoreText1.color = new Color32(93, 255, 151, 255); //green
                winCounter++;
                receivedCounter += doubleDowned1 ? 4 : 2;
            }
            else if (!split1Busted && !dealerBusted)
            {
                if (splitScore1 > dealerScore)
                {
                    splitScoreText1.color = new Color32(93, 255, 151, 255); //green
                    winCounter++;
                    receivedCounter += doubleDowned1 ? 4 : 2;
                }
                else if (dealerScore > splitScore1)
                {
                    splitScoreText1.color = new Color32(255, 119, 94, 255); //red
                    loseCounter++;
                }
                else if (dealerScore == splitScore1)
                {
                    splitScoreText1.color = new Color32(255, 255, 255, 255); //white
                    drawCounter++;
                    receivedCounter += doubleDowned1 ? 2 : 1;
                }
            }



            winRateCalculation = true;
            FadeOutButtons();
        }



    }

    public void FadeInButtons()
    {
        float fadeTime = 0.4f;

        hitButton.gameObject.SetActive(true);
        standButton.gameObject.SetActive(true);
        splitButton.gameObject.SetActive(true);
        DDButton.gameObject.SetActive(true);

        continueButton.GetComponentInChildren<TextMeshProUGUI>().DOFade(0f, fadeTime);
        continueButton.image.DOFade(0f, fadeTime).OnComplete(() =>
        {
            continueButton.gameObject.SetActive(false);

            hitButton.GetComponentInChildren<TextMeshProUGUI>().DOFade(1f, fadeTime);
            hitButton.image.DOFade(1f, fadeTime);

            standButton.GetComponentInChildren<TextMeshProUGUI>().DOFade(1f, fadeTime);
            standButton.image.DOFade(1f, fadeTime);

            splitButton.GetComponentInChildren<TextMeshProUGUI>().DOFade(1f, fadeTime);
            splitButton.image.DOFade(1f, fadeTime);

            DDButton.GetComponentInChildren<TextMeshProUGUI>().DOFade(1f, fadeTime);
            DDButton.image.DOFade(1f, fadeTime);

        });

    }

    public void FadeOutButtons()
    {
        continueButton.gameObject.SetActive(true);

        float fadeTime = 0.4f;

        hitButton.GetComponentInChildren<TextMeshProUGUI>().DOFade(0f, fadeTime);
        hitButton.image.DOFade(0f, fadeTime).OnComplete(() =>
        {
            hitButton.gameObject.SetActive(false);
        });

        standButton.GetComponentInChildren<TextMeshProUGUI>().DOFade(0f, fadeTime);
        standButton.image.DOFade(0f, fadeTime).OnComplete(() =>
        {
            standButton.gameObject.SetActive(false);
        });

        splitButton.GetComponentInChildren<TextMeshProUGUI>().DOFade(0f, fadeTime);
        splitButton.image.DOFade(0f, fadeTime).OnComplete(() =>
        {
            splitButton.gameObject.SetActive(false);
        });


        DDButton.GetComponentInChildren<TextMeshProUGUI>().DOFade(0f, fadeTime);
        DDButton.image.DOFade(0f, fadeTime).OnComplete(() =>
        {
            DDButton.gameObject.SetActive(false);


            continueButton.GetComponentInChildren<TextMeshProUGUI>().DOFade(1f, fadeTime);
            continueButton.image.DOFade(1f, fadeTime).OnComplete(() =>
            {
                continueButton.interactable = true;
            });
        });


    }

    private IEnumerator DealerHit(float waitTime)
    {

        WaitForSeconds wait = new WaitForSeconds(waitTime);

        dealerCardsGO[1].GetComponent<CardScript>().GetCardSprite(dealerCards[1]);
        yield return wait;

        if (dealerScore < 17 && !gameEnd && dealerHitAnimationEnd)
        {
            do
            {
                HitDealer();
                yield return wait;
            }
            while (dealerScore < 17);

        }

        if (dealerScore >= 17)
        {
            gameEnd = true;
        }


    }

    private void winRateCounter()
    {
        //total hand

        totalHand = winCounter + loseCounter + drawCounter;
        totalHandText.text = $"{totalHand}";


        //winrate

        if (winCounter + loseCounter > 0)
        {
            winRate = ((float)winCounter / (winCounter + loseCounter)) * 100;
        }

        if (winRate % 1 == 0)
        {
            winRateText.text = $"{winRate:F0}%";
        }
        else
        {
            winRateText.text = $"{winRate:F1}%";
        }


        //diff
        diffCounter = receivedCounter - givenCounter;

        receivedText.text = $"{receivedCounter}";
        givenText.text = $"{givenCounter}";
        diffText.text = $"{diffCounter}";

        if (diffCounter == 0)
        {
            diffText.color = new Color32(255, 255, 255, 255); //white
        }
        else if (diffCounter > 0)
        {
            diffText.color = new Color32(93, 255, 151, 255); //green
        }
        else if (diffCounter < 0)
        {
            diffText.color = new Color32(255, 119, 94, 255); //red
        }

        HighScoreChecker();

    }

    private void HighScoreStarter()
    {
        if (!PlayerPrefs.HasKey("totalHands25"))
        {
            PlayerPrefs.SetInt("totalHands25", 0);
            PlayerPrefs.SetFloat("winRate25", 0);
            PlayerPrefs.SetInt("given25", 0);
            PlayerPrefs.SetInt("received25", 0);
            PlayerPrefs.SetFloat("diff25", -99);

            PlayerPrefs.SetInt("totalHands50", 0);
            PlayerPrefs.SetFloat("winRate50", 0);
            PlayerPrefs.SetInt("given50", 0);
            PlayerPrefs.SetInt("received50", 0);
            PlayerPrefs.SetFloat("diff50", -99);

            PlayerPrefs.SetInt("totalHands100", 0);
            PlayerPrefs.SetFloat("winRate100", 0);
            PlayerPrefs.SetInt("given100", 0);
            PlayerPrefs.SetInt("received100", 0);
            PlayerPrefs.SetFloat("diff100", -99);
        }
    }

    private void HighScoreChecker()
    {

        if (totalHand == 25)
        {
            if (totalHand > PlayerPrefs.GetInt("totalHands25"))
                PlayerPrefs.SetInt("totalHands25", totalHand);

            if (winRate > PlayerPrefs.GetFloat("winRate25"))
                PlayerPrefs.SetFloat("winRate25", winRate);

            if (diffCounter > PlayerPrefs.GetFloat("diff25"))
            {
                PlayerPrefs.SetInt("given25", givenCounter);
                PlayerPrefs.SetFloat("diff25", diffCounter);
                PlayerPrefs.SetInt("received25", receivedCounter);
            }

        }

        if (totalHand == 50)
        {
            if (totalHand > PlayerPrefs.GetInt("totalHands50"))
                PlayerPrefs.SetInt("totalHands50", totalHand);

            if (winRate > PlayerPrefs.GetFloat("winRate50"))
                PlayerPrefs.SetFloat("winRate50", winRate);

            if (diffCounter > PlayerPrefs.GetFloat("diff50"))
            {
                PlayerPrefs.SetInt("given50", givenCounter);
                PlayerPrefs.SetFloat("diff50", diffCounter);
                PlayerPrefs.SetInt("received50", receivedCounter);
            }

        }

        if (totalHand == 100)
        {
            if (totalHand > PlayerPrefs.GetInt("totalHands100"))
                PlayerPrefs.SetInt("totalHands100", totalHand);

            if (winRate > PlayerPrefs.GetFloat("winRate100"))
                PlayerPrefs.SetFloat("winRate100", winRate);

            if (diffCounter > PlayerPrefs.GetFloat("diff100"))
            {
                PlayerPrefs.SetInt("received100", receivedCounter);
                PlayerPrefs.SetFloat("diff100", diffCounter);
                PlayerPrefs.SetInt("given100", givenCounter);
            }

        }



    }

    public void openHighScores()
    {
        highScoreState = 25;
        highScoreArea.SetActive(true);
        highScoreHeaderText.text = "High Scores - 25 Hands";

        highScoreHandsText.text = $"{PlayerPrefs.GetInt("totalHands25")}";

        float winRateTmp = PlayerPrefs.GetFloat("winRate25");
        if (winRateTmp % 1 == 0)
        {
            highScoreWinRateText.text = $"{winRateTmp:F0}%";
        }
        else
        {
            highScoreWinRateText.text = $"{winRateTmp:F1}%";
        }

        highScoreGivenText.text = $"{PlayerPrefs.GetInt("given25")}";
        highScoreReceivedText.text = $"{PlayerPrefs.GetInt("received25")}";
        highScoreDiffText.text = $"{PlayerPrefs.GetFloat("diff25")}";

        highScoreHeaderText.color = pinkColor;
        highScoreHandsText.color = pinkColor;
        highScoreWinRateText.color = pinkColor;
        highScoreGivenText.color = pinkColor;
        highScoreReceivedText.color = pinkColor;
        highScoreDiffText.color = pinkColor;


    }

    public void nextHighScore()
    {
        if (highScoreState == 25)
        {
            highScoreState = 50;
            highScoreArea.SetActive(true);
            highScoreHeaderText.text = "High Scores - 50 Hands";

            highScoreHandsText.text = $"{PlayerPrefs.GetInt("totalHands50")}";

            float winRateTmp = PlayerPrefs.GetFloat("winRate50");
            if (winRateTmp % 1 == 0)
            {
                highScoreWinRateText.text = $"{winRateTmp:F0}%";
            }
            else
            {
                highScoreWinRateText.text = $"{winRateTmp:F1}%";
            }

            highScoreGivenText.text = $"{PlayerPrefs.GetInt("given50")}";
            highScoreReceivedText.text = $"{PlayerPrefs.GetInt("received50")}";
            highScoreDiffText.text = $"{PlayerPrefs.GetFloat("diff50")}";

            highScoreHeaderText.color = cyanColor;
            highScoreHandsText.color = cyanColor;
            highScoreWinRateText.color = cyanColor;
            highScoreGivenText.color = cyanColor;
            highScoreReceivedText.color = cyanColor;
            highScoreDiffText.color = cyanColor;
        }
        else if (highScoreState == 50)
        {
            highScoreState = 100;
            highScoreArea.SetActive(true);
            highScoreHeaderText.text = "High Scores - 100 Hands";

            highScoreHandsText.text = $"{PlayerPrefs.GetInt("totalHands100")}";

            float winRateTmp = PlayerPrefs.GetFloat("winRate100");
            if (winRateTmp % 1 == 0)
            {
                highScoreWinRateText.text = $"{winRateTmp:F0}%";
            }
            else
            {
                highScoreWinRateText.text = $"{winRateTmp:F1}%";
            }

            highScoreGivenText.text = $"{PlayerPrefs.GetInt("given100")}";
            highScoreReceivedText.text = $"{PlayerPrefs.GetInt("received100")}";
            highScoreDiffText.text = $"{PlayerPrefs.GetFloat("diff100")}";


            highScoreHeaderText.color = yellowColor;
            highScoreHandsText.color = yellowColor;
            highScoreWinRateText.color = yellowColor;
            highScoreGivenText.color = yellowColor;
            highScoreReceivedText.color = yellowColor;
            highScoreDiffText.color = yellowColor;

        }
        else if (highScoreState == 100)
        {
            highScoreState = 0;
            highScoreArea.SetActive(false);
        }

    }

    public void Split()
    {
        //status
        givenCounter++;
        winRateCounter();

        hp.isSplitted = true;
        hp.ResetColors();
        hp.helperDone = true;

        hitButton.interactable = false;
        standButton.interactable = false;
        splitButton.interactable = false;
        DDButton.interactable = false;

        splitCardsGO1[0].SetActive(false);
        splitCardsGO2[0].SetActive(false);
        splitCardsGO1[1].SetActive(false);
        splitCardsGO2[1].SetActive(false);

        splitCards1.Clear();
        splitCards2.Clear();

        PlayerCardArea.SetActive(false);
        SplitCardArea.SetActive(true);

        splitCards1.Add(playerCards[0]);

        splitCards2.Add(playerCards[1]);

        splitCardsGO1[0].GetComponent<CardScript>().GetCardSprite(playerCards[0]);
        splitCardsGO2[0].GetComponent<CardScript>().GetCardSprite(playerCards[1]);

        Vector3 tmpTargetPos1 = splitCardsGO1[0].transform.position;
        Vector3 tmpTargetPos2 = splitCardsGO2[0].transform.position;

        splitCardsGO1[0].transform.position = playerCardsGO[0].transform.position;
        splitCardsGO2[0].transform.position = playerCardsGO[1].transform.position;


        splitCardsGO1[0].SetActive(true);
        splitCardsGO2[0].SetActive(true);

        splitCardsGO1[0].transform.DOMove(tmpTargetPos1, 0.3f).SetEase(Ease.InCubic);

        splitCardsGO2[0].transform.DOMove(tmpTargetPos2, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {

            GiveCardsToSplittedCards();

        });

    }

    private void HitSplit1()
    {

        hp.isHit1 = true;
        hp.ResetColors();

        hitButton.interactable = false;
        standButton.interactable = false;
        splitButton.interactable = false;
        DDButton.interactable = false;


        if (gapSizeForSplit1 > 80)
        {
            gapSizeForSplit1 -= 40;
        }

        card = deck.DrawCard();
        splitCards1.Add(card);

        var newCard = Instantiate(cardPrefab);
        splitCardsGO1.Add(newCard);

        newCard.GetComponent<CardScript>().GetCardBack();

        newCard.transform.parent = splitCardsGO1[0].transform.parent;
        newCard.transform.position = splitCardsGO1[0].transform.position;
        newCard.transform.localScale = splitCardsGO1[0].transform.localScale;
        newCard.GetComponent<SpriteRenderer>().sortingOrder = splitCardsGO1[splitCardsGO1.Count - 2].GetComponent<SpriteRenderer>().sortingOrder + 1;
        ArrangeObjects(splitCardsGO1, gapSizeForSplit1);
        Vector3 targetPos = splitCardsGO1[splitCardsGO1.Count - 1].transform.localPosition;
        newCard.transform.localPosition = new Vector3(1000, 0, 0);
        newCard.transform.DOLocalMove(targetPos, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            StartCoroutine(FlipCard(newCard, 0.1f, card));
            splitScore1 = Calculator(splitCards1);
            splitScoreText1.text = splitScore1.ToString();

            hitButton.interactable = true;
            standButton.interactable = true;
            if (splitScore1 == 21)
            {
                standSplit1();
            }
            else
            {
                hp.getHelp();
            }
        });


    }

    private void HitSplit2()
    {
        hp.isHit2 = true;
        hp.ResetColors();

        hitButton.interactable = false;
        standButton.interactable = false;
        splitButton.interactable = false;
        DDButton.interactable = false;


        if (gapSizeForSplit2 > 80)
        {
            gapSizeForSplit2 -= 40;
        }

        card = deck.DrawCard();
        splitCards2.Add(card);

        var newCard = Instantiate(cardPrefab);
        splitCardsGO2.Add(newCard);

        newCard.GetComponent<CardScript>().GetCardBack();

        newCard.transform.parent = splitCardsGO2[0].transform.parent;
        newCard.transform.position = splitCardsGO2[0].transform.position;
        newCard.transform.localScale = splitCardsGO2[0].transform.localScale;
        newCard.GetComponent<SpriteRenderer>().sortingOrder = splitCardsGO2[splitCardsGO2.Count - 2].GetComponent<SpriteRenderer>().sortingOrder + 1;
        ArrangeObjects(splitCardsGO2, gapSizeForSplit2);
        Vector3 targetPos = splitCardsGO2[splitCardsGO2.Count - 1].transform.localPosition;
        newCard.transform.localPosition = new Vector3(1000, 0, 0);
        newCard.transform.DOLocalMove(targetPos, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            StartCoroutine(FlipCard(newCard, 0.1f, card));
            splitScore2 = Calculator(splitCards2);
            splitScoreText2.text = splitScore2.ToString();

            hitButton.interactable = true;
            standButton.interactable = true;
            hp.getHelp();

        }); ;


    }

    private void GiveCardsToSplittedCards()
    {

        if (gapSizeForPlayer > 80)
        {
            gapSizeForPlayer -= 40;
        }

        card = deck.DrawCard();
        splitCards1.Add(card);
        splitCardsGO1[1].GetComponent<CardScript>().GetCardBack();
        Vector3 tmpTargetPos3 = splitCardsGO1[1].transform.localPosition;


        splitCardsGO1[1].transform.localPosition = new Vector3(1000, 0, 0);
        splitCardsGO1[1].SetActive(true);
        splitCardsGO1[1].transform.DOLocalMove(tmpTargetPos3, 0.2f).OnComplete(() =>
        {
            StartCoroutine(FlipCard(splitCardsGO1[1], 0.1f, card));
            splitScore1 = Calculator(splitCards1);
            splitScoreText1.text = splitScore1.ToString();

            card = deck.DrawCard();
            splitCards2.Add(card);
            splitCardsGO2[1].GetComponent<CardScript>().GetCardBack();
            Vector3 tmpTargetPos4 = splitCardsGO2[1].transform.localPosition;
            splitCardsGO2[1].SetActive(true);
            splitCardsGO2[1].transform.DOLocalMove(tmpTargetPos4, 0.2f).OnComplete(() =>
            {
                StartCoroutine(FlipCard(splitCardsGO2[1], 0.1f, card));

                splitScore2 = Calculator(splitCards2);
                splitScoreText2.text = splitScore2.ToString();

                phase = 1;
                PhaseColors();
                PlaySplits();
                if (splitScore1 == 21)
                {
                    standSplit1();
                }
                else
                {
                    hp.getHelp();
                }


            });

        });

    }

    private void PlaySplits()
    {
        hitButton.interactable = true;
        standButton.interactable = true;
        DDButton.interactable = true;
    }

    private void PhaseColors()
    {
        if (phase == 1)
        {
            foreach (var item in splitCardsGO1)
            {
                item.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            }
            splitScoreText1.color = new Color32(255, 255, 255, 255);

            foreach (var item in splitCardsGO2)
            {
                item.GetComponent<SpriteRenderer>().color = new Color32(160, 160, 160, 255);
            }
            splitScoreText2.color = new Color32(160, 160, 160, 255);
        }
        else if (phase == 2)
        {
            foreach (var item in splitCardsGO2)
            {
                item.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            }
            splitScoreText2.color = new Color32(255, 255, 255, 255);

            foreach (var item in splitCardsGO1)
            {
                item.GetComponent<SpriteRenderer>().color = new Color32(160, 160, 160, 255);
            }
            splitScoreText1.color = new Color32(160, 160, 160, 255);
        }

        else if (phase == 3)
        {
            foreach (var item in splitCardsGO2)
            {
                item.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            }

            foreach (var item in splitCardsGO1)
            {
                item.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            }
        }

    }


}
