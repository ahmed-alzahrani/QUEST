using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Maybe we can have the description of a card where when we click a card it shows its description data on the screen
public class GameController : MonoBehaviour
{
    public Button button;
    public Player playerOne;
    public CardUIScript script;
    public deckBuilder decks;
    public Deck storyDeck;
    public Deck rankDeck;
    public Deck adventureDeck;

    // Use this for initialization
    void Start ()
    {
        decks = new deckBuilder();
        storyDeck = decks.buildStoryDeck();
        rankDeck = decks.buildRankDeck();
        adventureDeck = decks.buildAdventureDeck();
        playerOne = new Player("Ahmed", generateHand(adventureDeck));

    }

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButton("Fire1"))
        {
            //script.flipCard();
           // script.ChangeVisibility();
        }

        //if (Input.GetButton("Fire2")) script.flipCard();

    }

    public List<Card> generateHand(Deck deckToDrawFrom)
    {
        List<Card> hand = new List<Card>();

        for(int i = 0; i < 11; i++)
        {
            hand.Add(deckToDrawFrom.drawCard());
        }

        return hand;
        
    }
}
