<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


class Deck
{
    // member variables

    public string type { get; private set; }
    public int count { get; private set; }

    //set to lists 
    //private Card[] discard;
    //private Card[] deck;

    //might be changed!!!!
    public List<Card> discard { get; private set; }
    public List<Card> deck { get; private set; }

    // member functions
    public Deck(string deckType, int deckCount, List<Card> actualDeck, List<Card> currentDiscard)
    {
        type = deckType;
        count = deckCount;
        discard = currentDiscard;
        deck = actualDeck;
    }

    /*
    public string getType(){
        return this.type;
    }

    public int getCount(){
        return this.count;
    }
    */

    public void decCount(){
        count -= 1;
    }

    // public Card drawNext(){}
}
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


class Deck
{
    // member variables

    public string type { get; private set; }
    public int count { get; private set; }

    //set to lists 
    //private Card[] discard;
    //private Card[] deck;

    //might be changed!!!!
    public List<Card> discard { get; private set; }
    public List<Card> deck { get; private set; }

    // member functions
    public Deck(string deckType, int deckCount, List<Card> actualDeck, List<Card> currentDiscard)
    {
        type = deckType;
        count = deckCount;
        discard = currentDiscard;
        deck = actualDeck;
    }

    /*
    public string getType(){
        return this.type;
    }

    public int getCount(){
        return this.count;
    }
    */

    public void decCount(){
        count -= 1;
    }

    // public Card drawNext(){}
}
>>>>>>> master
