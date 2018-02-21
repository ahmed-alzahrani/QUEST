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
  public void setUpFoeStage_EarlyFoe_StrongestFoe_NoDuplicateWeapons(){
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
    Assert.AreEqual(foeStage[0].name, "Robber Knight");
    Assert.AreEqual(player1.hand[0].name, "Saxons");
    Assert.AreEqual(player1.hand[1].name, "Thieves");
    Assert.AreEqual(player1.hand[2].name, "Horse");
    Assert.AreEqual(player1.hand[3].name, "Sword");
  }

  [Test]
  public void setUpFoeStage_EarlyFoe_StrongestFoe_1DuplicateWeapon(){
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
    Assert.AreEqual(foeStage[0].name, "Robber Knight");
    Assert.AreEqual(foeStage[1].name, "Sword");
    Assert.AreEqual(player1.hand[0].name, "Saxons");
    Assert.AreEqual(player1.hand[1].name, "Thieves");
    Assert.AreEqual(player1.hand[2].name, "Horse");
    Assert.AreEqual(player1.hand[3].name, "Sword");
  }

  [Test]
  public void setUpFoeStage_EarlyFoe_StrongestFoe_MultipleDuplicateWeapons(){

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
    Assert.AreEqual(foeStage[0].name, "Robber Knight");
    Assert.AreEqual(foeStage[1].name, "Horse");
    Assert.AreEqual(foeStage[2].name, "Sword");
    Assert.AreEqual(player1.hand[0].name, "Saxons");
    Assert.AreEqual(player1.hand[1].name, "Thieves");
    Assert.AreEqual(player1.hand[2].name, "Horse");
    Assert.AreEqual(player1.hand[3].name, "Sword");
  }


  // Shows the user exhausting their weapon/ally options but not reaching 50bp
  [Test]
  public void setUpFinalFoe_CantReach50BP(){
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    // should skip past this foe for the strongest foe
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "Textures/foe/thieves", 5, 5));
    // Should only play 1 of these two duplicates
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));
    player1.hand.Add(new FoeCard("Foe Card", "Robber Knight", "Textures/foe/robberKnight", 15, 15));

    // Should only play one of the following duplicate weapons
    player1.hand.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Sword", "Textures/weapons/sword", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "Textures/weapons/horse", 10));

    // Amour and Ally should be ignored by the computer playing a final foe stage
    player1.hand.Add(new AllyCard("Ally Card", "Sir Gawain", "Textures/Ally/sirGawain", 10, "+20 on the Test of the Green Knight Quest"));
    player1.hand.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));

    // For the final stage this CPU should play a Robber Knight, with a Sword and a Horse
    List<Card> finalFoeStage = player1.strategy.setupFoeStage(4, 4, player1.hand);
    player1.discardCards(finalFoeStage);

    /*
    Assert that the player's hand has a count of 5 and includes the Thieves, One robber Knight, one Sword, Sir Gawain and the Amour

    Also Assert that the player's foe stage is the robber knight, sword and horse
    */

    // Check that the player's hand is what we expect
    Assert.AreEqual(player1.hand.Count, 5);
    Assert.AreEqual(player1.hand[0].name, "Thieves");
    Assert.AreEqual(player1.hand[1].name, "Robber Knight");
    Assert.AreEqual(player1.hand[2].name, "Sword");
    Assert.AreEqual(player1.hand[3].name, "Sir Gawain");
    Assert.AreEqual(player1.hand[4].name, "Amour");

    // Check that the finalFoeStage list is as we'd expect
    Assert.AreEqual(3, finalFoeStage.Count);
    Assert.AreEqual(finalFoeStage[0].name, "Robber Knight");
    Assert.AreEqual(finalFoeStage[1].name, "Sword");
    Assert.AreEqual(finalFoeStage[2].name, "Horse");  
    
    /*
    for(int i = 0; i < player1.hand.Count; i++)
    {
            player1.hand[i].display();    
    } */
    }

  // Tests that the user automatically selects their best test card for the test stage
  [Test]
  public void setUpTestStage_AlwaysGetsBestTestCard(){
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
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

  // Checks a user's set up quest with no Test Stage, shows that the BP automatically decreases
  [Test]
  public void setUpQuest_NoTestStage_FoeStagesDecrease(){
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
    player1.hand.Add(new FoeCard("Foe Card", "Giant", "texture", 30, 30));
    player1.hand.Add(new FoeCard("Foe Card", "Black Knight", "texture", 25, 25));
    player1.hand.Add(new FoeCard("Foe Card", "Evil Knight", "texture", 20, 20));
    player1.hand.Add(new FoeCard("Foe Card", "Thieves", "texture", 5, 5));
    player1.hand.Add(new WeaponCard("Weapon Card", "Excalibur", "texture", 30));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));
    player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));

    // add an amour / ally to know that these cards arent being involved in the quest set up
    player1.hand.Add(new AmourCard("Amour Card", "Amour", "", 1, 10));
    player1.hand.Add(new AllyCard("Ally Card", "Sir Gawain", "Textures/Ally/sirGawain", 10, "+20 on the Test of the Green Knight Quest"));

    List<List<Card>> questLine = player1.strategy.setupQuest(4, player1.hand);

    for (int i = 0; i < questLine.Count; i++)
    {
        player1.discardCards(questLine[i]);
    }

        /*
        Now we make our assertions about the resulting questLine

        1) The player's hand should only have a count of 2, the amour card and the ally card

        2) The questLine should have a count of 4, and should be in ascending order of stage (or BP)

        3) questLine[0] should have a count of 1, and include the Thieves

        4) questLine[1] should have a count of 2, and include  the Evil Knight and a Horse

        5) questLine[2] should have a count of 2, and include the balck knight and a horse

        6) questLine[3] should have a count of 2, and include the Giant and Excalibur
        */

        for (int i = 0; i < questLine.Count; i++)
        {
            Debug.Log("Ok I am printing the next stage of the quest....");
            for (int x = 0; x < questLine[i].Count; x++)
            {
                questLine[i][x].display();
            }
        }

        // Check the hand
        Assert.AreEqual(3, player1.hand.Count);
        Assert.AreEqual(player1.hand[0].name, "Horse");
        Assert.AreEqual(player1.hand[1].name, "Amour");
        Assert.AreEqual(player1.hand[2].name, "Sir Gawain");
    
        // check the questLine length
        Assert.AreEqual(questLine.Count, 4);

        // check the first stage
        Assert.AreEqual(questLine[0].Count, 1);
        Assert.AreEqual(questLine[0][0].name, "Thieves");

        // check the second stage
        Assert.AreEqual(questLine[1].Count, 2);
        Assert.AreEqual(questLine[1][0].name, "Evil Knight");
        Assert.AreEqual(questLine[1][1].name, "Horse");

        // check the third stage
        Assert.AreEqual(questLine[2].Count, 2);
        Assert.AreEqual(questLine[2][0].name, "Black Knight");
        Assert.AreEqual(questLine[2][1].name, "Horse");

        // check the last stage
        Assert.AreEqual(questLine[3].Count, 2);
        Assert.AreEqual(questLine[3][0].name, "Giant");
        Assert.AreEqual(questLine[3][1].name, "Excalibur");
    }

  // Sets up a quest with test stage in hand and verifies test comes second last
  [Test]
  public void setUpQuest_TestStage_AlwaysSecondLast(){
        Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
        player1.hand.Add(new FoeCard("Foe Card", "Giant", "texture", 30, 30));
        player1.hand.Add(new FoeCard("Foe Card", "Black Knight", "texture", 25, 25));
        player1.hand.Add(new FoeCard("Foe Card", "Evil Knight", "texture", 20, 20));
        player1.hand.Add(new FoeCard("Foe Card", "Thieves", "texture", 5, 5));
        player1.hand.Add(new WeaponCard("Weapon Card", "Excalibur", "texture", 30));
        player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));
        player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));
        player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));
        player1.hand.Add(new TestCard("Test Card", "Test of the Questing Beast", "Textures/tests/testOfTheQuestingBeast", 4));

        List<List<Card>> questLine = player1.strategy.setupQuest(4, player1.hand);

        //Assert that the player second last stage of the quest is the Test stage;

        List<Card> secondLastStage = questLine[2];
        Assert.AreEqual(secondLastStage[0].name, "Test of the Questing Beast");
        Assert.AreEqual(secondLastStage.Count, 1);
    }


  // Testing CPU1 implementation of set up quest
  // The final foe stage is fewest cards to get to 50BP.
  // After that, if there is a test card that comes next
  // Then earlier foe encounters are just the weakest foe with any duplicate weapons

  // Tests that the player won't participate in quest if there aren't 2 cards to play per stage
  [Test]
  public void participateInQuest_returnsFalse_NotEnoughCards_True_WhenLastCardAdded()
  {
        Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
        // Add two foe cards of under 20BP so the computer would feel comfortable discarding for a test
        player1.hand.Add(new FoeCard("Foe Card", "Thieves", "texture", 5, 5));
        player1.hand.Add(new FoeCard("Foe Card", "Thieves", "texture", 5, 5));

        // Add 3 weapons to the player's hand, one short of the 4 card threshold (2 per stage of a 2 stage quest)
        player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));
        player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));
        player1.hand.Add(new WeaponCard("Weapon Card", "Sword", "texture", 20));
        Assert.AreEqual(player1.strategy.participateInQuest(2, player1.hand), false);

        // add one more weapon and it should work
        player1.hand.Add(new WeaponCard("Weapon Card", "Sword", "texture", 20));
        Assert.AreEqual(player1.strategy.participateInQuest(2, player1.hand), true);

    }

  // Tests that the player won't particiapte in the quest if they don't have 2 foes handy for discarding
  [Test]
  public void participateInQuest_returnsFalse_canNotDiscard()
  {
      Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
      player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));
      player1.hand.Add(new WeaponCard("Weapon Card", "Horse", "texture", 10));
      player1.hand.Add(new WeaponCard("Weapon Card", "Sword", "texture", 20));
      player1.hand.Add(new WeaponCard("Weapon Card", "Sword", "texture", 20));

        // Even though we have enough weapons to play we will return false without the right number of foes to discard
        Assert.AreEqual(player1.strategy.participateInQuest(2, player1.hand), false);
    }


  // Tests that the player will participate in this early foe encounter with 1 amour 1 ally (lowest ally with BP)
  [Test]
  public void playFoeEncounter_EarlierFoe_2Allies_AmourInHand_AmourFalse()
  {
        Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
        player1.hand.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
        player1.hand.Add(new AllyCard("Ally Card", "Merlin", "Textures/Ally/merlin", 0, "Player may preview any 1 stage per Quest"));
        player1.hand.Add(new AllyCard("Ally Card", "Sir Percival", "Textures/Ally/sirPercival", 5, "+ 20 on the Search for the Holy Grail Quest"));
        player1.hand.Add(new AllyCard("Ally Card", "King Arthur", "Textures/Ally/kingArthur", 10, "+ 2 Bids"));
        player1.hand.Add(new WeaponCard("Weapon Card", "Battle-Axe", "Textures/weapons/battle-ax", 15));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 30, 30));

        List<Card> foeEncounter = player1.strategy.playFoeEncounter(1, 6, player1.hand, 0, false);

        /*
        We can assert that:
        1) The Count of the foeEncounter is 2
        2) The first card is the amour
        3) The second card is the ally with the lowest BP (sir percival)
        */
        Assert.AreEqual(foeEncounter.Count, 2);
        Assert.AreEqual(foeEncounter[0].name, "Amour");
        Assert.AreEqual(foeEncounter[1].name, "Sir Percival");
    }

  //Test that the player will particiapte in the early foe encounter with 2 allies because amour is true
  [Test]
  public void playFoeEncounter_EarlierFoe_2Allies_AmourInHand_AmourTrue()
  {
        Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
        player1.hand.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
        player1.hand.Add(new AllyCard("Ally Card", "Merlin", "Textures/Ally/merlin", 0, "Player may preview any 1 stage per Quest"));
        player1.hand.Add(new AllyCard("Ally Card", "Sir Percival", "Textures/Ally/sirPercival", 5, "+ 20 on the Search for the Holy Grail Quest"));
        player1.hand.Add(new AllyCard("Ally Card", "King Arthur", "Textures/Ally/kingArthur", 10, "+ 2 Bids"));
        player1.hand.Add(new WeaponCard("Weapon Card", "Battle-Axe", "Textures/weapons/battle-ax", 15));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 30, 30));

        List<Card> foeEncounter = player1.strategy.playFoeEncounter(1, 6, player1.hand, 0, true);

        /*
        We can assert that:
        1) The Count of the foeEncounter is 2
        2) The first card is Sir Percival (Ally with lowest BP)
        3) The second card is King Arthur
        4) Amour is not a part of the foeEncuonter bc Amour is set to true (already been played)
        */
        Assert.AreEqual(foeEncounter.Count, 2);
        Assert.AreEqual(foeEncounter[0].name, "Sir Percival");
        Assert.AreEqual(foeEncounter[1].name, "King Arthur");


    }

  //Test that the player will participate in the early foe encounter with 1 weak ally and 1 weak weapon
  [Test]
  public void playFoeEncounter_EarlierFoe_1Ally_1Weapon()
  {
        Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
        player1.hand.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
        player1.hand.Add(new AllyCard("Ally Card", "Merlin", "Textures/Ally/merlin", 0, "Player may preview any 1 stage per Quest"));
        player1.hand.Add(new AllyCard("Ally Card", "Sir Percival", "Textures/Ally/sirPercival", 5, "+ 20 on the Search for the Holy Grail Quest"));
        player1.hand.Add(new WeaponCard("Weapon Card", "Battle-Axe", "Textures/weapons/battle-ax", 15));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 30, 30));

        List<Card> foeEncounter = player1.strategy.playFoeEncounter(1, 6, player1.hand, 0, true);

        /*
        We can assert that:
        1) The Count of the foeEncounter is 2
        2) The first card is Sir Percival (Ally with lowest BP)
        3) The second card is King Arthur
        4) Amour is not a part of the foeEncuonter bc Amour is set to true (already been played)
        */
        Assert.AreEqual(foeEncounter.Count, 2);
        Assert.AreEqual(foeEncounter[0].name, "Sir Percival");
        Assert.AreEqual(foeEncounter[1].name, "Battle-Axe");

    }

  // Tests that the player will participate in the early foe encounter with 2 weapons
  [Test]
  public void playFoeEncounter_EarlierFoe_2Weapons()
  {
        Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
        player1.hand.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
        player1.hand.Add(new AllyCard("Ally Card", "Merlin", "Textures/Ally/merlin", 0, "Player may preview any 1 stage per Quest"));
        player1.hand.Add(new WeaponCard("Weapon Card", "Battle-Axe", "Textures/weapons/battle-ax", 15));
        player1.hand.Add(new WeaponCard("Weapon Card", "Sword", "textures/weapons/sword", 10));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 30, 30));

        List<Card> foeEncounter = player1.strategy.playFoeEncounter(1, 6, player1.hand, 0, true);

        /*
        We can assert that:
        1) The Count of the foeEncounter is 2
        2) The first card is Sir Percival (Ally with lowest BP)
        3) The second card is King Arthur
        4) Amour is not a part of the foeEncuonter bc Amour is set to true (already been played)
        */
        Assert.AreEqual(foeEncounter.Count, 2);
        Assert.AreEqual(foeEncounter[0].name, "Sword");
        Assert.AreEqual(foeEncounter[1].name, "Battle-Axe");

    }

  // Tests that the player enters with 1 weapon if thats all they have
  [Test]
  public void playFoeEncounter_EarlierFoe_1Weapon_NoAllies()
  {
        Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
        player1.hand.Add(new AmourCard("Amour Card", "Amour", "Textures/Amour/amour", 1, 10));
        player1.hand.Add(new AllyCard("Ally Card", "Merlin", "Textures/Ally/merlin", 0, "Player may preview any 1 stage per Quest"));
        player1.hand.Add(new WeaponCard("Weapon Card", "Battle-Axe", "Textures/weapons/battle-ax", 15));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 30, 30));

        List<Card> foeEncounter = player1.strategy.playFoeEncounter(1, 6, player1.hand, 0, true);

        Assert.AreEqual(foeEncounter.Count, 1);
        Assert.AreEqual(foeEncounter[0].name, "Battle-Axe");

    }

  // Tests that the player will play the earlier foe encounter with no cards if none are available
  [Test]
  public void playFoeEncounter_EarlierFoe_NoCards()
  {
        Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
        List<Card> foeEncounter = player1.strategy.playFoeEncounter(1, 6, player1.hand, 0, true);
        Assert.AreEqual(0, foeEncounter.Count);
    }

  // Tests that the player enters a Final Foe Encounter with the strongest possible hand
  [Test]
  public void playFoeEncounter_FinalFoe_StrongestHand()
  {
        Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 30, 30));
        player1.hand.Add(new WeaponCard("Weapon Card", "Lance", " ", 20));
        player1.hand.Add(new WeaponCard("Weapon Card", "Excalibur", " ", 30));
        player1.hand.Add(new WeaponCard("Weapon Card", "Excalibur", " ", 30));
        player1.hand.Add(new WeaponCard("Weapon Card", "Dagger", " ", 5));
        player1.hand.Add(new WeaponCard("Weapon Card", "Horse", " ", 10));
        player1.hand.Add(new AllyCard("Ally Card", "Merlin", "Textures/Ally/merlin", 0, "Player may preview any 1 stage per Quest"));
        player1.hand.Add(new AllyCard("Ally Card", "Sir Gaiwan", " ", 10, "some effect"));

        List<Card> foeEncounter = player1.strategy.playFoeEncounter(6, 6, player1.hand, 0, true);

        // player should play their strongest hand, this means more than 2 cards, should play the Lance, 1 Excalibur, the dagger, the horse, and Gaiwan

        Assert.AreEqual(5, foeEncounter.Count);
        Assert.AreEqual(foeEncounter[0].name, "Lance");
        Assert.AreEqual(foeEncounter[1].name, "Excalibur");
        Assert.AreEqual(foeEncounter[2].name, "Dagger");
        Assert.AreEqual(foeEncounter[3].name, "Horse");
        Assert.AreEqual(foeEncounter[4].name, "Sir Gaiwan");
    }

  // Tests WillIBid on round 2, Player has enough cards in hand but this CPU doesn't bid on round 2
  [Test]
  public void playBid_Round2_Returns0()
  {
        Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));

        int willIBid = player1.strategy.willIBid(5, player1.hand, 2);
        Assert.AreEqual(willIBid, 0);
    }

  // Tests PlayBid player has Foes, but none with less than 20 minBP
  [Test]
  public void playBid_Round1_NoWeakFoes()
  {
        Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 30, 30));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 30, 30));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 30, 30));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 30, 30));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 30, 30));

        int willIBid = player1.strategy.willIBid(4, player1.hand, 1);
        Assert.AreEqual(willIBid, 0);

    }

  // Tests playBid player has multiple weak foes, enough to bid more than the min bid
  [Test]
  public void playBid_Round1_ReturnsTrue_CanBid()
  {
        Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));

        int willIBid = player1.strategy.willIBid(4, player1.hand, 1);
        Assert.AreEqual(willIBid, 5);

    }

  // Tests playbid player has multiple weak foes but not enoughto bid
  [Test]
  public void playBid_Round1_ReturnsFalse_CantBid()
  {
        Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU1());
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));
        player1.hand.Add(new FoeCard("Foe Card", "Mordred", "Textures/foe/mordred", 10, 10));

        // has 4 but bid is 5 will return 0
        int willIBid = player1.strategy.willIBid(5, player1.hand, 1);

    }
}
