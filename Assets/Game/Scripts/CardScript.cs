using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    [SerializeField] private List<Sprite> SpadesSprites;
    [SerializeField] private List<Sprite> HearthSprites;
    [SerializeField] private List<Sprite> ClubsSprites;
    [SerializeField] private List<Sprite> DiamondsSprites;
    [SerializeField] public List<Sprite> CardBacks;

    public Card currentCard;

    SpriteRenderer sr;


    public void GetCardSprite(Card card)
    {
        //print("Drew card: " + card.number + " of " + card.suit);
        currentCard = card;
        
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = CardBacks[0];

        if(card != null)
        {

        switch (card.suit)
        {
            case "Hearts":

                switch (card.number)
                {
                    case 1:
                        sr.sprite = HearthSprites[card.number - 1];
                        break;

                    case 2:
                        sr.sprite = HearthSprites[card.number - 1];
                        break;

                    case 3:
                        sr.sprite = HearthSprites[card.number - 1];
                        break;

                    case 4:
                        sr.sprite = HearthSprites[card.number - 1];
                        break;

                    case 5:
                        sr.sprite = HearthSprites[card.number - 1];
                        break;

                    case 6:
                        sr.sprite = HearthSprites[card.number - 1];
                        break;

                    case 7:
                        sr.sprite = HearthSprites[card.number - 1];
                        break;

                    case 8:
                        sr.sprite = HearthSprites[card.number - 1];
                        break;

                    case 9:
                        sr.sprite = HearthSprites[card.number - 1];
                        break;

                    case 10:
                        sr.sprite = HearthSprites[card.number - 1];
                        break;

                    case 11:
                        sr.sprite = HearthSprites[card.number - 1];
                        break;

                    case 12:
                        sr.sprite = HearthSprites[card.number - 1];
                        break;

                    case 13:
                        sr.sprite = HearthSprites[card.number - 1];
                        break;

                }

                break;

            case "Diamonds":

                switch (card.number)
                {
                    case 1:
                        sr.sprite = DiamondsSprites[card.number - 1];
                        break;

                    case 2:
                        sr.sprite = DiamondsSprites[card.number - 1];
                        break;

                    case 3:
                        sr.sprite = DiamondsSprites[card.number - 1];
                        break;

                    case 4:
                        sr.sprite = DiamondsSprites[card.number - 1];
                        break;

                    case 5:
                        sr.sprite = DiamondsSprites[card.number - 1];
                        break;

                    case 6:
                        sr.sprite = DiamondsSprites[card.number - 1];
                        break;

                    case 7:
                        sr.sprite = DiamondsSprites[card.number - 1];
                        break;

                    case 8:
                        sr.sprite = DiamondsSprites[card.number - 1];
                        break;

                    case 9:
                        sr.sprite = DiamondsSprites[card.number - 1];
                        break;

                    case 10:
                        sr.sprite = DiamondsSprites[card.number - 1];
                        break;

                    case 11:
                        sr.sprite = DiamondsSprites[card.number - 1];
                        break;

                    case 12:
                        sr.sprite = DiamondsSprites[card.number - 1];
                        break;

                    case 13:
                        sr.sprite = DiamondsSprites[card.number - 1];
                        break;

                }

                break;

            case "Clubs":

                switch (card.number)
                {
                    case 1:
                        sr.sprite = ClubsSprites[card.number - 1];
                        break;

                    case 2:
                        sr.sprite = ClubsSprites[card.number - 1];
                        break;

                    case 3:
                        sr.sprite = ClubsSprites[card.number - 1];
                        break;

                    case 4:
                        sr.sprite = ClubsSprites[card.number - 1];
                        break;

                    case 5:
                        sr.sprite = ClubsSprites[card.number - 1];
                        break;

                    case 6:
                        sr.sprite = ClubsSprites[card.number - 1];
                        break;

                    case 7:
                        sr.sprite = ClubsSprites[card.number - 1];
                        break;

                    case 8:
                        sr.sprite = ClubsSprites[card.number - 1];
                        break;

                    case 9:
                        sr.sprite = ClubsSprites[card.number - 1];
                        break;

                    case 10:
                        sr.sprite = ClubsSprites[card.number - 1];
                        break;

                    case 11:
                        sr.sprite = ClubsSprites[card.number - 1];
                        break;

                    case 12:
                        sr.sprite = ClubsSprites[card.number - 1];
                        break;

                    case 13:
                        sr.sprite = ClubsSprites[card.number - 1];
                        break;

                }

                break;

            case "Spades":

                switch (card.number)
                {
                    case 1:
                        sr.sprite = SpadesSprites[card.number - 1];
                        break;

                    case 2:
                        sr.sprite = SpadesSprites[card.number - 1];
                        break;

                    case 3:
                        sr.sprite = SpadesSprites[card.number - 1];
                        break;

                    case 4:
                        sr.sprite = SpadesSprites[card.number - 1];
                        break;

                    case 5:
                        sr.sprite = SpadesSprites[card.number - 1];
                        break;

                    case 6:
                        sr.sprite = SpadesSprites[card.number - 1];
                        break;

                    case 7:
                        sr.sprite = SpadesSprites[card.number - 1];
                        break;

                    case 8:
                        sr.sprite = SpadesSprites[card.number - 1];
                        break;

                    case 9:
                        sr.sprite = SpadesSprites[card.number - 1];
                        break;

                    case 10:
                        sr.sprite = SpadesSprites[card.number - 1];
                        break;

                    case 11:
                        sr.sprite = SpadesSprites[card.number - 1];
                        break;

                    case 12:
                        sr.sprite = SpadesSprites[card.number - 1];
                        break;

                    case 13:
                        sr.sprite = SpadesSprites[card.number - 1];
                        break;

                }

                break;

        }

        }
        else
        {
            print("Deck is finished!");
        }

    }

    public void GetCardBack()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = CardBacks[0];
    }

}
