using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestState
{
    public static string state = "FindingSponsor";
    public static int numberOfParticipants = 0;
    public static int currentStage = 0;
    public static int sponsorIndex = 0;

    //Initializing the list of quest stages;
    public static List<Card>[] stages;
    public static List<Card>[] amours;

    public static int[] participatingInTest;
    public static bool testBidSubmitted;
    public static int questDrawer;
}

// ADD LOGS TO TEST CORRECTNESS OF THIS 
public class iStoryQuest : iStory
{
    public void execute(List<Player> players, Card storyCard, GameController game)
    {
        // implement Quest logic
        //QuestStage.questStage == "Sponsoring";
        //check for sponsors (if we have a sponsor continue else end this quest)
        //check for participants (if we don't have participants end quest)
        //Have sponsor setup the quest

        if (QuestState.state == "FindingSponsor")
        {
            //SPONSORING WORKS
            game.SponsorCheck();

            if (game.numIterations >= game.numPlayers)
            {

                Debug.Log("CheckSponsorship: " + game.CheckSponsorship());
                if (game.CheckSponsorship() > -1)
                {
                    QuestState.state = "Sponsoring";

                    //game.populatePlayerBoard();
                    game.numIterations = 0;
                    game.userInput.DeactivateUI();
                    game.userInput.ActivateCardUIPanel("What FOE or TEST cards would you like to add to the Quest?");
                }
                else
                {
                    // no sponsors end quest
                    game.isDoneStoryEvent = true;
                }
            }
        }
        else if (QuestState.state == "Sponsoring")
        {
            //QUERYING CARDS WORKS

            //querying sponsor for cards
            game.SponsorQuery();
            
            //could be game.currentQuest.stages - 1 ?????????????????????
            if (game.numIterations >= game.currentQuest.stages)
            {
                //received sponsor's quest stages 
                //sponsored cards is always of size 5 
                QuestState.stages = new List<Card>[game.sponsorQueriedCards.Length];
                //copy values
                System.Array.Copy(game.sponsorQueriedCards, QuestState.stages, game.sponsorQueriedCards.Length);

                Debug.Log("NUMBER OF STAGES: " + QuestState.stages.Length);

                //debug cards
                for (int i = 0; i < QuestState.stages.Length; i++)
                {
                    if (i >= game.currentQuest.stages) break;

                    for (int j = 0; j < QuestState.stages[i].Count; j++)
                    {
                        Debug.Log("STAGE " + (i + 1).ToString() + ": " + QuestState.stages[i][j].name);
                    }
                }

                //go to next turn and ask for participants in the quest and reset UI
                QuestState.state = "CheckingForParticipants";
                game.UpdatePlayerTurn();
                game.numIterations = 0;
                game.userInput.DeactivateUI();
                game.userInput.ActivateBooleanCheck("Participate in the QUEST?");
            }
        }
        else if (QuestState.state == "CheckingForParticipants")
        {
            //PARTICIPANTS WORK

            //Check participating players
            game.ParticipationCheck("Quest");

            //game participants
            if (game.numIterations >= game.numPlayers)
            {

                for (int i = 0; i < game.players.Count; i++)
                {
                    if (game.players[i].participating)
                    {
                        Debug.Log("Participant" + (i + 1).ToString() + ": " + game.players[i].name);
                    }
                }

                //done checking for participation
                if (game.CheckParticipation() < 1)
                {
                    //no one participated in the quest
                    EndQuest(game);
                }
                else
                {
                    //someone participated
                    QuestState.state = "PlayingQuest";
                    QuestState.amours = new List<Card>[game.numPlayers];

                    QuestState.participatingInTest = new int[game.numPlayers];
                    QuestState.testBidSubmitted = false;

                    DrawForStageStart(game);
                    game.populatePlayerBoard();
                    game.populateQuestBoard(false);
                    game.numIterations = 0;
                    game.userInput.DeactivateUI();

                    //if we have a test card run test code
                    //definetely not [0][0] u mean current stage then [0]
                    if (QuestState.stages[QuestState.currentStage][0].type == "Test Card")
                    {
                        TestCard tempTestCard = (TestCard)QuestState.stages[QuestState.currentStage][0];
                        string[] minBidString = new string[1];
                        minBidString[0] = tempTestCard.getMinimum().ToString();
                        //check for minimum bids
                        game.userInput.ActivateUserInputCheck("A Test is in play, the minimum bid is: " + minBidString);
                    }
                    else
                    {
                        //We have a foe encounter check participants for cards
                        game.userInput.ActivateCardUIPanel("Play Ally, Weapon and/or Amour cards for this stage of the quest");
                    }
                }
            }
        }
        else if (QuestState.state == "PlayingQuest")
        {
            //FOE ENCOUNTERS WORKS

            if (QuestState.currentStage > game.currentQuest.stages - 1  || game.CheckParticipation() < 1)
            {
                EndQuest(game);
            }
            else
            {
                //test cards
                if (QuestState.stages[QuestState.currentStage][0].type == "Test Card")
                {
                   
                    //running a test is done here
                    TestQuery(game);
                    if (game.CheckParticipation() < 2 && QuestState.testBidSubmitted == true)
                    {
                        game.numIterations = game.numPlayers + 1;
                    }



                }
                else
                {
                    // we are in a foe encounter asking for current stages's cards from participants 
                    game.CardQuerying();

                    //horrible but i have no choice 
                    //when u select a card it will reset the quest to the next stage
                    if (game.userInput.selectedCards.Count > 0)
                    {
                        //reset board
                        game.populateQuestBoard(false);
                    }
                    if (game.numIterations >= game.numPlayers)
                    {
                        // we are done querying for this stage of the quest from participants 

                        for (int i = 0; i < game.queriedCards.Length; i++)
                        {
                            //if the queried cards are null then the player is either a sponsor or a 
                            //participant
                            int sum = 0;

                            //players that query nothing still don't have a null its just empty so this still works
                            if (game.queriedCards[i] != null)
                            {
                                for (int j = 0; j < game.queriedCards[i].Count; j++)
                                {
                                    if (game.queriedCards[i][j] == null) continue;
                                    if (game.queriedCards[i][j].type == "Weapon Card")
                                    {
                                        WeaponCard playerWeapon = (WeaponCard)game.queriedCards[i][j];
                                        sum += playerWeapon.battlePoints;
                                    }
                                    else if (game.queriedCards[i][j].type == "Amour Card")
                                    {
                                        // REMOVE AMOURS FROM THE QUERIED CARDS DECK
                                        QuestState.amours[i].Add(game.queriedCards[i][j]);
                                        game.queriedCards[i].RemoveAt(j);
                                        j--;
                                    }
                                }

                                
                                //if player has amours 
                                if (QuestState.amours[i] != null)
                                {
                                    for (int j = 0; j < QuestState.amours[i].Count; j++)
                                    {
                                        AmourCard tempAmourCard = (AmourCard)QuestState.amours[i][j];
                                        sum += tempAmourCard.battlePoints;
                                    }
                                }

                                //summed up all cards now discard the one that aren't amour cards
                                game.DiscardAdvenureCards(game.queriedCards[i]);

                                sum += game.players[i].CalculateBP();

                                if (sum < GetStageBP(QuestState.currentStage, game.currentQuest))
                                {
                                    //player lost and is no longer participating in tournament
                                    game.players[i].participating = false;
                                }
                            }
                        }
                    }
                }
            }

            if (game.numIterations >= game.numPlayers)
            {
                //debug which stage we are in 
                Debug.Log("currentStage: " + QuestState.currentStage);
                game.populateQuestBoard(true);

                QuestState.currentStage++;
                if (QuestState.currentStage > game.currentQuest.stages - 1 || game.CheckParticipation() < 1)
                {
                    EndQuest(game);
                }
                else
                {
                    //quest is not done 
                    //discard all cards that are not the amours so weapons and foes
                    

                    DrawForStageStart(game);
                    game.populatePlayerBoard();
                    game.numIterations = 0;
                    game.userInput.DeactivateUI();
                    //clear queried cards
                    System.Array.Clear(game.queriedCards, 0, game.queriedCards.Length);

                    //definetely not [0][0] u mean current stage then [0]
                    //setup next stage
                    if (QuestState.stages[QuestState.currentStage][0].type == "Test Card")
                    {
                        TestCard tempTestCard = (TestCard)QuestState.stages[QuestState.currentStage][0];
                        string[] minBidString = new string[1];
                        minBidString[0] = tempTestCard.getMinimum().ToString();
                        game.userInput.ActivateUserInputCheck("A Test is in play, the minimum bid is: " + minBidString);
                    }
                    else
                    {
                        game.userInput.ActivateCardUIPanel("Play Ally, Weapon and/or Amour cards for this stage of the quest");
                    }
                }
            }
        }
    }




    public void EndQuest(GameController game)
    {
        //sponsors draw cards equal to the number of cards they added to all stages + number of stages  
        //winners earn shields equal to the number of stages 

        //CHECK FOR KINGS RECOGNITION
        int numDrawCount = 0;

        //get number of cards sponsors draw
        for (int i = 0; i < QuestState.stages.Length; i++)
        {
            if (QuestState.stages[i] != null)
            {
                numDrawCount += QuestState.stages[i].Count + 1;
            }
        }

        //sponsor draws cards 
        int sponsor = game.CheckSponsorship();
        Debug.Log("My Sponsor: " + sponsor);

        for (int i = 0; i < numDrawCount; i++)
        {
            Card drawnCard = game.DrawFromDeck(game.adventureDeck, 1)[0];
            game.players[sponsor].hand.Add(drawnCard);
        }

        //DISCARD QUEST STAGES ARRAY
        // discard amours and give winning players shields
        if (QuestState.state == "PlayingQuest")
        {
            for (int i = 0; i < QuestState.amours.Length; i++)
            {
                if (QuestState.amours[i] != null && QuestState.amours[i].Count > 0)
                {
                    game.DiscardAdvenureCards(QuestState.amours[i]);
                }
            }

            for (int i = 0; i < game.numPlayers; i++)
            {
                if (game.players[i].participating)
                {
                    game.players[i].addShields(game.currentQuest.stages);
                }
            }
        }

        game.currentPlayerIndex = QuestState.questDrawer;

        //reset static values
        QuestState.state = "FindingSponsor";
        QuestState.numberOfParticipants = 0;
        QuestState.currentStage = 0;
        QuestState.sponsorIndex = 0;
        QuestState.questDrawer = 0;
        QuestState.testBidSubmitted = false;

        //Initializing the list of quest stages;
        System.Array.Clear(QuestState.stages, 0, QuestState.stages.Length);
        System.Array.Clear(QuestState.amours, 0, QuestState.amours.Length);
        System.Array.Clear(QuestState.participatingInTest, 0, QuestState.participatingInTest.Length);

        game.isDoneStoryEvent = true;
    }

    public void DrawForStageStart(GameController game)
    {
        for (int i = 0; i < game.numPlayers; i++)
        {
            if (game.players[i].participating)
                game.players[i].hand.Add(game.DrawFromDeck(game.adventureDeck, 1)[0]);
        }
    }


    public int GetStageBP(int index, QuestCard q)
    {
        if (index >= QuestState.stages.Length)
            return -1;
        else
        {
            int sum = 0;
            for (int i = 0; i < QuestState.stages[index].Count; i++)
            {
                if (QuestState.stages[index][i].type == "Foe Card")
                {
                    FoeCard ____ = (FoeCard)QuestState.stages[index][i];
                    if (q.foe == ____.name)
                    {
                        sum += ____.getMaxBP();
                    }
                    else
                        sum += ____.getMinBP();
                }
            }
            return sum;
        }

        return 0;
    }

    //queries for number of card bids that can be made by players 
    public void TestQuery(GameController game)
    {
        if (game.userInput.UIEnabled)
        {
            if (game.userInput.keyboardInputUIEnabled)
            {
                if (game.numIterations < game.numPlayers)
                {
                    if (game.userInput.KeyboardInput.text.Length > 0 && Input.GetKeyDown("return"))
                    {




                    }
                }
                else
                {
                    game.userInput.DeactivateUI();
                }
            }
        }
    }
    

    //Asks player to discard cards after winning test 
    public void DiscardCards()
    {

    }
}