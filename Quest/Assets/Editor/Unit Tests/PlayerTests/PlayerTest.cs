using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class PlayerTest{

  [Test]
  public void Player_CreatedWithGiven_WillHaveTheVariables() {
    var player = new Player("Ahmed", new List<Card>(), new iStrategyPlayer());
    Assert.AreEqual("Ahmed", player.name);
    Assert.AreEqual(0, player.score);
    Assert.AreEqual(0, player.activeAllies.Count);
    Assert.AreEqual(2, player.rankCards.Count);
    Assert.IsNotNull(player.hand);
    Assert.IsNotNull(player.activeAllies);
    Assert.IsNotNull(player.rankCard);
    Assert.IsNotNull(player.rankCards);
    Assert.IsNotNull(player.strategy);
  }

  [Test]
  public void Player_StartsWithRankCard_Squire(){
    var player = new Player("Ahmed", new List<Card>(), new iStrategyPlayer());
    Assert.AreEqual("Squire", player.rankCard.name);
    Assert.AreEqual("Rank Card", player.rankCard.type);
    Assert.AreEqual("Textures/Ranks/squire", player.rankCard.texturePath);
    Assert.AreEqual(5, player.rankCard.battlePoints);
  }

  public void Player_RankCardsInOrder_KnightThenChampionKnight(){
    var player = new Player("Ahmed", new List<Card>(), new iStrategyPlayer());

    var knightCard = player.rankCards[0];
    Assert.AreEqual("Knight", knightCard.name);
    Assert.AreEqual("Rank Card", knightCard.type);
    Assert.AreEqual("Textures/Ranks/knight", knightCard.texturePath);
    Assert.AreEqual(10, knightCard.battlePoints);

    var champKnightCard = player.rankCards[1];
    Assert.AreEqual("Champion Knight", champKnightCard.name);
    Assert.AreEqual("Rank Card", champKnightCard.type);
    Assert.AreEqual("Textures/Ranks/championKnight", champKnightCard.texturePath);
    Assert.AreEqual(20, champKnightCard.battlePoints);
  }

// Testing that a player has shields added properly in a case where he doesn't rank up
  [Test]
  public void Player_EarnsShields_NoRankUp(){
    var player = new Player("Ahmed", new List<Card>(), new iStrategyPlayer());
    player.addShields(2);
    Assert.AreEqual(2, player.score);
    Assert.AreEqual("Squire", player.rankCard.name);
  }

  // Testing that a player has shields added properly in a case where they rank up from Squire to Knight
  [Test]
  public void Player_EarnsShields_SquireToKnight(){
    var player = new Player("Ahmed", new List<Card>(), new iStrategyPlayer());
    player.addShields(5);
    Assert.AreEqual(5, player.score);
    Assert.AreEqual("Knight", player.rankCard.name);
    Assert.AreEqual(10, player.rankCard.battlePoints);
    for(int i = 0; i < player.rankCards.Count; i++)
    {
      player.rankCards[i].display();
    }
    Debug.Log("Ok now the player's card....");
    player.rankCard.display();
  }

  // Testing that a player has shields added properly in a case where they rank up from Knight to Champion Knight
  [Test]
  public void Player_EarnsShields_KnightToChampionKnight(){
    var player = new Player("Ahmed", new List<Card>(), new iStrategyPlayer());
    player.addShields(5);
    player.addShields(7);
    Assert.AreEqual(12, player.score);
    Assert.AreEqual("Champion Knight", player.rankCard.name);
    Assert.AreEqual(20, player.rankCard.battlePoints);
  }

  // Testing that a player has shields added properly in a case where they rank up from Squire to Champion Knight
  [Test]
  public void Player_EarnsShields_SquireToChampionKnight(){
    var player = new Player("Ahmed", new List<Card>(), new iStrategyPlayer());
    player.addShields(12);
    Assert.AreEqual(12, player.score);
    Assert.AreEqual("Champion Knight", player.rankCard.name);
    Assert.AreEqual(20, player.rankCard.battlePoints);
  }

  // Testing that a player has shields deducted properly in a case where they do not rank down
  [Test]
  public void Player_LosesShields_NoRankDown(){
    var player = new Player("Ahmed", new List<Card>(), new iStrategyPlayer());
    player.addShields(2);

    player.removeShields(1);
    Assert.AreEqual(1, player.score);

    player.removeShields(2);
    Assert.AreEqual(0, player.score);
    Assert.AreEqual("Squire", player.rankCard.name);

  }

  // Testing that a player has shields deducted properly deducted in a case where they rank down from Knight to Squire
  [Test]
  public void Player_LosesShields_KnightToSquire(){
    var player = new Player("Ahmed", new List<Card>(), new iStrategyPlayer());
    player.addShields(5);
    Assert.AreEqual("Knight", player.rankCard.name);

    player.removeShields(1);
    Assert.AreEqual("Squire", player.rankCard.name);
    Assert.AreEqual(4, player.score);
    Assert.AreEqual(5, player.rankCard.battlePoints);
  }

  // Testing that a player has shields properly deducted in a case where they rank down from Champion Knight to Knight
  [Test]
  public void Player_LosesShields_ChampKnightToKnight(){
    var player = new Player("Ahmed", new List<Card>(), new iStrategyPlayer());
    player.addShields(12);
    Assert.AreEqual(20, player.rankCard.battlePoints);

    player.removeShields(1);
    Assert.AreEqual("Knight", player.rankCard.name);
    Assert.AreEqual(10, player.rankCard.battlePoints);
  }

  [Test]
  public void courtCalled_emptiesActiveAllies()
  {
    var player = new Player("Ahmed", new List<Card>(), new iStrategyPlayer());
    player.activeAllies.Add(new AllyCard("Ally Card", "Sir Gaiwan", "Textures/Ally/sirGawain", 10, 0,"+20 on the Test of the Green Knight Quest", "", "", 0, 0, new NoBuff()));
    player.activeAllies.Add(new AllyCard("Ally Card", "King Pellinore", "Textures/Ally/kingPellinore", 10, 0, "4 Bids on the Search for the Questing Beast Quest", "", "", 0, 0, new NoBuff()));
    player.activeAllies.Add(new AllyCard("Ally Card", "Sir Percival", "Textures/Ally/sirPercival", 5, 0, "+ 20 on the Search for the Holy Grail Quest", "", "", 0, 0, new NoBuff()));
    player.activeAllies.Add(new AllyCard("Ally Card", "Sir Tristan", "Textures/Ally/sirTristan", 10, 0, "+ 20 when Queen Iseult is in play", "", "", 0, 0, new NoBuff()));
    player.activeAllies.Add(new AllyCard("Ally Card", "King Arthur", "Textures/Ally/kingArthur", 10, 0, "+ 2 Bids", "", "", 0, 0, new NoBuff()));
    player.activeAllies.Add(new AllyCard("Ally Card", "Queen Guinevere", "Textures/Ally/queenGuinevere", 0, 0, "+ 2 Bids", "", "", 0, 0, new NoBuff()));

    Assert.AreEqual(6, player.activeAllies.Count);

    List<Card> allies = player.courtCalled();

    Assert.AreEqual(6, allies.Count);
    Assert.AreEqual("Sir Gaiwan", allies[0].name);
    Assert.AreEqual("King Pellinore", allies[1].name);
    Assert.AreEqual("Sir Percival", allies[2].name);
    Assert.AreEqual("Sir Tristan", allies[3].name);
    Assert.AreEqual("King Arthur", allies[4].name);
    Assert.AreEqual("Queen Guinevere", allies[5].name);
    Assert.AreEqual(0, player.activeAllies.Count);
  }

  [Test]
  public void hasWeapon_returnsTrueWhenHandHasWeapon()
  {
      var player = new Player("Ahmed", new List<Card>(), new iStrategyPlayer());
      player.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
      Assert.IsTrue(player.hasWeapon());
  }

  [Test]
  public void hasWeapon_returnsFalseWhenHandHasNoWeapon()
  {
      var player = new Player("Ahmed", new List<Card>(), new iStrategyPlayer());
      Assert.IsFalse(player.hasWeapon());
  }

  public void hasFoe_returnsTrueWhenHandHasFoe()
  {
    var player = new Player("Ahmed", new List<Card>(), new iStrategyPlayer());
    player.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    Assert.IsTrue(player.hasFoe());
  }

  public void hasFoe_returnsFalseWhenHandHasNoFoe()
  {
    var player = new Player("Ahmed", new List<Card>(), new iStrategyPlayer());
    Assert.IsFalse(player.hasFoe());
  }

  [Test]
  public void kingsCall_PlayerHasWeapon()
  {
    var player = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    player.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
    player.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));

    List<Card> discard = player.kingsCall();
    Assert.AreEqual(discard.Count, 1);
    Assert.AreEqual(discard[0].type, "Weapon Card");
    Assert.AreEqual(player.hand.Count, 1);
    Assert.AreEqual(player.hand[0].type, "Foe Card");
  }

  [Test]
  public void kingsCall_PlayerHasOneFoe()
  {
    var player = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    player.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));

    List<Card> discard = player.kingsCall();
    Assert.AreEqual(discard.Count, 1);
    Assert.AreEqual(discard[0].type, "Foe Card");
    Assert.AreEqual(player.hand.Count, 0);
  }

  [Test]
  public void kingsCall_PlayerHasMultipleFoes()
  {
    var player = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
      player.hand.Add(new FoeCard("Foe Card", "Robber Knight1", "Textures/foe/robberKnight", 15, 15));
      player.hand.Add(new FoeCard("Foe Card", "Robber Knight2", "Textures/foe/robberKnight", 20, 20));
      player.hand.Add(new FoeCard("Foe Card", "Robber Knight3", "Textures/foe/robberKnight", 25, 25));

      List<Card> discard = player.kingsCall();
      Assert.AreEqual(2, discard.Count);
      Assert.AreEqual(1, player.hand.Count);
      FoeCard someFoe = (FoeCard)player.hand[0];
      Assert.AreEqual(25, someFoe.minBP);
      FoeCard foe1 = (FoeCard)discard[0];
      FoeCard foe2 = (FoeCard)discard[1];
      Assert.AreEqual(15, foe1.minBP);
      Assert.AreEqual(20, foe2.minBP);
  }

  [Test]
  public void kingsCall_PlayerHasNeither()
  {
    var player = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    List<Card> cards = player.kingsCall();
    Assert.AreEqual(0, cards.Count);
  }

  [Test]
  public void calculateBid_Sums_A_Players_ActiveAllies()
  {
    List<Player> players = new List<Player>();

    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    player1.activeAllies.Add(new AllyCard("Ally Card", "Some Ally", "texture", 5, 1, "some ability", "", "", 0, 0, new NoBuff()));
    players.Add(player1);

    Player player2 = new Player("Kazma", new List<Card>(), new iStrategyPlayer());
    player2.activeAllies.Add(new AllyCard("Ally Card", "Some Ally", "texture", 5, 1, "some ability", "", "", 0, 0, new NoBuff()));
    player2.activeAllies.Add(new AllyCard("Ally Card", "Some Ally", "texture", 5, 1, "some ability", "", "", 0, 0, new NoBuff()));
    players.Add(player2);

    Player player3 = new Player("Rotharn", new List<Card>(), new iStrategyPlayer());
    player3.activeAllies.Add(new AllyCard("Ally Card", "Some Ally", "texture", 5, 1, "some ability", "", "", 0, 0, new NoBuff()));
    player3.activeAllies.Add(new AllyCard("Ally Card", "Some Ally", "texture", 5, 1, "some ability", "", "", 0, 0, new NoBuff()));
    player3.activeAllies.Add(new AllyCard("Ally Card", "Some Ally", "texture", 5, 1, "some ability", "", "", 0, 0, new NoBuff()));
    players.Add(player3);

    Player player4 = new Player("Cheldon", new List<Card>(), new iStrategyPlayer());
    players.Add(player4);

    Assert.AreEqual(0, player4.calculateBid("", players));
    Assert.AreEqual(1, player1.calculateBid("", players));
    Assert.AreEqual(2, player2.calculateBid("", players));
    Assert.AreEqual(3, player3.calculateBid("", players));
  }

  [Test]
  public void calculateBP_Sums_A_Playes_BP_BasedOnRankAnd_ActiveAllies()
  {
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    List<Player> players = new List<Player>();
    players.Add(player1);
    player1.activeAllies.Add(new AllyCard("Ally Card", "Some Ally", "texture", 5, 1, "some ability", "None", "None", 0, 0, new NoBuff()));
    Assert.AreEqual(player1.CalculateBP("", players), 10);
  }
}
