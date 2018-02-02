using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Deck
{
    // member variables

    private string type { get; set; }
    private int count { get; set; }
    private List<Card> deck { get; set; }
    private List<Card> discard { get; set; }
    static System.Random rnd = new System.Random();

    // member functions
    public Deck(string deckType, int deckCount, List<Card> initialDeck)
    {
        type = deckType;
        //count = deck.Count;
        deck = initialDeck;
        discard = new List<Card>();
    }

    public Deck() { }

    public void display()
    {
        Debug.Log("Deck Type: " + type);
        Debug.Log("Deck Count: " + count);
        Debug.Log("Discard: ");
        displayList(discard);
        Debug.Log("Deck: ");
        displayList(deck);
    }

    public void displayList(List<Card> listToPrint)
    {
        foreach (Card c in listToPrint)
        {
            c.display();
        }
    }

    public Card drawCard()
    {
       
        int r = rnd.Next(count);
        return deck[r];
    }

    public void discardCard(Card cardToDiscard)
    {
        discard.Add(cardToDiscard);
    }

    // public Card drawNext(){}
}
