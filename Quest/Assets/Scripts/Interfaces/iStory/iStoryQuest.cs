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
    public static int round;
    public static int questDrawer;

    public static bool invalidQuestSubmitted;
    public static int previousQuestBP = 0;
    public static int minBid;
}

public class iStoryQuest : iStory
{
    public void execute(List<Player> players, Card storyCard, GameController game)
    {
        //FIND A SPONSOR FOR THE QUEST 
        if (QuestState.state == "FindingSponsor")
        {
            QuestState.currentQuest = game.currentQuest;
            QueryingUtil.SponsorCheck(game);

            if (game.numIterations >= game.numPlayers)
            {
                game.numIterations = 0;

                Debug.Log("CheckSponsorship" + GameUtil.CheckSponsorship(game.players));
                if (GameUtil.CheckSponsorship(game.players) != -1)
                {
                    QuestState.state = "Sponsoring";

                    QuestState.invalidQuestSubmitted = false;

                    game.userInput.DeactivateUI();
                    game.userInput.ActivateCardUIPanel("What FOE or TEST cards would you like to add?");

                }
                else
                {
                    game.isDoneStoryEvent = true;
                }
            }
        }

        // GET SPONSOR'S CARDS FOR THE QUEST 
        if (QuestState.state == "Sponsoring")
        {
            //Displays a prompt for the player if the quest they submitted is invalid.
            if (QuestState.invalidQuestSubmitted)
            {
                InvalidQuestPrompt(game);
            }
            else
            {
                QueryingUtil.SponsorQuery(game);

                if (game.numIterations >= game.currentQuest.stages)
                {
                    QuestState.stages = new List<Card>[game.currentQuest.stages];
                    System.Array.Copy(game.sponsorQueriedCards, QuestState.stages, game.currentQuest.stages);
                    game.numIterations = 0;

                    if (ValidQuest())
                    {
                        QuestState.state = "CheckingForParticipants";
                        game.UpdatePlayerTurn();
                        game.userInput.DeactivateUI();
                        game.userInput.ActivateBooleanCheck("Participate in the QUEST?");
                    }
                    else
                    {
                        QuestState.invalidQuestSubmitted = true;

                        for (int i = 0; i < game.sponsorQueriedCards.Length; i++)
                        {
                            if (game.sponsorQueriedCards[i] != null)
                                for (int j = 0; j < game.sponsorQueriedCards[i].Count; j++)
                                {
                                    game.players[game.currentPlayerIndex].hand.Add(game.sponsorQueriedCards[i][j]);
                                    UIUtil.AddCardToPanel(UIUtil.CreateUIElement(game.sponsorQueriedCards[i][j] , game.cardPrefab), game.handPanel);
                                    game.sponsorQueriedCards[i].RemoveAt(j);
                                    j--;
                                }
                        }

                        UIUtil.PopulatePlayerBoard(game);
                        game.userInput.DeactivateUI();
                        game.userInput.ActivateBooleanCheck("Invalid Quest Submitted. Please try again.");
                    }
                }
            }
        }

        //CHECKING FOR QUEST PARTICIPANTS
        if (QuestState.state == "CheckingForParticipants")
        {
            QueryingUtil.ParticipationCheck("Quest" , game);

            if (game.numIterations >= game.numPlayers)
            {
                Debug.Log("Done Participation Check");
                if (GameUtil.CheckParticipation(game.players) < 1)
                {
                    EndQuest(game);
                }
                else
                {
                    QuestState.state = "PlayingQuest";
                    QuestState.amours = new List<Card>[game.numPlayers];

                    for(int i = 0; i < QuestState.amours.Length; i++)
                    {
                        QuestState.amours[i] = new List<Card>();
                    }

                    QuestState.testBids = new int[game.numPlayers];
                    QuestState.testBidSubmitted = false;
                    QuestState.bidsOver = false;

                    DrawForStageStart(game);
                    //game.populatePlayerBoard();
                    //game.populateQuestBoard(false);
                    UIUtil.PopulatePlayerBoard(game);
                    UIUtil.PopulateQuestBoard(game, false);
                    game.numIterations = 0;
                    game.userInput.DeactivateUI();
                    if (QuestState.stages[QuestState.currentStage][0].type == "Test Card")
                    {
                        QuestState.round = 0;
                        TestCard tempTestCard = (TestCard)QuestState.stages[0][0];
                        string[] minBidString = new string[1];
                        minBidString[0] = GetMinRequiredBid().ToString();
                        game.userInput.ActivateUserInputCheck("A Test is in play, the minimum bid is: " + GetMinRequiredBid().ToString());
                    }
                    else
                        game.userInput.ActivateCardUIPanel("Play Ally, Weapon and/or Amour cards for this stage of the quest");

                }
            }
        }

        //QUEST GAMEPLAY HERE
        else if (QuestState.state == "PlayingQuest")
        {
            if (QuestState.currentStage >= game.currentQuest.getStages() || GameUtil.CheckParticipation(game.players) < 1)
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
                        if (GameUtil.CheckParticipation(game.players) < 2 && QuestState.testBidSubmitted == true)
                        {
                            //check for test card thing 

                            QuestState.bidsOver = true;
                            game.userInput.DeactivateUI();
                            //game.populatePlayerBoard();
                            UIUtil.PopulatePlayerBoard(game);
                            game.userInput.ActivateCardUIPanel("Please submit " + (Mathf.Max(QuestState.testBids[game.currentPlayerIndex] - game.players[game.currentPlayerIndex].calculateBid(game.currentQuest.name, game.players), 0)).ToString() + " cards to discard.");
                        }
                    }
                    else
                    {
                        DiscardCards(game);
                    }
                }
                else
                {
                    QueryingUtil.CardQuerying("Quest" , game);

                    //get player's cards for the quest
                    if (game.userInput.cardPrompt.selectedCards.Count > 0)
                    {
                        //game.populateQuestBoard(false);

                        UIUtil.PopulateQuestBoard(game , false);
                    }
                    if (game.numIterations >= game.numPlayers)
                    {

                        for (int i = 0; i < game.queriedCards.Length; i++)
                        {
                            if (GameUtil.CheckSponsorship(game.players) == i)
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

                                        Debug.Log("Weapon Card at " + j.ToString() + " bp value is " + ((WeaponCard)game.queriedCards[i][j]).battlePoints.ToString());
                                        sum += ((WeaponCard)game.queriedCards[i][j]).battlePoints;
                                    }
                                    else if (game.queriedCards[i][j].type == "Amour Card")
                                    {
                                        QuestState.amours[i].Add(game.queriedCards[i][j]);
                                        game.queriedCards[i].RemoveAt(j);
                                        j--;
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

                                    Debug.Log("Stage Failed Result: GetStageBP = " + GetStageBP(QuestState.currentStage, game.currentQuest).ToString() + " Sum of player strength = " + sum.ToString());
                                    game.players[i].participating = false;
                                }
                            }
                        }
                    }
                }
            }

            if (game.numIterations >= game.numPlayers)
            {
                Debug.Log("currentStage: " + QuestState.currentStage);
                //game.populateQuestBoard(true);
                UIUtil.PopulateQuestBoard(game , true);

                if (!(QuestState.stages[QuestState.currentStage][0].type == "Test Card"))
                    QuestState.previousQuestBP = GetStageBP(QuestState.currentStage, QuestState.currentQuest);

                QuestState.currentStage++;
                if (QuestState.currentStage >= game.currentQuest.getStages())
                    EndQuest(game);
                else if (GameUtil.CheckParticipation(game.players) < 1)
                    EndQuest(game);
                else
                {

                    DrawForStageStart(game);
                    UIUtil.PopulatePlayerBoard(game);
                    game.numIterations = 0;
                    game.userInput.DeactivateUI();
                    System.Array.Clear(game.queriedCards, 0, game.queriedCards.Length);

                    //check for discard at the end of each stage
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
                        game.userInput.ActivateDiscardCheck("You need to Discard " + (players[game.currentPlayerIndex].hand.Count - 12).ToString() + " Cards");
                    }

                    if (QuestState.stages[QuestState.currentStage][0].type == "Test Card")
                    {

                        QuestState.round = 0;
                        TestCard tempTestCard = (TestCard)QuestState.stages[QuestState.currentStage][0];
                        string[] minBidString = new string[1];
                        minBidString[0] = GetMinRequiredBid().ToString();
                        game.userInput.ActivateUserInputCheck("A Test is in play, the minimum bid you must make is: " + (GetMinRequiredBid()).ToString());
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
            game.players[GameUtil.CheckSponsorship(game.players)].hand.Add(GameUtil.DrawFromDeck(game.adventureDeck, 1)[0]);

        if (QuestState.state == "PlayingQuest")
        {
            int kingsUsed = 0;
            if (game.kingsRecognition)
                kingsUsed += 2;
            for (int i = 0; i < QuestState.amours.Length; i++)
                if (QuestState.amours[i] != null && QuestState.amours[i].Count > 0)
                    GameUtil.DiscardCards(game.adventureDeck , QuestState.amours[i] , game.adventureDeckDiscardPileUIButton);

            for (int i = 0; i < game.numPlayers; i++)
            {

                if (game.players[i].participating)
                    game.players[i].addShields(game.currentQuest.getStages() + kingsUsed);

            }
            if (kingsUsed == 2)
                game.kingsRecognition = false;

        }

        //reset static values
        QuestState.state = "FindingSponsor";
        QuestState.currentQuest = null;
        QuestState.numberOfParticipants = 0;
        QuestState.currentStage = 0;
        QuestState.sponsorIndex = 0;
        QuestState.questDrawer = 0;
        QuestState.round = 0;
        QuestState.testBidSubmitted = false;
        QuestState.bidsOver = false;
        QuestState.testBids = null;

        //DO I NEED TO UPDATE TURNS
        game.currentPlayerIndex = QuestState.questDrawer;

        //Initializing the list of quest stages;
        System.Array.Clear(QuestState.stages, 0, QuestState.stages.Length);
        System.Array.Clear(QuestState.amours, 0, QuestState.amours.Length);
        if (QuestState.testBids != null)
            System.Array.Clear(QuestState.testBids, 0, QuestState.testBids.Length);


        //Maybe add this to a function

        //Check for players with over 12 cards
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

        game.isDoneStoryEvent = true;
    }

    public void DrawForStageStart(GameController game)
    {
        for (int i = 0; i < game.numPlayers; i++)
        {
            if (game.players[i].participating)
                game.players[i].hand.Add(GameUtil.DrawFromDeck(game.adventureDeck, 1)[0]);
        }
    }


    public int GetMinRequiredBid()
    {
        if (QuestState.stages[QuestState.currentStage][0].type != "Test Card")
            return -1;

        int currentMinBid = ((TestCard)QuestState.stages[QuestState.currentStage][0]).getMinimum();

        if (QuestState.stages[QuestState.currentStage][0].name == "Test of the Questing Beast" && QuestState.currentQuest.name == "Search for the Questing Beast")
        {
            currentMinBid++;
        }

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
            if (game.userInput.keyboardPrompt.isActive)
            {
                if (game.players[game.currentPlayerIndex].sponsoring || !game.players[game.currentPlayerIndex].participating)
                {
                    game.UpdatePlayerTurn();
                }
                else
                {
                    int aiBid = game.players[game.currentPlayerIndex].strategy.willIBid(GetMinRequiredBid(), game.players[game.currentPlayerIndex].hand, QuestState.round, game);

                    if (aiBid == 0)
                    {
                        game.players[game.currentPlayerIndex].participating = false;
                        game.UpdatePlayerTurn();
                        //game.populatePlayerBoard();
                        UIUtil.PopulatePlayerBoard(game);
                        game.userInput.DeactivateUI();
                        game.userInput.ActivateUserInputCheck("A Test is in play, the minimum bid is: " + GetMinRequiredBid().ToString());
                    }
                    else if (aiBid != -1)
                    {
                        //ai bid smthg other than 0
                        int submittedBid = 0;
                        bool result = int.TryParse(game.userInput.keyboardPrompt.KeyboardInput.text, out submittedBid);

                        if (submittedBid >= GetMinRequiredBid() && submittedBid <= game.players[game.currentPlayerIndex].hand.Count)
                        {
                            QuestState.round++;
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
                    else if (game.userInput.keyboardPrompt.KeyboardInput.text.Length > 0 && Input.GetKeyDown("return"))
                    {
                        // player finished bidding
                        int submittedBid = 0;
                        bool result = int.TryParse(game.userInput.keyboardPrompt.KeyboardInput.text, out submittedBid);

                        if (submittedBid >= GetMinRequiredBid() && submittedBid <= game.players[game.currentPlayerIndex].hand.Count)
                        {
                            QuestState.round++;
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

                    else if (game.userInput.keyboardPrompt.KeyboardInput.text.Length == 0 && Input.GetKeyDown("return"))
                    {

                        game.players[game.currentPlayerIndex].participating = false;
                        game.UpdatePlayerTurn();
                        //game.populatePlayerBoard();
                        UIUtil.PopulatePlayerBoard(game);
                        game.userInput.DeactivateUI();
                        game.userInput.ActivateUserInputCheck("A Test is in play, the minimum bid is: " + GetMinRequiredBid().ToString());
                    }
                }
            }
        }
    }

    public void DiscardCards(GameController game)
    {
        if (game.userInput.UIEnabled)
        {
            if (game.userInput.cardPrompt.isActive)
            {
                List<Card> testDiscardCards = new List<Card>(game.userInput.cardPrompt.selectedCards);

                if (game.userInput.cardPrompt.doneAddingCards)
                {

                    if (testDiscardCards.Count != Mathf.Max(QuestState.testBids[game.currentPlayerIndex] - game.players[game.currentPlayerIndex].calculateBid(game.currentQuest.name, game.players), 0))
                    {

                        //game.returnToPlayerHand();
                        //game.populatePlayerBoard();
                        UIUtil.ReturnToPlayerHand(game.players[game.currentPlayerIndex] , game.userInput.cardPrompt.selectedCards , game);
                        UIUtil.PopulatePlayerBoard(game);

                        game.userInput.DeactivateUI();
                        game.userInput.ActivateCardUIPanel("Invalid number of cards submitted, please submit " + (Mathf.Max(QuestState.testBids[game.currentPlayerIndex] - game.players[game.currentPlayerIndex].calculateBid(game.currentQuest.name, game.players), 0)).ToString() + " cards.");
                    }

                    else
                    {
                        game.numIterations = game.numPlayers + 1;
                        for (int i = 0; i < testDiscardCards.Count; i++)
                        {
                            if (testDiscardCards[i].type == "Amour Card")
                            {
                                if (QuestState.amours[game.currentPlayerIndex] != null)
                                {
                                    if (QuestState.amours[game.currentPlayerIndex].Count == 0)
                                    {
                                        QuestState.amours[game.currentPlayerIndex].Add(testDiscardCards[i]);

                                        testDiscardCards.RemoveAt(i);
                                        i--;
                                    }
                                }
                                else
                                {
                                    QuestState.amours[game.currentPlayerIndex] = new List<Card>();

                                    QuestState.amours[game.currentPlayerIndex].Add(testDiscardCards[i]);

                                    testDiscardCards.RemoveAt(i);
                                    i--;
                                }
                            }
                            //game.DiscardAdvenureCards(testDiscardCards);
                            GameUtil.DiscardCards(game.adventureDeck , testDiscardCards , game.adventureDeckDiscardPileUIButton);
                        }
                    }
                }
            }
        }
    }

    public int GetStageBP(int index, QuestCard q)
    {
        strategyUtil util = new strategyUtil();
        int sum = 0;

        Debug.Log("GetStageBP index: " + index.ToString());

        if (index >= QuestState.stages.Length)
        {
            Debug.Log("Invalid index given to GetStageBP");
            return -2;
        }
        if (QuestState.stages[index] == null)
        {
            Debug.Log("null index given to GetStageBP");
            return -2;
        }
        if (QuestState.stages[index].Count < 1)
        {
            Debug.Log("Blank Stage");
            return -2;

        }
        if (QuestState.stages[index][0] == null)
        {
            Debug.Log("Blank Stage");
            return -2;
        }
        if (QuestState.stages[index][0].type != "Foe Card" && QuestState.stages[index][0].type != "Test Card")
        {
            Debug.Log("First card in stage[" + index.ToString() + "] is not a Foe or Test card");
            return -2;
        }
        else
        {
            if (QuestState.stages[index][0].type == "Test Card")
                return -1;
            else
            {
                //We have a foe and weapon card
                sum += util.getContextBP((FoeCard)QuestState.stages[index][0], QuestState.currentQuest.foe);
                for (int i = 1; i < QuestState.stages[index].Count; i++)
                {
                    if (QuestState.stages[index][i].type != "Weapon Card")
                    {
                        Debug.Log("Invalid quest stage config, stage[" + index.ToString() + "] has a " + QuestState.stages[index][i].type + " at index " + i.ToString() + ".");
                        return -2;
                    }
                    else
                    {
                        sum += ((WeaponCard)QuestState.stages[index][i]).battlePoints;
                    }
                }
            }
            return sum;
        }
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

            Debug.Log(bpCheck.ToString());

            if (bpCheck == -2)
            {
                Debug.Log("In ValidQuest(), error recieved from GetStageBP");
                return false;
            }

            else if (i > 0)
            {
                if (stageBP[i - 1] < bpCheck)
                {
                    if (stageBP[i - 1] == -1 && i > 1)
                    {
                        if (stageBP[i - 2] < bpCheck)
                            stageBP[i] = bpCheck;
                        else
                            return false;
                    }
                    else
                        stageBP[i] = bpCheck;

                }

                else if (bpCheck == -1 && testAdded == false)
                {
                    testAdded = true;
                    stageBP[i] = bpCheck;
                }

                else
                {
                    Debug.Log("Failed on stage number: " + (i + 1).ToString() + " BP of current stage: " + bpCheck.ToString() + "BP of previous stage: " + stageBP[(int)Mathf.Max(i - 1, 0)]);
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
            if (game.userInput.booleanPrompt.isActive)
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
