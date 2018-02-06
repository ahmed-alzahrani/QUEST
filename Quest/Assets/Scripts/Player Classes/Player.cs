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

    //member functions

    public Player(string playerName, List<Card> startingHand) {
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

    }

    public Player() { }

    // public void drawCard(deckToDrawFrom){}

    //public Card playCard(){}
}
