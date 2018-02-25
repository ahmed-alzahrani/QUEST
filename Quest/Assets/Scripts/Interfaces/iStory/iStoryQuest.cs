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
    public static QuestCard currentQuest;
    public static List<Card>[] stages;
    public static List<Card>[] amours;

    public static int[] testBids;
    public static bool testBidSubmitted;
    public static bool bidsOver;
    public static int questDrawer;

    public static bool invalidQuestSubmitted;

}

public class iStoryQuest : iStory
{
    public void execute(List<Player> players, Card storyCard, GameController game)
    {



        if (QuestState.state == "FindingSponsor")
        {
            QuestState.currentQuest = game.currentQuest;
            game.SponsorCheck();

            if (game.numIterations >= game.numPlayers)
            {

                Debug.Log("CheckSponsorship" + game.CheckSponsorship());
                if (game.CheckSponsorship() != -1)
                {
                    QuestState.state = "Sponsoring";

                    QuestState.invalidQuestSubmitted = false;

                    game.numIterations = 0;
                    game.userInput.DeactivateUI();
                    game.userInput.ActivateCardUIPanel("What FOE or TEST cards would you like to add?");

                }
                else
                {
                    game.isDoneStoryEvent = true;
                }
            }
        }

        if (QuestState.state == "Sponsoring")
        {
            //Displays a prompt for the player if the quest they submitted is invalid.
            if (QuestState.invalidQuestSubmitted)
            {
                InvalidQuestPrompt(game);
            }
            else
            {
                game.SponsorQuery();

                QuestState.stages = new List<Card>[game.currentQuest.getStages()];
                System.Array.Copy(game.sponsorQueriedCards, QuestState.stages, game.currentQuest.getStages());

                if (game.numIterations >= game.currentQuest.getStages())
                {
                    if (ValidQuest())
                    {
                        QuestState.state = "CheckingForParticipants";
                        game.UpdatePlayerTurn();
                        game.numIterations = 0;
                        game.userInput.DeactivateUI();
                        game.userInput.ActivateBooleanCheck("Participate in the QUEST?");
                    }
                    else
                    {
                        QuestState.invalidQuestSubmitted = true;
                        game.numIterations = 0;

                        for (int i = 0; i < game.sponsorQueriedCards.Length; i++)
                        {
                            if (game.sponsorQueriedCards[i] != null)
                                for (int j = 0; j < game.sponsorQueriedCards[i].Count; j++)
                                {
                                    game.players[game.currentPlayerIndex].hand.Add(game.sponsorQueriedCards[i][j]);
                                    game.AddToPanel(game.CreateUIElement(game.sponsorQueriedCards[i][j]), GameObject.FindGameObjectWithTag("CurrentHand"));
                                    game.sponsorQueriedCards[i].RemoveAt(j);
                                    j--;
                                }
                        }


                        game.userInput.DeactivateUI();
                        game.userInput.ActivateBooleanCheck("Invalid Quest Submitted. Please try again.");
                    }
                }
            }
        }

        if (QuestState.state == "CheckingForParticipants")
        {
            game.ParticipationCheck("Quest");

            if (game.numIterations >= game.numPlayers)
            {
                if (game.CheckParticipation() < 1)
                    EndQuest(game);
                else
                {
                    QuestState.state = "PlayingQuest";
                    QuestState.amours = new List<Card>[game.numPlayers];
                    QuestState.testBids = new int[game.numPlayers];
                    QuestState.testBidSubmitted = false;
                    QuestState.bidsOver = false;


                    DrawForStageStart(game);
                    game.populatePlayerBoard();
                    game.populateQuestBoard(false);
                    game.numIterations = 0;
                    game.userInput.DeactivateUI();
                    if (QuestState.stages[QuestState.currentStage][0].type == "Test Card")
                    {

                        TestCard tempTestCard = (TestCard)QuestState.stages[0][0];
                        string[] minBidString = new string[1];
                        minBidString[0] = tempTestCard.getMinimum().ToString();
                        game.userInput.ActivateUserInputCheck("A Test is in play, the minimum bid is: " + tempTestCard.getMinimum().ToString());
                    }
                    else
                        game.userInput.ActivateCardUIPanel("Play Ally, Weapon and/or Amour cards for this stage of the quest");

                }
            }

        }
        if (QuestState.state == "PlayingQuest")
        {
            if (QuestState.currentStage >= game.currentQuest.getStages())
            {
                Debug.Log("Called EndQuest here");
                EndQuest(game);
            }

            else if (game.CheckParticipation() < 1)
            {

                Debug.Log("Called EndQuest here");
                EndQuest(game);
            }

            else
            {
                if (QuestState.stages[QuestState.currentStage][0].type == "Test Card")
                {

                    if (!QuestState.bidsOver)
                    {
                        TestQuery(game);
                        if (game.CheckParticipation() < 2 && QuestState.testBidSubmitted == true)
                        {
                            QuestState.bidsOver = true;
                            game.userInput.DeactivateUI();
                            game.populatePlayerBoard();
                            game.userInput.ActivateCardUIPanel("Please submit " + QuestState.testBids[game.currentPlayerIndex].ToString() + " cards to discard.");
                        }
                    }
                    else
                    {
                        DiscardCards(game);
                    }
                }
                else
                {
                    game.CardQuerying();

                    if (game.userInput.selectedCards.Count > 0)
                    {
                        game.populateQuestBoard(false);
                    }

                    if (game.numIterations >= game.numPlayers)
                    {

                        for (int i = 0; i < game.queriedCards.Length; i++)
                        {
                            if (game.CheckSponsorship() == i)
                                continue;
                            if (!game.players[i].participating)
                                continue;

                            int sum = 0;
                            if (game.queriedCards[i] != null)
                            {
                                for (int j = 0; j < game.queriedCards[i].Count; j++)
                                {
                                    if (game.queriedCards[i][j] == null)
                                        continue;

                                    if (game.queriedCards[i][j].type == "Weapon Card")
                                    {
                                        WeaponCard submittedWeaponCard = (WeaponCard)game.queriedCards[i][j];
                                        sum += submittedWeaponCard.battlePoints;
                                    }
                                    else if (game.queriedCards[i][j].type == "Amour Card")
                                    {
                                        QuestState.amours[i].Add(game.queriedCards[i][j]);
                                        game.queriedCards[i].RemoveAt(j);
                                        j--;
                                    }
                                }
                            }
                            if (QuestState.amours[i] != null)
                            {
                                for (int j = 0; j < QuestState.amours[i].Count; j++)
                                {
                                    AmourCard tempAmourCard = (AmourCard)QuestState.amours[i][j];
                                    sum += tempAmourCard.battlePoints;
                                }
                            }

                            sum += game.players[i].CalculateBP(storyCard.name, players);

                            if (sum < GetStageBP(QuestState.currentStage, game.currentQuest))
                            {
                                game.players[i].participating = false;
                            }
                        }
                    }
                }
            }

            if (game.numIterations >= game.numPlayers)
            {
                Debug.Log("currentStage: " + QuestState.currentStage);
                game.populateQuestBoard(true);


                QuestState.currentStage++;
                if (QuestState.currentStage >= game.currentQuest.getStages())
                    EndQuest(game);
                else if (game.CheckParticipation() < 1)
                    EndQuest(game);
                else
                {

                    DrawForStageStart(game);
                    game.populatePlayerBoard();
                    game.numIterations = 0;
                    game.userInput.DeactivateUI();
                    System.Array.Clear(game.queriedCards, 0, game.queriedCards.Length);

                    if (QuestState.stages[QuestState.currentStage][0].type == "Test Card")
                    {
                        TestCard tempTestCard = (TestCard)QuestState.stages[QuestState.currentStage][0];
                        string[] minBidString = new string[1];
                        minBidString[0] = tempTestCard.getMinimum().ToString();
                        game.userInput.ActivateUserInputCheck("A Test is in play, the minimum bid you must make is: " + (tempTestCard.getMinimum()).ToString());
                    }
                    else
                        game.userInput.ActivateCardUIPanel("Play Ally, Weapon and/or Amour cards for this stage of the quest");
                }
            }
        }


    }

    public void EndQuest(GameController game)
    {

        int numDrawCount = 0;

        for (int i = 0; i < QuestState.stages.Length; i++)
            if (QuestState.stages[i] != null)
                numDrawCount += QuestState.stages[i].Count + 1;

        for (int i = 0; i < numDrawCount; i++)
            game.players[game.CheckSponsorship()].hand.Add(game.DrawFromDeck(game.adventureDeck, 1)[0]);

        if (QuestState.state == "PlayingQuest")
        {
            for (int i = 0; i < QuestState.amours.Length; i++)
                if (QuestState.amours[i] != null && QuestState.amours[i].Count > 0)
                    game.DiscardAdvenureCards(QuestState.amours[i]);

            for (int i = 0; i < game.numPlayers; i++)
            {
                if (game.players[i].participating)
                    game.players[i].addShields(game.currentQuest.getStages());
            }
        }



        //reset static values

        QuestState.state = "FindingSponsor";
        QuestState.currentQuest = null;
        QuestState.numberOfParticipants = 0;
        QuestState.currentStage = 0;
        QuestState.sponsorIndex = 0;
        QuestState.questDrawer = 0;
        QuestState.testBidSubmitted = false;
        QuestState.bidsOver = false;
        QuestState.testBids = null;


        //game.EmptyQuestPanel();


        //Initializing the list of quest stages;
        System.Array.Clear(QuestState.stages, 0, QuestState.stages.Length);
        System.Array.Clear(QuestState.amours, 0, QuestState.amours.Length);
        if(QuestState.testBids != null)
            System.Array.Clear(QuestState.testBids, 0, QuestState.testBids.Length);

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


    public int GetMinRequiredBid()
    {
        if (QuestState.stages[QuestState.currentStage][0].type != "Test Card")
            return -1;

        int currentMinBid = ((TestCard)QuestState.stages[QuestState.currentStage][0]).getMinimum();

        if (QuestState.testBids != null)
        {
            for (int i = 0; i < QuestState.testBids.Length; i++)
            {
                if (QuestState.testBids[i] < 20 && QuestState.testBids[i] > 0)
                {
                    if (QuestState.testBids[i] >= currentMinBid)
                        currentMinBid = QuestState.testBids[i] + 1;
                }
            }
        }

        return currentMinBid;


    }


    public void TestQuery(GameController game)
    {
        if (game.userInput.UIEnabled)
        {
            if (game.userInput.keyboardInputUIEnabled)
            {
                if (game.players[game.currentPlayerIndex].sponsoring || !game.players[game.currentPlayerIndex].participating)
                {
                    game.UpdatePlayerTurn();
                }

                else if (game.userInput.KeyboardInput.text.Length > 0 && Input.GetKeyDown("return"))
                {
                    int submittedBid = 0;
                    bool result = int.TryParse(game.userInput.KeyboardInput.text, out submittedBid);

                    if (submittedBid >= GetMinRequiredBid() && submittedBid <= game.players[game.currentPlayerIndex].hand.Count)
                    {
                        QuestState.testBids[game.currentPlayerIndex] = submittedBid;
                        if (!QuestState.testBidSubmitted)
                            QuestState.testBidSubmitted = true;

                        game.players[game.currentPlayerIndex].participating = true; // just in case
                        game.userInput.ActivateUserInputCheck("A Test is in play, the minimum bid is: " + GetMinRequiredBid().ToString());
                    }
                    else
                    {

                        game.userInput.DeactivateUI();

                        //submits query again with error statement
                        game.userInput.ActivateUserInputCheck("Please enter a bid equal to, or greater than: " + GetMinRequiredBid().ToString() + ". Press enter with without typing a bid to quit.");



                    }

                }
                else if (game.userInput.KeyboardInput.text.Length == 0 && Input.GetKeyDown("return"))
                {

                    game.players[game.currentPlayerIndex].participating = false;
                    game.UpdatePlayerTurn();
                    game.populatePlayerBoard();
                    game.userInput.DeactivateUI();
                    game.userInput.ActivateUserInputCheck("A Test is in play, the minimum bid is: " + GetMinRequiredBid().ToString());
                }
            }
        }
    }



    public void DiscardCards(GameController game)
    {
        if (game.userInput.UIEnabled)
        {
            if (game.userInput.cardPanelUIEnabled)
            {
                if (game.userInput.doneAddingCards)
                {
                    List<Card> testDiscardCards = new List<Card>(game.userInput.selectedCards);

                    if (testDiscardCards.Count != QuestState.testBids[game.currentPlayerIndex])
                    {
                        game.players[game.currentPlayerIndex].hand.AddRange(testDiscardCards);
                        game.userInput.DeactivateUI();
                        game.userInput.ActivateCardUIPanel("Invalid number of cards submitted, please submit " + QuestState.testBids[game.currentPlayerIndex].ToString() + " cards.");
                    }

                    else
                    {
                        game.DiscardAdvenureCards(testDiscardCards);
                        game.numIterations = game.numPlayers + 1;
                    }

                }
            }
        }
    }

    public int GetStageBP(int index, QuestCard q)
    {

        int sum = 0;

        if (QuestState.stages[index] == null)
        {
            Debug.Log("null index given to GetStageBP");
            return -2;
        }
        if (index >= QuestState.stages.Length)
        {
            Debug.Log("Invalid index given to GetStageBP");
            return -2;
        }
        else
        {
            for (int i = 0; i < QuestState.stages[index].Count; i++)
            {
                if (QuestState.stages[index][i].type == "Foe Card")
                {
                    FoeCard stageFoe = (FoeCard)QuestState.stages[index][i];
                    sum += GetFoeBP(q, stageFoe);
                }
                else if (QuestState.stages[index][i].type == "Test Card")
                {
                    if (QuestState.stages[index].Count > 1)
                    {
                        Debug.Log("Attempted to add more than 1 card to a stage with a Test");
                        sum = -2;
                    }
                    else
                        sum = -1;
                    return sum;
                }
                else
                {
                    Debug.Log("Invalid card type added to quest stage: " + i.ToString());
                    return -2;
                }
            }
            return sum;
        }
    }

    public int GetFoeBP(QuestCard q, FoeCard f)
    {
        if (q.foe == f.name)
        {
            return f.getMaxBP();
        }
        else
            return f.getMinBP();

    }

    public bool ValidQuest()
    {
        int[] stageBP = new int[QuestState.currentQuest.stages];

        for (int i = 0; i < stageBP.Length; i++)
            stageBP[i] = 0;
        bool testAdded = false;
        for (int i = 0; i < QuestState.stages.Length; i++)
        {
            if (QuestState.stages[i] == null)
            {
                if (i > (QuestState.currentQuest.stages - 1))
                    continue;
                else
                {
                    Debug.Log("null quest stage");

                    return false;
                }
            }
            int bpCheck = GetStageBP(i, QuestState.currentQuest);

            if (bpCheck == -2)
            {
                Debug.Log("In ValidQuest(), error recieved from GetStageBP");
                return false;
            }

            else if (i > 0)
            {
                if (stageBP[i - 1] < bpCheck)
                    stageBP[i] = bpCheck;

                else if (bpCheck == -1 && testAdded == false)
                {
                    testAdded = true;
                    stageBP[i] = bpCheck;
                }

                else
                {
                    Debug.Log("Failed on stage number: " + i.ToString() + "BP of current stage: " + bpCheck.ToString() + "BP of previous stage: " + stageBP[(int)Mathf.Max(i - 1, 0)]);
                    return false;
                }
            }
            else
            {
                if (bpCheck == -1)
                    testAdded = true;

                stageBP[i] = bpCheck;
            }

        }

        return true;
    }


    public void InvalidQuestPrompt(GameController game)
    {
        if (game.userInput.UIEnabled)
        {
            if (game.userInput.booleanUIEnabled)
            {
                //check with currentplayerIndex
                bool responseGiven = false;
                int check = game.players[game.currentPlayerIndex].strategy.respondToPrompt(game);

                if (check == 0)
                {
                    if (game.players[game.currentPlayerIndex].hand != null)
                    {
                        for (int i = 0; i < game.players[game.currentPlayerIndex].hand.Count; i++)
                        {
                            if (game.players[game.currentPlayerIndex].hand[i] != null)
                                Debug.Log("Card " + i.ToString() + ": " + game.players[game.currentPlayerIndex].hand[i].name);
                        }
                    }


                    game.userInput.DeactivateUI();
                    game.userInput.ActivateBooleanCheck("I assure you, the Quest you submitted was indeed invalid. If it was valid, please submit a bug report.");
                }
                else if (check == 1)
                {

                    QuestState.invalidQuestSubmitted = false;

                    game.numIterations = 0;
                    game.userInput.DeactivateUI();
                    game.userInput.ActivateCardUIPanel("What FOE or TEST cards would you like to add?");

                }

            }
        }
    }

}
