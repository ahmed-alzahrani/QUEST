using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryCourtCalled : iStory{
  public iStoryCourtCalled() {}
  public bool execute(List<Player> players, Card storyCard, Deck adventure){
    // implement Court Called to Camelot
    if (players != null) {
      for(var i = 0; i < players.Count; i++) {
        // Calling courtCalled on the player class will empty the player's ally card List
        // however, for each player, allyCards needs to be added back to the adventure Deck discard
        List<AllyCard> allyCards = players[i].courtCalled();
      }
      Debug.Log("All Allies in play must be discarded");
      return true;
    }
      return false;
  }
}
