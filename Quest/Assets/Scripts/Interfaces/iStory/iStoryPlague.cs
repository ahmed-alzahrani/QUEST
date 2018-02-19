using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryPlague : iStory{
  public iStoryPlague(){}
  public void execute(List<Player> players, int shields){
    // implement Plague --> Drawer loses 2 shields if possible
    if (players != null) {
      Debug.Log("Drawer loses 2 shields if possible");
      players[0].removeShields(2);
    }
  }
}
