using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class DeckTest{

  [Test]
  public void Deck_CreatedWithGiven_WillHaveTheVariables(){
    var deck = new Deck("Type", new List<Card>());

    Assert.AreEqual("Type", deck.type);
    Assert.IsNotNull(deck.deck);
    Assert.IsNotNull(deck.discard);
    Assert.AreEqual(0, deck.discard.Count);
  }

  [Test]
  // ensures that when a card is drawn from the deck it is both returned and NO LONGER in the deck
  public void Deck_drawCard_returnsCard_andCardIsNotInDeck(){
    deckBuilder decks = new deckBuilder();

    var deck = decks.buildAdventureDeck();
    Card cardDrawnFrom = deck.drawCard();
    foreach (Card c in deck.deck)
    {
      Assert.AreNotEqual(c, cardDrawnFrom);
      c.display();
    }
    Debug.Log("And now... the card that was taken");
    cardDrawnFrom.display();
  }

  [Test]
  public void Deck_discardCards_putsCardsInDiscard(){
    deckBuilder decks = new deckBuilder();

    Deck deck = decks.buildAdventureDeck();
    // discard starts off empty
    Assert.AreEqual(0, deck.discard.Count);

    List<Card> cards = new List<Card>();

    for(int i = 0; i < 10; i++)
    {
      Card cardDrawnFrom = deck.drawCard();
      cards.Add(cardDrawnFrom);
    }
    deck.discardCards(cards);

    for (int i = 0; i < cards.Count; i++)
    {
      Assert.AreEqual(cards[i], deck.discard[i]);
    }
    Assert.AreEqual(deck.discard.Count, cards.Count);
  }

/*
  [Test]
	public void Deck_CreatedWithGiven_WillHaveTheVariables() {
    var deck = new Deck("Type", new List<Card>());

    Assert.AreEqual("Type", deck.type);
    Assert.IsNotNull(deck.deck);
    Assert.IsNotNull(deck.discard);
    Assert.AreEqual(0, deck.discard.Count);
		// Use the Assert class to test conditions.
	} */
}
