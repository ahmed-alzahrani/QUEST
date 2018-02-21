using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class iStoryKingsCallToArmsTest{

  [Test]
  public void executeKingsCall_ForcesHighestToDiscard(){
    iStoryKingsCallToArms call = new iStoryKingsCallToArms();
    EventCard eCard = new EventCard("Event Card", "Test Event", "description", "path", call);
    Deck deck = new Deck("Deck", new List<Card>());
    List<Player> players = new List<Player>();

    // Player 0 has 1 weapon and 1 foe, they will discard their only weapon
    Player player0 = new Player("Test Guy2", new List<Card>(), new iStrategyCPU2() , "");
    player0.addShields(1);
    player0.hand.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
    player0.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player0.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
    players.Add(player0);



    // Player 1 has 2 weapons and 1 foe, they will discard their lesser weapon
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1() , "");
    player1.addShields(1);
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Big Sword", " ", 30));
    player1.hand.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
    players.Add(player1);

    // Player 2 will have 1 foe and 0 weapons, they will discard that one foe
    Player player2 = new Player("Kazma", new List<Card>(), new iStrategyCPU2() , "");
    player2.addShields(1);
    player2.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player2.hand.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
    players.Add(player2);

    // Player 3 will have 3 foes and no weapons, and they will discard the 2 lowest foes
    Player player3 = new Player("Rotharn", new List<Card>(), new iStrategyCPU1() , "");
    player3.addShields(1);
    player3.hand.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
    player3.hand.Add(new FoeCard("Foe Card", "Robber Knight1", "Textures/foe/robberKnight", 15, 15));
    player3.hand.Add(new FoeCard("Foe Card", "Robber Knight2", "Textures/foe/robberKnight", 20, 20));
    player3.hand.Add(new FoeCard("Foe Card", "Robber Knight3", "Textures/foe/robberKnight", 25, 25));
    players.Add(player3);

    // player 4, although being tied for 1st, will have a hand that remains unchanged, because they have neither foes nor weapons
    Player player4 = new Player("Cheldon", new List<Card>(), new iStrategyCPU2() , "");
    player4.addShields(1);
    player4.hand.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
    player4.hand.Add(new AllyCard("Ally Card", "Sir Gaiwan", "Textures/Ally/sirGawain", 10, "+20 on the Test of the Green Knight Quest"));
    players.Add(player4);

    // Player 5 will have a weapon and a foe, but will have a hand that remains unchanged becauset they are not tied for 1st
    Player player5 = new Player("Test Guy", new List<Card>(), new iStrategyCPU1() , "");
    player5.hand.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
    player5.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player5.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
    players.Add(player5);


    // Counts should be equal to cards added into each player
    Assert.AreEqual(player0.hand.Count, 3);
    Assert.AreEqual(player1.hand.Count, 4);
    Assert.AreEqual(player2.hand.Count, 2);
    Assert.AreEqual(player3.hand.Count, 4);
    Assert.AreEqual(player4.hand.Count, 2);
    Assert.AreEqual(player5.hand.Count, 3);

    // execute the call
    call.execute(players, eCard, deck);

    // PLayer 0 shouldve discarded their only weapon card
    Assert.AreEqual(player0.hand.Count, 2);
    foreach(Card c in player0.hand){
      Assert.AreNotEqual(c.type, "Weapon Card");
    }

    // Player 1 should discard their lesser weapon
    Assert.AreEqual(player1.hand.Count, 3);
    foreach(Card c in player0.hand){
      Assert.AreNotEqual(c.name, "Big Sword");
    }

    // Player 2 Should discard their only foe
    Assert.AreEqual(player2.hand.Count, 1);
    foreach(Card c in player2.hand){
      Assert.AreNotEqual(c.type, "Foe Card");
    }

    //Player 3 Should discard their two lowest foes
    Assert.AreEqual(player3.hand.Count, 2);
    foreach(Card c in player3.hand){
      Assert.AreNotEqual(c.name, "Robber Knight1");
      Assert.AreNotEqual(c.name, "Robber Knight2");
    }

    //Player 4 hand will remain unchanged
    Assert.AreEqual(player4.hand.Count, 2);

    //Player 5 hand will remain unchanged
    Assert.AreEqual(player5.hand.Count, 3);
  }




  // ensures that if one player has a higher score than the rest they will be returned
  [Test]
  public void getHighestPlayers_givesPlayerWithSingleHighest() {
    iStoryKingsCallToArms call = new iStoryKingsCallToArms();
    List<Player> players = new List<Player>();

    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyPlayer() , "");
    player1.addShields(1);
    players.Add(player1);

    Player player2 = new Player("Kazma", new List<Card>(), new iStrategyPlayer() , "");
    players.Add(player2);

    Player player3 = new Player("Rotharn", new List<Card>(), new iStrategyPlayer() , "");
    players.Add(player3);

    Player player4 = new Player("Cheldon", new List<Card>(), new iStrategyPlayer() , "");
    players.Add(player4);

    List<Player> highestPlayers = call.getHighestPlayers(players);
    Player highestPlayer = highestPlayers[0];
    Assert.AreEqual(highestPlayers.Count, 1);
    Assert.AreEqual(highestPlayer, player1);
  }

  // Tests that multiple highest players will be returned in event of a tie
  [Test]
  public void getHighestPlayers_returnsMultipleWhenTied(){
    iStoryKingsCallToArms call = new iStoryKingsCallToArms();
    List<Player> players = new List<Player>();

    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyPlayer() , "");
    player1.addShields(1);
    players.Add(player1);

    Player player2 = new Player("Kazma", new List<Card>(), new iStrategyPlayer() , "");
    player2.addShields(1);
    players.Add(player2);

    Player player3 = new Player("Rotharn", new List<Card>(), new iStrategyPlayer() , "");
    players.Add(player3);

    Player player4 = new Player("Cheldon", new List<Card>(), new iStrategyPlayer() , "");
    players.Add(player4);

    List<Player> highestPlayers = call.getHighestPlayers(players);
    Assert.AreEqual(2, highestPlayers.Count);
    Assert.AreEqual(player1, highestPlayers[0]);
    Assert.AreEqual(player2, highestPlayers[1]);

    player3.addShields(1);
    highestPlayers = call.getHighestPlayers(players);
    Assert.AreEqual(3, highestPlayers.Count);
    Assert.AreEqual(player1, highestPlayers[0]);
    Assert.AreEqual(player2, highestPlayers[1]);
    Assert.AreEqual(player3, highestPlayers[2]);

    player4.addShields(1);
    highestPlayers = call.getHighestPlayers(players);
    Assert.AreEqual(4, highestPlayers.Count);
    Assert.AreEqual(player1, highestPlayers[0]);
    Assert.AreEqual(player2, highestPlayers[1]);
    Assert.AreEqual(player3, highestPlayers[2]);
    Assert.AreEqual(player4, highestPlayers[3]);
  }

  // Testing that the highest score amongst the players in the game can be returned properly
  [Test]
  public void getHighest_returnsCorrectInteger(){
    iStoryKingsCallToArms call = new iStoryKingsCallToArms();
    List<Player> players = new List<Player>();

    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyPlayer() , "");
    player1.addShields(1);
    players.Add(player1);

    Player player2 = new Player("Kazma", new List<Card>(), new iStrategyPlayer() , "");
    players.Add(player2);

    Player player3 = new Player("Rotharn", new List<Card>(), new iStrategyPlayer() , "");
    players.Add(player3);

    Player player4 = new Player("Cheldon", new List<Card>(), new iStrategyPlayer() , "");
    players.Add(player4);

    Assert.AreEqual(1, player1.score);
    Assert.AreEqual(player1.score, call.getHighest(players));
  }

/*
  [Test]
  public void executeChivalrousDeeds_adds3ShieldsToLowestPlayer()
  {
    iStoryChivalrousDeed deed = new iStoryChivalrousDeed();
    List<Player> players = new List<Player>();

    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyPlayer());
    player1.addShields(1);
    players.Add(player1);

    Player player2 = new Player("Kazma", new List<Card>(), new iStrategyPlayer());
    player2.addShields(1);
    players.Add(player2);

    Player player3 = new Player("Rotharn", new List<Card>(), new iStrategyPlayer());
    player3.addShields(1);
    players.Add(player3);

    Player player4 = new Player("Cheldon", new List<Card>(), new iStrategyPlayer());
    players.Add(player4);

    // Check the scores before the event
    Assert.AreEqual(1, player1.score);
    Assert.AreEqual(1, player2.score);
    Assert.AreEqual(1, player3.score);
    Assert.AreEqual(0, player4.score);

    // Run the deed
    deed.execute(players, 3);

    // Check the scores
    Assert.AreEqual(1, player1.score);
    Assert.AreEqual(1, player2.score);
    Assert.AreEqual(1, player3.score);
    Assert.AreEqual(3, player4.score);

    // Check that it works for a multiplayer tie
    deed.execute(players, 3);
    Assert.AreEqual(4, player1.score);
    Assert.AreEqual(4, player2.score);
    Assert.AreEqual(4, player3.score);
    Assert.AreEqual(3, player4.score);

  }
  */
}
