using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryQueensFavor : iStory{
  public iStoryQueensFavor() {}
  public void execute(List<Player> players, int shields){
    // implement Queen's Favor
    if (players != null) {
      Debug.Log("The lowest ranked player(s) immediately receive 2 Adventure Cards");
    }
  }
}
