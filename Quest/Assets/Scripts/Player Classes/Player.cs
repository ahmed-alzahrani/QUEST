using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player {

    //member variables
    public string name { get; set; }
    public int score { get; set; }
    public string rank { get; set; }
    public RankCard rankCard { get; set; }
    public List<Card> hand { get; set; }
    public List<AllyCard> activeAllies{ get; set; }

    //member functions

    public Player(string playerName, List<Card> startingHand) {
        name = playerName;
        score = 0;
        rank = "Squire";
        hand = startingHand;
        activeAllies = new List<AllyCard>();
    }

    public Player() { }

    public void rankUpToKnight(){
        this.rank = "Knight";
    }

    public void rankUpToChampion(){
        this.rank = "Champion Knight";
    }

    public void rankDecrease(){
      score -=1;
      if (score == 4 ){
        rank = "Squire";
      }
      if (score == 6 ){
        rank = "Knight";
      }
    }

    // public void drawCard(deckToDrawFrom){}

    //public Card playCard(){}
}
