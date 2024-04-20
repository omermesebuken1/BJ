using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Card {
    public string suit;
    public int number;

    public Card(string s, int n) {
        suit = s;
        number = n;
    }
}

public class Deck{

    
    public List<Card> cards;
    private int currentCardIndex;

    public Deck() {

        cards = new List<Card>();
        currentCardIndex = 0;

        // Create deck of cards
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        for (int i = 0; i < suits.Length; i++) {
            for (int j = 1; j <= 13; j++) {
                cards.Add(new Card(suits[i], j));
            }
        }

        Shuffle();
    }

    // Shuffle deck of cards
    public void Shuffle() {
        for (int i = 0; i < cards.Count; i++) {
            int randIndex = UnityEngine.Random.Range(i, cards.Count);
            Card temp = cards[i];
            cards[i] = cards[randIndex];
            cards[randIndex] = temp;
        }

        currentCardIndex = 0;
    }

    // Draw a card from the deck
    public Card DrawCard() {
        
        if (currentCardIndex >= cards.Count) {
            return null;
        }

        Card card = cards[currentCardIndex];
        currentCardIndex++;
        return card;
    }

    
}
