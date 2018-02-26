using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class iStoryPlagueTest{
  [Test]
  public void execute_removes2ShieldsFromDrawer()
  {
    iStoryPlague plague = new iStoryPlague();
    GameController game = new GameController();
    game.isDoneStoryEvent = false;
    EventCard eCard = new EventCard("Event Card", "Test Event", "description", "path", plague);
    List<Player> players = new List<Player>();

    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyPlayer() , "");
    player1.addShields(3);
    players.Add(player1);

    Player player2 = new Player("Kazma", new List<Card>(), new iStrategyPlayer() , "");
    player2.addShields(2);
    players.Add(player2);

    Player player3 = new Player("Rotharn", new List<Card>(), new iStrategyPlayer() , "");
    player3.addShields(2);
    players.Add(player3);

    Player player4 = new Player("Cheldon", new List<Card>(), new iStrategyPlayer() , "");
    player4.addShields(2);
    players.Add(player4);


    Assert.AreEqual(player1.score, 3);
    Assert.AreEqual(player2.score, 2);
    Assert.AreEqual(player3.score, 2);
    Assert.AreEqual(player4.score, 2);

    plague.execute(players, eCard, game);

    Assert.AreEqual(player1.score, 1);
    Assert.AreEqual(player2.score, 2);
    Assert.AreEqual(player3.score, 2);
    Assert.AreEqual(player4.score, 2);
    Assert.AreEqual(true, game.isDoneStoryEvent);
  }
}
