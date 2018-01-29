using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class cardLogicController : MonoBehaviour {

  private buildStoryDeck storyBuilder;
  private Deck storyDeck;

  private buildRankDeck rankBuilder;
  private Deck rankDeck;

  private buildAdventureDeck adventureBuilder;
  private Deck adventureDeck;

  private Button button;

  void Awake(){
    storyDeck = storyBuilder.build();
    rankDeck = rankBuilder.build();
    adventureDeck = adventureBuilder.build();

    button = gameObject.GetComponent<Button>();
    button.onClick.AddListener(display);
  }

  void Update(){

  }

  public void display(){
    storyDeck.display();
    rankDeck.display();
    adventureDeck.display();
  }

   public Deck getStoryDeck()
    {
        List<Card> storyList = new List<Card>();
        storyList = storyBuilder.buildDeck();
        return new Deck("Story Deck", 28, storyList);
    }
}
