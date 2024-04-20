using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HiLo : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private MainCards mainCards;
    [SerializeField] private List<GameObject> hiloCards = new List<GameObject>();
    [SerializeField] private GameObject playerCard;

    private Card currentCard;
    private int HiLoCardCounter;
    private Card card;
    private bool? winStatus;


    [SerializeField] private GameObject lowerButton;
    [SerializeField] private GameObject higherButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject redealButton;


    private void Update()
    {
        GameStatus();
    }


    public void GuessHiLo(string guess)
    {
        if (HiLoCardCounter < 3)
        {
            gm.Vibrate("soft");
            card = mainCards.deck.DrawCard();
            StartCoroutine(FlipCard(hiloCards[HiLoCardCounter],card,0.1f));
            

            if (guess == "Higher")
            {
                if (currentCard.number < card.number)
                {
                    winStatus = true;
                    currentCard = card;
                }
                else
                {
                    winStatus = false;
                }

            }
            else if (guess == "Lower")
            {
                if (currentCard.number > card.number)
                {
                    winStatus = true;
                    currentCard = card;
                }
                else
                {
                    winStatus = false;
                }
            }
        }




    }

    public void StartHiLo()
    {
        lowerButton.SetActive(true);
        higherButton.SetActive(true);
        continueButton.SetActive(false);
        redealButton.SetActive(false);

        playerCard.GetComponent<CardScript>().GetCardSprite(mainCards.theFiveCard[0]); 

        foreach (var item in hiloCards)
        {
            item.GetComponent<CardScript>().GetCardBack();
        }
        winStatus = null;
        currentCard = mainCards.theFiveCard[0];
        HiLoCardCounter = 0;

    }

    private void GameStatus()
    {
          
        if (winStatus == false)
        {
            lowerButton.SetActive(false);
            higherButton.SetActive(false);
            redealButton.SetActive(true);
        }


        if (HiLoCardCounter >= 2) // end
        {
            lowerButton.SetActive(false);
            higherButton.SetActive(false);

            if (winStatus == true) // win
            {
                continueButton.SetActive(true);

            }
            else if (winStatus == false) // lose
            {
                redealButton.SetActive(true);
                
            }
        }


    }

    private IEnumerator FlipCard(GameObject cardGO, Card card, float waitTime)
    {
        gm.CardSound();
        HiLoCardCounter++;
        
        WaitForSeconds wait = new WaitForSeconds(waitTime);

        cardGO.transform.DOLocalRotate(new Vector3(0,90,0),0.1f);
        yield return wait;

        cardGO.GetComponent<CardScript>().GetCardSprite(card);

        cardGO.transform.DOLocalRotate(new Vector3(0,0,0),0.1f);
        yield return wait;

        

    }

    

    





}
