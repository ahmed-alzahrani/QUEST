using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class iStrategyCPU2Test{
  [Test]
  public void participateInTourney_True_Regardless(){
    List<Player> players = new List<Player>();
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    players.Add(player1);

    Assert.AreEqual(player1.strategy.participateInTourney(players, 3, null), 1);

    player1.addShields(4);
    Assert.AreEqual(player1.strategy.participateInTourney(players, 3, null), 1);
  }



  // Tests the player will play their most powerful combination if they can't hit 50BP
  [Test]
  public void playTournament_returnBestHandPossible_LessThan50BP()
  {
    List<Player> players = new List<Player>();
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());

    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Dagger", "Textures/weapons/dagger", 6));
    player1.hand.Add(new AllyCard("Ally Card", "Sir Gawain", "Textures/Ally/sirGawain", 4, 0, "Gawain tingz", "Test of the Green Knight", "None", 0, 20, new BuffOnQuestEffect()));
    player1.hand.Add(new AllyCard("Ally Card", "Sir Gawain", "Textures/Ally/sirGawain", 4, 0, "Gawain yadig?", "Test of the Green Knight", "None", 0, 20, new BuffOnQuestEffect()));

    List<Card> playedCards = player1.strategy.playTournament(players, player1.hand, player1.CalculateBP("", players), 2);

    // Make suret that playing cards for tourneys is removing them from hands properly
    Assert.AreEqual(1, player1.hand.Count);

    // Should have played one of each weapons and the duplicate allies
    Assert.AreEqual(4, playedCards.Count);
    Assert.AreEqual(playedCards[0].name, "Horse");
    Assert.AreEqual(playedCards[1].name, "Dagger");
    Assert.AreEqual(playedCards[2].name, "Sir Gawain");
    Assert.AreEqual(playedCards[3].name, "Sir Gawain");
  }

  // Tests that the player will play their fastest hand possible to 50BP
  [Test]
  public void playTournament_fastestPlayTo50BP()
  {
    List<Player> players = new List<Player>();
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());

    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Dagger", "Textures/weapons/dagger", 6));
    player1.hand.Add(new AllyCard("Ally Card", "Sir Gawain", "Textures/Ally/sirGawain", 4, 0, "Gawain tingz", "Test of the Green Knight", "None", 0, 20, new BuffOnQuestEffect()));
    player1.hand.Add(new AllyCard("Ally Card", "Sir Gawain", "Textures/Ally/sirGawain", 4, 0, "Gawain yadig?", "Test of the Green Knight", "None", 0, 20, new BuffOnQuestEffect()));
    player1.hand.Add(new WeaponCard("Weapon Card", "Mega Weapon", "Textures/weapons/dagger", 45));

    List<Card> playedCards = player1.strategy.playTournament(players, player1.hand, player1.CalculateBP("", players), 2);

    Debug.Log(player1.CalculateBP("", players));

    Assert.AreEqual(5, player1.hand.Count);
    Assert.AreEqual(1, playedCards.Count);
    Assert.AreEqual("Mega Weapon", playedCards[0].name);
  }

  //Tests that the player wont sponsor the quest if someone can rank up
  [Test]
  public void sponsorQuest_ReturnsFalse_BecauseOfRankUp()
  {
    List<Player> players = new List<Player>();
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    // give the player requisite cards to sponsor the quest to ensure it fails because someone can rank up
    player1.hand.Add(new TestCard("Test Card", "Test of the Questing Beast", "Textures/tests/testOfTheQuestingBeast", 4));
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new FoeCard("Foe Card", "Saxons", "Textures/foe/saxons", 10, 20));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));

    player1.addShields(3);
    players.Add(player1);

    Assert.AreEqual(player1.strategy.sponsorQuest(players, 3, player1.hand, null), 0);
  }

  // Tests that the player won't sponsor the quest if they don't have enough foes
  [Test]
  public void sponsorQuest_ReturnsFalse_NotEnoughFoes()
  {
    List<Player> players = new List<Player>();
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    players.Add(player1);

    Assert.AreEqual(player1.strategy.sponsorQuest(players, 3, player1.hand, null), 0);
  }

  [Test]
  public void sponsorQuest_ReturnFalse_Because_DuplicateFoes(){
    List<Player> players = new List<Player>();
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    players.Add(player1);
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));

    Assert.AreEqual(player1.strategy.sponsorQuest(players, 3, player1.hand, null), 0);
  }

  [Test]
  public void sponsorQuest_ReturnsTrue_Because_CanSponsor_BecauseTest(){
    List<Player> players = new List<Player>();
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    players.Add(player1);
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new TestCard("Test Card", "Test of the Questing Beast", "Textures/tests/testOfTheQuestingBeast", 4));

    Assert.AreEqual(player1.strategy.sponsorQuest(players, 3, player1.hand, null), 1);
  }

  [Test]
  public void sponsorQuest_ReturnsTrue_Because_CanSponsor_WithoutTest(){
    List<Player> players = new List<Player>();
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    players.Add(player1);
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new FoeCard("Foe Card", "Saxons", "Textures/foe/saxons", 10, 20));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));

    Assert.AreEqual(player1.strategy.sponsorQuest(players, 3, player1.hand, null), 1);
  }

  // Test setting up a final foe stage --> SAME AS CPU1 EXCEPT 40 BP THRESHOLD NOT 50
  [Test]
  public void setUpFinalFoe_CantReach40BP()
  {
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    // should skip past this foe for the strongest foe
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Green Knight", "Textures/foe/greenKnight", 10, 40));      // Should only play 1 of these two duplicates
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));

    // Should only play one of the following duplicate weapons
    player1.hand.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));

    // Amour and Ally should be ignored by the computer playing a final foe stage
    player1.hand.Add(new AllyCard("Ally Card", "Sir Gawain", "Textures/Ally/sirGawain", 10, 0, "+20 on the Test of the Green Knight Quest", "", "", 0, 0, new NoBuff()));
    player1.hand.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
    string foe = "Dragon";

    // For the final stage this CPU should play a Robber Knight, with a Sword and a Horse
    List<Card> finalFoeStage = player1.strategy.setupFoeStage(4, 4, player1.hand, foe, 0);
    player1.discardCards(finalFoeStage);

    // Check that the finalFoeStage list is as we'd expect
    Assert.AreEqual(2, finalFoeStage.Count);
    Assert.AreEqual(finalFoeStage[0].name, "Robber Knight");
    Assert.AreEqual(finalFoeStage[1].name, "Sword");
  }


  [Test]
  public void setUpFinalFoe_QuickestTo40BP()
  {
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());

    player1.hand.Add(new FoeCard("Foe Card", "Green Knight", "Textures/foe/greenKnight", 30, 40));      // Should only play 1 of these two duplicates
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));

    // Should only play one of the following duplicate weapons
    player1.hand.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));

    string foe = "dragon";
    List<Card> finalFoeStage = player1.strategy.setupFoeStage(4, 4, player1.hand, foe, 0);
    // Check that the finalFoeStage list is as we'd expect
    Assert.AreEqual(2, finalFoeStage.Count);
    Assert.AreEqual(finalFoeStage[0].name, "Green Knight");
    Assert.AreEqual(finalFoeStage[1].name, "Sword");
  }

  // Test setting up an earlier foe stage --> WEAKEST foe and that is it
  [Test]
  public void setUpEarlyFoe_LowestBPFoe()
  {
      Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
      player1.hand.Add(new FoeCard("Foe Card", "Green Knight", "Textures/foe/greenKnight", 30, 40));      // Should only play 1 of these two duplicates
      player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/robberKnight", 5, 5));
      player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
      string foe = "dragon";
      List<Card> finalFoeStage = player1.strategy.setupFoeStage(1, 4, player1.hand, foe, 0);

      Assert.AreEqual(1, finalFoeStage.Count);
      Assert.AreEqual("Thieves", finalFoeStage[0].name);
  }

  [Test]
  public void setUpEarlyFoe_LowerBecauseQuestContext()
  {
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    player1.hand.Add(new FoeCard("Foe Card", "Green Knight", "Textures/foe/greenKnight", 30, 40));      // Should only play 1 of these two duplicates
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/robberKnight", 5, 20));
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    string foe = "Thieves";
    List<Card> finalFoeStage = player1.strategy.setupFoeStage(1, 4, player1.hand, foe, 0);

    Assert.AreEqual(1, finalFoeStage.Count);
    Assert.AreEqual("Robber Knight", finalFoeStage[0].name);
  }

  // Test setting up a test stage --> Should be the test card with the highest minimum
  [Test]
  public void setUpTestStage_AlwyasGetsBestTest()
  {
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    player1.hand.Add(new TestCard("Test Card", "Test with lower minimum", "Textures/tests/lowTestTexture", 4));
    player1.hand.Add(new TestCard("Test Card", "Test with higher minimum", "Textures/tests/highTestTexture", 5));

    List<Card> testStage = player1.strategy.setupTestStage(player1.hand);
    player1.discardCards(testStage);

    /*
    We want to assert that the player's hand, along with the testStage each have a count of 1.
    We also want to make sure that Test with higher minimum is in the testStage and Test with lower minimum is in the player hand
    */

    Assert.AreEqual(1, player1.hand.Count);
    Assert.AreEqual(1, testStage.Count);
    Assert.AreEqual(player1.hand[0].name, "Test with lower minimum");
    Assert.AreEqual(testStage[0].name, "Test with higher minimum");
  }

  // Test setting up a quest with no test stage and make sure the BP increases (no test)
  [Test]
  public void setUpQuest_NoTestStage_FoeStagesDecrease()
  {
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    player1.hand.Add(new FoeCard("Foe Card", "Giant", "texture", 30, 30));
    player1.hand.Add(new FoeCard("Foe Card", "Black Knight", "texture", 25, 25));
    player1.hand.Add(new FoeCard("Foe Card", "Evil Knight", "texture", 20, 20));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "texture", 5, 5));
    player1.hand.Add(new WeaponCard("Weapon Card", "Excalibur", "texture", 30));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));
    string foe = "dragon";

    // add an amour / ally to know that these cards arent being involved in the quest set up
    player1.hand.Add(new AmourCard("Amour Card", "Amour", "", 1, 10));
    player1.hand.Add(new AllyCard("Ally Card", "Sir Gawain", "Textures/Ally/sirGawain", 10, 0, "+20 on the Test of the Green Knight Quest", "", "", 0, 0, new NoBuff()));

    List<List<Card>> questLine = player1.strategy.setupQuest(3, player1.hand, foe);
    for (int i = 0; i < questLine.Count; i++)
    {
        player1.discardCards(questLine[i]);
    }

    // Check the hand
    Assert.AreEqual(6, player1.hand.Count);
    Assert.AreEqual(player1.hand[0].name, "Black Knight");
    Assert.AreEqual(player1.hand[1].name, "Horse");
    Assert.AreEqual(player1.hand[2].name, "Horse");
    Assert.AreEqual(player1.hand[3].name, "Horse");
    Assert.AreEqual(player1.hand[4].name, "Amour");
    Assert.AreEqual(player1.hand[5].name, "Sir Gawain");

    // Check the first stage of the quest
    Assert.AreEqual(questLine[0].Count, 1);
    Assert.AreEqual(questLine[0][0].name, "Thieves");

    // Check the second stage
    Assert.AreEqual(questLine[1].Count, 1);
    Assert.AreEqual(questLine[1][0].name, "Evil Knight");

    // Check the third stage
    Assert.AreEqual(questLine[2].Count, 2);
    Assert.AreEqual(questLine[2][0].name, "Giant");
    Assert.AreEqual(questLine[2][1].name, "Excalibur");
  }

  // Set up a quest and make sure the test is second last
  [Test]
  public void setUpQuest_TestStage_AlwaysSecondLast()
  {
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    player1.hand.Add(new FoeCard("Foe Card", "Giant", "texture", 30, 30));
    player1.hand.Add(new FoeCard("Foe Card", "Black Knight", "texture", 25, 25));
    player1.hand.Add(new FoeCard("Foe Card", "Evil Knight", "texture", 20, 20));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "texture", 5, 5));
    player1.hand.Add(new WeaponCard("Weapon Card", "Excalibur", "texture", 30));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));
    player1.hand.Add(new TestCard("Test Card", "Test of the Questing Beast", "Textures/tests/testOfTheQuestingBeast", 4));
    string foe = "";

    List<List<Card>> questLine = player1.strategy.setupQuest(3, player1.hand, foe);
    List<Card> secondLastStage = questLine[1];
    Assert.AreEqual(secondLastStage[0].name, "Test of the Questing Beast");
    Assert.AreEqual(secondLastStage.Count, 1);
  }

  // Test participate in quest false because there aren't enough foe cards in the user's hand
  [Test]
  public void participateInQuest_returnsFalse_canNotDiscard()
  {
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    iStrategyCPU2 strat = new iStrategyCPU2();
    player1.hand.Add(new FoeCard("Foe Card", "Giant", "texture", 30, 30));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "texture", 5, 5));
    player1.hand.Add(new AllyCard("Ally Card", "Merlin", "Textures/Ally/merlin", 30, 30, "Player may preview any 1 stage per Quest", "", "", 0, 0, new NoBuff()));
    player1.hand.Add(new AllyCard("Ally Card", "Sir Percival", "Textures/Ally/sirPercival", 5, 0, "+ 20 on the Search for the Holy Grail Quest", "", "", 0, 0, new NoBuff()));
    player1.hand.Add(new AllyCard("Ally Card", "King Arthur", "Textures/Ally/kingArthur", 10, 0, "+ 2 Bids", "", "", 0, 0, new NoBuff()));
    player1.hand.Add(new WeaponCard("Weapon Card", "Excalibur", "texture", 30));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));

    Assert.AreEqual(strat.canIDiscard(player1.hand), false);
  }

  // Test participate in quest false because can't increment (TAKE A GOOD LOOK AT INCREMENTATION)
  [Test]
  public void participateInQuest_returnsFalse_canNotIncrement()
  {
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    QuestCard quest = new QuestCard("Quest Card", "Journey through the Enchanted Forest", "Textures/Quests/journeyThroughTheEnchantedForest", 3, "Evil Knight");
    iStrategyCPU2 strat = new iStrategyCPU2();
    List<Player> players = new List<Player>();
    players.Add(player1);
    player1.hand.Add(new AllyCard("Ally Card", "Merlin", "Textures/Ally/merlin", 10, 10, "Player may preview any 1 stage per Quest", "", "", 0, 0, new NoBuff()));
    player1.hand.Add(new AllyCard("Ally Card", "Sir Percival", "Textures/Ally/sirPercival", 5, 0, "+ 20 on the Search for the Holy Grail Quest", "", "", 0, 0, new NoBuff()));
    player1.hand.Add(new WeaponCard("Weapon Card", "Excalibur", "texture", 30));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));

    // player can discard so if they cant sponsor its because they cant increment
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "texture", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "texture", 5, 5));

    Assert.AreEqual(false, strat.canIIncrement(3, player1.hand, players, quest));
  }

  // Test participate in quest true because both conditions good
  [Test]
  public void participateInQuest_returnsTrue_BothConditionsGood()
  {
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    QuestCard quest = new QuestCard("Quest Card", "Journey through the Enchanted Forest", "Textures/Quests/journeyThroughTheEnchantedForest", 3, "Evil Knight");
    iStrategyCPU2 strat = new iStrategyCPU2();
    List<Player> players = new List<Player>();
    player1.hand.Add(new AllyCard("Ally Card", "Merlin", "Textures/Ally/merlin", 10, 10, "Player may preview any 1 stage per Quest", "", "", 0, 0, new NoBuff()));
    player1.hand.Add(new AllyCard("Ally Card", "Sir Percival", "Textures/Ally/sirPercival", 5, 0, "+ 20 on the Search for the Holy Grail Quest", "", "", 0, 0, new NoBuff()));
    player1.hand.Add(new WeaponCard("Weapon Card", "Excalibur", "texture", 35));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Sword", "texture", 15));

    // player can discard so if they cant sponsor its because they cant increment
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "texture", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "texture", 5, 5));
    players.Add(player1);

    Assert.AreEqual(true, strat.canIIncrement(3, player1.hand, players, quest));
  }

  // Test playing a final foe encounter, strongest possible with amour == false
  [Test]
  public void playFoeEncounter_FinalFoe_StrongestHand_AmourFalse()
  {
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    List<Player> players = new List<Player>();
    player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 30, 30));
    player1.hand.Add(new WeaponCard("Weapon Card", "Lance", " ", 20));
    player1.hand.Add(new WeaponCard("Weapon Card", "Excalibur", " ", 30));
    player1.hand.Add(new WeaponCard("Weapon Card", "Excalibur", " ", 30));
    player1.hand.Add(new WeaponCard("Weapon Card", "Dagger", " ", 5));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", " ", 10));
    player1.hand.Add(new AllyCard("Ally Card", "Merlin", "Textures/Ally/merlin", 0, 0, "Player may preview any 1 stage per Quest", "None", "None", 0, 0, new NoBuff()));
    player1.hand.Add(new AllyCard("Ally Card", "Sir Gaiwan", " ", 10, 0, "some effect", "None", "None", 0, 0, new NoBuff()));
    player1.hand.Add(new AmourCard("Amour Card", "Amour", "texture", 1, 10));
    players.Add(player1);


    List<Card> cards = player1.strategy.playFoeEncounter(4, 4, player1.hand, 10, false, "", players);

    // player should play their strongest hand, this means more than 2 cards, should play the Lance, 1 Excalibur, the dagger, the horse, and Gaiwan

    Assert.AreEqual(6, cards.Count);
    Assert.AreEqual(cards[0].name, "Lance");
    Assert.AreEqual(cards[1].name, "Excalibur");
    Assert.AreEqual(cards[2].name, "Dagger");
    Assert.AreEqual(cards[3].name, "Horse");
    Assert.AreEqual(cards[4].name, "Sir Gaiwan");
    Assert.AreEqual(cards[5].name, "Amour");
  }

  // Test playing a final foe encounter, strongest possible with amour == true
  // should be able to copy from iStrategyCPU1
  [Test]
  public void playFoeEncounter_FinalFoe_StrongestHand_AmourTrue()
  {
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    List<Player> players = new List<Player>();
    player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 30, 30));
    player1.hand.Add(new WeaponCard("Weapon Card", "Lance", " ", 20));
    player1.hand.Add(new WeaponCard("Weapon Card", "Excalibur", " ", 30));
    player1.hand.Add(new WeaponCard("Weapon Card", "Excalibur", " ", 30));
    player1.hand.Add(new WeaponCard("Weapon Card", "Dagger", " ", 5));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", " ", 10));
    player1.hand.Add(new AllyCard("Ally Card", "Merlin", "Textures/Ally/merlin", 0, 0, "Player may preview any 1 stage per Quest", "", "", 0, 0, new NoBuff()));
    player1.hand.Add(new AllyCard("Ally Card", "Sir Gaiwan", " ", 10, 0, "some effect", "", "", 0, 0, new NoBuff()));
    player1.hand.Add(new AmourCard("Amour Card", "Amour", "texture", 1, 10));
    players.Add(player1);

    List<Card> cards = player1.strategy.playFoeEncounter(4, 4, player1.hand, 10, true, "", players);

    Assert.AreEqual(5, cards.Count);
    Assert.AreEqual(cards[0].name, "Lance");
    Assert.AreEqual(cards[1].name, "Excalibur");
    Assert.AreEqual(cards[2].name, "Dagger");
    Assert.AreEqual(cards[3].name, "Horse");
    Assert.AreEqual(cards[4].name, "Sir Gaiwan");
  }

  // Test playing an earlier foe encounter, the logic of earlier foe encounters is as follows:
  /*
    Increment from the previous round by +10 with this order of importance:

      - Amour first
      - Ally
      - Weapons
  */

  [Test]
  public void playFirstFoeEncounter_WithAmour(){
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    List<Player> players = new List<Player>();
    iStrategyCPU2 strat = new iStrategyCPU2();
    player1.hand.Add(new AmourCard("Amour Card", "Amour", "texture", 1, 10));
    player1.hand.Add(new AllyCard("Ally Card", "Sir Gaiwan", " ", 10, 0, "some effect", "None", "None", 0, 0, new NoBuff()));
    player1.hand.Add(new WeaponCard("Weapon Card", "Lance", " ", 20));
    player1.hand.Add(new WeaponCard("Weapon Card", "Excalibur", " ", 30));
    player1.hand.Add(new WeaponCard("Weapon Card", "Excalibur", " ", 30));
    player1.hand.Add(new WeaponCard("Weapon Card", "Dagger", " ", 5));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", " ", 10));
    players.Add(player1);

    List<Card> foeStage = strat.playEarlierFoe(player1.hand, 0, false, "", players);
    Assert.AreEqual(foeStage.Count, 1);
    Assert.AreEqual(foeStage[0].name, "Amour");
  }

  [Test]
  public void playEarlyFoe_WithPreviousOf15_AmourIsTrue_OnlyAllies(){
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    List<Player> players = new List<Player>();
    iStrategyCPU2 strat = new iStrategyCPU2();
    player1.hand.Add(new AmourCard("Amour Card", "Amour", "texture", 1, 10));
    player1.hand.Add(new AllyCard("Ally Card", "Sir Gaiwan", " ", 25, 25, "some effect", "None", "None", 0, 0, new NoBuff()));
    player1.hand.Add(new WeaponCard("Weapon Card", "Lance", " ", 5));
    player1.hand.Add(new WeaponCard("Weapon Card", "Scissors", " ", 5));
    player1.hand.Add(new WeaponCard("Weapon Card", "Excalibur", " ", 5));
    player1.hand.Add(new WeaponCard("Weapon Card", "Dagger", " ", 5));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", " ", 5));
    players.Add(player1);

    List<Card> foeStage = strat.playEarlierFoe(player1.hand, 0, true, "", players);
    Assert.AreEqual(foeStage.Count, 1);
    Assert.AreEqual(foeStage[0].name, "Sir Gaiwan");
  }


  // Test willIBid making sure it gives right integer for round 1 or round 2
  [Test]
  public void willIBid_returnsNonDuplicateInt_Round1()
  {
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    iStrategyCPU2 strat = new iStrategyCPU2();
    player1.hand.Add(new FoeCard("Foe Card", "Saxons", "Textures/foe/saxons", 10, 20));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Losers", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Idiots", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Dummies", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Dummies", "Textures/foe/thieves", 5, 5));

    int bid = strat.willIBid(3, player1.hand, 1, null);
    Assert.AreEqual(5, bid);
  }

  [Test]
  public void willIBid_returnsWithDuplicateInts_Round2()
  {
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    iStrategyCPU2 strat = new iStrategyCPU2();

    player1.hand.Add(new FoeCard("Foe Card", "Saxons", "Textures/foe/saxons", 10, 20));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Losers", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Idiots", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Dummies", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Dummies", "Textures/foe/thieves", 5, 5));

    int bid = strat.willIBid(3, player1.hand, 2, null);
    Assert.AreEqual(7, bid);
  }

  [Test]
  public void playBid_returnsNonDuplicates_Round1()
  {
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    iStrategyCPU2 strat = new iStrategyCPU2();

    player1.hand.Add(new FoeCard("Foe Card", "Saxons", "Textures/foe/saxons", 10, 20));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Losers", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Idiots", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Dummies", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Dummies", "Textures/foe/thieves", 5, 5));

    List<Card> cards = strat.playBid(player1.hand, 1);
    Assert.AreEqual(5, cards.Count);

  }

  [Test]
  public void playBid_returnsWithDuplicates_Round2()
  {
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
    iStrategyCPU2 strat = new iStrategyCPU2();

    player1.hand.Add(new FoeCard("Foe Card", "Saxons", "Textures/foe/saxons", 10, 20));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Losers", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Idiots", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Dummies", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
    player1.hand.Add(new FoeCard("Foe Card", "Dummies", "Textures/foe/thieves", 5, 5));

    List<Card> cards = strat.playBid(player1.hand, 2);
    Assert.AreEqual(7, cards.Count);
  }

  [Test]
  public void fixesHandDiscrepancyByDiscardingLatestAdditions()
  {
      Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
      player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
      player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
      player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
      player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
      player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
      player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
      player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
      player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
      player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
      player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
      player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
      player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
      player1.hand.Add(new FoeCard("Foe Card", "Random Foe", "", 45, 45));

      List<Card> discard = player1.strategy.fixHandDiscrepancy(player1.hand);

      Assert.AreEqual(12, player1.hand.Count);
      Assert.AreEqual(1, discard.Count);
      Assert.AreEqual("Random Foe", discard[0].name);
  }
}
