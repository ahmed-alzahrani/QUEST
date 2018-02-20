using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iStoryTournament : iStory {
  public bool execute(List<Player> players, Card storyCard, Deck adventure){
        TournamentCard tournament = (TournamentCard)storyCard;
        List<Player> participants = new List<Player>();

        // Query the list of player's strategy to see how wants to participate in the tournament
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].strategy.participateInTourney(players, tournament.shields)) // if its a CPU, we just get a List<Card> back, if its Player, we should stop and prompt them on UI
            {
                participants.Add(players[i]);
            }

        }

        // If there are no participants we are done
        if (participants.Count == 0)
        {
            Debug.Log("Nobody elected to participate in this tournament!");
            return true;
        }
        // If there is one participant simply award him his shield and continue
        if (participants.Count == 1)
        {
            int shieldsEarned = tournament.shields + 1;
            participants[0].addShields(shieldsEarned);
            return true;
        }
        else
        {
            // Run the first round of the tournament
            var round1 = new Dictionary<Player, List<Card>>();
            for (var i = 0; i < participants.Count; i++)
            {
                int basePower = participants[i].CalculateBP();
                round1[participants[i]] = participants[i].strategy.playTournament(participants, participants[i].hand, basePower, tournament.shields);
            }

            //roun1 is a dictionary where each (K,V) is (Player, cardsTheyPlayed)


            // Loop through and total everyones BP 
        }

        // implement Tournament logic

        return false;
  }

    // Some of these functions 
    public int calculateTotalBP(Player player, List<Card> playedCards)
    {
        int BP = 0;
        for (int i = 0; i < playedCards.Count; i++)
        {
            BP += getValidCardBP(playedCards[i]);

        }

        return BP += player.CalculateBP();
    }

    public int getValidCardBP(Card card)
    {
        if (card.type == "Ally Card")
        {
            AllyCard ally = (AllyCard)card;
            return ally.battlePoints;
        }
        if (card.type == "Weapon Card")
        {
            WeaponCard weapon = (WeaponCard)card;
            return weapon.battlePoints;
        }
        else {
            // amour
            return 10;
        }
    }
}
