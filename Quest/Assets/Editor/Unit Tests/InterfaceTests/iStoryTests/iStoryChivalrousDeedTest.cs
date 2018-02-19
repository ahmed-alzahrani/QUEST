using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class iStoryChivalrousDeedTest{

  // ensures that if one player is lower than all the rest, they will be the only player in the result of getLowestPlayers
  [Test]
  public void getLowestPlayers_givesPlayerWithSingleLowest() {
    iStoryChivalrousDeed deed = new iStoryChivalrousDeed();
    List<Player> players = new List<Player>();

    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyPlayer(), "");
    player1.addShields(1);
    players.Add(player1);

    Player player2 = new Player("Kazma", new List<Card>(), new iStrategyPlayer(), "");
    player2.addShields(1);
    players.Add(player2);

    Player player3 = new Player("Rotharn", new List<Card>(), new iStrategyPlayer() , "");
    player3.addShields(1);
    players.Add(player3);

    Player player4 = new Player("Cheldon", new List<Card>(), new iStrategyPlayer() , "");
    players.Add(player4);

    List<Player> lowestPlayers = deed.getLowestPlayers(players);
    Player lowestPlayer = lowestPlayers[0];
    Assert.AreEqual(lowestPlayers.Count, 1);
    Assert.AreEqual(lowestPlayer, player4);
  }

  [Test]
  public void getLowestPlayers_returnsMultipleWhenTied(){
    iStoryChivalrousDeed deed = new iStoryChivalrousDeed();
    List<Player> players = new List<Player>();

    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyPlayer() ,"");
    player1.addShields(1);
    players.Add(player1);

    Player player2 = new Player("Kazma", new List<Card>(), new iStrategyPlayer() , "");
    player2.addShields(1);
    players.Add(player2);

    Player player3 = new Player("Rotharn", new List<Card>(), new iStrategyPlayer() , "");
    players.Add(player3);

    Player player4 = new Player("Cheldon", new List<Card>(), new iStrategyPlayer() , "");
    players.Add(player4);

    List<Player> lowestPlayers = deed.getLowestPlayers(players);
    Assert.AreEqual(2, lowestPlayers.Count);
    Assert.AreEqual(player3, lowestPlayers[0]);
    Assert.AreEqual(player4, lowestPlayers[1]);

    player2.removeShields(1);
    lowestPlayers = deed.getLowestPlayers(players);
    Assert.AreEqual(3, lowestPlayers.Count);
    Assert.AreEqual(player2, lowestPlayers[0]);
    Assert.AreEqual(player3, lowestPlayers[1]);
    Assert.AreEqual(player4, lowestPlayers[2]);

    player1.removeShields(1);
    lowestPlayers = deed.getLowestPlayers(players);
    Assert.AreEqual(4, lowestPlayers.Count);
    Assert.AreEqual(player1, lowestPlayers[0]);
    Assert.AreEqual(player2, lowestPlayers[1]);
    Assert.AreEqual(player3, lowestPlayers[2]);
    Assert.AreEqual(player4, lowestPlayers[3]);
  }

  // Testing that a player has shields properly deducted in a case where they rank down from Champion Knight to Knight
  [Test]
  public void getLowest_returnsCorrectInteger(){
    iStoryChivalrousDeed deed = new iStoryChivalrousDeed();
    List<Player> players = new List<Player>();

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

    Assert.AreEqual(0, player4.score);
    Assert.AreEqual(player4.score, deed.getLowest(players));
  }

  [Test]
  public void executeChivalrousDeeds_adds3ShieldsToLowestPlayer()
  {
    iStoryChivalrousDeed deed = new iStoryChivalrousDeed();
    List<Player> players = new List<Player>();

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
}
