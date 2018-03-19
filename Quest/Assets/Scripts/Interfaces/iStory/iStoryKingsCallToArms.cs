using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventState
{
    public static string state = "Setup";
    public static List<int> highestRankedPlayers;
    public static bool discardTwoFoes = false;
    public static bool discardWeapons = false;
}

public class iStoryKingsCallToArms : iStory
{
    public iStoryKingsCallToArms() { }

    public void execute(List<Player> players, Card storyCard, Controller game)
    {
        // implement iEventKingsCallToArmss
        if (EventState.state == "Setup")
        {
            //find highest ranked player/s and store their positions starting from current player turn
            //gonna use that to iterate through the list of players 
            game.numIterations = 0;   
            EventState.highestRankedPlayers = getHighestPlayers(game);

            //find what to remove whether it is a weapon or 2 foe cards if there is nthg he can discard go to next player and check again
            DecideDiscards(game);

            //setup user input accordingly
            if (EventState.discardWeapons)
            {
                game.userInput.ActivateCardUIPanel("Discard a weapon card");
            }
            else if (EventState.discardTwoFoes)
            {
                game.userInput.ActivateCardUIPanel("Discard 2 foe cards");
            }
    
            EventState.state = "Discarding";
        }
        else if (EventState.state == "Discarding")
        {
            //discarding cards 
            DiscardQuery(game);

            //check for ending the event
            if (game.numIterations >= game.numPlayers)
            {
                //we are done here 
                game.userInput.DeactivateUI();

                //reset all values
                EventState.state = "Setup";
                EventState.highestRankedPlayers.Clear();
                EventState.discardTwoFoes = false;
                EventState.discardWeapons = false;
                game.isDoneStoryEvent = true;
            }
        }
    }

    //get name of highest rank
    public int getHighest(List<Player> players)
    {
        int highest = players[0].score;
        for (var i = 0; i < players.Count; i++)
        {
            if (players[i].score > highest)
            {
                highest = players[i].score;
            }
        }
        return highest;
    }

    //change this to go through the right order using current Player Index will still be fine since we go through all players
    public List<int> getHighestPlayers(Controller game)
    {

        //get highest rank
        int highest = getHighest(game.players);
        List<int> highestPlayers = new List<int>();

        //iterate through the players from currentPlayerIndex that way we always have the correct order of players afterwards
        while (game.numIterations < game.numPlayers)
        {
            if (game.players[game.currentPlayerIndex].score == highest)
            {
                //add the index of the players that need to discard
                highestPlayers.Add(game.currentPlayerIndex);
            }

            UIUtil.UpdatePlayerTurn(game);
            game.numIterations++;
        }

        game.numIterations = 0;

        for (int i = 0; i < highestPlayers.Count; i++)
        {
            Debug.Log("Discarding Player: " + highestPlayers[i]);
        }

        //return indeces of highest players
        return highestPlayers;
    }

    //decides for current player what he needs to discard
    public void DecideDiscards(Controller game)
    {
        int numFoes = 0;
        //reset bools for next players
        EventState.discardTwoFoes = false;
        EventState.discardWeapons = false;

        //this will check what to discard
        for (int i = 0; i < game.players[game.currentPlayerIndex].hand.Count; i++)
        {
            if (game.players[game.currentPlayerIndex].hand[i].type == "Weapon Card")
            {
                EventState.discardWeapons = true;
                break;
            }
            else if (game.players[game.currentPlayerIndex].hand[i].type == "Foe Card")
            {
                numFoes++;
            }
        }

        //no weapons
        if (!EventState.discardWeapons)
        {
            if (numFoes > 1)
            {
                EventState.discardTwoFoes = true;
            }
        }
    }

    //querying for discards
    public void DiscardQuery(Controller game)
    {
        //almost all the input checks occur here        
        if (game.userInput.UIEnabled)
        {
            if (game.userInput.cardPrompt.isActive)
            {
                if (game.numIterations < game.numPlayers)
                {

                    if (EventState.highestRankedPlayers.Count == 0)
                    {
                        //MIGHT NEED TO BE FIXEDDD!!
                        game.numIterations++;
                        UIUtil.UpdatePlayerTurn(game);
                    }
                    //if he is not the highest or he is but doesn't have proper cards to discard go to next player  
                    else if (game.currentPlayerIndex != EventState.highestRankedPlayers[0] || (!EventState.discardTwoFoes && !EventState.discardWeapons))
                    {
                        //player shouldn't need to discard
                        UIUtil.UpdatePlayerTurn(game);
                        game.numIterations++;
                        DecideDiscards(game);

                        game.userInput.DeactivateUI();

                        if (EventState.discardWeapons)
                        {
                            game.userInput.ActivateCardUIPanel("Discard a weapon card");
                        }
                        else if (EventState.discardTwoFoes)
                        {
                            game.userInput.ActivateCardUIPanel("Discard 2 foe cards");
                        }
                    }
                    else
                    {
                        //human players will return null
                        List<Card> result = game.players[game.currentPlayerIndex].kingsCall();

                        if (result != null)
                        {
                            //ai here 
                            //no need to check since we knw its right
                            //game.DiscardAdvenureCards(result);
                            GameUtil.DiscardCards(game.adventureDeck , result , game.adventureDeckDiscardPileUIButton);
                            EventState.highestRankedPlayers.RemoveAt(0);
                            UIUtil.UpdatePlayerTurn(game);
                            //decide discards for next player 
                            DecideDiscards(game);
                            game.numIterations++;
                            game.userInput.DeactivateUI();

                            if (EventState.discardWeapons)
                            {
                                game.userInput.ActivateCardUIPanel("Discard a weapon card");
                            }
                            else if (EventState.discardTwoFoes)
                            {
                                game.userInput.ActivateCardUIPanel("Discard 2 foe cards");
                            }
                        }
                        else if (game.userInput.cardPrompt.doneAddingCards)
                        {
                            // human player card querying
                            //check if cards added are correct
                            if (EventState.discardWeapons)
                            {
                                if (game.userInput.cardPrompt.selectedCards.Count == 1 && game.userInput.cardPrompt.selectedCards[0].type == "Weapon Card")
                                {
                                    //game.DiscardAdvenureCards(game.userInput.cardPrompt.selectedCards);
                                    GameUtil.DiscardCards(game.adventureDeck, game.userInput.cardPrompt.selectedCards, game.adventureDeckDiscardPileUIButton);

                                    EventState.highestRankedPlayers.RemoveAt(0);
                                    UIUtil.UpdatePlayerTurn(game);
                                    //decide discards for next player 
                                    DecideDiscards(game);
                                    game.numIterations++;
                                    game.userInput.DeactivateUI();

                                    if (EventState.discardWeapons)
                                    {
                                        game.userInput.ActivateCardUIPanel("Discard a weapon card");
                                    }
                                    else if (EventState.discardTwoFoes)
                                    {
                                        game.userInput.ActivateCardUIPanel("Discard 2 foe cards");
                                    }
                                }
                                else
                                {
                                    //reset cards to hand and try again
                                    for (int i = 0; i < game.userInput.cardPrompt.selectedCards.Count; i++)
                                    {
                                        //just in case
                                        game.players[game.currentPlayerIndex].hand.Add(game.userInput.cardPrompt.selectedCards[i]);
                                        game.userInput.RemoveFromCardUIPanel(i);
                                        i--;
                                        //game.populatePlayerBoard();
                                        UIUtil.PopulatePlayerBoard(game);
                                    }

                                    game.userInput.cardPrompt.doneAddingCards = false;
                                }
                            }
                            else if (EventState.discardTwoFoes)
                            {
                                if (game.userInput.cardPrompt.selectedCards.Count == 2 && game.userInput.cardPrompt.selectedCards[0].type == "Foe Card" && game.userInput.cardPrompt.selectedCards[1].type == "Foe Card")
                                {
                                    //game.DiscardAdvenureCards(game.userInput.cardPrompt.selectedCards);
                                    GameUtil.DiscardCards(game.adventureDeck, game.userInput.cardPrompt.selectedCards, game.adventureDeckDiscardPileUIButton);

                                    EventState.highestRankedPlayers.RemoveAt(0);
                                    UIUtil.UpdatePlayerTurn(game);
                                    //decide discards for next player 
                                    DecideDiscards(game);
                                    game.numIterations++;
                                    game.userInput.DeactivateUI();

                                    if (EventState.discardWeapons)
                                    {
                                        game.userInput.ActivateCardUIPanel("Discard a weapon card");
                                    }
                                    else if (EventState.discardTwoFoes)
                                    {
                                        game.userInput.ActivateCardUIPanel("Discard 2 foe cards");
                                    }
                                }
                                else
                                {
                                    //reset cards to hand and try again
                                    for (int i = 0; i < game.userInput.cardPrompt.selectedCards.Count; i++)
                                    {
                                        //just in case
                                        game.players[game.currentPlayerIndex].hand.Add(game.userInput.cardPrompt.selectedCards[i]);
                                        game.userInput.RemoveFromCardUIPanel(i);
                                        i--;
                                        //game.populatePlayerBoard();
                                        UIUtil.PopulatePlayerBoard(game);
                                    }
                                    game.userInput.cardPrompt.doneAddingCards = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    game.userInput.DeactivateUI();
                    Debug.Log("all cards have been selected");
                }
            }
        }       
    }
    
}
