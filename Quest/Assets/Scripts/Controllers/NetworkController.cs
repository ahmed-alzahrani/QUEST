using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class NetworkController : Controller
{
  public Prototype.NetworkLobby.LobbyPlayer lobbyGuy;
  public UnityEngine.Networking.NetworkIdentity playerId;
  public GameObject[] lobbyGuys;
  public List<Player> otherPlayers;

    // Use this for initialization
    void Start()
    {

      Debug.Log("HELLO!!!!!!!!!!!!!!!!!");


      lobbyGuy =  GameObject.FindGameObjectWithTag("LobbyPlayer").GetComponent<Prototype.NetworkLobby.LobbyPlayer>();
      playerId =  GameObject.FindGameObjectWithTag("LobbyPlayer").GetComponent<UnityEngine.Networking.NetworkIdentity>();

      lobbyGuys = GameObject.FindGameObjectsWithTag("LobbyPlayer");

      Debug.Log(lobbyGuy.playerName);
      Debug.Log(playerId.playerControllerId);
      if (playerId.isServer)
      {
        Debug.Log("He is a server!");
      }

      if (playerId.isClient)
      {
        Debug.Log("He is a client!");
      }

      Debug.Log("The length of the array of players is...");
      Debug.Log(lobbyGuys.Length);



        //setup game variables
    
        // Game Variables like this need to become syncVars, so that they sync up across all clients to retain game state?
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
        cheatPanel = GameObject.FindGameObjectWithTag("CheatPanel");
        GameObject.FindGameObjectWithTag("CheatPanelButton").GetComponent<Button>().onClick.AddListener(CheatPanelToggle);
        cheatPanel.SetActive(false);
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

        otherPlayers = UIUtil.CreateNetworkPlayers(this, lobbyGuys);

        for (int i = 0; i < otherPlayers.Count; i++)
        {
          otherPlayers[i].display();
        }
    }

    //Update is called once per frame
    //player discard piles for over 12 cards and etc...
    void Update()
    {
        if (!foundWinner)
        {
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
}
