using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class cardLogicController : MonoBehaviour
{
    private deckBuilder decks;
    private Deck storyDeck;
    private Deck rankDeck;
    private Deck adventureDeck;
    private Button button;

    void Awake()
    {

        decks = new deckBuilder();
        storyDeck = decks.buildStoryDeck();
        rankDeck = decks.buildRankDeck();
        adventureDeck = decks.buildAdventureDeck();

        button = gameObject.GetComponent<Button>();
        //button.onClick.AddListener(display);
    }

    void Update()
    {

    }

    public void display()
    {
        storyDeck.display();
        rankDeck.display();
        adventureDeck.display();
    }
}
