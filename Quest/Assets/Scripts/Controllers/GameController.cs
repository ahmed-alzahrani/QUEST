using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

/*
    Tournaments:
    Participants are only allowed to use ally , amour , weapon cards
*/

/*
    Quests:
    sponsors only use foes with weapons
    or one test card  

    Participants: only use weapon and amour cards to boost bp only
*/

//CANNOT USE MORDRED WHEN WE HAVE TESTS CARDS ACTIVE

//TO DO LIST:
//MERLIN MAYBE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

//GO TO MORDRED CHECK

//Shield names CHANGEEEEEE         DONNNEEEEEEEEEE 
//REDO DISCARDING                  DONNNEEEEEEEEEE
//CHECK REPOPULATION OF DECKS      DONNNEEEEEEEEEE 
//FIX KINGS CALL TO ARMS           DONNNEEEEEEEEEE
//LOOK AT MORDRED                  DONNNEEEEEEEEEE 
//LOOK AT ALL ISTORIES             DONNNEEEEEEEEEE 
//CHECK THAT EVERYTHING STILL WORKS FINE AFTERWARDS     DONNNNEEEEEEEEEEEEE

public class GameController : Controller
{

    // Use this for initialization
    void Start()
    {
        //setup game variables
        shieldPaths = new List<string> { "Textures/Backings/Blue", "Textures/Backings/Green", "Textures/Backings/Red", "Textures/Backings/Gold" };
        isSettingUpGame = true;
        currentPlayerIndex = 0;
        setupState = 0;
        players = new List<Player>();
        drawnAdventureCards = new List<Card>();
        cpuStrategies = new List<int>();
        winners = new List<int>();
        playerStillOffending = false;
        foundWinner = false;
        kingsRecognition = false;

        adventureDeckDiscardPileUIButton = GameObject.FindGameObjectWithTag("AdventureDiscard").GetComponent<CardUIScript>();
        storyDeckDiscardPileUIButton = GameObject.FindGameObjectWithTag("StoryDiscard").GetComponent<CardUIScript>();

        //Whenever a change in drawing states is done toggle the deck animations
        drawStoryCard = true;
        //drawAdventureCard = false;
        GameUtil.ToggleDeckAnimation(storyDeckUIButton , drawStoryCard);

        //numCardsToBeDrawn = 1;

        //Deck building
        decks = new deckBuilder();
        storyDeck = decks.buildStoryDeck();
        adventureDeck = decks.buildAdventureDeck();

        
        //uncomment here for different scenarios
        Debug.Log(Directory.GetCurrentDirectory());
        //cards = File.ReadAllLines(Directory.GetCurrentDirectory() + "/Assets/Resources/TextAssets/Scenarios/Scenario1/Scenario1Adventure.txt");
        //cardsStory = File.ReadAllLines(Directory.GetCurrentDirectory() + "/Assets/Resources/TextAssets/Scenarios/Scenario1/Scenario1Story.txt");

        //cards = File.ReadAllLines(Directory.GetCurrentDirectory() + "/Assets/Resources/TextAssets/Scenarios/Scenario2/Scenario2Adventure.txt");
        //cardsStory = File.ReadAllLines(Directory.GetCurrentDirectory() + "/Assets/Resources/TextAssets/Scenarios/Scenario2/Scenario2Story.txt");

        cards = File.ReadAllLines(Directory.GetCurrentDirectory() + "/Assets/Resources/TextAssets/Scenarios/Scenario3/Scenario3Adventure.txt");
        cardsStory = File.ReadAllLines(Directory.GetCurrentDirectory() + "/Assets/Resources/TextAssets/Scenarios/Scenario3/Scenario3Story.txt");

        //cards = File.ReadAllLines(Directory.GetCurrentDirectory() + "/Assets/Resources/TextAssets/Scenarios/Scenario4/Scenario4Adventure.txt");
        //cardsStory = File.ReadAllLines(Directory.GetCurrentDirectory() + "/Assets/Resources/TextAssets/Scenarios/Scenario4/Scenario4Story.txt");

        for (int i = 0; i < adventureDeck.deck.Count; i++)
        {
            //shuffle according to the following values
            if (adventureDeck.deck[i].name != cards[i])
            {
                //find a card that matches and switch
                for (int j = i; j < adventureDeck.deck.Count; j++)
                {
                    if (adventureDeck.deck[j].name == cards[i])
                    {
                        //Debug.Log("here");
                        //move from j to i 
                        Card tempCard = adventureDeck.deck[j];
                        adventureDeck.deck[j] = adventureDeck.deck[i];
                        adventureDeck.deck[i] = tempCard;
                    }
                }
            }
        }

        List<Card> tempDeck = new List<Card>();
        //storyCards now
        for (int i = 0; i < storyDeck.deck.Count; i++)
        {
            //shuffle according to the following values
            if (storyDeck.deck[i].name != cardsStory[i])
            {
                //find a card that matches and switch
                for (int j = i; j < storyDeck.deck.Count; j++)
                {
                    if (storyDeck.deck[j].name == cardsStory[i])
                    {
                        tempDeck.Add(storyDeck.deck[j]);
                        break;
                        //move from j to i 
                        //Card tempCard = storyDeck.deck[j];
                        //storyDeck.deck[j] = storyDeck.deck[i];
                        //storyDeck.deck[i] = tempCard;
                    }
                }
            }
        }

        storyDeck.deck.Clear();
        storyDeck.deck = tempDeck;

        //Setup UI buttons for cards (event listeners etc....)
        adventureDeckUIButton.GetComponent<Button>().onClick.AddListener(DrawFromAdventureDeck);
        storyDeckUIButton.GetComponent<Button>().onClick.AddListener(DrawFromStoryDeck);

        //Store gameboard cards
        shieldsCard = GameObject.FindGameObjectWithTag("Shields").GetComponent<CardUIScript>();
        rankCard = GameObject.FindGameObjectWithTag("RankCard").GetComponent<CardUIScript>();

        //Store gameBoard panels
        questPanel = GameObject.FindGameObjectWithTag("QuestCard");
        handPanel = GameObject.FindGameObjectWithTag("CurrentHand");
        weaponPanel = GameObject.FindGameObjectWithTag("WeaponCards");
        questStagePanel = GameObject.FindGameObjectWithTag("QuestStageCards");
        amourPanel = GameObject.FindGameObjectWithTag("AmourCards");
        activatedPanel = GameObject.FindGameObjectWithTag("ActivatedCard");

        //UI building
        userInput.SetupUI();
        userInput.ActivateUserInputCheck("How many players are playing the game??");

        //Setup mordred functionality
        mordredControllerBeta = new int[3];

        for (int i = 0; i < mordredControllerBeta.Length; i++)
        {
            mordredControllerBeta[i] = -1;
        }

        mordCurr = 0;
    }

    //Update is called once per frame
    //player discard piles for over 12 cards and etc...
    void Update()
    {
        if (!foundWinner)
        {
            //MAYBEEEEEE!!!!!
            // MORDRED CHECK 
            MordredCheck();

            if (!playerStillOffending)
            {
                if (isSettingUpGame)
                {
                    QueryingUtil.PlayerSetup(this);
                    QueryingUtil.QueryStrategies(this);
                }
                else
                {
                    //CHECK FOR DRAWN STORY CARDS
                    SetupStoryCardCheck();
                    //CHECK IF STORY CARD IS DONE ELSE EXECUTE STORY CARD 
                    if (!StoryCardDone()) { RunStoryCard(); }
                    //ADDING CARDS
                    CardAdditionCheck();
                }
            }
            else
            {
                //We need to discard some cards 
                QueryingUtil.DiscardCards(this);
                //Check for addition of cards to card panel
                DiscardCardAddition();
                //End discard state
                CheckDiscardPhase();
            }
            //Calculate player info
            UIUtil.CalculateUIPlayerInfo(this);
            //Get winners here
            UIUtil.FindWinners(players , winners , userInput);
            //Check if game is over
            if (winners.Count > 1) { foundWinner = true; }
        }
        else
        {
            //Check for yes or no and repeat game (go back to main menu)
            if (userInput.booleanPrompt.buttonResult != "") { SceneManager.LoadScene("MainMenu", LoadSceneMode.Single); }
        }
    }


    /* FUNCTIONS BASED ON THIS GAME */

    public void DrawFromAdventureDeck()
    {
        //have a draw from adventure deck that takes a certain number of cards to be drawn instead doing it one by one that way click only happens once
        if (drawAdventureCard)
        {
            drawnAdventureCards = GameUtil.DrawFromDeck(adventureDeck, numCardsToBeDrawn);
            drawAdventureCard = false;
        }

        GameUtil.ToggleDeckAnimation(storyDeckUIButton , storyDeckDiscardPileUIButton);
    }

    public void DrawFromStoryDeck()
    {
        //only one story card is ever drawn
        if (drawStoryCard)
        {
            drawnStoryCard = GameUtil.DrawFromDeck(storyDeck, 1)[0];
            drawStoryCard = false;
        }

        GameUtil.ToggleDeckAnimation(storyDeckUIButton , storyDeckDiscardPileUIButton);
    }

    void MordredCheck()
    {
        //reset input if u change ur mind
        if (Input.GetKeyDown(KeyCode.Q))
        {
            mordCurr = 0;
            Debug.Log("Reset Mordred Input");
            return;
        }

        //shift and then numbers
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (mordCurr < 3)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                {
                    mordredControllerBeta[mordCurr] = 0;
                    mordCurr++;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                {
                    mordredControllerBeta[mordCurr] = 1;
                    mordCurr++;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                {
                    mordredControllerBeta[mordCurr] = 2;
                    mordCurr++;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                {
                    mordredControllerBeta[mordCurr] = 3;
                    mordCurr++;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
                {
                    mordredControllerBeta[mordCurr] = 4;
                    mordCurr++;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
                {
                    mordredControllerBeta[mordCurr] = 5;
                    mordCurr++;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
                {
                    mordredControllerBeta[mordCurr] = 6;
                    mordCurr++;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
                {
                    mordredControllerBeta[mordCurr] = 7;
                    mordCurr++;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
                {
                    mordredControllerBeta[mordCurr] = 8;
                    mordCurr++;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
                {
                    mordredControllerBeta[mordCurr] = 9;
                    mordCurr++;
                }

            }


            if (mordCurr >= mordredControllerBeta.Length)
            {
                Debug.Log("Player " + (mordredControllerBeta[0] + 1).ToString() + " Casting Mordred on Player " +
                    (mordredControllerBeta[1] + 1).ToString() + "'s Ally number " + (mordredControllerBeta[2] + 1).ToString());
                if (mordredControllerBeta[0] < numPlayers && mordredControllerBeta[1] < numPlayers)
                {
                    if (players[mordredControllerBeta[1]].activeAllies.Count > mordredControllerBeta[2])
                    {
                        for (int i = 0; i < players[mordredControllerBeta[0]].hand.Count; i++)
                        {
                            if (players[mordredControllerBeta[0]].hand[i].name == "Mordred")
                            {
                                List<Card> tempCards = new List<Card>();

                                tempCards.Add(players[mordredControllerBeta[0]].hand[i]);
                                tempCards.Add(players[mordredControllerBeta[1]].activeAllies[mordredControllerBeta[2]]);

                                players[mordredControllerBeta[0]].hand.RemoveAt(i);
                                players[mordredControllerBeta[1]].activeAllies.RemoveAt(mordredControllerBeta[2]);

                                GameUtil.DiscardCards(adventureDeck, tempCards, adventureDeckDiscardPileUIButton);
                            }
                        }
                        UIUtil.PopulatePlayerBoard(this);
                    }
                }
                mordCurr = 0;
            }
        }
    }

    void SetupStoryCardCheck()
    {
        if (drawnStoryCard != null)
        {
            Debug.Log("DRAWN STORY CARD: " + drawnStoryCard.name);

            UIUtil.EmptyPanel(questStagePanel);
            questStageBPTotal.text = "";
            questStageNumber.text = "";

            //setup quest , tournament or event
            if (drawnStoryCard.type == "Event Card")
            {
                currentEvent = (EventCard)drawnStoryCard;
            }
            else if (drawnStoryCard.type == "Tournament Card")
            {
                userInput.ActivateBooleanCheck("Do you want to participate?");
                currentTournament = (TournamentCard)drawnStoryCard;
            }
            else if (drawnStoryCard.type == "Quest Card")
            {
                //store player that first drew the quest card
                QuestState.questDrawer = currentPlayerIndex;
                userInput.ActivateBooleanCheck("Do you want to sponsor this quest?");
                currentQuest = (QuestCard)drawnStoryCard;
            }

            numIterations = 0;
            UIUtil.AddCardToPanel(UIUtil.CreateUIElement(drawnStoryCard, cardPrefab), questPanel);
            drawnStoryCard = null;
        }
    }

    bool StoryCardDone()
    {
        if (isDoneStoryEvent)
        {
            //discard story card into story discard pile
            if (currentEvent != null)
            {
                List<Card> cards = new List<Card>();
                cards.Add(currentEvent);
                GameUtil.DiscardCards(storyDeck, cards, storyDeckDiscardPileUIButton);
                // storyDeckDiscardPileUIButton.myCard = currentEvent;
                //storyDeckDiscardPileUIButton.ChangeTexture();
                //storyDeck.discard.Add(currentEvent);
            }
            else if (currentQuest != null)
            {
                List<Card> cards = new List<Card>();
                cards.Add(currentQuest);
                GameUtil.DiscardCards(storyDeck, cards, storyDeckDiscardPileUIButton);
                //storyDeckDiscardPileUIButton.myCard = currentQuest;
                //storyDeckDiscardPileUIButton.ChangeTexture();
                //storyDeck.discard.Add(currentQuest);
            }
            else if (currentTournament != null)
            {
                List<Card> cards = new List<Card>();
                cards.Add(currentTournament);
                GameUtil.DiscardCards(storyDeck, cards, storyDeckDiscardPileUIButton);
                //storyDeckDiscardPileUIButton.myCard = currentTournament;
                //storyDeckDiscardPileUIButton.ChangeTexture();
                //storyDeck.discard.Add(currentTournament);
            }

            isDoneStoryEvent = false;
            currentEvent = null;
            currentQuest = null;
            currentTournament = null;

            //reset players participation
            GameUtil.ResetPlayers(players);
            UIUtil.EmptyPanel(questPanel);
            System.Array.Clear(queriedCards, 0, queriedCards.Length);

            //reset turn to draw new story card
            drawStoryCard = true;
            GameUtil.ToggleDeckAnimation(storyDeckUIButton, drawStoryCard);
            userInput.DeactivateUI(); // just in case
            UIUtil.UpdatePlayerTurn(this);       // go to next turn

            return true;
        }

        return false;
    }

    void RunStoryCard()
    {
        if (currentEvent != null)
        {
            currentEvent.effect.execute(players, currentEvent, this);
        }
        else if (currentTournament != null)
        {
            currentTournament.tournament.execute(null, currentTournament, this);
        }
        else if (currentQuest != null)
        {
            currentQuest.quest.execute(players, currentQuest, this);

            //CHECK HERE IF CARDS TO BE ADDED BY SPONSOR BREAK THE RULES RETURN ALL THESE CARDS TO HIS HAND AND FORCE HIM TO REDECIDE
            if (QuestState.state == "Sponsoring")
            {
                if (!GameUtil.AnyFoes(userInput.cardPrompt.selectedCards) && GameUtil.AnyWeapons(userInput.cardPrompt.selectedCards))
                {
                    //there are foes but no weapons return all cards to hand and start again
                    UIUtil.ReturnToPlayerHand(players[GameUtil.CheckSponsorship(players)], userInput.cardPrompt.selectedCards, this);
                }
            }
        }
    }

    void CardAdditionCheck()
    {
        if (selectedCard != null)
        {
            //all ally cards activated are added to the board and active allies
            if (selectedCard.type == "Ally Card")
            {
                //ally cards always get added to active allies
                AllyCard ally = (AllyCard)selectedCard;
                players[currentPlayerIndex].activeAllies.Add(ally);
                players[currentPlayerIndex].hand.Remove(selectedCard);
                UIUtil.AddCardToPanel(UIUtil.CreateUIElement(selectedCard, cardPrefab) , playerAllyPanels[currentPlayerIndex]);
            }
            else
            {
                //check cards for removal from card panel
                bool removedCard = userInput.CheckCard(selectedCard);

                if (removedCard)
                {
                    //removed card from panel So add it back to hand
                    players[currentPlayerIndex].hand.Add(selectedCard);
                    UIUtil.AddCardToPanel(UIUtil.CreateUIElement(selectedCard, cardPrefab), handPanel);
                }
                else
                {
                    bool addToPanel = true;

                    //CLUSTERFUCK
                    addToPanel = EventCheck(QuestCheck(TournamentCheck()));

                    //result
                    if (addToPanel)
                    {
                        //add it into the panel
                        players[currentPlayerIndex].hand.Remove(selectedCard);
                        userInput.AddToUICardPanel(UIUtil.CreateUIElement(selectedCard, cardPrefab));
                    }
                    else
                    {
                        //return it to hand since we didn't delete it from player we are fine
                        UIUtil.AddCardToPanel(UIUtil.CreateUIElement(selectedCard, cardPrefab), handPanel);
                    }
                }
            }

            //maybe we can check for certain cards like mordred here with some state
            selectedCard = null;
        }
    }

    void DiscardCardAddition()
    {
        if (selectedCard != null)
        {
            //all ally cards activated are added to the board and active allies
            if (selectedCard.type == "Ally Card")
            {
                //ally cards always get added to active allies
                AllyCard ally = (AllyCard)selectedCard;
                players[currentPlayerIndex].activeAllies.Add(ally);
                players[currentPlayerIndex].hand.Remove(selectedCard);
                UIUtil.AddCardToPanel(UIUtil.CreateUIElement(selectedCard, cardPrefab), playerAllyPanels[currentPlayerIndex]);
            }
            else
            {
                bool removed = userInput.CheckDiscardCard(selectedCard);
                if (removed)
                {
                    //add card back to player 
                    players[currentPlayerIndex].hand.Add(selectedCard);
                    UIUtil.PopulatePlayerBoard(this);
                }
                else
                {
                    //add it into the panel
                    players[currentPlayerIndex].hand.Remove(selectedCard);

                    //Debug.Log(selectedCard.name);
                    userInput.AddToUIDiscardPanel(UIUtil.CreateUIElement(selectedCard, cardPrefab));
                }
            }

            selectedCard = null;
        }
    }

    void CheckDiscardPhase()
    {
        if (numIterations >= numPlayers)
        {
            //done discarding
            numIterations = 0;
            userInput.DeactivateDiscardPanel();
            playerStillOffending = false;
        }
    }

    bool TournamentCheck()
    {
        bool addToPanel = true;

        if (currentTournament != null)
        {
            // we are in a tournament check for weapon, amour cards allies are always added
            if (selectedCard.type == "Weapon Card")
            {
                for (int i = 0; i < userInput.cardPrompt.selectedCards.Count; i++)
                {
                    if (userInput.cardPrompt.selectedCards[i].name == selectedCard.name)
                    {
                        //duplicate exists cannot add it in
                        addToPanel = false;
                    }
                }
            }
            else if (selectedCard.type == "Amour Card")
            {
                //check for any amours already played 
                if (!GameUtil.AnyAmours(TournamentState.undiscardedCards[currentPlayerIndex]))
                {
                    addToPanel = true;
                }
                else
                {
                    addToPanel = false;
                }

                //check amours at the moment
                for (int i = 0; i < userInput.cardPrompt.selectedCards.Count; i++)
                {
                    if (userInput.cardPrompt.selectedCards[i].type == "Amour Card")
                    {
                        //more than one amour cannot add it
                        addToPanel = false;
                    }
                }
            }
            else
            {
                // not weapon amour or ally
                return false;
            }
        }

        return addToPanel;
    }

    bool QuestCheck(bool result)
    {
        bool addToPanel = result;

        if (currentQuest != null)
        {
            //if it is the sponsor check that he added cards properly
            if (players[currentPlayerIndex].sponsoring)
            {
                if (GameUtil.AnyTests(userInput.cardPrompt.selectedCards)) { addToPanel = false; }
                else if (selectedCard.type == "Test Card")
                {
                    //check that it is the only card in the panel and do not allow him to any more cards
                    if (userInput.cardPrompt.selectedCards.Count > 0) { addToPanel = false; }
                    else { addToPanel = true; }
                }
                else if (selectedCard.type == "Foe Card")
                {
                    if (!GameUtil.AnyFoes(userInput.cardPrompt.selectedCards))
                    {
                        addToPanel = true;
                    }
                    else
                    {
                        addToPanel = false;
                    }
                }
                else if (selectedCard.type == "Weapon Card")
                {
                    // we have a weapon card which needs a special check
                    for (int i = 0; i < userInput.cardPrompt.selectedCards.Count; i++)
                    {
                        if (userInput.cardPrompt.selectedCards[i].name == selectedCard.name)
                        {
                            //duplicate exists cannot add it in
                            addToPanel = false;
                            break;
                        }
                    }

                    //if no duplication check if there is a foe in there
                    if (addToPanel)
                    {
                        addToPanel = GameUtil.AnyFoes(userInput.cardPrompt.selectedCards);
                    }
                }
                else
                {
                    addToPanel = false;
                }
            }
            else if (players[currentPlayerIndex].participating)
            {
                //only allowed weapon and amour cards
                if (selectedCard.type == "Weapon Card")
                {
                    for (int i = 0; i < userInput.cardPrompt.selectedCards.Count; i++)
                    {
                        if (userInput.cardPrompt.selectedCards[i].name == selectedCard.name)
                        {
                            //duplicate exists cannot add it in
                            addToPanel = false;
                            break;
                        }
                    }
                }
                else if (selectedCard.type == "Amour Card")
                {
                    //MAYBEEEEEE NEEEEEEDDSSSSSSS A CHANGEEEEE
                    //check for any amours already played 
                    if (!GameUtil.AnyAmours(QuestState.amours[currentPlayerIndex]))
                    {
                        addToPanel = true;
                    }
                    else
                    {
                        addToPanel = false;
                    }

                    //check for amours played at this instance 
                    for (int i = 0; i < userInput.cardPrompt.selectedCards.Count; i++)
                    {
                        if (userInput.cardPrompt.selectedCards[i].type == "Amour Card")
                        {
                            //more than one amour cannot add it
                            addToPanel = false;
                            break;
                        }
                    }
                }
                else if (selectedCard.type == "Foe Card")
                {
                    if (QuestState.stages != null)
                    {
                        if (QuestState.stages[QuestState.currentStage] != null)
                        {
                            if (QuestState.stages[QuestState.currentStage][0] != null)
                            {
                                if (QuestState.stages[QuestState.currentStage][0].type == "Test Card")
                                    addToPanel = true;
                                else
                                    addToPanel = false;
                            }
                            else
                                addToPanel = false;
                        }
                        else
                            addToPanel = false;
                    }
                    else
                        addToPanel = false;
                }
                else
                {
                    // not weapon amour or ally
                    addToPanel = false;
                }
            }
        }

        return addToPanel;
    }

    bool EventCheck(bool result)
    {
        // if we have an event we are fine else return previous result
        if (currentEvent != null)
        {
            return true;
        }
        else
        {
            return result;
        }
    }

    /*
    //////////////////////////////////
    public bool PlayerOffending()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].handCheck())
            {
                // we are over 12 cards on a player 
                return true;
            }
        }

        return false;
    }

    //////////////////////////////////
    //check the size of find winner if it is greater than 1 we have a winner and we stop the game
    public void FindWinners()
    {
        //check for winners
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].rankUpCheck(players[i].score, 0))
            {
                //store the index of the winners of the game
                winners.Add(i);
            }
        }

        //create a userPrompt for the winners
        if (winners.Count > 0)
        {
            string winner = "";

            //code for showing winners here
            for (int i = 0; i < winners.Count; i++)
            {
                winner += players[winners[i]].name;
            }

            winner += " Won the Game!!!";

            userInput.ActivateBooleanCheck(winner);
        }
    }

    //////////////////////////////////
    public void returnToPlayerHand()
    {
        //add userInput selected hand back to player
        for (int i = 0; i < userInput.cardPrompt.selectedCards.Count; i++)
        {
            //just in case
            players[CheckSponsorship()].hand.Add(userInput.cardPrompt.selectedCards[i]);
            userInput.RemoveFromCardUIPanel(i);
            i--;
        }

        populatePlayerBoard();
    }

    /////////////////////////////////////
    public bool AnyTests()
    {
        for (int i = 0; i < userInput.cardPrompt.selectedCards.Count; i++)
        {
            if (userInput.cardPrompt.selectedCards[i].type == "Test Card")
            {
                return true;
            }
        }

        return false;
    }

    ////////////////////////////////
    public bool AnyFoes()
    {
        for (int i = 0; i < userInput.cardPrompt.selectedCards.Count; i++)
        {
            if (userInput.cardPrompt.selectedCards[i].type == "Foe Card")
            {
                return true;
            }
        }

        return false;
    }


    //////////////////////////////////////
    public bool AnyWeapons()
    {
        for (int i = 0; i < userInput.cardPrompt.selectedCards.Count; i++)
        {
            if (userInput.cardPrompt.selectedCards[i].type == "Weapon Card")
            {
                return true;
            }
        }

        return false;
    }

    ////////////////////////////////////// (AnyAmours() in util file)
    public bool PlayedAmour(List<Card>[] playedCards)
    {
        if (playedCards == null || playedCards[currentPlayerIndex] == null) { return false; }
        else
        {
            for (int i = 0; i < playedCards[currentPlayerIndex].Count; i++)
            {
                if (playedCards[currentPlayerIndex][i].type == "Amour Card")
                {
                    return true;
                }
            }
        }

        return false;
    }

    //////////////////////////////
    public void DiscardAdvenureCards(List<Card> discardCards)
    {
        //logic discard as well as UI one
        adventureDeck.discardCards(discardCards);
        adventureDeckDiscardPileUIButton.myCard = adventureDeck.discard[adventureDeck.discard.Count - 1];
        adventureDeckDiscardPileUIButton.ChangeTexture();
    }

    //////////////////////////////
    //update quest data of a certain stage
    public void AddQuestData(int stageNumber, int totalBP)
    {
        questStageNumber.text = "Stage: " + stageNumber.ToString();
        questStageBPTotal.text = "BP: " + totalBP.ToString();
    }

    ///////////////////////////////
    public void CalculateUIPlayerInfo()
    {
        //might need a function in player that calculates the BP at any time
        for (int i = 0; i < players.Count; i++)
        {
            UINames[i].text = "Name: " + players[i].name;
            UINumCards[i].text = "Cards: " + players[i].hand.Count.ToString();
            UIRanks[i].text = "Rank: " + players[i].rankCard.name;
            UIShields[i].text = "Shields: " + players[i].score.ToString();
        }

        //adds player BPS for a quest or generally
        PopulatePlayerBPS();

        playerPanels[currentPlayerIndex].GetComponent<Outline>().enabled = true;
    }

    //////////////////////////////////
    public GameObject CreateUIElement(Card cardLogic)
    {
        //create game object of card prefab
        GameObject UICard = Instantiate(cardPrefab, new Vector3(0, 0, 0), new Quaternion());

        //add card logic to the ui card
        CardUIScript script = UICard.GetComponent<CardUIScript>();
        script.myCard = cardLogic;
        script.ChangeTexture();

        return UICard;
    }

    //CALLED AddCardToPanel in UIUtil
    ///////////////////////
    // Add separate additions to separate panels of the game board by creating a function here (same style just different panel)
    public void AddToPanel(GameObject UICard, GameObject panel)
    {
        CardUIScript script = UICard.GetComponent<CardUIScript>();
        if (panel.name == "CurrentHand")
        {
            script.isHandCard = true;
        }
        else
        {
            script.isHandCard = false;
        }

        //set as a child of the ui card
        UICard.transform.SetParent(panel.transform);
    }

    //////////////////////////
    public List<Card> DrawFromDeck(Deck deckToDrawFrom, int numCards)
    {
        List<Card> drawnCards = new List<Card>();

        for (int i = 0; i < numCards; i++)
        {
            drawnCards.Add(deckToDrawFrom.drawCard());
        }

        return drawnCards;
    }

    ///////////////////////////////
    //checking number of participants
    public int CheckParticipation()
    {
        int numberOfParticipants = 0;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].participating)
            {
                numberOfParticipants++;
            }
        }
        return numberOfParticipants;
    }

    ///////////////////////////////
    //checking for if there is a sponsor
    public int CheckSponsorship()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].sponsoring)
            {
                return i;
            }
        }
        return -1;
    }

    ////////////////////////////////////
    //toggle the story deck and adventure deck ui animations
    public void ToggleDeckAnimation()
    {
        storyDeckUIButton.GetComponent<Animator>().enabled = drawStoryCard;
        adventureDeckUIButton.GetComponent<Animator>().enabled = drawAdventureCard;
    }

    //////////////////////////////
    //if you want to empty any panel
    public void EmptyPanel(GameObject myPanel)
    {
        for (int i = 0; i < myPanel.transform.childCount; i++)
        {
            Destroy(myPanel.transform.GetChild(i).gameObject);
        }
    }

    ////////////////////////////////////////
    //to be called by players to setup rank and shields
    public void SetRankCardUI()
    {
        rankCard.myCard = players[currentPlayerIndex].rankCard;
        rankCard.ChangeTexture();
        UIShieldNum.text = ": " + players[currentPlayerIndex].score;
    }

    ////////////////////////////////////
    //adds ui elements to game board of current player
    public void PopulatePlayerBoard()
    {
        //using current player once we setup player turn change will update this
        EmptyPanel(handPanel);
        EmptyPanel(allyPanel);

        Player myPlayer = players[currentPlayerIndex];

        //rank card and shield numbers
        SetRankCardUI();

        //shield cards
        shieldsCard.myCard = new RankCard(null, null, myPlayer.shieldPath, 0);
        shieldsCard.ChangeTexture();

        //Setup Hand
        for (int i = 0; i < myPlayer.hand.Count; i++)
        {
            AddToPanel(CreateUIElement(myPlayer.hand[i]), handPanel);
        }

        //Setup Ally cards
        for (int i = 0; i < myPlayer.activeAllies.Count; i++)
        {
            AddToPanel(CreateUIElement(myPlayer.activeAllies[i]), allyPanel);
        }
    }

    //takes gamecontroller in Ui util
    /////////////////////////////
    public void PopulatePlayerBPS()
    {
        //sets up player BPS for quests tourneys etc .... and returns them back when done check current event and quest and tourney
        for (int i = 0; i < players.Count; i++)
        {
            //check for special ally abilities
            if (currentQuest != null)
            {
                UIBPS[i].text = "BP: " + players[i].CalculateBP(currentQuest.name, players);
            }
            else
            {
                UIBPS[i].text = "BP: " + players[i].CalculateBP("", players);

            }

            //participating in a quest tourneys don't matter really
            if (players[i].participating)
            {
                int totalPlayedBP = 0;

                //go through amours and foe / weapon cards

                //go through amours of quest will be empty if in tournament or anything else
                if (QuestState.amours != null)
                {
                    if (QuestState.amours[i] != null)
                    {
                        //add amour values
                        for (int j = 0; j < QuestState.amours[i].Count; j++)
                        {
                            AmourCard amour = (AmourCard)QuestState.amours[i][j];
                            totalPlayedBP += amour.battlePoints;
                        }
                    }
                }

                //go through weapons of quest
                if (queriedCards[i] != null && currentQuest != null)
                {
                    for (int k = 0; k < queriedCards[i].Count; k++)
                    {
                        if (queriedCards[i][k].type == "Weapon Card")
                        {
                            //these should be weapons
                            totalPlayedBP += ((WeaponCard)queriedCards[i][k]).battlePoints;
                        }
                    }
                }

                //go through weapons and amours of tournament add them up
                if (currentTournament != null)
                {
                    if (TournamentState.undiscardedCards != null && TournamentState.undiscardedCards[i] != null)
                    {
                        for (int k = 0; k < TournamentState.undiscardedCards[i].Count; k++)
                        {
                            //there probably is a function that does this ????
                            if (TournamentState.undiscardedCards[i][k].type == "Weapon Card")
                            {
                                totalPlayedBP += ((WeaponCard)TournamentState.undiscardedCards[i][k]).battlePoints;
                            }
                            else if (TournamentState.undiscardedCards[i][k].type == "Amour Card")
                            {
                                totalPlayedBP += ((AmourCard)TournamentState.undiscardedCards[i][k]).battlePoints;
                            }
                        }
                    }
                }

                //add up result
                UIBPS[i].text += " (" + totalPlayedBP.ToString() + ")";
            }
        }
    }


    ///////////////////////////
    public void populateQuestBoard(bool faceUp)
    {
        EmptyQuestPanel();
        questStageNumber.text = "Stage: " + (QuestState.currentStage + 1).ToString();

        //check if it is flipped b4 adding the card and set its current backing
        //maybe show bp total maybe not
        if (QuestState.stages[QuestState.currentStage] != null)
        {
            for (int i = 0; i < QuestState.stages[QuestState.currentStage].Count; i++)
            {
                //flip the cards
                GameObject card = CreateUIElement(QuestState.stages[QuestState.currentStage][i]);
                if (!faceUp)
                {
                    card.GetComponent<CardUIScript>().flipCard();
                }
                else
                {
                    card.GetComponent<CardUIScript>().ChangeTexture();
                }
                AddToPanel(card, questStagePanel);
            }
        }
    }

    //removed not really needed
    ///////////////////////////
    public void EmptyQuestPanel()
    {
        EmptyPanel(questStagePanel);
        questStageNumber.text = "";
        questStageBPTotal.text = "";
    }

    //////////////////////////////////
    //adds ui elements to game board of current player
    public void populatePlayerBoard()
    {
        //using current player once we setup player turn change will update this
        EmptyPanel(handPanel);
        //EmptyPanel(amourPanel);
        EmptyPanel(allyPanel);
        //EmptyPanel(weaponPanel);
        //EmptyPanel(activatedPanel);

        Player myPlayer = players[currentPlayerIndex];

        //rank card and shield numbers
        SetRankCardUI();

        //shield cards
        shieldsCard.myCard = new RankCard(null, null, myPlayer.shieldPath, 0);
        shieldsCard.ChangeTexture();

        //Setup Hand
        for (int i = 0; i < myPlayer.hand.Count; i++)
        {
            AddToPanel(CreateUIElement(myPlayer.hand[i]), handPanel);
        }

        //Setup Ally cards
        for (int i = 0; i < myPlayer.activeAllies.Count; i++)
        {
            AddToPanel(CreateUIElement(myPlayer.activeAllies[i]), allyPanel);
        }
    }

    //////////////////////////////////////
    public void ResetPlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].participating = false;
            players[i].sponsoring = false;
        }
    }

    ///////////////////////////////////
    //initializes player information and hands
    public List<Player> CreatePlayers(List<string> humanNames, List<string> cpuNames)
    {
        List<Player> myPlayers;
        myPlayers = new List<Player>();

        //create human players (set up their references and give them a hand)
        for (int i = 0; i < numHumanPlayers; i++)
        {
            int shield = Random.Range(0, shieldPaths.Count - 1);
            Player myPlayer = new Player("Player" + (i + 1).ToString(), DrawFromDeck(adventureDeck, 12), new iStrategyPlayer(), shieldPaths[shield]);
            shieldPaths.RemoveAt(shield);       //Each player has unique shields
            myPlayer.gameController = this;
            playerPanels[i].SetActive(true);    //add a ui panel for each player
            myPlayers.Add(myPlayer);
        }

        // create AI players

        for (int i = 0; i < numCpus; i++)
        {
            //int rnd = Random.Range(0 , 2);
            int shield = Random.Range(0, shieldPaths.Count - 1);
            Player newPlayer;

            Debug.Log(cpuStrategies[i]);

            //log resulting strategy
            if (cpuStrategies[i] == 1) { newPlayer = new Player("CPU" + (i + 1).ToString(), DrawFromDeck(adventureDeck, 12), new iStrategyCPU1(), shieldPaths[shield]); }
            else { newPlayer = new Player("CPU" + (i + 1).ToString(), DrawFromDeck(adventureDeck, 12), new iStrategyCPU2(), shieldPaths[shield]); }

            //newPlayer = new Player("CPU" + (i + 1).ToString(), DrawFromDeck(adventureDeck, 12), new iStrategyCPU1(), shieldPaths[shield]);

            playerPanels[i + numHumanPlayers].SetActive(true);    //add a ui panel for each player
            shieldPaths.RemoveAt(shield); //Each player has unique shields
            myPlayers.Add(newPlayer);
        }

        //create a queried cards array
        queriedCards = new List<Card>[numPlayers];
        sponsorQueriedCards = new List<Card>[5];
        return myPlayers;
    }







    /////////////////////////////////////////
    //players over 12 cards 
    public void DiscardCards()
    {
        Debug.Log("IS DONE DISCARDING: " + userInput.discardPrompt.doneAddingCards);

        if (userInput.discardPrompt.isActive)
        {
            //Debug.Log("Whatever@#23@#");
            if (numIterations < numPlayers)
            {
                List<Card> discards = players[currentPlayerIndex].strategy.fixHandDiscrepancy(players[currentPlayerIndex].hand);

                if (discards != null)
                {
                    //ai discarded we are done just discard his cards
                    DiscardAdvenureCards(discards);

                    //get rid of those cards 
                    discards.Clear();
                    numIterations++;
                    UpdatePlayerTurn();

                    while (!players[currentPlayerIndex].handCheck() && numIterations < numPlayers)
                    {
                        //doesn't need to discard update turn
                        numIterations++;
                        UpdatePlayerTurn();
                    }

                    userInput.DeactivateDiscardPanel();
                    userInput.ActivateDiscardCheck("You need to Discard " + (players[currentPlayerIndex].hand.Count - 12).ToString() + " Cards");
                    Debug.Log("discarded AI Stuff");
                }
                else if (userInput.discardPrompt.doneAddingCards)
                {
                    //if things don't go well
                    if (players[currentPlayerIndex].handCheck())
                    {
                        Debug.Log("Not enough cards where discarded");
                        //add back to players hand 
                        returnToPlayerHand();
                        userInput.discardPrompt.doneAddingCards = false;
                    }
                    else
                    {
                        Debug.Log("Discarded player Cards");
                        //if things go well
                        DiscardAdvenureCards(userInput.discardPrompt.selectedCards);

                        numIterations++;
                        UpdatePlayerTurn();

                        while (!players[currentPlayerIndex].handCheck() && numIterations < numPlayers)
                        {
                            //doesn't need to discard update turn
                            numIterations++;
                            UpdatePlayerTurn();
                        }

                        userInput.DeactivateDiscardPanel();
                        userInput.ActivateDiscardCheck("You need to Discard " + (players[currentPlayerIndex].hand.Count - 12).ToString() + " Cards");
                    }
                }
            }
        }
    }






    ///////////////////////////////////////////
    //Asks user for input then builds initial game board
    public void PlayerSetup()
    {
        // do ui checks here then build board when done
        if (userInput.UIEnabled)
        {
            if (userInput.keyboardPrompt.isActive)
            {
                if (setupState == 0)
                {
                    if (userInput.keyboardPrompt.KeyboardInput.text.Length > 0 && Input.GetKeyDown("return"))
                    {
                        bool result = int.TryParse(userInput.keyboardPrompt.KeyboardInput.text, out numPlayers);

                        if (result && numPlayers > 1 && numPlayers < 5)
                        {
                            userInput.DeactivateUI();
                            setupState = 1;
                            userInput.ActivateUserInputCheck("How Many Human Players?");
                        }
                        else
                        {
                            userInput.keyboardPrompt.KeyboardInput.text = "";
                        }
                    }
                }
                else if (setupState == 1)
                {
                    if (userInput.keyboardPrompt.KeyboardInput.text.Length > 0 && Input.GetKeyDown("return"))
                    {
                        bool result = int.TryParse(userInput.keyboardPrompt.KeyboardInput.text, out numHumanPlayers);

                        if (result && numHumanPlayers > 0 && numPlayers - numHumanPlayers >= 0)
                        {
                            userInput.DeactivateUI();
                            setupState = 2;
                            numCpus = numPlayers - numHumanPlayers;
                        }
                        else
                        {
                            userInput.keyboardPrompt.KeyboardInput.text = "";
                        }
                    }
                }
            }
        }
        else if (setupState == 2)
        {
            //done checking user input
            Debug.Log("NUMBER OF PLAYERS: " + numPlayers);
            Debug.Log("NUMBER OF HUMAN PLAYERS: " + numHumanPlayers);
            Debug.Log("NUMBER OF CPUS: " + numCpus);

            numIterations = 0;
            userInput.ActivateUserInputCheck("what strategy do you want for cpu " + (numIterations + 1).ToString() + " ?");
            setupState = 3; //won't go in here again
        }
    }

    ///////////////////////////////////////
    public void QueryStrategies()
    {
        if (userInput.UIEnabled)
        {
            if (userInput.keyboardPrompt.isActive)
            {
                if (setupState >= 3)
                {
                    if (numIterations < numCpus)
                    {
                        if (userInput.keyboardPrompt.KeyboardInput.text.Length > 0 && Input.GetKeyDown("return"))
                        {
                            int strategy = 0;
                            bool result = int.TryParse(userInput.keyboardPrompt.KeyboardInput.text, out strategy);

                            if (result && strategy > 0 && strategy < 3)
                            {
                                //either 1 or 2
                                cpuStrategies.Add(strategy);
                                userInput.DeactivateUI();
                                numIterations++;
                                userInput.ActivateUserInputCheck("what strategy do you want for cpu " + (numIterations + 1).ToString() + " ?");
                            }
                            else
                            {
                                userInput.keyboardPrompt.KeyboardInput.text = "";
                            }
                        }
                    }
                    else
                    {
                        //we are done
                        userInput.DeactivateUI();
                        //at the end
                        players = CreatePlayers(null, null);
                        populatePlayerBoard();
                        isSettingUpGame = false;
                        for (int i = 0; i < cpuStrategies.Count; i++)
                        {
                            Debug.Log("strategies" + cpuStrategies[i]);
                        }
                    }
                }
            }
        }
    }








    ///////////////////////////////////////
    //SOME CHANGES HERE FOR PARTICIPATE IN TOURNEY FUNCTION
    public void ParticipationCheck(string state)
    {
        if (userInput.UIEnabled)
        {
            if (userInput.booleanPrompt.isActive)
            {
                //check with currentplayerIndex
                if (numIterations < numPlayers)
                {
                    int participation = -1;
                    if (state == "Tournament")
                    {
                        //NEED TO CHANGE THAT!!!!!!!!!!!!!!!!!!
                        participation = players[currentPlayerIndex].strategy.participateInTourney(players, currentTournament.shields, this);

                    }
                    else if (state == "Quest")
                    {
                        //NEED TO CHANGE THISSSS!!!!!!!!!!!!!!!!
                        participation = players[currentPlayerIndex].strategy.participateInQuest(currentQuest.stages, players[currentPlayerIndex].hand, this);
                    }

                    if (players[currentPlayerIndex].sponsoring)
                    {
                        //skip if sponsoring
                        //this will help generalize participation in both tournamnets and quests
                        numIterations++;
                        UpdatePlayerTurn();
                    }
                    else if (participation == 0)
                    {
                        //not joining
                        players[currentPlayerIndex].participating = false; // just in case
                        UpdatePlayerTurn();
                        populatePlayerBoard();
                        userInput.DeactivateUI();
                        userInput.ActivateBooleanCheck("Do you want to participate?");
                        numIterations++;
                    }
                    else if (participation == 1)
                    {
                        //joining
                        players[currentPlayerIndex].participating = true;
                        UpdatePlayerTurn();
                        populatePlayerBoard();
                        userInput.DeactivateUI();
                        userInput.ActivateBooleanCheck("Do you want to participate?");
                        numIterations++;
                    }
                }
                else
                {
                    //we are done
                    userInput.DeactivateUI();
                }
            }
        }
    }






    /////////////////////////////////////////
    //check if player wants to sponsor
    public void SponsorCheck()
    {
        //checking for sponsors
        if (userInput.UIEnabled)
        {
            if (userInput.booleanPrompt.isActive)
            {
                if (numIterations < numPlayers)
                {
                    int sponsoring = players[currentPlayerIndex].strategy.sponsorQuest(players, currentQuest.stages, players[currentPlayerIndex].hand, this);

                    if (currentPlayerIndex < numHumanPlayers)
                    {
                        if (!SponsorCapabilityCheck())
                        {
                            userInput.DeactivateUI();
                            userInput.ActivateBooleanCheck("You cannot sponsor this quest, please select No");
                            Debug.Log("Current Player index: " + currentPlayerIndex.ToString());
                            if (sponsoring == 1)
                                sponsoring = 3;
                        }
                    }
                    if (sponsoring == 0)
                    {
                        print("No");
                        // not sponsoring

                        UpdatePlayerTurn();
                        populatePlayerBoard();
                        userInput.DeactivateUI();
                        userInput.ActivateBooleanCheck("Do you want to Sponsor This Quest?");
                        numIterations++;
                    }
                    else if (sponsoring == 1)
                    {
                        print("Yes");
                        players[currentPlayerIndex].sponsoring = true;

                        userInput.DeactivateUI();

                        //circumvent this
                        numIterations = 5;
                    }
                    else if (sponsoring == 2)
                    {
                        //Do Nothing
                    }
                    else if (sponsoring == 3)
                    {
                        print("Cannot sponsor.");
                    }
                    else
                    {
                        print("Yes");
                        players[currentPlayerIndex].sponsoring = true;

                        userInput.DeactivateUI();

                        //circumvent this
                        numIterations = 5;
                    }
                }
            }
        }
        else
        {
            //we are done
            userInput.DeactivateUI();

            //return participants
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].sponsoring)
                    Debug.Log("Sponsor" + (i + 1) + ": " + players[i].name);
            }
        }
    }

    ///////////////////////////////////////////
    //query sponsor for his cards for the quest
    //IF SPONSOR ADDS NOTHING OR WE DO NOT
    //CHECK USER INPUT IF IT DOESN'T FIT INTO REQUIREMENTS END SPONSORSHIP AND END QUEST
    public void SponsorQuery()
    {
        if (userInput.UIEnabled)
        {
            if (userInput.cardPrompt.isActive)
            {
                //check for as much as the quest stages
                if (numIterations < currentQuest.stages)
                {
                    //calling check sponsorship for debugging since it should never come in here if there is no player sponsoring
                    List<List<Card>> returnVal = new List<List<Card>>();
                    returnVal = players[CheckSponsorship()].strategy.setupQuest(currentQuest.stages, players[CheckSponsorship()].hand, currentQuest.foe);

                    if (returnVal != null)
                    {
                        //ai here
                        //conversion
                        for (int i = 0; i < returnVal.Count; i++)
                        {
                            sponsorQueriedCards[i] = new List<Card>(returnVal[i]);
                        }

                        // break we are done here
                        numIterations = 5;
                        userInput.DeactivateUI();
                        UpdatePlayerTurn();
                        populatePlayerBoard();
                    }
                    else if (userInput.cardPrompt.doneAddingCards)
                    {
                        sponsorQueriedCards[numIterations] = new List<Card>(userInput.cardPrompt.selectedCards);
                        userInput.DeactivateUI();
                        numIterations++;
                        userInput.ActivateCardUIPanel("What FOE or TEST cards do you want to use for this Quest?");
                    }
                }
                else
                {
                    userInput.DeactivateUI();
                    UpdatePlayerTurn();
                    populatePlayerBoard();

                    Debug.Log("all cards have been selected");
                    //we are done

                    for (int i = 0; i < sponsorQueriedCards.Length; i++)
                    {
                        if (sponsorQueriedCards[i] != null)
                        {
                            for (int j = 0; j < sponsorQueriedCards[i].Count; j++)
                            {
                                Debug.Log(i.ToString() + ": " + sponsorQueriedCards[i][j].name);
                            }
                        }
                    }
                }
            }
        }
    }

    /////////////////////////////////////
    public bool SponsorCapabilityCheck()
    {
        if (!SponsorCapabilitySoftCheck())
            return false;
        else if (SponsorCapabilityHardCheck())
            return true;
        else
            return false;
    }


    /////////////////////////////////////
    public bool SponsorCapabilitySoftCheck()
    {
        int validStageCardsCount = 0;
        bool testInhand = false;

        for (int i = 0; i < players[currentPlayerIndex].hand.Count; i++)
        {
            if (players[currentPlayerIndex].hand[i] != null)
            {
                if (players[currentPlayerIndex].hand[i].type == "Foe Card")
                    validStageCardsCount++;
                else if (!testInhand && players[currentPlayerIndex].hand[i].type == "Test Card")
                {
                    testInhand = true;
                    validStageCardsCount++;
                }
            }
        }

        if (validStageCardsCount >= currentQuest.stages)
            return true;
        else
            return false;
    }

    /////////////////////////////////////////////
    public bool SponsorCapabilityHardCheck()
    {
        //Current number of possible valid stages that can be created.
        int validStageCardsCount = 0;
        //Number of weapons in the player's hand.
        int weaponCount = 0;
        bool testInhand = false;
        strategyUtil util = new strategyUtil();
        //Parallel array of booleans for the player's hand representing available weapon slots in the hand.
        bool[] weaponAvailable = new bool[players[currentPlayerIndex].hand.Count];

        //Integer array which stores the number of instances of a given BP found in the player's hand.
        //All battle points are a multiple of 5, which means they're translatable from a 5 - 70 scale to a 0 - 14 scale
        //This is then widened to account for the highest possible BP to prevent a index out of bounds error    
        //Maximum BP size of a stage 
        //(Max Dragon + Excalibur + Lance + Battle Axe + Sword + Horse + Dagger)
        //(70 + 30 + 20 + 15 + 10 + 10 + 5) / 5 = 32
        int[] foeValues = new int[(70 + 30 + 20 + 15 + 10 + 10 + 5) / 5];

        //Setting values to 0
        for (int i = 0; i < foeValues.Length; i++)
        {
            foeValues[i] = 0;
        }

        for (int i = 0; i < players[currentPlayerIndex].hand.Count; i++)
        {
            if (players[currentPlayerIndex].hand[i] != null)
            {
                if (players[currentPlayerIndex].hand[i].type == "Foe Card")
                {
                    foeValues[(util.getContextBP((FoeCard)players[currentPlayerIndex].hand[i], currentQuest.name) / 5) - 1]++;
                    //Non-weapon found, thus weapon unavailable at i
                    weaponAvailable[i] = false;
                }

                //If a test is found in the hand, validStageCardsCount is incremented and will never be less than 1, 
                //reducing the number of unique BPs required by 1.
                else if (!testInhand && players[currentPlayerIndex].hand[i].type == "Test Card")
                {
                    testInhand = true;
                    validStageCardsCount++;
                    //Non-weapon found, thus weapon unavailable at i
                    weaponAvailable[i] = false;
                }
                else if (players[currentPlayerIndex].hand[i].type == "Weapon Card")
                {
                    weaponCount++;
                    //Weapon found, thus weapon available at i
                    weaponAvailable[i] = true;
                }
            }
        }

        //Checks to see if there are 3 foes with unique BPs
        for (int i = 0; i < foeValues.Length; i++)
        {
            if (foeValues[i] != 0)
            {
                validStageCardsCount++;
            }
        }
        if (validStageCardsCount >= currentQuest.stages)
            return true;

        //If the number foes with unique BPs plus
        else if (validStageCardsCount + weaponCount < currentQuest.stages)
            return false;

        //Begins the check of whether or not adding weapon cards can create a valid hand
        else
        {
            //If there is a test in the player's hand, reduce the total
            //number of uniqueBP values required by one.
            if (testInhand)
                validStageCardsCount = 1;
            else
                validStageCardsCount = 0;

            //Goes through the array of foe values, if a given index has a value greater than 1
            //weapon cards are introduced to attempt finding a unique BP.
            for (int i = 0; i < foeValues.Length; i++)
            {
                if (foeValues[i] > 1)
                {
                    for (int j = 0; j < players[currentPlayerIndex].hand.Count; j++)
                    {
                        if (players[currentPlayerIndex].hand[j].type == "Weapon Card" && weaponAvailable[j])
                        {
                            //Stores the BP value of the weaponcard found.
                            int wbp = ((WeaponCard)players[currentPlayerIndex].hand[j]).battlePoints;

                            //Second check if foeValues[i] is a unique BP as this for loop will be 
                            //run for the length of the hand, this prevents reducing foeValues[i] below 1.
                            if (foeValues[i] > 1)
                            {
                                //A new valid unique BP has been found using one weapon card, the value at foeValues[i]
                                //is decremented and the value at a new index equal to the the current index plus the weapon bp
                                //is incremented.
                                if (foeValues[i + (wbp / 5)] < 1)
                                {
                                    foeValues[i]--;
                                    foeValues[i + (wbp / 5)]++;
                                    weaponAvailable[j] = false;
                                    //Checks to see if this addition of a new unique BP value creates a valid quest configuration
                                    if (CVQP(validStageCardsCount, foeValues))
                                        return true;
                                }
                                else
                                {
                                    //Itterates for each of the duplicate BPs at foeValues[i]
                                    for (int dupe_it = 0; dupe_it < foeValues[i]; dupe_it++)
                                    {
                                        //List of weapons already added to the foe.
                                        List<WeaponCard> addedWeapons = new List<WeaponCard>();
                                        addedWeapons.Add((WeaponCard)players[currentPlayerIndex].hand[j]);
                                        List<int> usedWeapons = new List<int>();

                                        //A "Hopping Index" which carries the current foeBP + sum of weapon BPs added
                                        int ii = i + (wbp / 5);
                                        //This is the section which tests if adding multiple weapons to a single foe allows the creation of a valid Quest
                                        for (int k = j + 1; k < players[currentPlayerIndex].hand.Count; k++)
                                        {
                                            if (players[currentPlayerIndex].hand[k].type == "Weapon Card" && weaponAvailable[k])
                                            {
                                                //holds the weapon card found in the player's hand, done in order to reduce line length
                                                WeaponCard foundWeaponCard = (WeaponCard)players[currentPlayerIndex].hand[k];

                                                //Checks to see if the weapon card found is a duplicate of 
                                                //a weapon card already applied to the foe.
                                                bool duplicateWeaponCheck = false;
                                                for (int aW_it = 0; aW_it < addedWeapons.Count; aW_it++)
                                                    if (addedWeapons[aW_it].name == foundWeaponCard.name)
                                                        duplicateWeaponCheck = true;
                                                if (duplicateWeaponCheck)
                                                    continue;

                                                //alternate variable name performing the same duties as wbp
                                                int wbp_VERSION_K = foundWeaponCard.battlePoints;
                                                usedWeapons.Add(k);

                                                //A new valid unique BP has been found using multiple weapon cards, the value at foeValues[i]
                                                //is decremented and the value at a new index equal to the the current index plus the sum bp
                                                //of all weapons used is incremented.
                                                if (foeValues[ii + (wbp_VERSION_K / 5)] < 1)
                                                {
                                                    foeValues[i]--;
                                                    foeValues[ii + (wbp_VERSION_K / 5)]++;

                                                    for (int clean_up_it = 0; clean_up_it < usedWeapons.Count; clean_up_it++)
                                                    {
                                                        weaponAvailable[clean_up_it] = false;
                                                    }

                                                    weaponAvailable[j] = false;
                                                    //Checks to see if this addition of a new unique BP value creates a valid quest configuration
                                                    if (CVQP(validStageCardsCount, foeValues))
                                                        return true;
                                                    //new unique BP found, break loop and attempt to find another valid BP for the next dupicate BP at i
                                                    break;
                                                }

                                                //A weapon is added to the current list of weapons being applied to the foe
                                                else
                                                {
                                                    addedWeapons.Add(foundWeaponCard);
                                                    ii += wbp_VERSION_K;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Final check if there are enough unique BP values to fill the quest before returning false
            if (CVQP(validStageCardsCount, foeValues))
                return true;
            else
                return false;
        }
    }

    //////////////////////////////////
    //Checks to see if the number of unique BP's available to the
    //player is equal to or greater than the number of stages in the quest.
    public bool CVQP(int vSCC, int[] foeValues)
    {

        for (int i = 0; i < foeValues.Length; i++)
        {
            if (foeValues[i] != 0)
            {
                vSCC++;
            }
        }

        if (vSCC >= currentQuest.stages)
            return true;
        else
            return false;
    }

    //////////////////////////////////////////////////
    //NEEDS FIXING TO ADD SHIELDS AND BASE BP AND ADD A STRING TO USE BOTH QUEST AND TOURNAMENTS
    public void CardQuerying(string state)
    {
        if (userInput.UIEnabled)
        {
            if (userInput.cardPrompt.isActive)
            {
                if (numIterations < numPlayers)
                {
                    //might need to set this to a new list
                    List<Card> result = new List<Card>();

                    if (players[currentPlayerIndex].participating)
                    {
                        if (state == "Tournament")
                        {
                            result = players[currentPlayerIndex].strategy.playTournament(players, players[currentPlayerIndex].hand, players[currentPlayerIndex].CalculateBP("", players), currentTournament.shields);
                        }
                        else if (state == "Quest")
                        {
                            // foe encounter stuff here
                            if (QuestState.amours != null && QuestState.amours[currentPlayerIndex] != null)
                            {
                                result = players[currentPlayerIndex].strategy.playFoeEncounter(QuestState.currentStage, currentQuest.stages, players[currentPlayerIndex].hand, QuestState.previousQuestBP, QuestState.amours[currentPlayerIndex].Count == 1, currentQuest.name, players);
                            }
                            else
                            {
                                result = players[currentPlayerIndex].strategy.playFoeEncounter(QuestState.currentStage, currentQuest.stages, players[currentPlayerIndex].hand, QuestState.previousQuestBP, false, currentQuest.name, players);
                            }
                        }
                    }

                    if (players[currentPlayerIndex].sponsoring || !(players[currentPlayerIndex].participating))
                    {
                        //this will help generalize querying in both tournaments and quests
                        numIterations++;
                        queriedCards[currentPlayerIndex] = null;
                        UpdatePlayerTurn();

                    }
                    else if (userInput.cardPrompt.doneAddingCards)
                    {
                        // human player card querying
                        queriedCards[currentPlayerIndex] = new List<Card>(userInput.cardPrompt.selectedCards);
                        userInput.DeactivateUI();
                        numIterations++;
                        UpdatePlayerTurn();
                        populatePlayerBoard();
                        userInput.ActivateCardUIPanel("What AMOUR , ALLY , OR WEAPON CARDS do you want to use?");
                    }
                    else if (result != null)
                    {
                        //ai player here
                        queriedCards[currentPlayerIndex] = new List<Card>(result);
                        userInput.DeactivateUI();
                        numIterations++;
                        UpdatePlayerTurn();
                        populatePlayerBoard();
                        userInput.ActivateCardUIPanel("What AMOUR , ALLY , OR WEAPON CARDS do you want to use?");
                    }
                }
                else
                {
                    userInput.DeactivateUI();

                    Debug.Log("all cards have been selected");
                    //we are done

                    for (int i = 0; i < queriedCards.Length; i++)
                    {
                        if (queriedCards[i] != null)
                        {
                            for (int j = 0; j < queriedCards[i].Count; j++)
                            {
                                Debug.Log(i.ToString() + ": " + queriedCards[i][j].name);
                            }
                        }
                    }
                }
            }
        }
    }



    */
}
