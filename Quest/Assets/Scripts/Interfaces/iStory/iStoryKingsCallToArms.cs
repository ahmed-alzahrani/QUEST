using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryKingsCallToArms : iStory{
  public void execute(List<Player> players, int shields){
    // implement iEventKingsCallToArmss
    if (players != null) {
      Debug.Log("The highest ranked player(s) must place 1 weapon in the discard pile, or if they can not, 2 foe cards");
    }
  }
}
