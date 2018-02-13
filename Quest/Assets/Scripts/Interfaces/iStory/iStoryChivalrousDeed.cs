using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryChivalrousDeed : iStory{
  public void execute(List<Player> players, int shields){
    // implement Chivalrous Deed
    if (players != null) {
      Debug.Log("Player(s) with both lowest rank and least amount of shields, receives 3 shields");
    }
  }
}
