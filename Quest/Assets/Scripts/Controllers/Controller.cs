using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public abstract class Controller : MonoBehaviour
{
    public deckBuilder decks;
    public Deck storyDeck;
    public Deck adventureDeck;
    public bool kingsRecognition;

    //turn setup
    public int turnCount;
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
    public GameObject weaponPanel;
    public GameObject amourPanel;
    public GameObject activatedPanel;

    // player panels UI names bp number of cards ranks etc...
    public List<GameObject> playerPanels;
    public List<GameObject> playerAllyPanels;
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

    public string[] cards;
    public string[] cardsStory;
    public int[] mordredControllerBeta;
    public int mordCurr;
}
