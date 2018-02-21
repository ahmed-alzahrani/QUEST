using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class iStoryCourtCalledTest{

  // ensures that if one player is lower than all the rest, they will be the only player in the result of getLowestPlayers
  [Test]
  public void execute_EmptiesAllPlayersAllyCards() {
    iStoryCourtCalled court = new iStoryCourtCalled();
    EventCard eCard = new EventCard("Event Card", "Test Event", "description", "path", court);
     Deck deck = new Deck("Deck", new List<Card>());
     List<Player> players = new List<Player>();

    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyPlayer() , "");
    player1.activeAllies.Add(new AllyCard("Ally Card", "Sir Gaiwan", "Textures/Ally/sirGawain", 10, "+20 on the Test of the Green Knight Quest"));
    players.Add(player1);

    Player player2 = new Player("Kazma", new List<Card>(), new iStrategyPlayer() , "");
    player2.activeAllies.Add(new AllyCard("Ally Card", "King Pellinore", "Textures/Ally/kingPellinore", 10, "4 Bids on the Search for the Questing Beast Quest"));
    players.Add(player2);

    Player player3 = new Player("Rotharn", new List<Card>(), new iStrategyPlayer() , "");
    player3.activeAllies.Add(new AllyCard("Ally Card", "Sir Percival", "Textures/Ally/sirPercival", 5, "+ 20 on the Search for the Holy Grail Quest"));
    players.Add(player3);

    Player player4 = new Player("Cheldon", new List<Card>(), new iStrategyPlayer() , "");
    player4.activeAllies.Add(new AllyCard("Ally Card", "Sir Tristan", "Textures/Ally/sirTristan", 10, "+ 20 when Queen Iseult is in play"));
    players.Add(player4);

    Assert.AreEqual(1, player1.activeAllies.Count);
    Assert.AreEqual(1, player2.activeAllies.Count);
    Assert.AreEqual(1, player3.activeAllies.Count);
    Assert.AreEqual(1, player4.activeAllies.Count);

    court.execute(players, eCard, deck);

    Assert.AreEqual(0, player1.activeAllies.Count);
    Assert.AreEqual(0, player2.activeAllies.Count);
    Assert.AreEqual(0, player3.activeAllies.Count);
    Assert.AreEqual(0, player4.activeAllies.Count);
  }
}
