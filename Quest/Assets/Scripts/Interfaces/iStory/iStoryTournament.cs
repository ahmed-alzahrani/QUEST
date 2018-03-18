using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentState
{
    public static string state = "Participation";
    public static int numberOfParticipants = 0;
    public static int tourneyRound = 1;
    public static List<Card>[] undiscardedCards;
    public static List<Card> cardsToBeDiscarded = new List<Card>();
}

public class iStoryTournament : iStory
{
    public void execute(List<Player> player, Card storyCard, GameController game)
    {
        //Ally cards on field do not matter !!!!!!! so only player rank and what he adds in
        if (TournamentState.state == "Participation" && TournamentState.tourneyRound == 1)
        {
            //do participation check code
            QueryingUtil.ParticipationCheck("Tournament" , game);

            //done checking for participation
            if (game.numIterations >= game.numPlayers)
            {
                TournamentState.numberOfParticipants = GameUtil.CheckParticipation(game.players);
                if (GameUtil.CheckParticipation(game.players) > 1)
                {
                    //there are more than 1 person participating
                    TournamentState.state = "CardQuery";

                    //participants draw one card
                    for (int i = 0; i < game.players.Count; i++)
                    {
                        if (game.players[i].participating)
                        {
                            game.players[i].hand.Add(GameUtil.DrawFromDeck(game.adventureDeck, 1)[0]);
                        }
                    }
                    UIUtil.PopulatePlayerBoard(game); //just in case
                    game.numIterations = 0; //for next part of tourney
                    game.userInput.DeactivateUI();
                    game.userInput.ActivateCardUIPanel("What AMOUR , ALLY , OR WEAPON CARDS do you want to use?");
                }
                else
                {
                    //game is done resolve the tournament no one or 1 person participating
                    //resolve tournament
                    TournamentCard myCard = (TournamentCard)storyCard;
                    for (int i = 0; i < game.players.Count; i++)
                    {
                        if (game.players[i].participating)
                        {
                            Debug.Log("number of shields" + myCard.shields);
                            game.players[i].addShields(1 + myCard.shields);
                        }
                    }

                    //reset values
                    game.isDoneStoryEvent = true;
                    TournamentState.state = "Participation";
                    TournamentState.numberOfParticipants = 0;
                    TournamentState.tourneyRound = 1;
                    TournamentState.undiscardedCards = null;
                    TournamentState.cardsToBeDiscarded = new List<Card>();

                }
            }
        }
        else if (TournamentState.state == "CardQuery")
        {
            //query players for cards
            QueryingUtil.CardQuerying("Tournament" , game);

            //done querying
            if (game.numIterations >= game.numPlayers)
            {
                int maximumBP = 0;
                int totalPlayer = 0;
                List<int> playerTotals = new List<int>();

                //concatonate results
                //ensuring deep copy
                List<List<Card>> temp = new List<List<Card>>(game.queriedCards);

                if (TournamentState.tourneyRound == 1)
                {
                    TournamentState.undiscardedCards = new List<Card>[game.queriedCards.Length];
                    //copy everything
                    System.Array.Copy(game.queriedCards, TournamentState.undiscardedCards, game.queriedCards.Length);
                }
                else
                {
                    //concat results
                    for (int i = 0; i < game.queriedCards.Length; i++)
                    {
                        if (game.queriedCards[i] != null)
                        {
                            for (int j = 0; j < game.queriedCards[i].Count; j++)
                            {
                                TournamentState.undiscardedCards[i].Add(game.queriedCards[i][j]);
                            }
                        }
                    }
                }

                //debugging purposes
                for (int i = 0; i < TournamentState.undiscardedCards.Length; i++)
                {
                    if (TournamentState.undiscardedCards[i] != null)
                    {
                        for (int j = 0; j < TournamentState.undiscardedCards[i].Count; j++)
                        {
                            Debug.Log(TournamentState.undiscardedCards[i][j].name);
                        }
                    }
                }

                for (int i = 0; i < TournamentState.undiscardedCards.Length; i++)
                {
                    //not participating
                    if (!(game.players[i].participating)) { playerTotals.Add(-100); continue; }
                    else
                    {
                        if (TournamentState.undiscardedCards[i] != null)
                        {
                            //sum up card battle pts
                            for (int j = 0; j < TournamentState.undiscardedCards[i].Count; j++)
                            {
                                if (TournamentState.undiscardedCards[i][j].type == "Weapon Card")
                                {
                                    WeaponCard weapon = (WeaponCard)TournamentState.undiscardedCards[i][j];
                                    totalPlayer += weapon.battlePoints;
                                }
                                else if (TournamentState.undiscardedCards[i][j].type == "Amour Card")
                                {
                                    AmourCard amour = (AmourCard)TournamentState.undiscardedCards[i][j];
                                    totalPlayer += amour.battlePoints;
                                }
                            }
                            //add player's rank BP only
                            totalPlayer += game.players[i].CalculateBP("", player);
                        }
                        else
                        {
                            //if he is participating and didn't add anything use his rank only
                            totalPlayer = game.players[i].CalculateBP("", player);
                        }
                    }

                    //add all totals to trace winner
                    playerTotals.Add(totalPlayer);
                    Debug.Log(i.ToString() + ": " + totalPlayer);

                    //check for maximum
                    if (maximumBP < totalPlayer)
                    {
                        maximumBP = totalPlayer;
                    }
                    totalPlayer = 0;
                }

                //discard weapon Cards
                for (int i = 0; i < TournamentState.undiscardedCards.Length; i++)
                {
                    if (TournamentState.undiscardedCards[i] != null)
                    {
                        for (int j = 0; j < TournamentState.undiscardedCards[i].Count; j++)
                        {
                            if (TournamentState.undiscardedCards[i][j].type == "Weapon Card")
                            {
                                //discard weapon card
                                TournamentState.cardsToBeDiscarded.Add(TournamentState.undiscardedCards[i][j]);
                                TournamentState.undiscardedCards[i].RemoveAt(j);
                                j--;
                            }
                        }
                    }
                }

                //discard weapon cards
                if (TournamentState.cardsToBeDiscarded != null && TournamentState.cardsToBeDiscarded.Count > 0)
                {
                    GameUtil.DiscardCards(game.adventureDeck , TournamentState.cardsToBeDiscarded , game.adventureDeckDiscardPileUIButton);
                }

                //discard all cards at the end of tourney
                if (TournamentState.tourneyRound == 2)
                {
                    //discard everything
                    //discard undiscarded cards
                    for (int i = 0; i < TournamentState.undiscardedCards.Length; i++)
                    {
                        if (TournamentState.undiscardedCards[i] != null && TournamentState.undiscardedCards[i].Count > 0)
                        {
                            GameUtil.DiscardCards(game.adventureDeck , TournamentState.undiscardedCards[i] , game.adventureDeckDiscardPileUIButton);
                        }
                    }
                }

                Debug.Log("Deciding winners");

                List<int> winners = new List<int>();
                for (int i = 0; i < playerTotals.Count; i++)
                {
                    if (playerTotals[i] < maximumBP)
                    {
                        //ppl who got beat
                        game.players[i].participating = false;
                    }
                    else
                    {
                        //winners
                        winners.Add(i);
                    }
                }

                //check for player discard
                game.playerStillOffending = GameUtil.PlayerOffending(game.players);

                if (game.playerStillOffending)
                {
                    game.numIterations = 0;

                    //find first offending player
                    while (!game.players[game.currentPlayerIndex].handCheck() && game.numIterations < game.numPlayers)
                    {
                        //doesn't need to discard update turn
                        game.numIterations++;
                        game.UpdatePlayerTurn();
                    }

                    game.userInput.ActivateDiscardCheck("You need to Discard " + (game.players[game.currentPlayerIndex].hand.Count - 12).ToString() + " Cards");
                }

                //undecided another round
                if (winners.Count >= 2 && TournamentState.tourneyRound == 1)
                {
                    Debug.Log("Couldn't find winners");

                    TournamentState.tourneyRound = 2;
                    game.numIterations = 0;
                    game.userInput.ActivateCardUIPanel("What AMOUR , ALLY , OR WEAPON CARDS do you want to use?");
                    Debug.Log("There is a draw");
                    //requery cards
                    System.Array.Clear(game.queriedCards , 0 , game.queriedCards.Length);

                    // game.queriedCards.Clear();
                    TournamentState.cardsToBeDiscarded.Clear();
                }
                else
                {
                    Debug.Log("found winners");

                    //we are done we have a winner or winners
                    for (int i = 0; i < winners.Count; i++)
                    {
                        TournamentCard myCard = (TournamentCard)storyCard;
                        Debug.Log("shields: " + myCard.shields);
                        game.players[winners[i]].addShields(TournamentState.numberOfParticipants + myCard.shields);
                    }

                    //reset everything
                    game.isDoneStoryEvent = true;
                    TournamentState.state = "Participation";
                    TournamentState.numberOfParticipants = 0;
                    TournamentState.tourneyRound = 1;
                    System.Array.Clear(game.queriedCards, 0, game.queriedCards.Length);
                    System.Array.Clear(TournamentState.undiscardedCards, 0, TournamentState.undiscardedCards.Length);
                    TournamentState.cardsToBeDiscarded.Clear();
                }
            }
        }
    }


}
