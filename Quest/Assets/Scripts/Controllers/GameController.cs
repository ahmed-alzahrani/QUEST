using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//MAYBE ADD AN EVENT HANDLER FOR WHEN WE NEED TO DRAW FROM A CERTAIN DECK ADD A CLICK HANDLER FOR IT AND REMOVE IT AFTERWARDS 
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
    public string buttonResult;

    //for actual input user check
    public GameObject inputPanel;
    public Text userMessage1;
    public InputField KeyboardInput;

    //for ui card panel prompt 
    public GameObject cardPanel;
    public Text userMessage2;
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
    }

    public GameObject CheckCard(Card card)
    {
        for (int i = 0; i < selectedCards.Count; i++)
        {
            //is this gonna check pointers or what exactlyyyyyy??
            //Maybe duplicates are a problem
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
        selectedCards.Clear();
        UICardsSelected.Clear();
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

    public Button adventureDeckUIButton;
    public Button adventureDeckDiscardPileUIButton; //to change image here for discard pile
    public Button storyDeckUIButton;
    public Button storyDeckDiscardPileUIButton; //to change image here for discard pile
    public Button rankDeckUIButton;

    public Card selectedCard;
    public Card drawnAdventureCard;
    public Card drawnStoryCard;

    // Use this for initialization
    void Start ()
    {
        //Deck building
        decks = new deckBuilder();
        storyDeck = decks.buildStoryDeck();
        adventureDeck = decks.buildAdventureDeck();

        //Setup UI buttons for cards (event listeners etc....)
        adventureDeckUIButton.onClick.AddListener(DrawFromAdventureDeck);
        storyDeckUIButton.onClick.AddListener(DrawFromStoryDeck);

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
        //userInput.ActivateUserInputCheck("How many players are playing the game??");
        //userInput.ActivateBooleanCheck("How many players are playing the game??");
        userInput.ActivateCardUIPanel("What Cards do you want to add?");

        //Check selected card within the UI update check 

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
            if (drawnAdventureCard != null)
            {
                //drawn card here
                AddToPanel(CreateUIElement(drawnAdventureCard) , handPanel);
                drawnAdventureCard = null;
            }

            if (drawnStoryCard != null)
            {
                //drawn card here
                AddToPanel(CreateUIElement(drawnStoryCard), handPanel);
                drawnStoryCard = null;
            }

            if (selectedCard != null)
            {
                //selected cards stuff
                print(selectedCard);
                selectedCard = null;
            }
        }
        else
        {
            //check user input here!!!!
            //if user is done
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
            else if (userInput.cardPanelUIEnabled)
            {
                //card panel enabled
                if (selectedCard != null)
                {
                    //error card detection should occur here!!!!!
                    GameObject removedCard = userInput.CheckCard(selectedCard);

                    if (removedCard != null)
                    {
                        //we want to remove this card from ui panel and back to player's hand
                        Destroy(removedCard);
                        AddToPanel(CreateUIElement(selectedCard), handPanel);
                    }
                    else
                    {
                        //add it to ui panel
                        userInput.AddToUICardPanel(CreateUIElement(selectedCard));
                    }

                    selectedCard = null;
                }

                if (userInput.doneAddingCards)
                {
                    //When selection is done do whatever here !!!!!!
                    //do stuff with cards
                    Debug.Log("NUMBER OF CARDS IS: " + userInput.selectedCards.Count);

                    for (int i = 0; i < userInput.selectedCards.Count; i++)
                    {
                        Debug.Log(userInput.selectedCards[i].name);
                    }

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

    public void DrawFromAdventureDeck()
    {
        //Animations can be added here
        //check gamestate before drawing
        drawnAdventureCard = adventureDeck.drawCard();
    }

    public void DrawFromStoryDeck()
    {
        //Animations can be added here
        //check gamestate before drawing
        drawnStoryCard = storyDeck.drawCard();
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
