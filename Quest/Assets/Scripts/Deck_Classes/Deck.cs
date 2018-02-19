using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Deck {
    // member variables

    public string type { get; set; }
    public int count { get; set; }
    public List<Card> deck { get; set; }
    public List<Card> discard { get; set; }
    static System.Random rnd = new System.Random ();

    // member functions
    public Deck (string deckType, List<Card> initialDeck) {
        type = deckType;
        deck = initialDeck;
        discard = new List<Card> ();
    }

    public Deck () { }

    public void display () {
        Debug.Log ("Deck Type: " + type);
        Debug.Log ("Deck Count: " + count);
        Debug.Log ("Discard: ");
        displayList (discard);
        Debug.Log ("Deck: ");
        displayList (deck);
    }

    public void displayList (List<Card> listToPrint) {
        foreach (Card c in listToPrint) {
            c.display ();
        }
    }

    public Card drawCard () {

        //int r = rnd.Next(count);
        int r = Random.Range (0, deck.Count - 1);
        Card card = deck[r];
        deck.Remove(card);
        return card;
    }

    public void discardCards (List<Card> cardsToDiscard) {
        for (int i = 0; i < cardsToDiscard.Count; i++) {
            discard.Add (cardsToDiscard[i]);
        }
    }

    // public Card drawNext(){}
}
