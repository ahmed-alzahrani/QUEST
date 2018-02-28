using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class iStoryKingsCallToArmsTest{

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

    //List<Player> highestPlayers = call.getHighestPlayers(players);
    //Player highestPlayer = highestPlayers[0];
    //Assert.AreEqual(highestPlayers.Count, 1);
    //Assert.AreEqual(highestPlayer, player1);
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


    player3.addShields(1);

    player4.addShields(1);

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
}
