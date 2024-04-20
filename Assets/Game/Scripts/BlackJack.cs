using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Linq;


public class BlackJack : MonoBehaviour
{   
    [SerializeField] private GameManager gm;
    [SerializeField] private MainCards mainCards;
    [SerializeField] private List<GameObject> playerCardsGO = new List<GameObject>();
    [SerializeField] private List<GameObject> dealerCardsGO = new List<GameObject>();

    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI dealerScoreText;
    [SerializeField] private TextMeshProUGUI statusText;

    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject redealButton;
    [SerializeField] private GameObject againButton;
    [SerializeField] private GameObject hitButton;
    [SerializeField] private GameObject standButton;



    private List<Card> dealerCards = new List<Card>();
    private List<Card> playerCards = new List<Card>();


    private Deck deck;
    private Card card;

    private int playerScore;
    private int dealerScore;

    [SerializeField] private float gapSizeForPlayer = 240;
    [SerializeField] private float gapSizeForDealer = 240;

    private bool playerIsStand;
    private bool dealerHitting;

    private bool playerBusted;
    private bool dealerBusted;
    private bool gameEnd;

    

    public void ResetBlackJack()
    {
        continueButton.SetActive(false);
        redealButton.SetActive(false);
        againButton.SetActive(false);
        hitButton.SetActive(true);
        standButton.SetActive(true);

        statusText.text = "";
        dealerScoreText.text = "";
        playerScoreText.text = "";



        prepareCards();
    }

    public void prepareCards()
    {
        ResetCards();

        //creating a new deck
        deck = new Deck();

        //removing players card from deck
        deck.cards.Remove(deck.cards.Find(x => x.number == mainCards.GetComponent<MainCards>().theFiveCard[0].number && x.suit == mainCards.GetComponent<MainCards>().theFiveCard[0].suit));
        deck.cards.Remove(deck.cards.Find(x => x.number == mainCards.GetComponent<MainCards>().theFiveCard[1].number && x.suit == mainCards.GetComponent<MainCards>().theFiveCard[1].suit));

        //adding players cards
        card = mainCards.GetComponent<MainCards>().theFiveCard[0];
        playerCards.Add(card);

        card = mainCards.GetComponent<MainCards>().theFiveCard[1];
        playerCards.Add(card);

        //adding dealer cards
        card = deck.DrawCard();
        dealerCards.Add(card);

        card = deck.DrawCard();
        dealerCards.Add(card);

        StartShowCards();

        playerScore = Calculator(playerCards);
        playerScoreText.text = playerScore.ToString();

    }

    private void StartShowCards()
    {
        dealerCardsGO[0].GetComponent<CardScript>().GetCardSprite(dealerCards[0]);
        dealerCardsGO[1].GetComponent<CardScript>().GetCardBack();

        playerCardsGO[0].GetComponent<CardScript>().GetCardSprite(playerCards[0]);
        playerCardsGO[1].GetComponent<CardScript>().GetCardSprite(playerCards[1]);

    }

    public void HitPlayer()
    {   
        gm.Vibrate("soft");
        gm.CardSound();
        if (gapSizeForPlayer > 80)
        {
            gapSizeForPlayer -= 40;
        }

        card = deck.DrawCard();
        playerCards.Add(card);

        var newCard = Instantiate(mainCards.CardPrefab);
        playerCardsGO.Add(newCard);

        newCard.GetComponent<CardScript>().GetCardSprite(card);

        newCard.transform.parent = playerCardsGO[0].transform.parent;
        newCard.transform.position = playerCardsGO[0].transform.position;
        newCard.transform.localScale = playerCardsGO[0].transform.localScale;
        newCard.GetComponent<SpriteRenderer>().sortingOrder = playerCardsGO[playerCardsGO.Count - 2].GetComponent<SpriteRenderer>().sortingOrder + 1;
        ArrangeObjects(playerCardsGO, gapSizeForPlayer);
        Vector3 targetPos = playerCardsGO[playerCardsGO.Count - 1].transform.localPosition;
        newCard.transform.localPosition = new Vector3(1000, 0, 0);
        newCard.transform.DOLocalMove(targetPos, 0.2f);

        playerScore = Calculator(playerCards);
        playerScoreText.text = playerScore.ToString();
    }

    private void HitDealer()
    {

        if (gapSizeForDealer > 80)
        {
            gapSizeForDealer -= 40;
        }

        card = deck.DrawCard();
        dealerCards.Add(card);

        var newCard = Instantiate(mainCards.CardPrefab);
        dealerCardsGO.Add(newCard);

        newCard.GetComponent<CardScript>().GetCardSprite(card);

        newCard.transform.parent = dealerCardsGO[0].transform.parent;
        newCard.transform.position = dealerCardsGO[0].transform.position;
        newCard.transform.localScale = dealerCardsGO[0].transform.localScale;
        newCard.GetComponent<SpriteRenderer>().sortingOrder = dealerCardsGO[dealerCardsGO.Count - 2].GetComponent<SpriteRenderer>().sortingOrder + 1;
        ArrangeObjects(dealerCardsGO, gapSizeForDealer);
        Vector3 targetPos = dealerCardsGO[dealerCardsGO.Count - 1].transform.localPosition;
        newCard.transform.localPosition = new Vector3(1000, 0, 0);
        newCard.transform.DOLocalMove(targetPos, 0.2f);

        dealerScore = Calculator(dealerCards);
        dealerScoreText.text = dealerScore.ToString();


    }

    private void ResetCards()
    {
        playerScore = 0;
        dealerScore = 0;

        playerIsStand = false;
        dealerHitting = false;
        playerBusted = false;
        dealerBusted = false;
        gameEnd = false;

        gapSizeForPlayer = 240;
        gapSizeForDealer = 240;

        playerCards.Clear();
        dealerCards.Clear();

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

        ArrangeObjects(playerCardsGO, gapSizeForPlayer);
        ArrangeObjects(dealerCardsGO, gapSizeForDealer);

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
            if(item.number > 1 && item.number < 10)
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
        gm.Vibrate("soft");
        if (!playerBusted)
        {
            dealerScore = Calculator(dealerCards);
            dealerScoreText.text = dealerScore.ToString();
            playerIsStand = true;
            dealerHitting = true;
            hitButton.SetActive(false);
            standButton.SetActive(false);
        }


    }


    private void Update()
    {

        GameStatus();

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

        if (gameEnd)
        {
            if (playerBusted)
            {
                statusText.text = "Busted";
                redealButton.SetActive(true);
                hitButton.SetActive(false);
                standButton.SetActive(false);
            }
            else if (!playerBusted && dealerBusted)
            {
                statusText.text = "Player Wins";
                continueButton.SetActive(true);
                hitButton.SetActive(false);
                standButton.SetActive(false);
            }
            else if (!playerBusted && !dealerBusted)
            {
                if (playerScore > dealerScore)
                {
                    statusText.text = "Player Wins";
                    continueButton.SetActive(true);
                    hitButton.SetActive(false);
                    standButton.SetActive(false);

                }
                else if (dealerScore > playerScore)
                {
                    statusText.text = "Dealer Wins";
                    redealButton.SetActive(true);
                    hitButton.SetActive(false);
                    standButton.SetActive(false);

                }
                else if (dealerScore == playerScore)
                {
                    statusText.text = "Draw";
                    againButton.SetActive(true);
                    hitButton.SetActive(false);
                    standButton.SetActive(false);
                }


            }

        }



    }

    private IEnumerator DealerHit(float waitTime)
    {
        gm.CardSound();
        WaitForSeconds wait = new WaitForSeconds(waitTime);

        dealerCardsGO[1].GetComponent<CardScript>().GetCardSprite(dealerCards[1]);
        yield return wait;

        if (dealerScore < 17)
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




}
