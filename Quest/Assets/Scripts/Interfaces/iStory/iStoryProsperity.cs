using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryProsperity : iStory{
  public iStoryProsperity(){}
    public void execute(List<Player> players, Card storyCard, GameController game)
    {
        // implement Propserity Throughout the Realm
        // All players immediately draw 2 adventure cards
        if (players != null)
        {
            Debug.Log("All Players may immedaitely draw 2 adventure cards");
            for (int i = 0; i < players.Count; i++)
            {
                // removes from deck, adds to the player's hand
                players[i].drawCards(game.adventureDeck.drawCards(2));
            }
        }
        //check for discard
        game.playerStillOffending = game.PlayerOffending();

        if (game.playerStillOffending)
        {
            game.userInput.ActivateDiscardCheck("Discard Cards");
        }


        game.isDoneStoryEvent = true;
    }
}
