using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class buildRankDeck : MonoBehaviour{

  private Deck deck;
  private Card card;
  private RankCard rankCard;
  private Button button;
  private Deck rankDeck;

  void Awake(){
    // Debug.Log("I am awake!");
    rankDeck = new Deck("Rank Deck", 12, buildDeck());
    button = gameObject.GetComponent<Button>();
    button.onClick.AddListener(rankDeck.display);
  }

  void Update(){
      //  Debug.Log("I exist!!!!!!!!!!!");

  }

  public List<Card> buildDeck(){
    List<Card> rankCards = new List<Card>();
    for (int i = 0; i < 4; i++){
      RankCard squireCard = new RankCard("Rank", "Squire", 5);
      rankCards.Add(squireCard);
    }
    for (int i = 0; i < 4; i++){
      RankCard KnightCard = new RankCard("Rank", "Knight", 10);
      rankCards.Add(KnightCard);
    }
    for (int i = 0; i < 4; i++){
      RankCard champKnightCard = new RankCard("Rank", "Champion Knight", 20);
      rankCards.Add(champKnightCard);
    }
    return rankCards;
  }
}
