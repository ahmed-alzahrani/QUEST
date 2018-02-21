using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class iStoryProsperityTest{

  [Test]
  public void iStoryProsperity_DrawsTwoCardsEach_FromGC_AdventureDeck(){
    // Instantiate the iStory, the story card, and the list of players
    iStoryProsperity prosper = new iStoryProsperity();
    RankCard storyCard = new RankCard("Test", " ", " ", 5);
    List<Player> players = new List<Player>();

    // instatiate a list of 4 test players
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyPlayer());
    Player player2 = new Player("Cheldon", new List<Card>(), new iStrategyPlayer());
    Player player3 = new Player("Julian", new List<Card>(), new iStrategyPlayer());
    Player player4 = new Player("Kazma", new List<Card>(), new iStrategyPlayer());

    //add players to list
    players.Add(player1);
    players.Add(player2);
    players.Add(player3);
    players.Add(player4);

    // Instantiate the list of test cards that will comprise our mock adventure deck
    List<Card> cards = new List<Card>();
    cards.Add(new RankCard("Test Card", "Card 1", " ", 5));
    cards.Add(new RankCard("Test Card", "Card 2", " ", 5));
    cards.Add(new RankCard("Test Card", "Card 3", " ", 5));
    cards.Add(new RankCard("Test Card", "Card 4", " ", 5));
    cards.Add(new RankCard("Test Card", "Card 5", " ", 5));
    cards.Add(new RankCard("Test Card", "Card 6", " ", 5));
    cards.Add(new RankCard("Test Card", "Card 7", " ", 5));
    cards.Add(new RankCard("Test Card", "Card 8", " ", 5));
    cards.Add(new RankCard("Test Card", "Card 9", " ", 5));
    cards.Add(new RankCard("Test Card", "Card 10", " ", 5));


    // Game Controller and Deck are instantiated
    Deck deck = new Deck("Adventure Deck", cards);
    GameController game = new GameController();
    game.adventureDeck = deck;

    // Assert that the players have empty hands
    Assert.AreEqual(player1.hand.Count, 0);
    Assert.AreEqual(player2.hand.Count, 0);
    Assert.AreEqual(player3.hand.Count, 0);
    Assert.AreEqual(player4.hand.Count, 0);
    Assert.AreEqual(cards.Count, 10);
    Assert.AreEqual(game.adventureDeck.deck.Count, 10);

    //Make the call to iStoryProsperity
    prosper.execute(players, storyCard, game);

    // Assert that the 10 cards are evenly divided now that everyone drew 2 after the prosperity event executed
    Assert.AreEqual(player1.hand.Count, 2);
    Assert.AreEqual(player2.hand.Count, 2);
    Assert.AreEqual(player3.hand.Count, 2);
    Assert.AreEqual(player4.hand.Count, 2);
    Assert.AreEqual(game.adventureDeck.deck.Count, 2);
  }
}
