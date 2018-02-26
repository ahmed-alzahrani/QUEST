using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class DeckTest{

  [Test]
  public void refillFromDiscard_PopulatesEmptyDeck_WithDiscardCards()
  {
    Deck deck = new Deck("Type", new List<Card>());

    // add 3 cards to the deck's discard pile
    deck.discard.Add(new AllyCard("Ally Card", "Some Ally", "texture", 5, 1, "some ability", "", "", 0, 0, new NoBuff()));
    deck.discard.Add(new AllyCard("Ally Card", "Some Ally", "texture", 5, 1, "some ability", "", "", 0, 0, new NoBuff()));
    deck.discard.Add(new AllyCard("Ally Card", "Some Ally", "texture", 5, 1, "some ability", "", "", 0, 0, new NoBuff()));

    // add a single card to the deck's deck
    deck.deck.Add(new AllyCard("Ally Card", "Some Ally", "texture", 5, 1, "some ability", "", "", 0, 0, new NoBuff()));

    // We know the counts before a card is removed from deck
    Assert.AreEqual(deck.discard.Count, 3);
    Assert.AreEqual(deck.deck.Count, 1);

    Card card = deck.drawCard();

    // Now the count of deck should be 3 (it was filled from discard) and the count of discard should be 0
    Assert.AreEqual(0, deck.discard.Count);
    Assert.AreEqual(3, deck.deck.Count);
  }

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
