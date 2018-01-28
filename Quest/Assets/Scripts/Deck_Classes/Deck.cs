using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Deck{
    // member variables

    private string type;
    private int count;
    private List<Card> deck;
    private List<Card> discard;

    // member functions
    public Deck(string deckType, int deckCount, List<Card> initialDeck) { 
        type = deckType;
        count = deckCount;
        deck = initialDeck;
        discard = new List<Card>();
    }

    public Deck(){}

    public string getType(){
        return this.type;
    }

    public int getCount(){
        return this.count;
    }

    public void decCount(){
        count -= 1;
    }

    public void display(){
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

    // public Card drawNext(){}
}
