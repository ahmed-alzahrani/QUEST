using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class PlayerManager : NetworkBehaviour
{
    [SyncVar]
    int networkId;

    private string playerName;

    [SyncVar]
    public int score;

    public List<AllyCard> activeAllies;

    [SyncVar] 
    public int handCount;

    [SyncVar]
    public int BP;

    [SyncVar]
    public string playerRank;

    public RankCard rankCard;
    public List<RankCard> rankCards { get; set; }
    public List<Card> hand { get; set; }
    public iStrategy strategy { get; set; }
    public int knightScore;
    public int champKnightScore;
    public int kotrkScore;
    public bool participating = false;
    public bool sponsoring = false;
    public string shieldPath;
    public Controller gameController;


    //UI information of each player
    public CardUIScript shieldsCard;
    public CardUIScript rankUICard;
    public Text UIShieldNum;
    public GameObject handPanel;
    public GameObject questPanel;
    public GameObject questStagePanel;
    public GameObject weaponPanel;
    public GameObject amourPanel;
    public GameObject activatedPanel;
    public GameObject cheatPanel;

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

    public List<GameObject> playerUICards;  //its gonna have almost entirely ui features 

    public NetworkClient client;

    // Use this for initialization
    void Start()
    {
        networkId = 0;

        // no idea testing out some shit
    }

    public override void OnStartServer()
    {
        connectionToClient.RegisterHandler(MsgType.Highest, HandleServerRequest);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return; }

        //send a message to the server
        //SendMessage("");

        //handCount = hand.Count;     
        //playerRank = rankCard.name;
        if (!isServer)
        {
            //SendMessageToServer("");
        }
        else
        {
            //Debug.Log("server attempting to send stuff");
        }
    }


    //give player an id and info to create a player

    public void SetupPlayer(int id)
    {
        //player's id for the server
        networkId = id;

        //all the attributes we need to setup a player

    }

    //sending a message from client to server (this will change)
    public void SendMessageToServer(string msg)
    {
        ClientMessage message = new ClientMessage();

        message.message = "hows it going bruh";

        if (connectionToServer != null)
        {
            connectionToServer.Send(MsgType.Highest, message);
            Debug.Log("CLIENT sent message to SERVER");
        }
        else
        {
            Debug.Log("we have no connection to the server!!!!");
        }
    }

    //player server commands here

    public void HandleServerRequest(NetworkMessage message)
    {
        Debug.Log("Message from Server!!");

        //we are receiving server messages
        var msg = message.ReadMessage<ServerMessage>();

        //print out the message
        Debug.Log(msg.message);
    }
}
