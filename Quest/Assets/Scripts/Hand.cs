<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class Hand
{
    // member variables

    static int limit = 12;
    private int count;
    //private Card[] cards;  //use list instead its just easier (list is just like arraylist)
    private List<Card> cards;

    // member functions
    public Hand(List<Card> drawnHand)
    {
        count   = drawnHand.Count;
        cards   = drawnHand;
        //cards = drawnHand; //set the list up we can then use cards.Count to get the size 
    }

    // public void addCard() {}

    // public void discardCard() {}
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class Hand
{
    // member variables

    static int limit = 12;
    private int count;
    //private Card[] cards;  //use list instead its just easier (list is just like arraylist)
    private List<Card> cards;

    // member functions
    public Hand(List<Card> drawnHand)
    {
        count   = drawnHand.Count;
        cards   = drawnHand;
        //cards = drawnHand; //set the list up we can then use cards.Count to get the size 
    }

    // public void addCard() {}

    // public void discardCard() {}
>>>>>>> master
}