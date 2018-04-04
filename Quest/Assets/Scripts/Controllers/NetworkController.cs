using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.IO;


//get lobby information (names and colors to display names)

/*
 * SyncVars: variables u want the server to change (they are sent back by the server back to the clients)
   * Hook: allows u to call a function when a sync var variable changes in value
 * Command: attribute that lets functions run on the server by sending a command to the server (function names need to start with Cmd)
 * Client: attribute for functions that are run only by clients (it is a redundancy of clientRpc)
 * ClientRPC: attribute for functions that allows running client functions on a server
 * TargetRPC: same as clientRpc but can only be run on one client and not all ready clients
*/

// create our own messages

//client messages
public class ClientMessage : MessageBase
{
    public string message;
    public enum messageType { CARDS , INT , BOOL , UI}; // type of message to be sent by user (this will be changed)
    public string color;                                //differenciate players by color
}

//server messages
public class ServerMessage : MessageBase
{
    public string message;
    public enum messageType { CARDS, INT, BOOL };    // type of message to be sent by user
    //public string name;                            // assuming that it is unique for the moment
}

public class NetworkController : Controller
{
    public Prototype.NetworkLobby.LobbyPlayer lobbyGuy;
    public NetworkIdentity playerId;

    //player information
    public GameObject[] lobbyGuys;
    public List<Player> otherPlayers;
    public List<NetworkIdentity> playerIds;
    public GameObject lobbyManager;

    public List<GameObject> otherLobbyGuys;

    private NetworkManager server;
    public bool waitingOnClient;

    // Use this for initialization
    void Start()
    {
        waitingOnClient = false;
        isSettingUpGame = true;

        //finding the lobby manager
        lobbyManager = GameObject.FindGameObjectWithTag("LobbyManager");
        if (lobbyManager){
          Debug.Log("Found it!!!!!..... " + lobbyManager.name);
        }


        Debug.Log("Printing out the child count of lobby manager");
        Debug.Log(lobbyManager.transform.childCount);

        for (int i = 0; i < lobbyManager.transform.childCount; i++){
          if (lobbyManager.transform.GetChild(i).gameObject.tag == "LobbyPanel"){

            Transform secondChild = lobbyManager.transform.GetChild(i);

            for(int j =0; j < secondChild.childCount; j++)
            {
              if (secondChild.GetChild(j).gameObject.tag == "PlayerListSubPanel")
              {
                Transform thirdChild = secondChild.GetChild(j);

                for (int w = 0; w < thirdChild.childCount; w++)
                {
                    if(thirdChild.GetChild(w).gameObject.tag == "PlayerList")
                    {
                      Transform fourthChild = thirdChild.GetChild(w);

                      for (int k = 0; k < fourthChild.childCount; k++)
                      {
                        if (fourthChild.GetChild(k).gameObject.tag == "LobbyPlayer")
                        {
                          otherLobbyGuys.Add(fourthChild.GetChild(k).gameObject);
                          Debug.Log(fourthChild.GetChild(k).gameObject.name);
                        }
                      }
                    }
                }
              }
            }
          }
        }

        Debug.Log(otherLobbyGuys.Count);

        //no idea what msgType.Highest does but iam using it anyway
        NetworkServer.RegisterHandler(MsgType.Highest, HandleClientRequest);

      //  lobbyGuy =  GameObject.FindGameObjectWithTag("LobbyPlayer").GetComponent<Prototype.NetworkLobby.LobbyPlayer>();
        //playerId =  GameObject.FindGameObjectWithTag("LobbyPlayer").GetComponent<NetworkIdentity>();
      //  Debug.Log("The name of the dude in the lobby is...");
      //  Debug.Log(lobbyGuy.name);

        //lobbyGuys = GameObject.FindGameObjectsWithTag("LobbyPlayer");


        //setup game variables
        // Game Variables like this need to become syncVars, so that they sync up across all clients to retain game state?

        shieldPaths = new List<string> { "Textures/Backings/Blue", "Textures/Backings/Green", "Textures/Backings/Red", "Textures/Backings/Gold" };
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

        /*
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
        */

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
    }

    //Update is called once per frame
    //player discard piles for over 12 cards and etc...
    void Update()
    {

        if (isSettingUpGame)
        {
            //setup game
            //get host
        //    playerId = GameObject.FindGameObjectWithTag("LobbyPlayer").GetComponent<NetworkIdentity>();
        //    lobbyGuys = GameObject.FindGameObjectsWithTag("LobbyPlayer");

            // Debug.Log("ppl in lobby" + lobbyGuys.Length);

            //store the player's ids to knw where our server is

            /*
            for (int i = 0; i < lobbyGuys.Length; i++)
            {
                //store the connections of players
                playerIds.Add(lobbyGuys[i].GetComponent<NetworkIdentity>());

                //get connection to server's id
                Debug.Log("player ids" + lobbyGuys[i].GetComponent<NetworkIdentity>().connectionToServer.connectionId);
            }
            */

            /*
            if (playerId.isServer)
            {
                Debug.Log("He is a server!");
            }

            if (playerId.isClient)
            {
                Debug.Log("He is a client!");
            }
            */

            // Debug.Log("The length of the array of players is...");
            // Debug.Log(lobbyGuys.Length);
            // otherPlayers = UIUtil.CreateNetworkPlayers(this, lobbyGuys);

            //print players
            /*
            for (int i = 0; i < otherPlayers.Count; i++)
            {
                otherPlayers[i].display();
            }

            isSettingUpGame = false;
        }

        */
        //only on server
        SendMessageToClient(true);


        for (int i = 0; i < playerIds.Count; i++)
        {
            //send a message
            if (playerIds[i].isServer && !waitingOnClient)
            {
                Debug.Log("sending message");
                SendMessageToClient(true);
            }

            if (playerIds[i].isLocalPlayer && !playerIds[i].isServer)
            {
                Debug.Log("Player " + i + "is a client");
                return;
            }
        }


        /*
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
        */
    }
  }

    //TO SEND CARDS TO PLAYERS
    //get cards as strings and then send them to players
    public List<string> EncryptCards(List<Card> cards)
    {
        List<string> cardPaths = new List<string>();

        for (int i = 0; i < cards.Count; i++)
        {
            cardPaths.Add(cards[i].texturePath);
        }

        return cardPaths;
    }

    /*
    //TO RECEIVE CARDS FROM PLAYERS
    //so we receive the player that sent the cards and the indeces of the cards he sent and get the actual cards from him
    public List<Card> DecryptCards(Player player , List<int> cardIndeces)
    {
        List<Card> cards = new List<Card>();

        for (int i = 0; i < cardIndeces.Count; i++)
        {
            //add card
            cards.Add(player.hand[cardIndeces[i]]);
        }

        for (int i = 0; i < cardIndeces.Count; i++)
        {
            //remove cards from hand
            player.hand.RemoveAt(cardIndeces[i]);
        }

        //just in case
        cardIndeces.Clear();
        return cards;
    }
    */

    //THIS ONE IS BETTER (needs to be fixed)
    //Alternate deserializing function
    public List<Card> DecryptCards(Player player , List<string> receivedCards)
    {
        List<Card> cards = new List<Card>();

        // go through hands and find cards using their textures
        for (int i = 0; i < receivedCards.Count; i++)
        {
            for (int j = 0; j < player.hand.Count; j++)
            {
                if (receivedCards[i] == player.hand[j].texturePath)
                {
                    cards.Add(player.hand[j]);
                    player.hand.RemoveAt(j);
                    //j--;
                    break;
                }
            }
        }

        return cards;
    }

    public void HandleClientRequest(NetworkMessage message)
    {
        //get a client message see which client sent the info and work accordingly

        var clientMessage = message.ReadMessage<ClientMessage>();
        Debug.Log(clientMessage.message);
    }

    //send messages to clients
    public void SendMessageToClient(bool waitOnClient)
    {

        ServerMessage msg = new ServerMessage();
        msg.message = "hello";

        //send to client and send to all i think is what we need here
         // NetworkServer.SendToClient(otherPlayers[0].connection.connectionToClient.connectionId , MsgType.Highest , msg);

        //check whether we want to wait for a user response
        waitingOnClient = waitOnClient;
        Debug.Log("SERVER sent message to CLIENT!!!!");
    }

    public void SendMessageToAllClients(bool waitOnClient)
    {
        ServerMessage msg = new ServerMessage();
        msg.message = "hello";

        //send to client and send to all i think is what we need here
        //NetworkServer.SendToClient(otherPlayers[0].connection.connectionToClient.connectionId , MsgType.Highest , msg);
        NetworkServer.SendToAll(MsgType.Highest, msg);

        //check whether we want to wait for a user response
        waitingOnClient = waitOnClient;
        Debug.Log("SERVER sent message to CLIENT!!!!");
    }
}
