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

    Assert.IsTrue(player1.strategy.participateInTourney(players, 3));

    player1.addShields(4);
    Assert.IsTrue(player1.strategy.participateInTourney(players, 3));
  }

  /*

  // Tests the player will play their most powerful combination if they can't hit 50BP
  [Test]
  public void playTournament_returnBestHandPossible_LessThan50BP()
  {

  }

  // Tests that the player will play their fastest hand possible to 50BP
  [Test]
  public void playTournament_fastestPlayTo50BP()
  {

  }

  //Tests that the player wont sponsor the quest if someone can rank up
  [Test]
  public void sponsorQuest_ReturnsFalse_BecauseOfRankUp()
  {

  }

  // Tests that the player won't sponsor the quest if they don't have enough foes
  [Test]
  public void sponsorQuest_ReturnsFalse_NotEnoughFoes()
  {

  }

  // Tests that the player will sponsor if they have enough foes with no tests
  [Test]
  public void sponsorQuest_ReturnsTrue_WithFoes()
  {

  }

  // Tests that the player will sponsor if they have 1 less Foe but have a test
  [Test]
  public void sponsorQuest_ReturnsTrue_WithTest()
  {

  }


/*
  [Test]
  void playTournament_fewestCardsTo50Bp(){
    List<Player> players = new List<Player>();
    Player player1 = new Player("Ahmed", new List<Card>(), new iStrategyCPU2());
  }
  */

}
