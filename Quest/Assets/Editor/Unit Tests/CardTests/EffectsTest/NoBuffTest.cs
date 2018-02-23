using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;


public class NoBuffTest{

  [Test]
  public void BuffOnQuest_alwasyReturns0_evenOnProperQuest(){
    NoBuff nB = new NoBuff();
    Assert.AreEqual(nB.BuffOnQuest("quest", "quest", 1), 0);
  }

  [Test]
  public void BuffOnCardInPlay_alwaysReturns0_evenOnProperAlly(){
    NoBuff nb = new NoBuff();
    List<Player> players = new List<Player>();
    Player player1 = new Player("ahmed", new List<Card>(), new iStrategyPlayer());
    player1.activeAllies.Add(new AllyCard("Ally Card", "name", "texture", 5, 5, "special", "", "", 0, 0, new NoBuff()));

    Assert.AreEqual(nb.BuffOnCardInPlay(players, "name", 1), 0);
  }
}
