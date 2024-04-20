using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Linq;


public class TexasHoldem : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private MainCards mainCards;
    [SerializeField] private List<GameObject> midCardsGO = new List<GameObject>();
    [SerializeField] private List<GameObject> opponentCardsGO = new List<GameObject>();
    [SerializeField] private List<GameObject> playerCardsGO = new List<GameObject>();

    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI opponentScoreText;

    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject redealButton;
    [SerializeField] private GameObject againButton;

    private List<Card> opponentCards = new List<Card>();
    private List<Card> midCards = new List<Card>();

    private List<int> evaluateList1 = new List<int>(); //for player
    private List<int> evaluateList2 = new List<int>(); //for opponent

    private List<int> bestHandForPlayer = new List<int>(); //for opponent
    private List<int> bestHandForOpponent = new List<int>(); //for opponent

    private List<Card> bestHandForPlayerCard = new List<Card>(); //for opponent
    private List<Card> bestHandForOpponentCard = new List<Card>(); //for opponent

    private Deck deck;
    private Card card;
    private bool gameEnd;
    private List<List<int>> combinations;
    private int opponentCardsCounter;
    private int midCardsCounter;

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
                winner = Kickers();
                switch (winner)
                {
                    case 1:
                    continueButton.SetActive(true);
                    startButton.SetActive(false);
                    UpdateBestHands(scoreOfPlayer, bestHandForPlayer[0], bestHandForPlayer[1], bestHandForPlayer[2], bestHandForPlayer[3], bestHandForPlayer[4]);
                    break;

                    case 0:
                    againButton.SetActive(true);
                    startButton.SetActive(false);
                    break;

                    case -1:
                    redealButton.SetActive(true);
                    startButton.SetActive(false);
                    break;
                }
            }
            else if (scoreOfPlayer > scoreOfOpponent)
            {
                continueButton.SetActive(true);
                startButton.SetActive(false);
                UpdateBestHands(scoreOfPlayer, bestHandForPlayer[0], bestHandForPlayer[1], bestHandForPlayer[2], bestHandForPlayer[3], bestHandForPlayer[4]);
                winner = 1;

            }
            else if (scoreOfPlayer < scoreOfOpponent)
            {
                redealButton.SetActive(true);
                startButton.SetActive(false);
                winner = -1;
            }
        }



    }

    public void PlayAgain()
    {
        gm.Vibrate("soft");
        ResetTHP();
    }

    public void ResetTHP()
    {

        playerCardsGO[0].GetComponent<CardScript>().GetCardSprite(mainCards.theFiveCard[0]);
        playerCardsGO[1].GetComponent<CardScript>().GetCardSprite(mainCards.theFiveCard[1]);

        gameEnd = false;

        startButton.SetActive(true);
        againButton.SetActive(false);
        continueButton.SetActive(false);
        redealButton.SetActive(false);

        playerScoreText.text = "";
        opponentScoreText.text = "";
        winner = 0;
        ResetWinnerHands();
        ResetToCardBacks();

    }

    private void ResetToCardBacks()
    {
        opponentCardsCounter = 0;
        midCardsCounter = 0;

        foreach (var item in opponentCards)
        {
            opponentCardsGO[opponentCardsCounter].GetComponent<CardScript>().GetCardBack();
            opponentCardsCounter++;
        }

        foreach (var item in midCards)
        {
            midCardsGO[midCardsCounter].GetComponent<CardScript>().GetCardBack();
            midCardsCounter++;
        }

        opponentCardsCounter = 0;
        midCardsCounter = 0;

    }

    public void StartTHP()
    {
        gm.Vibrate("soft");
        startButton.SetActive(false);

        prepareCards();

        scoreOfPlayer = GetMaxScore(GenerateCombinations(evaluateList1, 5), 1);

        scoreOfOpponent = GetMaxScore(GenerateCombinations(evaluateList2, 5), 2);

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
        opponentCards.Clear();
        midCards.Clear();
        evaluateList1.Clear();
        evaluateList2.Clear();

        //creating a new deck
        deck = new Deck();

        //removing players card from deck
        deck.cards.Remove(deck.cards.Find(x => x.number == mainCards.GetComponent<MainCards>().theFiveCard[0].number && x.suit == mainCards.GetComponent<MainCards>().theFiveCard[0].suit));
        deck.cards.Remove(deck.cards.Find(x => x.number == mainCards.GetComponent<MainCards>().theFiveCard[1].number && x.suit == mainCards.GetComponent<MainCards>().theFiveCard[1].suit));

        //adding players cards to evaluate list
        card = mainCards.GetComponent<MainCards>().theFiveCard[0];
        evaluateList1.Add(CardIntValue(card));


        card = mainCards.GetComponent<MainCards>().theFiveCard[1];
        evaluateList1.Add(CardIntValue(card));

        //adding opponent cards to evaluate list

        card = deck.DrawCard();
        opponentCards.Add(card);
        evaluateList2.Add(CardIntValue(card));


        card = deck.DrawCard();
        opponentCards.Add(card);
        evaluateList2.Add(CardIntValue(card));

        //creating mid cards

        for (int i = 0; i < 5; i++)
        {
            card = deck.DrawCard();
            midCards.Add(card);
            evaluateList1.Add(CardIntValue(card));
            evaluateList2.Add(CardIntValue(card));

        }

        evaluateList1 = evaluateList1.OrderByDescending(x => x % 100).ToList();
        evaluateList2 = evaluateList2.OrderByDescending(x => x % 100).ToList();


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

    private void ResetWinnerHands()
    {
        foreach (var playerGO in playerCardsGO)
        {
            GameObject loserChild = playerGO.transform.Find("loserfull").gameObject;
            GameObject winnerChild = playerGO.transform.Find("winnerfull").gameObject;

            loserChild.SetActive(false);
            winnerChild.SetActive(false);
        }

        foreach (var oppoGO in opponentCardsGO)
        {
            GameObject loserChild = oppoGO.transform.Find("loserfull").gameObject;
            GameObject winnerChild = oppoGO.transform.Find("winnerfull").gameObject;

            loserChild.SetActive(false);
            winnerChild.SetActive(false);

        }

        foreach (var midGO in midCardsGO)
        {
            GameObject losertopChild = midGO.transform.Find("losertop").gameObject;
            GameObject winnertopChild = midGO.transform.Find("winnertop").gameObject;
            GameObject loserbottomChild = midGO.transform.Find("loserbottom").gameObject;
            GameObject winnerbottomChild = midGO.transform.Find("winnerbottom").gameObject;

            losertopChild.SetActive(false);
            winnertopChild.SetActive(false);
            loserbottomChild.SetActive(false);
            winnerbottomChild.SetActive(false);
        }



    }

    private void ShowWinnerHands()
    {

        bestHandForPlayerCard.Clear();
        bestHandForOpponentCard.Clear();

        //for player

        foreach (var item in bestHandForPlayer)
        {

            bestHandForPlayerCard.Add(CardValueDecoder(item));

        }


        foreach (var playerGO in playerCardsGO)
        {
            GameObject loserChild = playerGO.transform.Find("loserfull").gameObject;
            GameObject winnerChild = playerGO.transform.Find("winnerfull").gameObject;

            foreach (var bestCard in bestHandForPlayerCard)
            {

                if (playerGO.GetComponent<CardScript>().currentCard.number == bestCard.number && playerGO.GetComponent<CardScript>().currentCard.suit == bestCard.suit)
                {

                    if (winner == 1)
                    {
                        winnerChild.SetActive(true);
                        loserChild.SetActive(false);
                    }
                    else if (winner == -1)
                    {
                        winnerChild.SetActive(false);
                        loserChild.SetActive(true);
                    }
                    else
                    {
                        winnerChild.SetActive(true);
                        loserChild.SetActive(false);
                    }

                }



            }

        }

        //for opponent

        foreach (var item in bestHandForOpponent)
        {

            bestHandForOpponentCard.Add(CardValueDecoder(item));

        }

        foreach (var oppoGO in opponentCardsGO)
        {
            GameObject loserChild = oppoGO.transform.Find("loserfull").gameObject;
            GameObject winnerChild = oppoGO.transform.Find("winnerfull").gameObject;

            foreach (var bestCard in bestHandForOpponentCard)
            {
                if (oppoGO.GetComponent<CardScript>().currentCard.number == bestCard.number && oppoGO.GetComponent<CardScript>().currentCard.suit == bestCard.suit)
                {

                    if (winner == -1)
                    {
                        winnerChild.SetActive(true);
                        loserChild.SetActive(false);
                    }
                    else if (winner == 1)
                    {
                        winnerChild.SetActive(false);
                        loserChild.SetActive(true);
                    }
                    else
                    {
                        winnerChild.SetActive(true);
                        loserChild.SetActive(false);
                    }

                }



            }

        }

        //for mid

        foreach (var midGO in midCardsGO)
        {
            GameObject losertopChild = midGO.transform.Find("losertop").gameObject;
            GameObject winnertopChild = midGO.transform.Find("winnertop").gameObject;
            GameObject loserbottomChild = midGO.transform.Find("loserbottom").gameObject;
            GameObject winnerbottomChild = midGO.transform.Find("winnerbottom").gameObject;

            foreach (var bestCard in bestHandForOpponentCard)
            {
                if (midGO.GetComponent<CardScript>().currentCard.number == bestCard.number && midGO.GetComponent<CardScript>().currentCard.suit == bestCard.suit)
                {

                    if (winner == -1)
                    {
                        winnertopChild.SetActive(true);
                        losertopChild.SetActive(false);
                    }
                    else if (winner == 1)
                    {
                        winnertopChild.SetActive(false);
                        losertopChild.SetActive(true);
                    }
                    else
                    {
                        winnertopChild.SetActive(true);
                        losertopChild.SetActive(false);
                    }

                }

            }


            foreach (var bestCard in bestHandForPlayerCard)
            {
                if (midGO.GetComponent<CardScript>().currentCard.number == bestCard.number && midGO.GetComponent<CardScript>().currentCard.suit == bestCard.suit)
                {

                    if (winner == 1)
                    {
                        winnerbottomChild.SetActive(true);
                        loserbottomChild.SetActive(false);
                    }
                    else if (winner == -1)
                    {
                        winnerbottomChild.SetActive(false);
                        loserbottomChild.SetActive(true);
                    }
                    else
                    {
                        winnerbottomChild.SetActive(true);
                        loserbottomChild.SetActive(false);
                    }

                }

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

    private int GetMaxScore(List<List<int>> tmpListlist, int whichPlayer)
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

                switch (whichPlayer)
                {

                    case 1:

                        bestHandForPlayer.Clear();
                        bestHandForPlayer = tmplist;

                        break;

                    case 2:

                        bestHandForOpponent.Clear();
                        bestHandForOpponent = tmplist;

                        break;

                }

            }


        }

        if (maxScore < 100)
        {
            switch (whichPlayer)
            {
                case 1:

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


                    break;

                case 2:

                    foreach (var item in evaluateList2)
                    {
                        int modScore = 0;
                        modScore = item % 100;

                        if (modScore > maxScore)
                            maxScore = modScore;
                    }

                    bestHandForOpponent.Clear();

                    evaluateList2.OrderByDescending(x => x % 100).ToList();

                    for (int i = 0; i < 5; i++)
                    {
                        bestHandForOpponent.Add(evaluateList2[i]);
                    }

                    break;

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
            
                if(hand[i] % 100 != highCards[0] && hand[i] % 100 != highCards[1])
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
            
                if(hand[i] % 100 != highCards[0])
                {
                    score += (hand[i] % 100);
                }
            
                }
            }

        }
        else if(score == 7000)
        {
            if (highCards[1] > highCards[0])
                Swap(highCards, 0, 1);
            score += 13 * highCards[0] + highCards[1];


        }

        else if(score == 8000)
        {
            score += 13*highCards[0];

            for (int i = 0; i < hand.Length; i++)
            {
            
                if(hand[i] % 100 != highCards[0])
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

    private int Kickers()
    {
        int winner = 0;

        bestHandForPlayer.OrderByDescending(x => x % 100).ToList();
        bestHandForOpponent.OrderByDescending(x => x % 100).ToList();

        if (scoreOfPlayer == scoreOfOpponent)
        {
            // 1 2 3 4 5 Straight sorunu

            if(scoreOfPlayer == 4000 || scoreOfPlayer == 9000)

            if (bestHandForPlayer[0] % 100 == 12
            && bestHandForPlayer[1] % 100 == 3
            && bestHandForPlayer[2] % 100 == 2
            && bestHandForPlayer[3] % 100 == 1
            && bestHandForPlayer[4] % 100 == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    if(bestHandForOpponent[i] % 100 > 3 && bestHandForOpponent[i] % 100 < 12)
                    {
                        return -1;
                    }
                }
                
                return 0;
            }

            if (bestHandForOpponent[0] % 100 == 12
            && bestHandForOpponent[1] % 100 == 3
            && bestHandForOpponent[2] % 100 == 2
            && bestHandForOpponent[3] % 100 == 1
            && bestHandForOpponent[4] % 100 == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    if(bestHandForPlayer[i] % 100 > 3 && bestHandForPlayer[i] % 100 < 12)
                    {
                        return 1;
                    }
                }
                
                return 0;
            }


        
            // genel


            for (int i = 0; i < 5; i++)
            {
                if (bestHandForPlayer[i] % 100 == bestHandForOpponent[i] % 100)
                {
                    winner = 0;
                }
                else if (bestHandForPlayer[i] % 100 > bestHandForOpponent[i] % 100)
                {
                    return 1;
                }
                else if (bestHandForPlayer[i] % 100 < bestHandForOpponent[i] % 100)
                {
                    return -1;
                }

            }

        }

        return winner;

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

    void PrintCombinations()
    {
        foreach (List<int> combination in combinations)
        {
            string combinationStr = string.Join(", ", combination);
            Debug.Log(combinationStr);
        }
    }

    private IEnumerator ShowOtherCards(float waitTime)
    {
        WaitForSeconds wait = new WaitForSeconds(waitTime);


        for (int i = 0; i < midCards.Count; i++)
        {
            gm.CardSound();
            midCardsGO[i].transform.DOLocalRotate(new Vector3(0, 90, 0), 0.1f);
            yield return wait;

            midCardsGO[i].GetComponent<CardScript>().GetCardSprite(midCards[i]);

            midCardsGO[i].transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f);
            yield return wait;

        }

        playerScoreText.text = ScoreToName(scoreOfPlayer);

        for (int i = 0; i < opponentCards.Count; i++)
        {
            gm.CardSound();
            opponentCardsGO[i].transform.DOLocalRotate(new Vector3(0, 90, 0), 0.1f);
            yield return wait;

            opponentCardsGO[i].GetComponent<CardScript>().GetCardSprite(opponentCards[i]);

            opponentCardsGO[i].transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f);
            yield return wait;

        }



        opponentScoreText.text = ScoreToName(scoreOfOpponent);

        gameEnd = true;

        GameStatus();

        ShowWinnerHands();

    }

    private void UpdateBestHands(int score, int h0, int h1, int h2, int h3, int h4)
    {
        int tmpScore = PlayerPrefs.GetInt("BestTexasScore");
        
        if(score > tmpScore)
        {
            PlayerPrefs.SetInt("BestTexasScore",score);

            PlayerPrefs.SetInt("BestHandTexas_0",h0);
            PlayerPrefs.SetInt("BestHandTexas_1",h1);
            PlayerPrefs.SetInt("BestHandTexas_2",h2);
            PlayerPrefs.SetInt("BestHandTexas_3",h3);
            PlayerPrefs.SetInt("BestHandTexas_4",h4);

        }

        
    }






}




