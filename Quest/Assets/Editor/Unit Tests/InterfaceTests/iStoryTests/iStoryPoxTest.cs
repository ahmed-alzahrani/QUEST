using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class iStoryPoxTest{
  [Test]
  public void execute_removes1ShieldFromAllButDrawer()
  {
    iStoryPox pox = new iStoryPox();
    EventCard eCard = new EventCard("Event Card", "Test Event", "description", "path", pox);
    Deck deck = new Deck("Deck", new List<Card>());
    List<Player> players = new List<Player>();

    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyPlayer() , "");
    player1.addShields(4);
    players.Add(player1);

    Player player2 = new Player("Kazma", new List<Card>(), new iStrategyPlayer() , "");
    player2.addShields(3);
    players.Add(player2);

    Player player3 = new Player("Rotharn", new List<Card>(), new iStrategyPlayer() , "");
    player3.addShields(2);
    players.Add(player3);

    Player player4 = new Player("Cheldon", new List<Card>(), new iStrategyPlayer() , "");
    player4.addShields(1);
    players.Add(player4);


    Assert.AreEqual(player1.score, 4);
    Assert.AreEqual(player2.score, 3);
    Assert.AreEqual(player3.score, 2);
    Assert.AreEqual(player4.score, 1);

    pox.execute(players, eCard, deck);

    Assert.AreEqual(player1.score, 4);
    Assert.AreEqual(player2.score, 2);
    Assert.AreEqual(player3.score, 1);
    Assert.AreEqual(player4.score, 0);
  }
}
