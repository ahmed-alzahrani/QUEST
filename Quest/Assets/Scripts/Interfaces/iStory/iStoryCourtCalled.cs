using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryCourtCalled : iStory
{
    public iStoryCourtCalled() { }

    public void execute(List<Player> players, Card storyCard, GameController game)
    {
        // implement Court Called to Camelot
        if (players != null)
        {
            for (int i = 0; i < players.Count; i++)
            {
                // Calling courtCalled on the player class will empty the player's ally card List
                // however, for each player, allyCards needs to be added back to the adventure Deck discard
                List<Card> allyCards = players[i].courtCalled();

                game.adventureDeck.discardCards(allyCards);
            }
            Debug.Log("All Allies in play must be discarded");
        }

        //repopulate board to show removed allies
        UIUtil.PopulatePlayerBoard(game);

        game.isDoneStoryEvent = true;
    }
}
