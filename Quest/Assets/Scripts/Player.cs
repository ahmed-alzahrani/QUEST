using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class Player
{
    //member variables
    public string name;
    public int score { get; private set; }
    public string rank { get; private set; }
    private Hand hand;

    //member functions

    // 1 param constructor, good for human and CPU players, human players simply need supply a name and CPU can be generated
    public Player(string playerName, Hand startingHand)
    {
        name = playerName;
        score = 0;
        rank = "Squire";
        hand = startingHand;
    }

    /*
    public string getName(){
        Debug.Log("Name: " + name);
        return this.name;
    }

    public int getScore(){
        Debug.Log("Score: " + score);
        return this.score;
    }
    */

    public void rankUpToKnight(){
        this.rank = "Knight";
    }

    public void rankUpToChampion(){
        this.rank = "Champion Knight";
    }

    // public void drawCard(deckToDrawFrom){}
    //public Card playCard(){}
}
