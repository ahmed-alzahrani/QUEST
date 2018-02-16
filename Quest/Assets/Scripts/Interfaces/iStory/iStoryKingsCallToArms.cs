using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryKingsCallToArms : iStory{
  public void execute(List<Player> players, int shields){
    // implement iEventKingsCallToArmss
    if (players != null) {
      List<Player> highest = getHighestPlayers(players);
      for (var i = 0; i < highest.Count; i++) {

        // again here we can get the discards per player but need to discard them
        List<Card> discard = highest[i].kingsCall();
      }
      Debug.Log("The highest ranked player(s) must place 1 weapon in the discard pile, or if they can not, 2 foe cards");
    }
  }

  public int getHighest(List<Player> players){
    int highest = players[0].score;
    for (var i = 0; i < players.Count; i++) {
      if (players[i].score > highest) {
        highest = players[i].score;
      }
    }
    return highest;
  }

  public List<Player> getHighestPlayers(List<Player> players) {
    int highest = getHighest(players);
    List<Player> highestPlayers = new List<Player>();
    for (var i = 0; i < players.Count; i++) {
      if (players[i].score == highest) {
        highestPlayers.Add(players[i]);
      }
    }
    return highestPlayers;
  }
}
