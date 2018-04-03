using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueryingUtil
{
    //util file for querying 

    //players over 12 cards 
    public static void DiscardCards(Controller game)
    {
        if (game.userInput.discardPrompt.isActive)
        {
            //Debug.Log("Whatever@#23@#");
            if (game.numIterations < game.numPlayers)
            {
                List<Card> discards = game.players[game.currentPlayerIndex].strategy.fixHandDiscrepancy(game.players[game.currentPlayerIndex].hand);

                if (discards != null)
                { 
                    //ai discarded we are done just discard his cards
                    GameUtil.DiscardCards(game.adventureDeck , discards , game.adventureDeckDiscardPileUIButton);

                    //get rid of those cards 
                    discards.Clear();
                    game.numIterations++;
                    UIUtil.UpdatePlayerTurn(game);

                    while (!game.players[game.currentPlayerIndex].handCheck() && game.numIterations < game.numPlayers)
                    {
                        //doesn't need to discard update turn
                        game.numIterations++;
                        UIUtil.UpdatePlayerTurn(game);
                    }

                    game.userInput.DeactivateDiscardPanel();
                    game.userInput.ActivateDiscardCheck("You need to Discard " + (game.players[game.currentPlayerIndex].hand.Count - 12).ToString() + " Cards");
                    Debug.Log("discarded AI Stuff");
                }
                else if (game.userInput.discardPrompt.doneAddingCards)
                {
                    //if things don't go well
                    if (game.players[game.currentPlayerIndex].handCheck())
                    {                        
                        //add back to players hand 
                        UIUtil.ReturnToPlayerHand(game.players[game.currentPlayerIndex] , game.userInput.discardPrompt.selectedCards , game);
                        
                        //remove UI stuff
                        for (int i = 0; i < game.userInput.discardPrompt.UICardsSelected.Count; i++)
                        {
                            Object.Destroy(game.userInput.discardPrompt.UICardsSelected[i]);
                            game.userInput.discardPrompt.UICardsSelected.RemoveAt(i);
                            i--;
                        }

                        game.userInput.discardPrompt.doneAddingCards = false;
                        Debug.Log("Not enough cards where discarded");
                    }
                    else
                    {
                        //if things go well
                        GameUtil.DiscardCards(game.adventureDeck , game.userInput.discardPrompt.selectedCards , game.adventureDeckDiscardPileUIButton);

                        game.numIterations++;
                        UIUtil.UpdatePlayerTurn(game);

                        while (!game.players[game.currentPlayerIndex].handCheck() && game.numIterations < game.numPlayers)
                        {
                            //doesn't need to discard update turn
                            game.numIterations++;
                            UIUtil.UpdatePlayerTurn(game);
                        }

                        game.userInput.DeactivateDiscardPanel();
                        game.userInput.ActivateDiscardCheck("You need to Discard " + (game.players[game.currentPlayerIndex].hand.Count - 12).ToString() + " Cards");
                        Debug.Log("Discarded player Cards");
                    }
                }
            }
        }
    }

    //Asks user for input then builds initial game board
    public static void PlayerSetup(Controller game)
    {
        // do ui checks here then build board when done
        if (game.userInput.UIEnabled)
        {
            if (game.userInput.keyboardPrompt.isActive)
            {
                if (game.setupState == 0)
                {
                    if (game.userInput.keyboardPrompt.KeyboardInput.text.Length > 0 && Input.GetKeyDown("return"))
                    {
                        bool result = int.TryParse(game.userInput.keyboardPrompt.KeyboardInput.text, out game.numPlayers);

                        if (result && game.numPlayers > 1 && game.numPlayers < 5)
                        {
                            game.userInput.DeactivateUI();
                            game.setupState = 1;
                            game.userInput.ActivateUserInputCheck("How Many Human Players?");
                        }
                        else
                        {
                            game.userInput.keyboardPrompt.KeyboardInput.text = "";
                        }
                    }
                }
                else if (game.setupState == 1)
                {
                    if (game.userInput.keyboardPrompt.KeyboardInput.text.Length > 0 && Input.GetKeyDown("return"))
                    {
                        bool result = int.TryParse(game.userInput.keyboardPrompt.KeyboardInput.text, out game.numHumanPlayers);

                        if (result && game.numHumanPlayers > 0 && game.numPlayers - game.numHumanPlayers >= 0)
                        {
                            game.userInput.DeactivateUI();
                            game.setupState = 2;
                            game.numCpus = game.numPlayers - game.numHumanPlayers;
                        }
                        else
                        {
                            game.userInput.keyboardPrompt.KeyboardInput.text = "";
                        }
                    }
                }
            }
        }
        else if (game.setupState == 2)
        {
            //done checking user input
            Debug.Log("NUMBER OF PLAYERS: " + game.numPlayers);
            Debug.Log("NUMBER OF HUMAN PLAYERS: " + game.numHumanPlayers);
            Debug.Log("NUMBER OF CPUS: " + game.numCpus);

            game.numIterations = 0;
            game.userInput.ActivateUserInputCheck("what strategy do you want for cpu " + (game.numIterations + 1).ToString() + " ?");
            game.setupState = 3; //won't go in here again
        }
    }

    public static void QueryStrategies(Controller game)
    {
        if (game.userInput.UIEnabled)
        {
            if (game.userInput.keyboardPrompt.isActive)
            {
                if (game.setupState >= 3)
                {
                    if (game.numIterations < game.numCpus)
                    {
                        if (game.userInput.keyboardPrompt.KeyboardInput.text.Length > 0 && Input.GetKeyDown("return"))
                        {
                            int strategy = 0;
                            bool result = int.TryParse(game.userInput.keyboardPrompt.KeyboardInput.text, out strategy);

                            if (result && strategy > 0 && strategy < 3)
                            {
                                //either 1 or 2
                                game.cpuStrategies.Add(strategy);
                                game.userInput.DeactivateUI();
                                game.numIterations++;
                                game.userInput.ActivateUserInputCheck("what strategy do you want for cpu " + (game.numIterations + 1).ToString() + " ?");
                            }
                            else
                            {
                                game.userInput.keyboardPrompt.KeyboardInput.text = "";
                            }
                        }
                    }
                    else
                    {
                        //we are done
                        game.userInput.DeactivateUI();
                        //at the end
                        game.players = UIUtil.CreatePlayers(game , null , null);
                        //game.populatePlayerBoard();
                        UIUtil.PopulatePlayerBoard(game);
                        game.isSettingUpGame = false;

                        for (int i = 0; i < game.cpuStrategies.Count; i++)
                        {
                            Debug.Log("strategies" + game.cpuStrategies[i]);
                        }
                    }
                }
            }
        }
    }

    //SOME CHANGES HERE FOR PARTICIPATE IN TOURNEY FUNCTION
    public static void ParticipationCheck(string state , Controller game)
    {
        if (game.userInput.UIEnabled)
        {
            if (game.userInput.booleanPrompt.isActive)
            {
                //check with currentplayerIndex
                if (game.numIterations < game.numPlayers)
                {
                    int participation = -1;
                    if (state == "Tournament")
                    {
                        //NEED TO CHANGE THAT!!!!!!!!!!!!!!!!!!
                        participation = game.players[game.currentPlayerIndex].strategy.participateInTourney(game.players, game.currentTournament.shields, game);

                    }
                    else if (state == "Quest")
                    {
                        //NEED TO CHANGE THISSSS!!!!!!!!!!!!!!!!
                        participation = game.players[game.currentPlayerIndex].strategy.participateInQuest(game.currentQuest.stages, game.players[game.currentPlayerIndex].hand, game);
                    }

                    if (game.players[game.currentPlayerIndex].sponsoring)
                    {
                        //skip if sponsoring
                        //this will help generalize participation in both tournamnets and quests
                        game.numIterations++;
                        UIUtil.UpdatePlayerTurn(game);
                    }
                    else if (participation == 0)
                    {
                        //not joining
                        game.players[game.currentPlayerIndex].participating = false; // just in case
                        UIUtil.UpdatePlayerTurn(game);
                        //game.populatePlayerBoard();
                        UIUtil.PopulatePlayerBoard(game);
                        game.userInput.DeactivateUI();
                        game.userInput.ActivateBooleanCheck("Do you want to participate?");
                        game.numIterations++;
                    }
                    else if (participation == 1)
                    {
                        //joining
                        game.players[game.currentPlayerIndex].participating = true;
                        UIUtil.UpdatePlayerTurn(game);
                        //game.populatePlayerBoard();
                        UIUtil.PopulatePlayerBoard(game);
                        game.userInput.DeactivateUI();
                        game.userInput.ActivateBooleanCheck("Do you want to participate?");
                        game.numIterations++;
                    }
                }
                else
                {
                    //we are done
                    game.userInput.DeactivateUI();
                }
            }
        }
    }

    //check if player wants to sponsor
    public static void SponsorCheck(Controller game)
    {
        //checking for sponsors
        if (game.userInput.UIEnabled)
        {
            if (game.userInput.booleanPrompt.isActive)
            {
                if (game.numIterations < game.numPlayers)
                {
                    int sponsoring = game.players[game.currentPlayerIndex].strategy.sponsorQuest(game.players, game.currentQuest.stages, game.players[game.currentPlayerIndex].hand, game);

                    if (game.currentPlayerIndex < game.numHumanPlayers)
                    {
                        if (!GameUtil.SponsorCapabilityCheck(game))
                        {
                            game.userInput.DeactivateUI();
                            game.userInput.ActivateBooleanCheck("You cannot sponsor this quest, please select No");
                            Debug.Log("Current Player index: " + game.currentPlayerIndex.ToString());
                            if (sponsoring == 1)
                                sponsoring = 3;
                        }
                    }
                    if (sponsoring == 0)
                    {
                        Debug.Log("No");
                        // not sponsoring

                        UIUtil.UpdatePlayerTurn(game);
                        UIUtil.PopulatePlayerBoard(game);
                        game.userInput.DeactivateUI();
                        game.userInput.ActivateBooleanCheck("Do you want to Sponsor This Quest?");
                        game.numIterations++;
                    }
                    else if (sponsoring == 1)
                    {
                        Debug.Log("Yes");
                        game.players[game.currentPlayerIndex].sponsoring = true;

                        game.userInput.DeactivateUI();

                        //circumvent this
                        game.numIterations = 5;
                    }
                    else if (sponsoring == 2)
                    {
                        //Do Nothing
                    }
                    else if (sponsoring == 3)
                    {
                        Debug.Log("Cannot sponsor.");
                    }
                    else
                    {
                        Debug.Log("Yes");
                        game.players[game.currentPlayerIndex].sponsoring = true;

                        game.userInput.DeactivateUI();

                        //circumvent this
                        game.numIterations = 5;
                    }
                }
            }
        }
        else
        {
            //we are done
            game.userInput.DeactivateUI();

            //return participants
            for (int i = 0; i < game.players.Count; i++)
            {
                if (game.players[i].sponsoring)
                    Debug.Log("Sponsor" + (i + 1) + ": " + game.players[i].name);
            }
        }
    }


    //query sponsor for his cards for the quest
    //IF SPONSOR ADDS NOTHING OR WE DO NOT
    //CHECK USER INPUT IF IT DOESN'T FIT INTO REQUIREMENTS END SPONSORSHIP AND END QUEST
    public static void SponsorQuery(Controller game)
    {
        if (game.userInput.UIEnabled)
        {
            if (game.userInput.cardPrompt.isActive)
            {
                //check for as much as the quest stages
                if (game.numIterations < game.currentQuest.stages)
                {
                    //calling check sponsorship for debugging since it should never come in here if there is no player sponsoring
                    List<List<Card>> returnVal = new List<List<Card>>();
                    returnVal = game.players[GameUtil.CheckSponsorship(game.players)].strategy.setupQuest(game.currentQuest.stages, game.players[GameUtil.CheckSponsorship(game.players)].hand, game.currentQuest.foe);

                    if (returnVal != null)
                    {
                        //ai here
                        //conversion
                        for (int i = 0; i < returnVal.Count; i++)
                        {
                            game.sponsorQueriedCards[i] = new List<Card>(returnVal[i]);
                        }

                        // break we are done here
                        game.numIterations = 5;
                        game.userInput.DeactivateUI();
                        UIUtil.UpdatePlayerTurn(game);
                        UIUtil.PopulatePlayerBoard(game);
                    }
                    else if (game.userInput.cardPrompt.doneAddingCards)
                    {
                        game.sponsorQueriedCards[game.numIterations] = new List<Card>(game.userInput.cardPrompt.selectedCards);
                        game.userInput.DeactivateUI();
                        game.numIterations++;
                        game.userInput.ActivateCardUIPanel("What FOE or TEST cards do you want to use for this Quest?");
                    }
                }
                else
                {
                    game.userInput.DeactivateUI();
                    UIUtil.UpdatePlayerTurn(game);
                    UIUtil.PopulatePlayerBoard(game);

                    Debug.Log("all cards have been selected");
                    //we are done

                    for (int i = 0; i < game.sponsorQueriedCards.Length; i++)
                    {
                        if (game.sponsorQueriedCards[i] != null)
                        {
                            for (int j = 0; j < game.sponsorQueriedCards[i].Count; j++)
                            {
                                Debug.Log(i.ToString() + ": " + game.sponsorQueriedCards[i][j].name);
                            }
                        }
                    }
                }
            }
        }
    }

    //NEEDS FIXING TO ADD SHIELDS AND BASE BP AND ADD A STRING TO USE BOTH QUEST AND TOURNAMENTS
    public static void CardQuerying(string state , Controller game)
    {
        if (game.userInput.UIEnabled)
        {
            if (game.userInput.cardPrompt.isActive)
            {
                if (game.numIterations < game.numPlayers)
                {
                    //might need to set this to a new list
                    List<Card> result = new List<Card>();

                    if (game.players[game.currentPlayerIndex].participating)
                    {
                        if (state == "Tournament")
                        {
                            result = game.players[game.currentPlayerIndex].strategy.playTournament(game.players, game.players[game.currentPlayerIndex].hand, game.players[game.currentPlayerIndex].CalculateBP("", game.players), game.currentTournament.shields);
                        }
                        else if (state == "Quest")
                        {
                            // foe encounter stuff here
                            if (QuestState.amours != null && QuestState.amours[game.currentPlayerIndex] != null)
                            {
                                result = game.players[game.currentPlayerIndex].strategy.playFoeEncounter(QuestState.currentStage, game.currentQuest.stages, QuestState.stages[QuestState.currentStage].Count, game.players[game.currentPlayerIndex].hand, QuestState.previousQuestBP, QuestState.amours[game.currentPlayerIndex].Count == 1, game.currentQuest.name, game.players);
                            }
                            else
                            {
                                result = game.players[game.currentPlayerIndex].strategy.playFoeEncounter(QuestState.currentStage, game.currentQuest.stages, QuestState.stages[QuestState.currentStage].Count, game.players[game.currentPlayerIndex].hand, QuestState.previousQuestBP, false, game.currentQuest.name, game.players);
                            }
                        }
                    }

                    if (game.players[game.currentPlayerIndex].sponsoring || !(game.players[game.currentPlayerIndex].participating))
                    {
                        //this will help generalize querying in both tournaments and quests
                        game.numIterations++;
                        game.queriedCards[game.currentPlayerIndex] = null;
                        UIUtil.UpdatePlayerTurn(game);

                    }
                    else if (game.userInput.cardPrompt.doneAddingCards)
                    {
                        // human player card querying
                        game.queriedCards[game.currentPlayerIndex] = new List<Card>(game.userInput.cardPrompt.selectedCards);
                        game.userInput.DeactivateUI();
                        game.numIterations++;
                        UIUtil.UpdatePlayerTurn(game);
                        UIUtil.PopulatePlayerBoard(game);
                        game.userInput.ActivateCardUIPanel("What AMOUR , ALLY , OR WEAPON CARDS do you want to use?");
                    }
                    else if (result != null)
                    {
                        //ai player here
                        game.queriedCards[game.currentPlayerIndex] = new List<Card>(result);
                        game.userInput.DeactivateUI();
                        game.numIterations++;
                        UIUtil.UpdatePlayerTurn(game);
                        UIUtil.PopulatePlayerBoard(game);
                        game.userInput.ActivateCardUIPanel("What AMOUR , ALLY , OR WEAPON CARDS do you want to use?");
                    }
                }
                else
                {
                    game.userInput.DeactivateUI();

                    Debug.Log("all cards have been selected");
                    //we are done

                    for (int i = 0; i < game.queriedCards.Length; i++)
                    {
                        if (game.queriedCards[i] != null)
                        {
                            for (int j = 0; j < game.queriedCards[i].Count; j++)
                            {
                                Debug.Log(i.ToString() + ": " + game.queriedCards[i][j].name);
                            }
                        }
                    }
                }
            }
        }
    }
}
