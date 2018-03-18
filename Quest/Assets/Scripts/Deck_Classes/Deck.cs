using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Deck
{
    // member variables

    public string type { get; set; }
    public int count { get; set; }
    public List<Card> deck { get; set; }
    public List<Card> discard { get; set; }
    static System.Random rnd = new System.Random();

    // member functions
    public Deck(string deckType, List<Card> initialDeck)
    {
        type = deckType;
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
        //CHANGE HERE FOR SCENARIO CHECKING 

        /*
        //int r = rnd.Next(count);
        int r = Random.Range(0, deck.Count - 1);
        Card card = deck[r];
        deck.Remove(card);
        emptyCheck();
        return card;
        */
        //int r = Random.Range(0, deck.Count - 1);
        Card card = deck[0];
        deck.Remove(card);
        emptyCheck();
        return card;
    }

    public List<Card> drawCards(int cardNum)
    {
        List<Card> drawnCards = new List<Card>();
        for (int i = 0; i < cardNum; i++)
        {
            int r = Random.Range(0, deck.Count - 1);
            Card card = deck[r];
            deck.Remove(card);
            emptyCheck();
            drawnCards.Add(card);
        }
        return drawnCards;
    }

    public void discardCards(List<Card> cardsToDiscard)
    {
        for (int i = 0; i < cardsToDiscard.Count; i++)
        {
            discard.Add(cardsToDiscard[i]);
            emptyCheck();
        }
        emptyCheck();
    }

    public void emptyCheck()
    {
        if (deck.Count == 0)
        {
            refillFromDiscard();
        }
    }

    public void refillFromDiscard()
    {    
        List<Card> cards = new List<Card>();

        deck = new List<Card>(discard);
        discard = new List<Card>();
        Debug.Log("refilled " + type);
        display();

        /*
        // fills deck with the contents of discard
        for (int i = 0; i < discard.Count; i++)
        {
            deck.Add(discard[i]);
        }

        // loops backward through discard and empties it
        for (int i = deck.Count; i > 0; i--)
        {
            discard.Remove(discard[i - 1]);
        }
        */
    }
}
