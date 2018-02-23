using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;


public class BuffOnQuestEffectTest{

  [Test]
  public void BuffOnQuest_returns0_ifQuestIsWrong(){
    BuffOnQuestEffect buff = new BuffOnQuestEffect();
    Assert.AreEqual(buff.BuffOnQuest("quest", "otherQuest", 1), 0);
  }

  [Test]
  public void BuffOnQuest_returns1_onProperQuest(){
    BuffOnQuestEffect buff = new BuffOnQuestEffect();
    Assert.AreEqual(buff.BuffOnQuest("quest", "quest", 1), 1);
  }

  [Test]
  public void BuffOnCardInPlay_returns0_ifCardNotInPlay(){
    BuffOnQuestEffect buff = new BuffOnQuestEffect();
    List<Player> players = new List<Player>();
    Player player1 = new Player("ahmed", new List<Card>(), new iStrategyPlayer());
    player1.activeAllies.Add(new AllyCard("Ally Card", "name", "texture", 5, 5, "special", "", "", 0, 0, new NoBuff()));
    players.Add(player1);
    Assert.AreEqual(buff.BuffOnCardInPlay(players, "other_name", 1), 0);

  }

  [Test]
  public void BuffOnCardInPlay_returns0_evenIfCardInPlay(){
    BuffOnQuestEffect buff = new BuffOnQuestEffect();
    List<Player> players = new List<Player>();
    Player player1 = new Player("ahmed", new List<Card>(), new iStrategyPlayer());
    player1.activeAllies.Add(new AllyCard("Ally Card", "name", "texture", 5, 5, "special", "", "", 0, 0, new NoBuff()));
    players.Add(player1);
    Assert.AreEqual(buff.BuffOnCardInPlay(players, "name", 1), 0);

  }
}
