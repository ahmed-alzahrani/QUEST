using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
//using NSubstitute;

public class iStoryQueensFavorTest{
  [Test]
  public void iStoryQueensFavor_adds2AdventureCards_toSingleLowestHand()
  {
    iStoryQueensFavor queen = new iStoryQueensFavor();
    GameController game = new GameController();
    List<Player> players = new List<Player>();
    List<Card> cards = new List<Card>();
    RankCard storyCard = new RankCard("Test", " ", " ", 5);

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


    Deck adventure = new Deck("Adventure Deck", cards);
    game.adventureDeck = adventure;

   Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyPlayer() , "");
   player1.addShields(1);
   players.Add(player1);

   Player player2 = new Player("Kazma", new List<Card>(), new iStrategyPlayer() , "");
   player2.addShields(1);
   players.Add(player2);

   Player player3 = new Player("Rotharn", new List<Card>(), new iStrategyPlayer() , "");
   player3.addShields(1);
   players.Add(player3);

   Player player4 = new Player("Cheldon", new List<Card>(), new iStrategyPlayer() , "");
   players.Add(player4);

   // Assert that the players have empty hands
   Assert.AreEqual(player1.hand.Count, 0);
   Assert.AreEqual(player2.hand.Count, 0);
   Assert.AreEqual(player3.hand.Count, 0);
   Assert.AreEqual(player4.hand.Count, 0);
   Assert.AreEqual(cards.Count, 10);
   Assert.AreEqual(game.adventureDeck.deck.Count, 10);

   //Make the call to iStoryProsperity
   queen.execute(players, storyCard, game);

   // Assert that the 10 cards are evenly divided now that everyone drew 2 after the prosperity event executed
   Assert.AreEqual(player1.hand.Count, 0);
   Assert.AreEqual(player2.hand.Count, 0);
   Assert.AreEqual(player3.hand.Count, 0);
   Assert.AreEqual(player4.hand.Count, 2);
   Assert.AreEqual(game.adventureDeck.deck.Count, 8);

  }

  [Test]
  public void iStoryQueensFavor_adds2AdventureCards_toMultipleLowestHands()
  {
    iStoryQueensFavor queen = new iStoryQueensFavor();
    GameController game = new GameController();
    List<Player> players = new List<Player>();
    List<Card> cards = new List<Card>();
    RankCard storyCard = new RankCard("Test", " ", " ", 5);

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


    Deck adventure = new Deck("Adventure Deck", cards);
    game.adventureDeck = adventure;

   Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyPlayer() , "");
   player1.addShields(1);
   players.Add(player1);

   Player player2 = new Player("Kazma", new List<Card>(), new iStrategyPlayer() , "");
   players.Add(player2);

   Player player3 = new Player("Rotharn", new List<Card>(), new iStrategyPlayer() , "");
   players.Add(player3);

   Player player4 = new Player("Cheldon", new List<Card>(), new iStrategyPlayer() , "");
   players.Add(player4);

   // Assert that the players have empty hands
   Assert.AreEqual(player1.hand.Count, 0);
   Assert.AreEqual(player2.hand.Count, 0);
   Assert.AreEqual(player3.hand.Count, 0);
   Assert.AreEqual(player4.hand.Count, 0);
   Assert.AreEqual(cards.Count, 10);
   Assert.AreEqual(game.adventureDeck.deck.Count, 10);

   //Make the call to iStoryProsperity
   queen.execute(players, storyCard, game);

   // Assert that the 10 cards are evenly divided now that everyone drew 2 after the prosperity event executed
   Assert.AreEqual(player1.hand.Count, 0);
   Assert.AreEqual(player2.hand.Count, 2);
   Assert.AreEqual(player3.hand.Count, 2);
   Assert.AreEqual(player4.hand.Count, 2);
   Assert.AreEqual(game.adventureDeck.deck.Count, 4);
  }
}
