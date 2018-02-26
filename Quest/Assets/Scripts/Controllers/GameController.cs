using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Tournaments:
    Participants are only allowed to use ally , amour , weapon cards
*/

/*
    Quests:
    sponsors only use foes with weapons
    or one test card   (CHECK THESE IN UPDATE??????????????)

    Participants: only use weapon and amour cards to boost bp only
*/

//WIN CONDITIONS
// what if a player wants to sponsor but doesn't have enough cards to sponsor a quest how would we check that
// uncomment deck builder stuff
//kings recognition quest stuff
//card querying stuff
//CAN ONLY SELECT CARDS DURING CARD UI PANEL CHECKS SINCE THERE IS PANEL OVER IT AT OTHER TIMES
//CHECK MERLIN"S ABILITY
//CHECK MORDRED

//TOURNAMENTS                          DONEEEEEEE
//QUESTS
//SPECIAL EFFECTS
//TEST CARDS
//Checking if player is over 12 cards ask for discarding
//mordred will use separate input for each button


//should create objects for the ui objects but too late now 
[System.Serializable]
public class UIInput
{
    //UIPanel
    public GameObject foregroundPanel;

    //for boolean user check
    public GameObject booleanPanel;
    public Text userMessage;
    public Button yesButton;
    public Button noButton;
    public string buttonResult;

    //for actual input user check
    public GameObject inputPanel;
    public Text userMessage1;
    public InputField KeyboardInput;

    //for ui card panel prompt
    public GameObject cardPanel;
    public Text userMessage2;
    public int totalBPCount;
    public Text totalBP;
    public GameObject chosenCardsPanel;
    public Button submitButton;
    public List<Card> selectedCards;
    public List<GameObject> UICardsSelected;

    //for ui discard panel prompt
    public GameObject discardPanel;
    public Text userDiscardMessage;
    public GameObject chosenDiscardsPanel;
    public Button discardSubmitButton;
    public List<Card> discardSelectedCards;
    public List<GameObject> UIDiscardsSelected;


    //bools for each ui value
    public bool UIEnabled;
    public bool booleanUIEnabled;
    public bool keyboardInputUIEnabled;
    public bool cardPanelUIEnabled;
    public bool doneAddingCards;

    public bool discardPanelUIEnabled;
    public bool doneDiscardingCards;

    public void SetupUI()
    {
        selectedCards = new List<Card>();
        UICardsSelected = new List<GameObject>();
        discardSelectedCards = new List<Card>();
        UIDiscardsSelected = new List<GameObject>();

        // setup event listeners
        yesButton.onClick.AddListener(Yes);
        noButton.onClick.AddListener(No);
        submitButton.onClick.AddListener(Submit);
        discardSubmitButton.onClick.AddListener(SubmitDiscard);
        buttonResult = "";
    }

    void Submit() { doneAddingCards = true; }
    void SubmitDiscard() { doneDiscardingCards = true; }
    void Yes() { buttonResult = "Yes"; }
    void No() { buttonResult = "No"; }

    public void ActivateDiscardCheck(string userMsg)
    {
        discardPanelUIEnabled = true;
        doneDiscardingCards = false;

        discardPanel.SetActive(true);
        userDiscardMessage.text = userMsg;
    }
    public void DeactivateDiscardPanel()
    {
        discardPanel.SetActive(false);
        doneDiscardingCards = false;
        discardPanelUIEnabled = false;

        discardSelectedCards.Clear();

        //Destroy panel cards
        for (int i = 0; i < UIDiscardsSelected.Count; i++)
        {
            Object.Destroy(UIDiscardsSelected[i]);
        }

        UIDiscardsSelected.Clear();
    }
    public void CheckDiscardCard(Card card)
    {
        for (int i = 0; i < discardSelectedCards.Count; i++)
        {
            if (card == discardSelectedCards[i])
            {
                RemoveFromCardUIPanel(i);
            }
        }
    }
    public void RemoveFromDiscardPanel(int index)
    {
        // if u change ur mind
        //remove the discard selected card
        discardSelectedCards.RemoveAt(index);
        Object.Destroy(UICardsSelected[index]);
        UICardsSelected.RemoveAt(index);
    }
    public void AddToUIDiscardPanel(GameObject card)
    {
        CardUIScript script = card.GetComponent<CardUIScript>();
        script.isHandCard = true;

        discardSelectedCards.Add(script.myCard);
        UIDiscardsSelected.Add(card);

        card.transform.SetParent(chosenDiscardsPanel.transform);
    }


    public void ActivateBooleanCheck(string userMsg)
    {
        buttonResult = "";
        UIEnabled = true;
        booleanUIEnabled = true;
        keyboardInputUIEnabled = false;
        cardPanelUIEnabled = false;
        discardPanelUIEnabled = false;
        doneAddingCards = false;

        foregroundPanel.SetActive(true);
        booleanPanel.SetActive(true);
        discardPanel.SetActive(false);
        cardPanel.SetActive(false);
        inputPanel.SetActive(false);
        userMessage.text = userMsg;
    }
    public void ActivateUserInputCheck(string userMsg)
    {
        UIEnabled = true;
        keyboardInputUIEnabled = true;
        booleanUIEnabled = false;
        cardPanelUIEnabled = false;
        discardPanelUIEnabled = false;
        doneAddingCards = false;

        foregroundPanel.SetActive(true);
        inputPanel.SetActive(true);
        cardPanel.SetActive(false);
        discardPanel.SetActive(false);
        booleanPanel.SetActive(false);
        KeyboardInput.text = "";
        userMessage1.text = userMsg;
    }
    public void ActivateCardUIPanel(string userMsg)
    {
        UIEnabled = true;
        cardPanelUIEnabled = true;
        discardPanelUIEnabled = false;
        keyboardInputUIEnabled = false;
        booleanUIEnabled = false;
        doneAddingCards = false;

        //foregroundPanel.SetActive(true);
        cardPanel.SetActive(true);
        discardPanel.SetActive(false);
        inputPanel.SetActive(false);
        booleanPanel.SetActive(false);
        userMessage2.text = userMsg;
        totalBP.text = "BP: " + totalBPCount.ToString();
    }


    public GameObject CheckCard(Card card)
    {
        for (int i = 0; i < selectedCards.Count; i++)
        {
            if (card == selectedCards[i])
            {
                return RemoveFromCardUIPanel(i);
            }
        }
        return null;
    }
    public void AddToUICardPanel(GameObject card)
    {
        CardUIScript script = card.GetComponent<CardUIScript>();
        script.isHandCard = true;

        selectedCards.Add(script.myCard);
        UICardsSelected.Add(card);

        CalculateTotalBP();

        //add card to panel
        card.transform.SetParent(chosenCardsPanel.transform);
    }


    //Destroy's the children of a given game object by finding said object by its tag.
    public void DestroyChildren(string gameObjectTag)
    {
        Transform uiHand = GameObject.FindGameObjectWithTag(gameObjectTag).transform;

        foreach (Transform child in uiHand)
            while (uiHand.transform.childCount > 1)
            {
                GameObject.Destroy(child.gameObject);
            }
    }


    public void CalculateTotalBP()
    {
        int total = 0;

        for (int i = 0; i < selectedCards.Count; i++)
        {
            if (selectedCards[i].type == "Foe Card")
            {
                FoeCard foe = (FoeCard)selectedCards[i];
                total += foe.minBP;
            }
            else if (selectedCards[i].type == "Weapon Card")
            {
                WeaponCard weapon = (WeaponCard)selectedCards[i];
                total += weapon.battlePoints;
            }
            else if (selectedCards[i].type == "Ally Card")
            {
                AllyCard ally = (AllyCard)selectedCards[i];
                total += ally.battlePoints;
            }
            else if (selectedCards[i].type == "Amour Card")
            {
                AmourCard amour = (AmourCard)selectedCards[i];
                total += amour.battlePoints;
            }
        }

        totalBP.text = "BP: " + total.ToString();
    }

    public GameObject RemoveFromCardUIPanel(int index)
    {
        selectedCards.RemoveAt(index);

        GameObject result = UICardsSelected[index];
        UICardsSelected.RemoveAt(index);

        CalculateTotalBP();
        return result;
    }

    public void DeactivateUI()
    {
        foregroundPanel.SetActive(false);
        cardPanel.SetActive(false);
        discardPanel.SetActive(false);
        UIEnabled = false;
        keyboardInputUIEnabled = false;
        booleanUIEnabled = false;
        discardPanelUIEnabled = false;
        cardPanelUIEnabled = false;
        doneAddingCards = false;
        buttonResult = "";
        totalBP.text = "";
        totalBPCount = 0;
        selectedCards.Clear();

        //Destroy panel cards
        for (int i = 0; i < UICardsSelected.Count; i++)
        {
            Object.Destroy(UICardsSelected[i]);
        }

        UICardsSelected.Clear();
    }
}

// Maybe we can have the description of a card where when we click a card it shows its description data on the screen
public class GameController : MonoBehaviour
{
    //deck initializer
    public deckBuilder decks;
    public Deck storyDeck;
    public Deck adventureDeck;
    public bool kingsRecognition = false;

    //turn setup
    public int turnCount = 0;
    public Turn nextTurn;

    //card ui prefab
    public GameObject cardPrefab;

    //checking user input
    public UIInput userInput;

    public List<Player> players;

    //UI game board information
    public CardUIScript shieldsCard;
    public CardUIScript rankCard;
    public Text UIShieldNum;
    public GameObject handPanel;
    public GameObject questPanel;
    public GameObject questStagePanel;
    public GameObject allyPanel;
    public GameObject weaponPanel;
    public GameObject amourPanel;
    public GameObject activatedPanel;

    // player panels UI names bp number of cards ranks etc...
    public List<GameObject> playerPanels;
    public List<Text> UINames;
    public List<Text> UIBPS;
    public List<Text> UINumCards;
    public List<Text> UIRanks;
    public List<Text> UIShields;

    //Quest data
    public Text questStageNumber;
    public Text questStageBPTotal;

    // deck UI information
    public GameObject adventureDeckUIButton;
    public CardUIScript adventureDeckDiscardPileUIButton;  //to change image here for discard pile
    public GameObject storyDeckUIButton;
    public CardUIScript storyDeckDiscardPileUIButton;      //to change image here for discard pile
    public Button rankDeckUIButton;

    public Card selectedCard;
    public List<Card> drawnAdventureCards;
    public Card drawnStoryCard;
    public int numCardsToBeDrawn;

    public int numPlayers;
    public int numHumanPlayers;
    public int numCpus;
    public int currentPlayerIndex;


    //for setting up gameboard and player information
    public bool isSettingUpGame;
    public int setupState;

    // Some names for players cpus etc....
    public List<string> playerNames;
    public List<string> cpuNames;
    public List<string> shieldPaths;

    //states for drawing an adventure or story card
    public bool drawAdventureCard;
    public bool drawStoryCard;

    //current story cards running
    public EventCard currentEvent;
    public QuestCard currentQuest;
    public TournamentCard currentTournament;
    public bool isDoneStoryEvent;

    public int numIterations;

    public List<Card>[] queriedCards;
    public List<Card>[] sponsorQueriedCards;

    public List<int> cpuStrategies;
    public List<int> winners;       //store the indeces of the winners

    public bool foundWinner;
    public bool playerStillOffending;      // players with over 12 cards exist

    // Use this for initialization
    void Start()
    {
        //setup game variables
        isSettingUpGame = true;
        currentPlayerIndex = 0;
        setupState = 0;
        shieldPaths = new List<string> { "Textures/Backings/s_backing", "Textures/Backings/s_backing", "Textures/Backings/s_backing", "Textures/Backings/s_backing" };
        players = new List<Player>();
        drawnAdventureCards = new List<Card>();
        cpuStrategies = new List<int>();
        winners = new List<int>();
        playerStillOffending = false;
        foundWinner = false;

        adventureDeckDiscardPileUIButton = GameObject.FindGameObjectWithTag("AdventureDiscard").GetComponent<CardUIScript>();
        storyDeckDiscardPileUIButton = GameObject.FindGameObjectWithTag("StoryDiscard").GetComponent<CardUIScript>();

        //Whenever a change in drawing states is done toggle the deck animations
        drawStoryCard = true;
        drawAdventureCard = false;
        ToggleDeckAnimation();

        numCardsToBeDrawn = 1;

        //Deck building
        decks = new deckBuilder();
        storyDeck = decks.buildStoryDeck();
        adventureDeck = decks.buildAdventureDeck();

        //Setup UI buttons for cards (event listeners etc....)
        adventureDeckUIButton.GetComponent<Button>().onClick.AddListener(DrawFromAdventureDeck);
        storyDeckUIButton.GetComponent<Button>().onClick.AddListener(DrawFromStoryDeck);

        //Store gameboard cards
        shieldsCard = GameObject.FindGameObjectWithTag("Shields").GetComponent<CardUIScript>();
        rankCard = GameObject.FindGameObjectWithTag("RankCard").GetComponent<CardUIScript>();

        //Store gameBoard panels
        questPanel = GameObject.FindGameObjectWithTag("QuestCard");
        handPanel = GameObject.FindGameObjectWithTag("CurrentHand");
        allyPanel = GameObject.FindGameObjectWithTag("AllyCards");
        weaponPanel = GameObject.FindGameObjectWithTag("WeaponCards");
        questStagePanel = GameObject.FindGameObjectWithTag("QuestStageCards");
        amourPanel = GameObject.FindGameObjectWithTag("AmourCards");
        activatedPanel = GameObject.FindGameObjectWithTag("ActivatedCard");

        //UI building
        userInput.SetupUI();
        userInput.ActivateUserInputCheck("How many players are playing the game??");

        //Other stuff
        /*
        To-Do Here:
            1. Somehow communicate with main menu to grab player's name and create player

            2. Create list of players with human player and CPU players
        */

    }

    // Update is called once per frame
    //player discard piles for over 12 cards and etc...
    void Update()
    {
        //Check if game is done with all players and if that is the case declare winner
        //check decks for emptiness
        if (!foundWinner)
        {
            if (!playerStillOffending)
            {
                if (isSettingUpGame)
                {
                    PlayerSetup();
                    QueryStrategies();
                }
                else
                {
                    //check for drawn adventure cards No need for that still need to check cards though
                    //have a state here that checks for having over 12 cards

                    //check for drawn story cards
                    if (drawnStoryCard != null)
                    {
                        Debug.Log("DRAWN STORY CARD: " + drawnStoryCard.type);
                        EmptyQuestPanel();

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
                        AddToPanel(CreateUIElement(drawnStoryCard), questPanel);
                        drawnStoryCard = null;
                    }




                    //check if story event is done
                    if (isDoneStoryEvent)
                    {
                        //discard story card into story discard pile
                        if (currentEvent != null)
                        {
                            storyDeckDiscardPileUIButton.myCard = currentEvent;
                            storyDeckDiscardPileUIButton.ChangeTexture();
                            storyDeck.discard.Add(currentEvent);

                        }
                        else if (currentQuest != null)
                        {
                            storyDeckDiscardPileUIButton.myCard = currentQuest;
                            storyDeckDiscardPileUIButton.ChangeTexture();
                            storyDeck.discard.Add(currentQuest);
                        }
                        else if (currentTournament != null)
                        {

                            storyDeckDiscardPileUIButton.myCard = currentTournament;
                            storyDeckDiscardPileUIButton.ChangeTexture();
                            storyDeck.discard.Add(currentTournament);
                        }

                        isDoneStoryEvent = false;
                        currentEvent = null;
                        currentQuest = null;
                        currentTournament = null;

                        //reset players participation
                        ResetPlayers();
                        EmptyPanel(questPanel);
                        System.Array.Clear(queriedCards, 0, queriedCards.Length);

                        //reset turn to draw new story card
                        drawStoryCard = true;
                        ToggleDeckAnimation();
                        userInput.DeactivateUI(); // just in case
                        UpdatePlayerTurn();       // go to next turn
                    }
                    //Run story card here
                    else if (currentEvent != null)
                    {
                        currentEvent.effect.execute(players, currentEvent, this);
                    }
                    else if (currentTournament != null)
                    {
                        currentTournament.tournament.execute(null, currentTournament, this);
                    }
                    else if (currentQuest != null)
                    {
                        // currentQuest.quest.execute(null, currentQuest, this);
                        currentQuest.quest.execute(players, currentQuest, this);

                        //CHECK HERE IF CARDS TO BE ADDED BY SPONSOR BREAK THE RULES RETURN ALL THESE CARDS TO HIS HAND AND FORCE HIM TO REDECIDE
                        //if (QuestState.state == "Sponsoring")
                        //{
                        //    if (!AnyFoes() && AnyWeapons())
                        //    {
                        //        //there are foes but no weapons return all cards to hand and start again
                        //        returnToPlayerHand();
                        //    }
                        //}
                    }

                    if (selectedCard != null)
                    {
                        //all ally cards activated are added to the board and active allies
                        if (selectedCard.type == "Ally Card")
                        {
                            //ally cards always get added to active allies
                            AllyCard ally = (AllyCard)selectedCard;
                            players[currentPlayerIndex].activeAllies.Add(ally);
                            players[currentPlayerIndex].hand.Remove(selectedCard);
                            AddToPanel(CreateUIElement(selectedCard), allyPanel);
                        }
                        else
                        {
                            GameObject removedCard = userInput.CheckCard(selectedCard);

                            if (removedCard != null)
                            {
                                //we want to remove this card from ui panel and back to player's hand since u changed ur mind
                                Destroy(removedCard);
                                players[currentPlayerIndex].hand.Add(selectedCard);
                                AddToPanel(CreateUIElement(selectedCard), handPanel);
                            }
                            else
                            {
                                bool addToPanel = true;

                                // rules for tournaments
                                if (currentTournament != null)
                                {
                                    // we are in a tournament check for weapon, amour cards allies are always added
                                    if (selectedCard.type == "Weapon Card")
                                    {
                                        for (int i = 0; i < userInput.selectedCards.Count; i++)
                                        {
                                            if (userInput.selectedCards[i].name == selectedCard.name)
                                            {
                                                //duplicate exists cannot add it in
                                                addToPanel = false;
                                            }
                                        }
                                    }
                                    else if (selectedCard.type == "Amour Card")
                                    {
                                        for (int i = 0; i < userInput.selectedCards.Count; i++)
                                        {
                                            if (userInput.selectedCards[i].type == "Amour Card")
                                            {
                                                //more than one amour cannot add it
                                                addToPanel = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // not weapon amour or ally
                                        addToPanel = false;
                                    }
                                }
                                else if (currentQuest != null)
                                {
                                    //if it is the sponsor check that he added cards properly
                                    if (players[currentPlayerIndex].sponsoring)
                                    {
                                        if (AnyTests()) { addToPanel = false; }
                                        else if (selectedCard.type == "Test Card")
                                        {
                                            //check that it is the only card in the panel and do not allow him to any more cards
                                            if (userInput.selectedCards.Count > 0) { addToPanel = false; }
                                            else { addToPanel = true; }
                                        }
                                        else if (selectedCard.type == "Foe Card")
                                        {
                                            // I think we always add foes when u r the sponsor
                                            addToPanel = true;
                                        }
                                        else if (selectedCard.type == "Weapon Card")
                                        {
                                            // we have a weapon card which needs a special check
                                            for (int i = 0; i < userInput.selectedCards.Count; i++)
                                            {
                                                if (userInput.selectedCards[i].name == selectedCard.name)
                                                {
                                                    //duplicate exists cannot add it in
                                                    addToPanel = false;
                                                    break;
                                                }
                                            }

                                            //if no duplication check if there is a foe in there
                                            if (addToPanel)
                                            {
                                                addToPanel = AnyFoes();
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
                                            for (int i = 0; i < userInput.selectedCards.Count; i++)
                                            {
                                                if (userInput.selectedCards[i].name == selectedCard.name)
                                                {
                                                    //duplicate exists cannot add it in
                                                    addToPanel = false;
                                                    break;
                                                }
                                            }
                                        }
                                        else if (selectedCard.type == "Amour Card")
                                        {
                                            for (int i = 0; i < userInput.selectedCards.Count; i++)
                                            {
                                                if (userInput.selectedCards[i].type == "Amour Card")
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

                                //result
                                if (addToPanel)
                                {
                                    //add it into the panel
                                    players[currentPlayerIndex].hand.Remove(selectedCard);
                                    userInput.AddToUICardPanel(CreateUIElement(selectedCard));
                                }
                                else
                                {
                                    //return it to hand since we didn't delete it from player we are fine
                                    AddToPanel(CreateUIElement(selectedCard), handPanel);
                                }
                            }
                        }

                        //maybe we can check for certain cards like mordred here with some state
                        selectedCard = null;
                    }

                }
            }
            else
            {
                //We need to discard some cards 
                //The checks for discarding are done in the iStories
                //querying discards occurs here
                DiscardCards();

                if (selectedCard != null)
                {
                    //all ally cards activated are added to the board and active allies
                    if (selectedCard.type == "Ally Card")
                    {
                        //ally cards always get added to active allies
                        AllyCard ally = (AllyCard)selectedCard;
                        players[currentPlayerIndex].activeAllies.Add(ally);
                        players[currentPlayerIndex].hand.Remove(selectedCard);
                        AddToPanel(CreateUIElement(selectedCard), allyPanel);
                    }
                }
                else
                {
                    //add it into the panel
                    players[currentPlayerIndex].hand.Remove(selectedCard);
                    userInput.AddToUICardPanel(CreateUIElement(selectedCard));
                }

                //end discard state
                if (numIterations >= numPlayers)
                {
                    //done discarding
                    numIterations = 0;
                    userInput.DeactivateDiscardPanel();
                    playerStillOffending = false;
                }
            }
            CalculateUIPlayerInfo();

            //check winners here
            FindWinners();
            if (winners.Count > 1)
            {
                //game is over
                foundWinner = true;
                //Add ui element here
                //code for showing winners here
                for (int i = 0; i < winners.Count; i++)
                {
                    Debug.Log("Winner: " + players[winners[i]].name);
                }           
            } 
        }
    }

    public bool PlayerOffending()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].handCheck())
            {
                // we are over 12 on a player 
                return true; 
            }
        }
        return false;
    }

    //check the size of find winner if it is greater than 1 we have a winner and we stop the game
    public void FindWinners()
    {
        //check for winners
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].rankUpCheck(players[i].score , 0))
            {
                //store the index of the winners of the game
                winners.Add(i);
            }
        }
    }

    public void returnToPlayerHand()
    {
        //add userInput selected hand back to player
        for (int i = 0; i < userInput.selectedCards.Count; i++)
        {
            //just in case
            players[CheckSponsorship()].hand.Add(userInput.selectedCards[i]);
            Destroy(userInput.RemoveFromCardUIPanel(i));
            i--;
            populatePlayerBoard();
        }
    }

    public bool AnyTests()
    {
        for (int i = 0; i < userInput.selectedCards.Count; i++)
        {
            if (userInput.selectedCards[i].type == "Test Card")
            {
                return true;
            }
        }

        return false;
    }

    public bool AnyFoes()
    {
        for (int i = 0; i < userInput.selectedCards.Count; i++)
        {
            if (userInput.selectedCards[i].type == "Foe Card")
            {
                return true;
            }
        }

        return false;
    }

    public bool AnyWeapons()
    {
        for (int i = 0; i < userInput.selectedCards.Count; i++)
        {
            if (userInput.selectedCards[i].type == "Weapon Card")
            {
                return true;
            }
        }

        return false;
    }

    public void DiscardAdvenureCards(List<Card> discardCards)
    {
        //logic discard as well as UI one
        adventureDeck.discardCards(discardCards);
        adventureDeckDiscardPileUIButton.myCard = adventureDeck.discard[adventureDeck.discard.Count - 1];
        adventureDeckDiscardPileUIButton.ChangeTexture();
    }

    //update quest data of a certain stage
    public void AddQuestData(int stageNumber, int totalBP)
    {
        questStageNumber.text = "Stage: " + stageNumber.ToString();
        questStageBPTotal.text = "BP: " + totalBP.ToString();
    }

    public void CalculateUIPlayerInfo()
    {
        Debug.Log("Players is ... " + players);
        //might need a function in player that calculates the BP at any time
        for (int i = 0; i < players.Count; i++)
        {
            UINames[i].text = "Name: " + players[i].name;
            //UIBPS[i].text = "BP: " + players[i].CalculateBP().ToString();
            UINumCards[i].text = "Cards: " + players[i].hand.Count.ToString();
            UIRanks[i].text = "Rank: " + players[i].rankCard.name;
            UIShields[i].text = "Shields: " + players[i].score.ToString();
        }

        //adds player BPS for a quest or generally
        PopulatePlayerBPS();

        playerPanels[currentPlayerIndex].GetComponent<Outline>().enabled = true;
    }

    public void DrawFromAdventureDeck()
    {
        //have a draw from adventure deck that takes a certain number of cards to be drawn instead doing it one by one that way click only happens once
        if (drawAdventureCard)
        {
            drawnAdventureCards = DrawFromDeck(adventureDeck, numCardsToBeDrawn);
            drawAdventureCard = false;
        }
        ToggleDeckAnimation();
    }

    public void DrawFromStoryDeck()
    {
        //only one story card is ever drawn
        if (drawStoryCard)
        {
            drawnStoryCard = DrawFromDeck(storyDeck, 1)[0];
            drawStoryCard = false;
        }
        ToggleDeckAnimation();
    }

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

    //called to re-assert that the state of the player's hand and the ui match
    //currently is destructively buggy, consumes about 200mb/s of your ram before eventually soft locking your PC
    //DO NOT CALL
    //public void MatchUIWithPlayerHand()
    //{

    //    userInput.DestroyChildren("CurrentHand");

    //    for (int i = 0; i < players[currentPlayerIndex].hand.Count; i++)
    //    {
    //        userInput.AddToUICardPanel(CreateUIElement(players[currentPlayerIndex].hand[i]));
    //    }

    //}

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

    public List<Card> DrawFromDeck(Deck deckToDrawFrom, int numCards)
    {
        List<Card> drawnCards = new List<Card>();

        for (int i = 0; i < numCards; i++)
        {
            drawnCards.Add(deckToDrawFrom.drawCard());
        }

        return drawnCards;
    }

    //may need to change that to be more general (have a deck and an int for number of cards drawn)
    public List<Card> generateHand(Deck deckToDrawFrom)
    {
        List<Card> hand = new List<Card>();

        for (int i = 0; i < 11; i++)
        {
            hand.Add(deckToDrawFrom.drawCard());
        }

        return hand;
    }

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

    //toggle the story deck and adventure deck ui animations
    public void ToggleDeckAnimation()
    {
        storyDeckUIButton.GetComponent<Animator>().enabled = drawStoryCard;
        adventureDeckUIButton.GetComponent<Animator>().enabled = drawAdventureCard;
    }

    //if you want to empty any panel
    public void EmptyPanel(GameObject myPanel)
    {
        for (int i = 0; i < myPanel.transform.childCount; i++)
        {
            Destroy(myPanel.transform.GetChild(i).gameObject);
        }
    }

    //to be called by players to setup rank and shields
    public void SetRankCardUI()
    {
        rankCard.myCard = players[currentPlayerIndex].rankCard;
        rankCard.ChangeTexture();
        UIShieldNum.text = ": " + players[currentPlayerIndex].score;
    }

    //checks for cycles will occur elsewhere or maybe later
    //updates turn to go to next turn
    public void UpdatePlayerTurn()
    {
        playerPanels[currentPlayerIndex].GetComponent<Outline>().enabled = false;

        //modify the value of the current player index to use the player array
        if (currentPlayerIndex >= numPlayers - 1) { currentPlayerIndex = 0; }
        else { currentPlayerIndex++; }

        //resetup ui messages
        CalculateUIPlayerInfo();
        populatePlayerBoard();
    }

    public void PopulatePlayerBPS()
    {
        //sets up player BPS for quests tourneys etc .... and returns them back when done check current event and quest and tourney
        for (int i = 0; i < players.Count; i++)
        {
            //check for special ally abilities
            if (currentQuest != null){
              UIBPS[i].text = "BP: " + players[i].CalculateBP(currentQuest.name, players);
            } else {
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

    public void EmptyQuestPanel()
    {
        EmptyPanel(questStagePanel);
        questStageNumber.text = "";
        questStageBPTotal.text = "";
    }

    //adds ui elements to game board of current player
    public void populatePlayerBoard()
    {
        //using current player once we setup player turn change will update this
        EmptyPanel(handPanel);
        EmptyPanel(amourPanel);
        EmptyPanel(allyPanel);
        EmptyPanel(weaponPanel);
        EmptyPanel(activatedPanel);

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
            else { newPlayer = new Player("CPU" + (i + 1).ToString(), DrawFromDeck(adventureDeck , 12) , new iStrategyCPU2() , shieldPaths[shield]); }

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


    public void DiscardCards()
    {
        if (userInput.discardPanelUIEnabled)
        {
            if (numIterations < numPlayers)
            {
                if (!players[currentPlayerIndex].handCheck())
                {
                    //doesn't need to discard update turn
                    numIterations++;
                    UpdatePlayerTurn();
                }
                else
                {
                    List<Card> discards = players[currentPlayerIndex].strategy.fixHandDiscrepancy(players[currentPlayerIndex].hand);

                    if (discards != null)
                    {
                        //ai discarded we are done
                        numIterations++;
                        UpdatePlayerTurn();
                        userInput.DeactivateDiscardPanel();
                        userInput.ActivateDiscardCheck("You need to Discard" + (12 - players[currentPlayerIndex].hand.Count).ToString() + "Cards");
                    }
                    else if (userInput.doneDiscardingCards)
                    {
                        //if things don't go well
                        if (players[currentPlayerIndex].handCheck())
                        {
                            //add back to players hand 
                            returnToPlayerHand();
                            userInput.doneDiscardingCards = false;
                        }
                        else
                        {
                            //if things go well
                            DiscardAdvenureCards(userInput.selectedCards);
                            numIterations++;
                            UpdatePlayerTurn();
                            userInput.DeactivateDiscardPanel();
                            userInput.ActivateDiscardCheck("You need to Discard" + (12 - players[currentPlayerIndex].hand.Count).ToString() + "Cards");
                        }
                    }
                }
            }
        }
    }

    //Asks user for input then builds initial game board
    public void PlayerSetup()
    {
        // do ui checks here then build board when done
        if (userInput.UIEnabled)
        {
            if (userInput.keyboardInputUIEnabled)
            {
                if (setupState == 0)
                {
                    if (userInput.KeyboardInput.text.Length > 0 && Input.GetKeyDown("return"))
                    {
                        bool result = int.TryParse(userInput.KeyboardInput.text, out numPlayers);

                        if (result && numPlayers > 1 && numPlayers < 5)
                        {
                            userInput.DeactivateUI();
                            setupState = 1;
                            userInput.ActivateUserInputCheck("How Many Human Players?");
                        }
                        else
                        {
                            userInput.KeyboardInput.text = "";
                        }
                    }
                }
                else if (setupState == 1)
                {
                    if (userInput.KeyboardInput.text.Length > 0 && Input.GetKeyDown("return"))
                    {
                        bool result = int.TryParse(userInput.KeyboardInput.text, out numHumanPlayers);

                        if (result && numHumanPlayers > 0 && numPlayers - numHumanPlayers >= 0)
                        {
                            userInput.DeactivateUI();
                            setupState = 2;
                            numCpus = numPlayers - numHumanPlayers;
                        }
                        else
                        {
                            userInput.KeyboardInput.text = "";
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
            setupState = 3; //won't go in here agaim
        }
    }

    public void ResetPlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].participating = false;
            players[i].sponsoring = false;
        }
    }


    public void QueryStrategies()
    {
        if (userInput.UIEnabled)
        {
            if (userInput.keyboardInputUIEnabled)
            {
                if (setupState >= 3)
                {
                    if (numIterations < numCpus)
                    {
                        if (userInput.KeyboardInput.text.Length > 0 && Input.GetKeyDown("return"))
                        {
                            int strategy = 0;
                            bool result = int.TryParse(userInput.KeyboardInput.text, out strategy);

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
                                userInput.KeyboardInput.text = "";
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

    //SOME CHANGES HERE FOR PARTICIPATE IN TOURNEY FUNCTION
    public void ParticipationCheck(string state)
    {
        if (userInput.UIEnabled)
        {
            if (userInput.booleanUIEnabled)
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

                        //do whatever
                        //print("No");
                        players[currentPlayerIndex].participating = false; // just in case
                        UpdatePlayerTurn();
                        populatePlayerBoard();
                        userInput.DeactivateUI();
                        userInput.ActivateBooleanCheck("Do you want to participate?");
                        numIterations++;
                    }
                    else if (participation == 1)
                    {
                        //print("Yes");
                        players[currentPlayerIndex].participating = true;
                        //participants.Add(players[currentPlayerIndex]);
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
        else
        {
            /*
            //return participants
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].participating)
                Debug.Log("Paricipant" + (i + 1) + ": " + players[i].name);
            }
            */
        }
    }

    //check if player wants to sponsor
    public void SponsorCheck()
    {
        //checking for sponsors
        if (userInput.UIEnabled)
        {
            if (userInput.booleanUIEnabled)
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
            if(foeValues[i] != 0)
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

                                        //A "Hopping Index" which carries the current foeBP + sum of weapon BPs added
                                        int ii = i + (wbp / 5);
                                        //This is the section which tests if adding multiple weapons to a single foe allows the creation of a valid Quest
                                        for (int k = j + 1; k < players[currentPlayerIndex].hand.Count; k++)
                                        {
                                            if (players[currentPlayerIndex].hand[k].type == "Weapon Card" && weaponAvailable[k])
                                            {
                                                //holds the weapon card found in the player's hand, done in order to reduce line length
                                                WeaponCard foundWeaponCard = (WeaponCard) players[currentPlayerIndex].hand[k];

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

                                                //A new valid unique BP has been found using multiple weapon cards, the value at foeValues[i]
                                                //is decremented and the value at a new index equal to the the current index plus the sum bp
                                                //of all weapons used is incremented.
                                                if (foeValues[ii + (wbp_VERSION_K / 5)] < 1)
                                                {
                                                    foeValues[i]--;
                                                    foeValues[ii + (wbp_VERSION_K / 5)]++;
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

    public bool SponsorCapabilityCheck()
    {
        if (!SponsorCapabilitySoftCheck())
            return false;
        else if (SponsorCapabilityHardCheck())
            return true;
        else
            return false;
    }

    public int GetFoeBP(FoeCard f)
    {
        if (currentQuest.foe == f.name)
        {
            return f.getMaxBP();
        }
        else
            return f.getMinBP();

    }


    //query sponsor for his cards for the quest
    //IF SPONSOR ADDS NOTHING OR WE DO NOT
    //CHECK USER INPUT IF IT DOESN'T FIT INTO REQUIREMENTS END SPONSORSHIP AND END QUEST
    public void SponsorQuery()
    {
        if (userInput.UIEnabled)
        {
            if (userInput.cardPanelUIEnabled)
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
                    else if (userInput.doneAddingCards)
                    {
                        //player check
                        //Debug.Log("player");
                        sponsorQueriedCards[numIterations] = new List<Card>(userInput.selectedCards);
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


    //NEEDS FIXING TO ADD SHIELDS AND BASE BP AND ADD A STRING TO USE BOTH QUEST AND TOURNAMENTS
    public void CardQuerying()
    {
        if (userInput.UIEnabled)
        {
            if (userInput.cardPanelUIEnabled)
            {
                if (numIterations < numPlayers)
                {
                    //might need to set this to a new list
                    List<Card> result = new List<Card>();

                    if (players[currentPlayerIndex].participating)
                    {
                         result = players[currentPlayerIndex].strategy.playTournament(players, players[currentPlayerIndex].hand, players[currentPlayerIndex].CalculateBP("", players), currentTournament.shields);
                    }
                    if (players[currentPlayerIndex].sponsoring || !(players[currentPlayerIndex].participating))
                    {
                        //skip if sponsoring or not participating
                        //this will help generalize querying in both tournaments and quests
                        queriedCards[currentPlayerIndex] = null;
                        numIterations++;
                        UpdatePlayerTurn();
                        //queriedCards.Add(null); //setup sizes of both queried lists to be empty
                    }
                    else if (userInput.doneAddingCards)
                    {
                        //Debug.Log("player");
                        queriedCards[currentPlayerIndex] = new List<Card>(userInput.selectedCards);
                        userInput.DeactivateUI();
                        numIterations++;
                        UpdatePlayerTurn();
                        populatePlayerBoard();
                        userInput.ActivateCardUIPanel("What AMOUR , ALLY , OR WEAPON CARDS do you want to use?");
                    }
                    else if (result != null)
                    {
                        //Debug.Log("AI here");
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

}