using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryQueensFavor : iStory{
  public iStoryQueensFavor() {}
  public bool execute(List<Player> players, Card storyCard, Deck adventure){
    // implement Queen's Favor
    if (players != null) {
      Debug.Log("The lowest ranked player(s) immediately receive 2 Adventure Cards");
    }
        return true;
  }
}
