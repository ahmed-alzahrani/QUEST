using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ComputerPlayer : Player
{
  // This class needs to implement:
          // 1. A way to randomly assign a name from a pre-determined list

          // 2. The iComputerStrategy interface and the specific strategy for iteration 1 (easy mode) to instantiate a computer player




  // private iComputerStrategy strategy;   This will correspond to the strategy this particular CPU player uses
  private string difficulty;

  public ComputerPlayer(string playerName, List<Card> startingHand) {
      name = playerName;
      score = 0;
      rank = "Squire";
      hand = startingHand;
      activeAllies = new List<AllyCard>();
      difficulty = "Beginner";
  }


}
