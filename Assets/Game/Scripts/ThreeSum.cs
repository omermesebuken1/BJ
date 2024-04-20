using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Linq;

public class ThreeSum : MonoBehaviour
{   
    [SerializeField] private GameManager gm;
    [SerializeField] private MainCards mainCards;
    [SerializeField] private List<GameObject> opponentCardsGO = new List<GameObject>();
    [SerializeField] private List<GameObject> playerCardsGO = new List<GameObject>();

    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI opponentScoreText;

    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject redealButton;
    [SerializeField] private GameObject againButton;

    private List<Card> opponentCards = new List<Card>();
    private List<Card> playerCards = new List<Card>();

    private Deck deck;
    private Card card;
    private bool gameEnd;

    private int scoreOfPlayer;
    private int scoreOfOpponent;
    private int winner;

    [SerializeField] private float waitTimeForCards;

    private void GameStatus()
    {
        if (gameEnd)
        {
            if (scoreOfPlayer == scoreOfOpponent)
            {
                againButton.SetActive(true);
                startButton.SetActive(false);

            }
            else if (scoreOfPlayer > scoreOfOpponent)
            {
                continueButton.SetActive(true);
                startButton.SetActive(false);

            }
            else if (scoreOfPlayer < scoreOfOpponent)
            {
                redealButton.SetActive(true);
                startButton.SetActive(false);

            }
        }


    }

    public void PlayAgain()
    {
        gm.Vibrate("soft");
        ResetThreeSum();
    }

    public void ResetThreeSum()
    {

        playerCardsGO[0].GetComponent<CardScript>().GetCardSprite(mainCards.theFiveCard[0]);
        playerCardsGO[1].GetComponent<CardScript>().GetCardSprite(mainCards.theFiveCard[1]);
        playerCardsGO[2].GetComponent<CardScript>().GetCardSprite(mainCards.theFiveCard[2]);

        gameEnd = false;

        startButton.SetActive(true);
        againButton.SetActive(false);
        continueButton.SetActive(false);
        redealButton.SetActive(false);
        
        scoreOfPlayer = 0;
        scoreOfOpponent = 0;

        playerScoreText.text = "";
        opponentScoreText.text = "";


        ResetToCardBacks();

    }

    private void ResetToCardBacks()
    {
        foreach (var item in opponentCardsGO)
        {
            item.GetComponent<CardScript>().GetCardBack();
        }

    }

    public void StartThreeSum()
    {
        startButton.SetActive(false);
        gm.Vibrate("soft");
        prepareCards();

        scoreOfPlayer = GetScore(1);

        scoreOfOpponent = GetScore(2);

        StartCoroutine(ShowOtherCards(waitTimeForCards));

    }

    private int GetScore(int whichPlayer)
    {
        int score = 0;

        switch (whichPlayer)
        {
            case 1:
                
                foreach (var item in playerCards)
                {
                    if(item.number >= 10)
                    {
                        score += 10;
                    }
                    else
                    {
                        score += item.number;
                    }
                }
                
            break;

            case 2:

            foreach (var item in opponentCards)
                {
                    if(item.number >= 10)
                    {
                        score += 10;
                    }
                    else
                    {
                        score += item.number;
                    }
                }

            break;
        }

        return score;
    }

    private void prepareCards()
    {
        opponentCards.Clear();
        playerCards.Clear();

        //creating a new deck
        deck = new Deck();

        //removing players card from deck
        deck.cards.Remove(deck.cards.Find(x => x.number == mainCards.GetComponent<MainCards>().theFiveCard[0].number && x.suit == mainCards.GetComponent<MainCards>().theFiveCard[0].suit));
        deck.cards.Remove(deck.cards.Find(x => x.number == mainCards.GetComponent<MainCards>().theFiveCard[1].number && x.suit == mainCards.GetComponent<MainCards>().theFiveCard[1].suit));
        deck.cards.Remove(deck.cards.Find(x => x.number == mainCards.GetComponent<MainCards>().theFiveCard[2].number && x.suit == mainCards.GetComponent<MainCards>().theFiveCard[2].suit));

        //adding players cards to evaluate list

        card = mainCards.GetComponent<MainCards>().theFiveCard[0];
        playerCards.Add(card);

        card = mainCards.GetComponent<MainCards>().theFiveCard[1];
        playerCards.Add(card);

        card = mainCards.GetComponent<MainCards>().theFiveCard[2];
        playerCards.Add(card);


        //adding opponent cards to evaluate list
        card = deck.DrawCard();
        opponentCards.Add(card);

        card = deck.DrawCard();
        opponentCards.Add(card);

        card = deck.DrawCard();
        opponentCards.Add(card);

    }

    private IEnumerator ShowOtherCards(float waitTime)
    {
        WaitForSeconds wait = new WaitForSeconds(waitTime);

        playerScoreText.text = scoreOfPlayer.ToString();

        for (int i = 0; i < opponentCards.Count; i++)
        {
            gm.CardSound();
            opponentCardsGO[i].transform.DOLocalRotate(new Vector3(0, 90, 0), 0.1f);
            yield return wait;

            opponentCardsGO[i].GetComponent<CardScript>().GetCardSprite(opponentCards[i]);

            opponentCardsGO[i].transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f);
            yield return wait;

        }

        opponentScoreText.text = scoreOfOpponent.ToString();

        gameEnd = true;

        GameStatus();

    }



}
