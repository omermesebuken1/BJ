using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Linq;

public class VideoPoker : MonoBehaviour
{
    [SerializeField] private GameManager gm;

    [SerializeField] private MainCards mainCards;

    [SerializeField] private List<GameObject> playerCardsGO = new List<GameObject>();
    [SerializeField] private List<GameObject> finalCardsGO = new List<GameObject>();
    [SerializeField] private List<GameObject> buttons = new List<GameObject>();


    [SerializeField] private TextMeshProUGUI playerScoreText;


    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject redealButton;

    [SerializeField] private Sprite buttonOK;
    [SerializeField] private Sprite buttonNOK;

    private List<Card> finalCards = new List<Card>();

    private List<int> evaluateList1 = new List<int>(); //for player

    private List<int> bestHandForPlayer = new List<int>(); //for opponent
    private List<Card> bestHandForPlayerCard = new List<Card>(); //for opponent

    [SerializeField] private List<bool> gonnaChange = new List<bool>(); //for opponent

    private Deck deck;
    private Card card;
    private bool gameEnd;
    private List<List<int>> combinations;

    private int scoreOfPlayer;

    [SerializeField] private float waitTimeForCards;

    

    private void ResetButtons()
    {
        for (int i = 0; i < 5; i++)
        {
            gonnaChange[i] = false;
            buttons[i].GetComponent<Image>().sprite = buttonNOK;
        }

    }

    private void buttonActiveStatus(bool status)
    {
        foreach (var item in buttons)
        {
            item.SetActive(status);
        }
    }

    public void SelectButton(int buttonNo)
    {
        gm.Vibrate("soft");
        if(buttons[buttonNo].GetComponent<Image>().sprite == buttonOK)
        {
            gonnaChange[buttonNo] = false;
            buttons[buttonNo].GetComponent<Image>().sprite = buttonNOK;
        }
        else if(buttons[buttonNo].GetComponent<Image>().sprite == buttonNOK)
        {
            gonnaChange[buttonNo] = true;
            buttons[buttonNo].GetComponent<Image>().sprite = buttonOK;
        }
    
    }

    private void GameStatus()
    {

        if (gameEnd)
        {
            if (scoreOfPlayer >= 2000)
            {
                continueButton.SetActive(true);
                startButton.SetActive(false);
                UpdateBestHands(scoreOfPlayer, bestHandForPlayer[0], bestHandForPlayer[1], bestHandForPlayer[2], bestHandForPlayer[3], bestHandForPlayer[4]);
                
            }

            else
            {
                redealButton.SetActive(true);
                startButton.SetActive(false);

            }
        }



    }

    public void ResetVideoPoker()
    {

        playerCardsGO[0].GetComponent<CardScript>().GetCardSprite(mainCards.theFiveCard[0]);
        playerCardsGO[1].GetComponent<CardScript>().GetCardSprite(mainCards.theFiveCard[1]);
        playerCardsGO[2].GetComponent<CardScript>().GetCardSprite(mainCards.theFiveCard[2]);
        playerCardsGO[3].GetComponent<CardScript>().GetCardSprite(mainCards.theFiveCard[3]);
        playerCardsGO[4].GetComponent<CardScript>().GetCardSprite(mainCards.theFiveCard[4]);
        
        gameEnd = false;

        startButton.SetActive(true);
        continueButton.SetActive(false);
        redealButton.SetActive(false);

        buttonActiveStatus(true);

        playerScoreText.text = "";

        ResetButtons();
        ResetToCardBacks();

    }

    private void ResetToCardBacks()
    {

        foreach (var item in finalCardsGO)
        {
            item.GetComponent<CardScript>().GetCardBack();
        }

    }

    public void StartVideoPoker()
    {
        startButton.SetActive(false);
        gm.Vibrate("soft");
        buttonActiveStatus(false);

        prepareCards();

        scoreOfPlayer = GetMaxScore(GenerateCombinations(evaluateList1, 5));

        StartCoroutine(ShowOtherCards(waitTimeForCards));

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

    private void prepareCards()
    {

        finalCards.Clear();
        evaluateList1.Clear();

        //creating a new deck
        deck = new Deck();

        //removing players card from deck
        deck.cards.Remove(deck.cards.Find(x => x.number == mainCards.GetComponent<MainCards>().theFiveCard[0].number && x.suit == mainCards.GetComponent<MainCards>().theFiveCard[0].suit));
        deck.cards.Remove(deck.cards.Find(x => x.number == mainCards.GetComponent<MainCards>().theFiveCard[1].number && x.suit == mainCards.GetComponent<MainCards>().theFiveCard[1].suit));
        deck.cards.Remove(deck.cards.Find(x => x.number == mainCards.GetComponent<MainCards>().theFiveCard[2].number && x.suit == mainCards.GetComponent<MainCards>().theFiveCard[2].suit));
        deck.cards.Remove(deck.cards.Find(x => x.number == mainCards.GetComponent<MainCards>().theFiveCard[3].number && x.suit == mainCards.GetComponent<MainCards>().theFiveCard[3].suit));
        deck.cards.Remove(deck.cards.Find(x => x.number == mainCards.GetComponent<MainCards>().theFiveCard[4].number && x.suit == mainCards.GetComponent<MainCards>().theFiveCard[4].suit));


        //adding players cards to evaluate list

        for (int i = 0; i < 5; i++)
        {
            if(gonnaChange[i])
            {
                card = deck.DrawCard();
                finalCards.Add(card);
                evaluateList1.Add(CardIntValue(card));
            }
            else
            {
                card = mainCards.GetComponent<MainCards>().theFiveCard[i];
                finalCards.Add(card);
                evaluateList1.Add(CardIntValue(card));
            }
            
        }

        evaluateList1 = evaluateList1.OrderByDescending(x => x % 100).ToList();

    }

    private int CardIntValue(Card card)
    {
        int value = 0;

        switch (card.suit)
        {
            case "Spades":
                value += 100;
                break;
            case "Hearts":
                value += 200;
                break;
            case "Clubs":
                value += 300;
                break;
            case "Diamonds":
                value += 400;
                break;
        }

        switch (card.number)
        {
            case 1:
                value += 12;
                break;
            case 13:
                value += 11;
                break;
            case 12:
                value += 10;
                break;
            case 11:
                value += 9;
                break;
            case 10:
                value += 8;
                break;
            case 9:
                value += 7;
                break;
            case 8:
                value += 6;
                break;
            case 7:
                value += 5;
                break;
            case 6:
                value += 4;
                break;
            case 5:
                value += 3;
                break;
            case 4:
                value += 2;
                break;
            case 3:
                value += 1;
                break;
            case 2:
                value += 0;
                break;

        }

        return value;

    }

    private int GetMaxScore(List<List<int>> tmpListlist)
    {

        int score = 0;
        int maxScore = 0;
        List<int> tmplist = new List<int>();

        for (int i = 0; i < tmpListlist.Count; i++)
        {
            tmplist = tmpListlist[i];

            tmplist = tmplist.OrderByDescending(x => x % 100).ToList();

            score = ScoreHand(tmplist.ToArray());

            if (score > maxScore)
            {
                maxScore = score;

                bestHandForPlayer.Clear();
                bestHandForPlayer = tmplist;

            }


        }

        if (maxScore < 100)
        {

            foreach (var item in evaluateList1)
            {
                int modScore = 0;
                modScore = item % 100;

                if (modScore > maxScore)
                    maxScore = modScore;
            }

            bestHandForPlayer.Clear();

            evaluateList1.OrderByDescending(x => x % 100).ToList();

            for (int i = 0; i < 5; i++)
            {
                bestHandForPlayer.Add(evaluateList1[i]);
            }


        }

        return maxScore;
    }

    public static int ScoreHand(int[] hand)
    {
        return ScoreSeries(hand) + ScoreKinds(hand);
    }

    public static int ScoreStraight(int[] hand)
    {

        if (hand[0] % 100 == 12 && hand[1] % 100 == 3 && hand[2] % 100 == 2 && hand[3] % 100 == 1 && hand[4] % 100 == 0)
        {
            return 4000;
        }

        for (int i = 1; i < hand.Length; i++)
        {
            if ((hand[i] % 100 + 1) != (hand[i - 1] % 100))
            {
                return 0;
            }

        }

        return 4000;
    }

    public static int ScoreFlush(int[] hand)
    {
        for (int i = 1; i < hand.Length; i++)
        {
            if ((hand[i] / 100) != (hand[i - 1] / 100))
            {
                return 0;
            }

        }

        return 5000;
    }

    public static int ScoreSeries(int[] hand)
    {
        int score = ScoreFlush(hand) + ScoreStraight(hand);
        if (hand[0] % 100 == 12 && score >= 9000)
            return 10000;
        if (score > 0)
            return score;

        return 0;
    }

    public static int ScoreKinds(int[] hand)
    {
        int[] counts = new int[13];
        int[] highCards = new int[2];
        int max = 1;
        int twoSets = 0;

        for (int i = 0; i < hand.Length; i++)
        {
            counts[hand[i] % 100]++;
            if (max > 1 && counts[hand[i] % 100] == 2)
            {
                twoSets = 1;
                highCards[1] = hand[i] % 100;
            }
            if (max < counts[hand[i] % 100])
            {
                max = counts[hand[i] % 100];
                highCards[0] = hand[i] % 100;
            }
        }

        if (max == 1)
            return 0;

        int score = (max * 2 + twoSets) * 1000;

        if (score < 7000)
        {
            score -= 3000;

            if (max == 2 && twoSets == 1)
            {
                if (highCards[1] > highCards[0])
                    Swap(highCards, 0, 1);

                score += 26 * highCards[0] + 13 * highCards[1];

                for (int i = 0; i < hand.Length; i++)
                {

                    if (hand[i] % 100 != highCards[0] && hand[i] % 100 != highCards[1])
                    {
                        score += hand[i] % 100;
                    }

                }

            }
            else
            {
                if (counts[highCards[1]] > counts[highCards[0]])
                    Swap(highCards, 0, 1);

                score += 50 * highCards[0];

                for (int i = 0; i < hand.Length; i++)
                {

                    if (hand[i] % 100 != highCards[0])
                    {
                        score += (hand[i] % 100);
                    }

                }
            }

        }
        else if (score == 7000)
        {
            if (highCards[1] > highCards[0])
                Swap(highCards, 0, 1);
            score += 13 * highCards[0] + highCards[1];


        }

        else if (score == 8000)
        {
            score += 13 * highCards[0];

            for (int i = 0; i < hand.Length; i++)
            {

                if (hand[i] % 100 != highCards[0])
                {
                    score += hand[i] % 100;
                }

            }

        }





        return score;
    }

    private static void Swap(int[] array, int index1, int index2)
    {
        int temp = array[index1];
        array[index1] = array[index2];
        array[index2] = temp;
    }

    List<List<int>> GenerateCombinations(List<int> elements, int k)
    {
        List<List<int>> result = new List<List<int>>();
        GenerateCombinationsRecursive(elements, k, 0, new List<int>(), result);
        return result;
    }

    void GenerateCombinationsRecursive(List<int> elements, int k, int start, List<int> currentCombination, List<List<int>> result)
    {
        if (k == 0)
        {
            result.Add(new List<int>(currentCombination));
            return;
        }

        for (int i = start; i <= elements.Count - k; i++)
        {
            currentCombination.Add(elements[i]);
            GenerateCombinationsRecursive(elements, k - 1, i + 1, currentCombination, result);
            currentCombination.RemoveAt(currentCombination.Count - 1);
        }
    }

    private IEnumerator ShowOtherCards(float waitTime)
    {
        WaitForSeconds wait = new WaitForSeconds(waitTime);


        for (int i = 0; i < finalCardsGO.Count; i++)
        {
            
            if(!gonnaChange[i])
            {
            gm.CardSound();
            finalCardsGO[i].transform.DOLocalRotate(new Vector3(0, 90, 0), 0.1f);
            yield return wait;

            finalCardsGO[i].GetComponent<CardScript>().GetCardSprite(finalCards[i]);

            finalCardsGO[i].transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f);
            yield return wait;

            }

        }

        wait = new WaitForSeconds(waitTime*5);
        yield return wait;
        wait = new WaitForSeconds(waitTime);

        for (int i = 0; i < finalCardsGO.Count; i++)
        {
            
            if(gonnaChange[i])
            {
            gm.CardSound();
            finalCardsGO[i].transform.DOLocalRotate(new Vector3(0, 90, 0), 0.1f);
            yield return wait;

            finalCardsGO[i].GetComponent<CardScript>().GetCardSprite(finalCards[i]);

            finalCardsGO[i].transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f);
            yield return wait;

            }
            

        }

        playerScoreText.text = ScoreToName(scoreOfPlayer);


        gameEnd = true;

        GameStatus();


    }

    private void UpdateBestHands(int score, int h0, int h1, int h2, int h3, int h4)
    {
        int tmpScore = PlayerPrefs.GetInt("BestVideoPokerScore");
        
        if(score > tmpScore)
        {
            PlayerPrefs.SetInt("BestVideoPokerScore",score);

            PlayerPrefs.SetInt("BestHandVideoPoker_0",h0);
            PlayerPrefs.SetInt("BestHandVideoPoker_1",h1);
            PlayerPrefs.SetInt("BestHandVideoPoker_2",h2);
            PlayerPrefs.SetInt("BestHandVideoPoker_3",h3);
            PlayerPrefs.SetInt("BestHandVideoPoker_4",h4);

        }

        
    }



}
