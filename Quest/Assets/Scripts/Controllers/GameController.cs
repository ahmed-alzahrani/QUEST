using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//MAYBE ADD AN EVENT HANDLER FOR WHEN WE NEED TO DRAW FROM A CERTAIN DECK ADD A CLICK HANDLER FOR IT AND REMOVE IT AFTERWARDS 
//FINISH THE PANEL WITH THE CARDS !!!!!!!!!!!!!!!!!!!!!!
// TO USE THIS INPUT BAR SET THE FOREGROUND PANEL TO ACTIVE THEN SET THE USER MESSAGE AND ACCORDING TO WHICH TYPE OF
// MESSAGE EITHER DEACTIVATE THE INPUT FIELD OR THE 2 BUTTONS (ASKING YES OR NO QUESTIONS OR ASKING FOR USER INPUT)

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

    //for actual input user check
    public GameObject inputPanel;
    public Text userMessage1;
    public InputField KeyboardInput;

    public bool UIEnabled;
    public bool booleanUIEnabled;
    public bool keyboardInputUIEnabled;

    public string buttonResult;

    public void SetupUI()
    {
        // setup event listener
        yesButton.onClick.AddListener(Yes);
        noButton.onClick.AddListener(No);
        buttonResult = "";
    }

    public void Yes()
    {
        buttonResult = "Yes";
    }

    public void No()
    {
        buttonResult = "No";
    }

    public void ActivateBooleanCheck(string userMsg)
    {
        buttonResult = "";
        UIEnabled = true;
        booleanUIEnabled = true;
        keyboardInputUIEnabled = false;
        foregroundPanel.SetActive(true);
        booleanPanel.SetActive(true);
        inputPanel.SetActive(false);
        userMessage.text = userMsg;
    }

    public void ActivateUserInputCheck(string userMsg)
    {
        buttonResult = "";
        UIEnabled = true;
        keyboardInputUIEnabled = true;
        booleanUIEnabled = false;
        foregroundPanel.SetActive(true);
        inputPanel.SetActive(true);
        booleanPanel.SetActive(false);
        userMessage1.text = userMsg;
    }

    public void DeactivateUI()
    {
        foregroundPanel.SetActive(false);
        UIEnabled = false;
        keyboardInputUIEnabled = false;
        booleanUIEnabled = false;
        buttonResult = "";
    }
}

// Maybe we can have the description of a card where when we click a card it shows its description data on the screen
public class GameController : MonoBehaviour
{
    public Button button;
    public List<Player> players;
    public CardUIScript script;
    public deckBuilder decks;
    public Deck storyDeck;
    public Deck adventureDeck;
    public int turnCount = 0;
    public Turn nextTurn;
    public Card selectedCard;
    public int numPlayers;

    public GameObject cardPrefab;
    public UIInput userInput;  //checking for user input

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

    // Use this for initialization
    void Start ()
    {
        //Deck building
        decks = new deckBuilder();
        storyDeck = decks.buildStoryDeck();
        adventureDeck = decks.buildAdventureDeck();

        // Store gameboard cards
        shieldsCard = GameObject.FindGameObjectWithTag("Shields").GetComponent<CardUIScript>();
        rankCard = GameObject.FindGameObjectWithTag("RankCard").GetComponent<CardUIScript>();

        //PLAYING AROUND
        players = new List<Player>();
        players.Add(new Player("" , null , null , "Textures/Backings/s_backing"));
        rankCard.myCard = players[0].rankCard;
        rankCard.ChangeTexture();
        shieldsCard.myCard = new RankCard(null, null, players[0].shieldPath , 0);
        shieldsCard.ChangeTexture();

        //Store gameBoard panels
        questPanel = GameObject.FindGameObjectWithTag("QuestCard");
        handPanel = GameObject.FindGameObjectWithTag("CurrentHand");
        allyPanel = GameObject.FindGameObjectWithTag("AllyCards");
        weaponPanel = GameObject.FindGameObjectWithTag("WeaponCards");
        questStagePanel = GameObject.FindGameObjectWithTag("QuestStageCards");
        amourPanel = GameObject.FindGameObjectWithTag("AmourCards");
        activatedPanel = GameObject.FindGameObjectWithTag("ActivatedCard");

        //generate Hand (DO NOT FORGET TO SET WHETHER IT IS A HANDCARD OR NOT)
        //Testing adding to all panels of the gameboard
        generateHand(adventureDeck);
        AddToPanel(CreateUIElement(storyDeck.drawCard()) , questPanel);
        AddToPanel(CreateUIElement(storyDeck.drawCard()) , questStagePanel);
        AddToPanel(CreateUIElement(storyDeck.drawCard()) , allyPanel);
        AddToPanel(CreateUIElement(storyDeck.drawCard()) , weaponPanel);
        AddToPanel(CreateUIElement(storyDeck.drawCard()) , amourPanel);
        AddToPanel(CreateUIElement(storyDeck.drawCard()) , activatedPanel);

        //UI building
        userInput.SetupUI();
        userInput.ActivateUserInputCheck("How many players are playing the game??");
        //userInput.ActivateBooleanCheck("How many players are playing the game??");

        //Other stuff

        /*
        To-Do Here:
            1. Somehow communicate with main menu to grab player's name and create player

            2. Create list of players with human player and CPU players
        */

    }

    // Update is called once per frame
    void Update ()
    {
        if (!userInput.UIEnabled)
        {
            if (selectedCard != null)
            {
                //add selected card to turn
                print(selectedCard);
                selectedCard = null;
            }

            // Once the list of players has been generated the turns need to be set up here
            if (Input.GetButton("Fire1"))
            {
                //script.flipCard();
                // script.ChangeVisibility();
            }

            //if (Input.GetButton("Fire2")) script.flipCard();
        }
        else
        {
            //check user input here!!!!
            //if user is done

            //have a while loop to check correctness
            //this is to check user input if it is a user input field
            //just use this block of code wherever you want this is just a demo like thing
            if (userInput.keyboardInputUIEnabled)
            {
                //do whatever you want do not forget to deactivate the gui though
                if (userInput.KeyboardInput.text.Length > 0 && Input.GetKeyDown("return"))
                {
                    bool result = int.TryParse(userInput.KeyboardInput.text, out numPlayers);

                    if (result && numPlayers > 1 && numPlayers < 5)
                    {
                        userInput.DeactivateUI();
                    }
                    else
                    {
                        userInput.KeyboardInput.text = "";
                    }
                    Debug.Log(numPlayers);
                  //  players = createPlayers(numPlayers)
                }
            }
            else if (userInput.booleanUIEnabled)
            {
                //check the yes / No buttons
                if (userInput.buttonResult == "Yes")
                {
                    print("Yes");
                    //do whatever you want
                    userInput.DeactivateUI();
                }
                else if (userInput.buttonResult == "No")
                {
                    //do whatever
                    print("No");
                    userInput.DeactivateUI();
                }
            }
        }
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
        script.isHandCard = true;

        //set as a child of the ui card
        UICard.transform.SetParent(panel.transform);
    }

    // Move this into Player class? As a drawHand function?

    public List<Card> generateHand(Deck deckToDrawFrom)
    {
        List<Card> hand = new List<Card>();

        for(int i = 0; i < 11; i++)
        {
            hand.Add(deckToDrawFrom.drawCard());

            //create a ui element and add it to the hand panel
            AddToPanel(CreateUIElement(hand[hand.Count - 1]) , handPanel);
        }

        return hand;
    }
/*
    public List<Player> createPlayers(int playerCount)
    {
      // create one human player
      Player humanPlayer = new Player()
    }

    /*
    public List<Card> getTurnList(List<Card> playerList, int turnCount){
      Debug.Log(turnCount);
      return playerList;
      // this function will provide a context based playerList based on turnCount

      List<Card> turnList = new List<Card>();
      for(int i = 0; i < playerList.size(); i++){
        turnList.add(playerList.get((i + turnCount) % 4)
      }
    }
    */
}
