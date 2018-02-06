using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Maybe we can have the description of a card where when we click a card it shows its description data on the screen
public class GameController : MonoBehaviour
{
    public Button button;
    public List<Player> players;
    public CardUIScript script;
    public deckBuilder decks;
    public Deck storyDeck;
    public Deck adventureDeck;
    public int turnCount = 0;
    public Turn nextTurn;

    // Use this for initialization
    void Start ()
    {
        decks = new deckBuilder();
        storyDeck = decks.buildStoryDeck();
        adventureDeck = decks.buildAdventureDeck();
        /*
        To-Do Here:
            1. Somehow communicate with main menu to grab player's name and create player

            2. Create list of players with human player and CPU players
        */

    }

	// Update is called once per frame
	void Update ()
    {

      // Once the list of players has been generated the turns need to be set up here
        if (Input.GetButton("Fire1"))
        {
            //script.flipCard();
           // script.ChangeVisibility();
        }

        //if (Input.GetButton("Fire2")) script.flipCard();

    }

    // Move this into Player class? As a drawHand function?

    public List<Card> generateHand(Deck deckToDrawFrom)
    {
        List<Card> hand = new List<Card>();

        for(int i = 0; i < 11; i++)
        {
            hand.Add(deckToDrawFrom.drawCard());
        }

        return hand;

    }

/*
    public List<Card> getTurnList(List<Card> playerList, int turnCount){
      Debug.Log(turnCount);
      return playerList;
      // this function will provide a context based playerList based on turnCount

      List<Card> turnList = new List<Card>();
      for(int i = 0; i < playerList.size(); i++){
        turnList.add(playerList.get((i + turnCount) % 4)
      }
    }
    */
}
