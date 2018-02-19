using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class iStrategyCPU1Test{
  [Test]
  public void participateInTourney_FalseWhenNoOneCanRankUp(){
    List<Player> players = new List<Player>();

    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    players.Add(player1);

    Player player2 = new Player("Kazma", new List<Card>(), new iStrategyPlayer());
    players.Add(player2);

    Player player3 = new Player("Rotharn", new List<Card>(), new iStrategyPlayer());
    players.Add(player3);

    Player player4 = new Player("Cheldon", new List<Card>(), new iStrategyPlayer());
    players.Add(player4);

    Assert.IsFalse(player1.strategy.participateInTourney(players, 3));
  }

  [Test]
  public void participateInTourney_TrueWhenSomeoneCanRankUp(){
    List<Player> players = new List<Player>();

    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    players.Add(player1);

    Player player2 = new Player("Kazma", new List<Card>(), new iStrategyPlayer());
    players.Add(player2);

    Player player3 = new Player("Rotharn", new List<Card>(), new iStrategyPlayer());
    players.Add(player3);

    Player player4 = new Player("Cheldon", new List<Card>(), new iStrategyPlayer());
    player4.addShields(2);
    players.Add(player4);

    Assert.IsTrue(player1.strategy.participateInTourney(players, 3));
  }

  // Tests that the player plays the strongest hand in a high stakes tournament
  [Test]
  public void highStakesTournament_CPU_playsStrongestHand(){
    List<Player> players = new List<Player>();

    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    players.Add(player1);
    player1.addShields(2);


    player1.hand.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));

    // Only one horse should show up in the cards played
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Excalibur", "Textures/weapons/excalibur", 30));

    //Queen Guinevere should be left over in the hand because it has 0bp, cant be played
    player1.hand.Add(new AllyCard("Ally Card", "Queen Guinevere", "Textures/Ally/queenGuinevere", 0, "+ 2 Bids"));
    player1.hand.Add(new AllyCard("Ally Card", "Sir Galahad", "Textures/Ally/sirGalahad", 15, ""));

    List<Card> strongestHand = player1.strategy.playTournament(players, player1.hand, 5, 3);
    Assert.AreEqual(strongestHand.Count, 4);
    Assert.AreEqual(strongestHand[0].name, "Amour");
    Assert.AreEqual(strongestHand[1].name, "Horse");
    Assert.AreEqual(strongestHand[2].name, "Excalibur");
    Assert.AreEqual(strongestHand[3].name, "Sir Galahad");

    Assert.AreEqual(player1.hand[0].name, "Horse");
    Assert.AreEqual(player1.hand[1].name, "Queen Guinevere");
  }

  [Test]
  public void lowStakesTournament_CPU_playsDuplicatWeapons(){
    List<Player> players = new List<Player>();

    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    players.Add(player1);

    player1.hand.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));

    // Only one horse should show up in the cards played
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Excalibur", "Textures/weapons/excalibur", 30));

    //Queen Guinevere should be left over in the hand because it has 0bp, cant be played
    player1.hand.Add(new AllyCard("Ally Card", "Queen Guinevere", "Textures/Ally/queenGuinevere", 0, "+ 2 Bids"));
    player1.hand.Add(new AllyCard("Ally Card", "Sir Galahad", "Textures/Ally/sirGalahad", 15, ""));

    List<Card> lowStakesPlay = player1.strategy.playTournament(players, player1.hand, 5, 3);

    Assert.AreEqual(lowStakesPlay.Count, 1);
    Assert.AreEqual(lowStakesPlay[0].name, "Horse");
    Assert.AreEqual(player1.hand.Count, 5);
  }

  [Test]
  public void sponsorQuest_ReturnFalse_BecauseSomeoneCanRankUp(){
    List<Player> players = new List<Player>();
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    // give the player requisite cards to sponsor the quest to ensure it fails because someone can rank up
    player1.hand.Add(new TestCard("Test Card", "Test of the Questing Beast", "Textures/tests/testOfTheQuestingBeast", 4));
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new FoeCard("Foe Card", "Saxons", "Textures/foe/saxons", 10, 20));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
    // Player 1 has 3 shields, the quest has 3 stages, 3 + 3 = 6, the player would rank up so decline to sponsor
    player1.addShields(3);
    players.Add(player1);

    Assert.IsFalse(player1.strategy.sponsorQuest(players, 3, player1.hand));
  }

  [Test]
  public void sponsorQuest_ReturnFalse_Because_NotEnoughFoes(){
    List<Player> players = new List<Player>();
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    players.Add(player1);

    Assert.IsFalse(player1.strategy.sponsorQuest(players, 3, player1.hand));
  }

  [Test]
  public void sponsorQuest_ReturnFalse_Because_DuplicateFoes(){
    List<Player> players = new List<Player>();
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    players.Add(player1);
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));

    Assert.IsFalse(player1.strategy.sponsorQuest(players, 3, player1.hand));
  }

  [Test]
  public void sponsorQuest_ReturnsTrue_Because_CanSponsor_BecauseTest(){
    List<Player> players = new List<Player>();
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    players.Add(player1);
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new TestCard("Test Card", "Test of the Questing Beast", "Textures/tests/testOfTheQuestingBeast", 4));

    Assert.IsTrue(player1.strategy.sponsorQuest(players, 3, player1.hand));
  }

  [Test]
  public void sponsorQuest_ReturnsTrue_Because_CanSponsor_WithoutTest(){
    List<Player> players = new List<Player>();
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    players.Add(player1);
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new FoeCard("Foe Card", "Saxons", "Textures/foe/saxons", 10, 20));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));

    Assert.IsTrue(player1.strategy.sponsorQuest(players, 3, player1.hand));
  }

  [Test]
  public void setUpFoeStage_EarlyFoe_WeakestFoe_NoDuplicateWeapons(){
    List<Player> players = new List<Player>();
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new FoeCard("Foe Card", "Saxons", "Textures/foe/saxons", 10, 20));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));

    List<Card> foeStage = player1.strategy.setupFoeStage(1, 5, player1.hand);

    Assert.AreEqual(player1.hand.Count, 4);
    Assert.AreEqual(foeStage.Count, 1);
    Assert.AreEqual(foeStage[0].name, "Thieves");
    Assert.AreEqual(player1.hand[0].name, "Robber Knight");
    Assert.AreEqual(player1.hand[1].name, "Saxons");
    Assert.AreEqual(player1.hand[2].name, "Horse");
    Assert.AreEqual(player1.hand[3].name, "Sword");
  }

  [Test]
  public void setUpFoeStage_EarlyFoe_WeakestFoe_1DuplicateWeapon(){
    List<Player> players = new List<Player>();
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new FoeCard("Foe Card", "Saxons", "Textures/foe/saxons", 10, 20));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));

    List<Card> foeStage = player1.strategy.setupFoeStage(1, 5, player1.hand);

    Assert.AreEqual(player1.hand.Count, 4);
    Assert.AreEqual(foeStage.Count, 2);
    Assert.AreEqual(foeStage[0].name, "Thieves");
    Assert.AreEqual(foeStage[1].name, "Sword");
    Assert.AreEqual(player1.hand[0].name, "Robber Knight");
    Assert.AreEqual(player1.hand[1].name, "Saxons");
    Assert.AreEqual(player1.hand[2].name, "Horse");
    Assert.AreEqual(player1.hand[3].name, "Sword");
  }

  [Test]
  public void setUpFoeStage_EarlyFoe_WeakestFoe_MultipleDuplicateWeapons(){
    List<Player> players = new List<Player>();
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new FoeCard("Foe Card", "Saxons", "Textures/foe/saxons", 10, 20));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));

    List<Card> foeStage = player1.strategy.setupFoeStage(1, 5, player1.hand);

    Assert.AreEqual(player1.hand.Count, 4);
    Assert.AreEqual(foeStage.Count, 3);
    Assert.AreEqual(foeStage[0].name, "Thieves");
    Assert.AreEqual(foeStage[1].name, "Horse");
    Assert.AreEqual(foeStage[2].name, "Sword");
    Assert.AreEqual(player1.hand[0].name, "Robber Knight");
    Assert.AreEqual(player1.hand[1].name, "Saxons");
    Assert.AreEqual(player1.hand[2].name, "Horse");
    Assert.AreEqual(player1.hand[3].name, "Sword");
  }

  /*

  // Shows the user exhausting their weapon/amour/ally options but not reaching 50bp
  [Test]
  public void setUpFinalFoe_CantReach50BP(){

  }

  // User gets to 50BP ASAP for final foe
  [Test]
  public void setUpFinalFoe_Reaches50BP(){

  }

  // Tests that the user automatically selects their best test card for the test stage
  [Test]
  public void setUpTestStage_AlwaysGetsBestTestCard(){

  }

  // Checks a user's set up quest with no Test Stage, shows that the BP automatically decreases
  [Test]
  public void setUpQuest_NoTestStage_FoeStagesDecrease(){

  }

  // Sets up a quest with test stage in hand and verifies test comes second last
  [Test]
  public void setUpQuest_TestStage_AlwaysSecondLast(){

  }


  // Testing CPU1 implementation of set up quest
  // The final foe stage is fewest cards to get to 50BP.
  // After that, if there is a test card that comes next
  // Then earlier foe encounters are just the weakest foe with any duplicate weapons

  // Tests that the player won't participate in quest if there aren't 2 cards to play per stage
  [Test]
  public void participateInQuest_returnsFalse_NotEnoughCards()
  {

  }

  // Tests that the player won't particiapte in the quest if they don't have 2 foes handy for discarding
  [Test]
  public void participateInQuest_returnsFalse_canNotDiscard()
  {

  }

  // Tests that the player will participate if both conditions (playing and discarding) are met
  [Test]
  public void participateInQuest_returnsTrue_canParticipate()
  {

  }

  // Tests that the player will participate in this early foe encounter with 1 amour 1 ally (lowest ally with BP)
  [Test]
  public void playFoeEncounter_EarlierFoe_2Allies_AmourInHand_AmourFalse()
  {

  }

  //Test that the player will particiapte in the early foe encounter with 2 allies because amour is true
  [Test]
  public void playFoeEncounter_EarlierFoe_2Allies_AmourInHand_AmourTrue()
  {

  }

  //Test that the player will participate in the early foe encounter with 1 weak ally and 1 weak weapon
  [Test]
  public void playFoeEncounter_EarlierFoe_1Ally_1Weapon()
  {

  }

  // Tests that the player will participate in the early foe encounter with 2 weapons
  [Test]
  public void playFoeEncounter_EarlierFoe_2Weapons()
  {

  }

  // Tests that the player enters with 1 weapon if thats all they have
  [Test]
  public void playFoeEncounter_EarlierFoe_1Weapon_NoAllies()
  {

  }

  // Tests that the player will play the earlier foe encounter with no cards if none are available
  [Test]
  public void playFoeEncounter_EarlierFoe_NoCards()
  {

  }

  // Tests that the player enters a Final Foe Encounter with the strongest possible hand
  [Test]
  public void playFoeEncounter_FinalFoe_StrongestHand()
  {

  }

  // Tests WillIBid on round 2, Player has enough cards in hand but this CPU doesn't bid on round 2
  [Test]
  public void playBid_Round2_Returns0()
  {

  }

  // Tests PlayBid player has Foes, but none with less than 20 minBP
  [Test]
  public void playBid_Round1_NoWeakFoes()
  {

  }

  // Tests playBid player has multiple weak foes, enough to bid more than the min bid
  [Test]
  public void playBid_Round1_ReturnsTrue_CanBid()
  {

  }

  // Tests playbid player has multiple weak foes but not enoughto bid
  [Test]
  public void playBid_Round1_ReturnsFalse_CantBid()
  {

  }
  */
}
