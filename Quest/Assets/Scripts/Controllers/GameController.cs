using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public UIInput userInput;  //checking for user input

    // Use this for initialization
    void Start ()
    {
        //Deck building
        decks = new deckBuilder();
        storyDeck = decks.buildStoryDeck();
        adventureDeck = decks.buildAdventureDeck();

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

    // Move this into Player class? As a drawHand function?

    public List<Card> generateHand(Deck deckToDrawFrom)
    {
        List<Card> hand = new List<Card>();

        for(int i = 0; i < 11; i++)
        {
            hand.Add(deckToDrawFrom.drawCard());
        }

        return hand;

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
