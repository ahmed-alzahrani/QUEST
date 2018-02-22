using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Modify UI functions to modify player Cards

//TOURNAMENTS DONEEEEEEE
//QUESTS 
//SPECIAL EFFECTS
//TEST CARDS
//Checking if player is over 12 cards ask for discarding 
//Checking for player rank up
//should we tell players in round 2 how much bp they have left

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

    public bool UIEnabled;
    public bool booleanUIEnabled;
    public bool keyboardInputUIEnabled;
    public bool cardPanelUIEnabled;
    public bool doneAddingCards;

    public void SetupUI()
    {
        selectedCards = new List<Card>();
        UICardsSelected = new List<GameObject>();

        // setup event listeners
        yesButton.onClick.AddListener(Yes);
        noButton.onClick.AddListener(No);
        submitButton.onClick.AddListener(Submit);
        buttonResult = "";
    }

    void Submit() { doneAddingCards = true; }
    void Yes() { buttonResult = "Yes"; }
    void No() { buttonResult = "No"; }

    public void ActivateBooleanCheck(string userMsg)
    {
        buttonResult = "";
        UIEnabled = true;
        booleanUIEnabled = true;
        keyboardInputUIEnabled = false;
        cardPanelUIEnabled = false;
        doneAddingCards = false;

        foregroundPanel.SetActive(true);
        booleanPanel.SetActive(true);
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
        doneAddingCards = false;

        foregroundPanel.SetActive(true);
        inputPanel.SetActive(true);
        cardPanel.SetActive(false);
        booleanPanel.SetActive(false);
        KeyboardInput.text = "";
        userMessage1.text = userMsg;
    }

    public void ActivateCardUIPanel(string userMsg)
    {
        UIEnabled = true;
        cardPanelUIEnabled = true;
        keyboardInputUIEnabled = false;
        booleanUIEnabled = false;
        doneAddingCards = false;

        //foregroundPanel.SetActive(true);
        cardPanel.SetActive(true);
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
                int BPLost = 0;

                if (card.type == "FoeCard")
                {
                    FoeCard foe = (FoeCard)card;
                    //DEPENDS !!!!!!!!!!!!! MAYBE
                    BPLost = foe.minBP;
                }
                else if (card.type == "Weapon Card")
                {
                    WeaponCard weapon = (WeaponCard)card;
                    BPLost = weapon.battlePoints;
                }
                else if (card.type == "Ally Card")
                {
                    AllyCard ally = (AllyCard)card;
                    BPLost = ally.battlePoints;
                }
                else if (card.type == "Amour Card")
                {
                    AmourCard amour = (AmourCard)card;
                    BPLost = amour.battlePoints;
                }

                totalBPCount -= BPLost;
                totalBP.text = "BP: " + totalBPCount.ToString();
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

        int BPGained = 0;

        if (script.myCard.type == "FoeCard")
        {
            FoeCard foe = (FoeCard)script.myCard;
            //DEPENDS !!!!!!!!!!!!! MAYBE
            BPGained = foe.minBP;
        }
        else if (script.myCard.type == "Weapon Card")
        {
            WeaponCard weapon = (WeaponCard)script.myCard;
            BPGained = weapon.battlePoints;
        }
        else if (script.myCard.type == "Ally Card")
        {
            AllyCard ally = (AllyCard)script.myCard;
            BPGained = ally.battlePoints;
        }
        else if (script.myCard.type == "Amour Card")
        {
            AmourCard amour = (AmourCard)script.myCard;
            BPGained = amour.battlePoints;
        }

        totalBPCount += BPGained;
        totalBP.text = "BP: " + totalBPCount.ToString();

        //add card to panel
        card.transform.SetParent(chosenCardsPanel.transform);
    }

    GameObject RemoveFromCardUIPanel(int index)
    {
        selectedCards.RemoveAt(index);

        GameObject result = UICardsSelected[index];
        UICardsSelected.RemoveAt(index);
        return result;
    }

    public void DeactivateUI()
    {
        foregroundPanel.SetActive(false);
        cardPanel.SetActive(false);
        UIEnabled = false;
        keyboardInputUIEnabled = false;
        booleanUIEnabled = false;
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

    //reference to players that have over 12 cards
    public List<Player> offendingPlayers;
    public bool playerStillOffending;

    public int numIterations;

    //public List<List<Card>> queriedCards;
    public List<Card>[] queriedCards;

    // Use this for initialization
    void Start()
    {
        //setup game variables
        isSettingUpGame = true;
        currentPlayerIndex = 0;
        setupState = 0;
        shieldPaths = new List<string> {"Textures/Backings/s_backing" , "Textures/Backings/s_backing" , "Textures/Backings/s_backing" , "Textures/Backings/s_backing"};
        players = new List<Player>();
        drawnAdventureCards = new List<Card>();
        //queriedCards = new List<List<Card>>();

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
        //Debug.Log("Player" + (currentPlayerIndex + 1).ToString() + "turn");

        //Check if game is done with all players and if that is the case declare winner
        if (isSettingUpGame)
        {
            PlayerSetup();
        }
        else
        {
            //check for drawn adventure cards
            if (drawnAdventureCards.Count > 0)
            {
                for (int i = 0; i < drawnAdventureCards.Count; i++)
                {
                    players[currentPlayerIndex].hand.Add(drawnAdventureCards[i]);
                    AddToPanel(CreateUIElement(drawnAdventureCards[i]), handPanel);
                    drawnAdventureCards.RemoveAt(i);
                }
                //check if player has over 12 cards and setup event for that
            }

            //check for drawn story cards
            if (drawnStoryCard != null)
            {
                Debug.Log(drawnStoryCard.type);
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
                    userInput.ActivateBooleanCheck("Do you want to sponsor this quest?");
                    currentQuest = (QuestCard)drawnStoryCard;
                }

                setupState = 0;
                numIterations = 0;
                AddToPanel(CreateUIElement(drawnStoryCard), questPanel);
                drawnStoryCard = null;
            }

            //check if story event is done
            if (isDoneStoryEvent)
            {
                //DISCARD THE ACTUAL CARD INTO DISCARD PILEEEE!!!!!!!!!!
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
                System.Array.Clear(queriedCards, 0 , queriedCards.Length);

                //reset turn to draw new story card
                drawStoryCard = true;
                ToggleDeckAnimation();
                userInput.DeactivateUI(); // just in case
                UpdatePlayerTurn(); //go to next turn
            }
            else if (currentEvent != null)
            {
                //isDoneStoryEvent = currentEvent.effect.execute(players , eventCard);
            }
            else if (currentTournament != null)
            {
                //isDoneStoryEvent = currentTournament.tournament.execute(players , tCard);
                currentTournament.tournament.execute(null, currentTournament, this);
            }
            else if (currentQuest != null)
            {
                //isDoneStoryEvent = currentQuest.quest.execute(players , questCard);
            }


            // selected cards here!!!
            if (selectedCard != null)
            {
                //error card detection should occur here!!!!!
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
                    if (currentTournament != null)
                    {
                        bool addToPanel = true;
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
                        else if (!(selectedCard.type == "Ally Card"))
                        {
                            addToPanel = false;
                        }

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
                    else if (currentQuest != null)
                    {
                        //do quest card selection checks here same as tournaments
                    }
                }

                //maybe we can check for certain cards like mordred here with some state 
                selectedCard = null;
            }


            //Calculates UI player information
            CalculateUIPlayerInfo();
        }
    }

    public void DiscardAdvenureCards(List<Card> discardCards)
    {

    }

    //update quest data of a certain stage
    public void AddQuestData(int stageNumber , int totalBP)
    {
        questStageNumber.text = "Stage: " + stageNumber.ToString();
        questStageBPTotal.text = "BP: " + totalBP.ToString();
    }

    public void CalculateUIPlayerInfo()
    {
        //might need a function in player that calculates the BP at any time
        for (int i = 0; i < players.Count; i++)
        {
            UINames[i].text = "Name: " + players[i].name;
            UIBPS[i].text = "BP: " + players[i].CalculateBP().ToString();
            UINumCards[i].text = "Cards: " + players[i].hand.Count.ToString();
            UIRanks[i].text = "Rank: " + players[i].rankCard.name;
            UIShields[i].text = "Shields: " + players[i].score.ToString();
        }

        playerPanels[currentPlayerIndex].GetComponent<Outline>().enabled = true;
    }

    public void DrawFromAdventureDeck()
    {
        //have a draw from adventure deck that takes a certain number of cards to be drawn instead doing it one by one that way click only happens once
        if (drawAdventureCard)
        {
            drawnAdventureCards = DrawFromDeck(adventureDeck , numCardsToBeDrawn);
            drawAdventureCard = false;
        }
        ToggleDeckAnimation();
    }

    public void DrawFromStoryDeck()
    {
        //only one story card is ever drawn
        if (drawStoryCard)
        {
            drawnStoryCard = DrawFromDeck(storyDeck , 1)[0];
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

    public List<Card> DrawFromDeck(Deck deckToDrawFrom , int numCards)
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
    }

    public void populateQuestBoard()
    {
        EmptyPanel(questPanel);
        EmptyPanel(questStagePanel);
        questStageBPTotal.text = "Stage: 1";
        questStageNumber.text = "";

        AddToPanel(CreateUIElement(currentQuest) , questPanel);
        //check if it is flipped b4 adding the card and set its current backing
        //AddToPanel( , questStagePanel);

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

        //Setup Amour

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
            Player myPlayer = new Player("Player" + (i + 1).ToString(), DrawFromDeck(adventureDeck , 12), new iStrategyPlayer(), shieldPaths[shield]);
            shieldPaths.RemoveAt(shield);       //Each player has unique shields
            myPlayer.gameController = this;
            playerPanels[i].SetActive(true);    //add a ui panel for each player
            myPlayers.Add(myPlayer);
        }

        // create AI players

        for (int i = 0; i < numCpus; i++)
        {
            int rnd = Random.Range(0 , 2);
            int shield = Random.Range(0, shieldPaths.Count - 1);
            Player newPlayer;

            Debug.Log(rnd);

            //if (rnd == 0) { newPlayer = new Player("CPU" + (i + 1).ToString(), DrawFromDeck(adventureDeck, 12), new iStrategyCPU1(), shieldPaths[shield]); }
            //else { newPlayer = new Player("CPU" + (i + 1).ToString(), DrawFromDeck(adventureDeck , 12) , new iStrategyCPU2() , shieldPaths[shield]); }

            newPlayer = new Player("CPU" + (i + 1).ToString(), DrawFromDeck(adventureDeck, 12), new iStrategyCPU1(), shieldPaths[shield]);

            playerPanels[i + numHumanPlayers].SetActive(true);    //add a ui panel for each player
            shieldPaths.RemoveAt(shield);                         //Each player has unique shields
            myPlayers.Add(newPlayer);
        }

        //create a queried cards array
        queriedCards = new List<Card>[numPlayers];
        return myPlayers;
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

            //after getting all player information set game up and end setup state
            players = CreatePlayers(null , null);
            populatePlayerBoard();
            isSettingUpGame = false;
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
                        // participation = players[currentPlayerIndex].strategy.participateInTourney(players, currentTournament.shields, this);
                        participation = players[currentPlayerIndex].strategy.participateInTourney(players, 10, this);

                    }
                    else if (state == "Quest")
                    {
                        //NEED TO CHANGE THISSSS!!!!!!!!!!!!!!!!
                         participation = players[currentPlayerIndex].strategy.participateInQuest(currentQuest.stages, players[currentPlayerIndex].hand);
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
                    else if (participation == 2)
                    {
                        //not done yet
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

    public void SponsorCheck()
    {
        //checking for sponsors 
        if (userInput.UIEnabled)
        {
            if (userInput.booleanUIEnabled)
            {
                //check with currentplayerIndex
                if (numIterations < numPlayers)
                {
                    int participation = -1;
                    //if (state == "Tournament")
                    //{
                        // participation = players[currentPlayerIndex].strategy.participateInTourney(players, currentTournament.shields, this);
                        participation = players[currentPlayerIndex].strategy.participateInTourney(players, 0, this);

                    //}
                    //else if (state == "Quest")
                   // {
                        participation = players[currentPlayerIndex].strategy.participateInQuest(currentQuest.stages, players[currentPlayerIndex].hand);
                    //}

                    //Debug.Log(userInput.buttonResult);
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
                        print("No");
                        players[currentPlayerIndex].participating = false; // just in case
                        UpdatePlayerTurn();
                        populatePlayerBoard();
                        userInput.DeactivateUI();
                        userInput.ActivateBooleanCheck("Do you want to participate?");
                        numIterations++;
                    }
                    else if (participation == 1)
                    {
                        print("Yes");
                        players[currentPlayerIndex].participating = true;
                        //participants.Add(players[currentPlayerIndex]);
                        UpdatePlayerTurn();
                        populatePlayerBoard();
                        userInput.DeactivateUI();
                        userInput.ActivateBooleanCheck("Do you want to participate?");
                        numIterations++;
                    }
                    else if (participation == 2)
                    {
                        //not done yet
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
            //return participants
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].participating)
                    Debug.Log("Paricipant" + (i + 1) + ": " + players[i].name);
            }
        }
    }

    //NEEDS FIXING TO ADD SHIELDS AND BASE BP
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
                         result = players[currentPlayerIndex].strategy.playTournament(players, players[currentPlayerIndex].hand, 0, 0);
                    }
                    if (players[currentPlayerIndex].sponsoring || !(players[currentPlayerIndex].participating))
                    {
                        //skip if sponsoring or not participating
                        //this will help generalize querying in both tournaments and quests
                        numIterations++;
                        UpdatePlayerTurn();
                        //queriedCards.Add(null); //setup sizes of both queried lists to be empty
                        queriedCards[currentPlayerIndex] = null;
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
                                Debug.Log(i.ToString()+ ": " + queriedCards[i][j].name);
                            }
                        }
                    }
                }
            }
        }
    }

}
