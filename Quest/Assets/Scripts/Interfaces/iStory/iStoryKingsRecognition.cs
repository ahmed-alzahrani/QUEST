using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryKingsRecognition : iStory{
  public void execute(List<Player> players, int shields){
    // implement King's Recongition
    if (players != null) {
      Debug.Log("The Next player(s) to complete a Quest will receive 2 extra shields");
    }
  }
}
