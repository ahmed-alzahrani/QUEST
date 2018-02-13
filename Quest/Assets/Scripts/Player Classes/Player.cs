using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player {

    //member variables
    public string name { get; set; }
    public int score { get; set; }
    public RankCard rankCard { get; set; }
    public List<RankCard> rankDeck {get; set; }
    public List<Card> hand { get; set; }
    public List<AllyCard> activeAllies{ get; set; }
    public iStrategy strategy{ get; set; }

    //member functions

    public Player(string playerName, List<Card> startingHand, iStrategy strat) {
        name = playerName;
        score = 0;
        hand = startingHand;
        activeAllies = new List<AllyCard>();
        rankCard = new RankCard("Rank", "Squire", 5);
        List<RankCard> rankCards = new List<RankCard>();
        RankCard KnightCard = new RankCard("Rank", "Knight", 10);
        RankCard champKnightCard = new RankCard("Rank", "Champion Knight", 20);
        rankCards.Add(KnightCard);
        rankCards.Add(champKnightCard);
        rankDeck = rankCards;
        strategy = strat;

    }

    public Player() { }

    // this is where the logic should be added for ranking up a player
    public void addShields(int shields) {
      score += shields;
    }

    // similiarly, the logic for implementing ranking down needs to be added here
    public void removeShields(int shields) {
      score -= shields;
      if (score < 0) {
        score = 0;
      }
    }

    public List<AllyCard> courtCalled(){
      List<AllyCard> allies = new List<AllyCard>();
      for (var i = 0; i < activeAllies.Count; i++){
        allies.Add(activeAllies[i]);
      }
      for (var i = 0; i < allies.Count; i++){
        activeAllies.Remove(allies[i]);
      }

      return allies;
    }

    public List<Card> kingsCall(){
      return strategy.kingsCall(hand);
    }

    // public void drawCard(deckToDrawFrom){}

    //public Card playCard(){}
}
