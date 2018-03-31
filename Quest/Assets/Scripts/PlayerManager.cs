using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/* SyncVars: variables u want the server to change (they are sent back by the server back to the clients)
   * Hook: allows u to call a function when a sync var variable changes in value
 * Command: attribute that lets functions run on the server by sending a command to the server (function names need to start with Cmd)
 * Client: attribute for functions that are run only by clients (it is a redundancy of clientRpc) 
 * ClientRPC: attribute for functions that allows running client functions on a server
 * TargetRPC: same as clientRpc but can only be run on one client and not all ready clients
 */

public class PlayerManager : NetworkBehaviour
{
    [SyncVar]
    int networkId;

    [SyncVar]
    public string name;

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

    //each player probably has his own board to hold cards on 


    // Use this for initialization
    void Awake()
    {
        networkId = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) { return; }

        handCount = hand.Count;     
        playerRank = rankCard.name;
    }


    //give player id and info to create a player here
    public void SetupPlayer(int id)
    {
        //player's id for the server
        networkId = id;

        //all the attributes we need to setup a player
    }

    //player server commands here


}
