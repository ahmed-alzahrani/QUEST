using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Maybe we can have the description of a card where when we click a card it shows its description data on the screen 
public class GameController : MonoBehaviour
{
    //add number of shields to the player's object
    private int numPlayers;
    public Button button;
    public CardUIScript script;
    public Text playerTurn;
    public Text playerBP;
    public Text gameState;

	// Use this for initialization
	void Start ()
    {
        playerTurn.text = "Player 1";
        playerBP.text = "BP: 0";
        gameState.text = "Draw";

        //maybe ask for number of players 
        numPlayers = 3;

        //Setup decks and hands here in accordance 

        script = button.GetComponent<CardUIScript>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButton("Fire1"))
        {
            //script.flipCard();
            script.ChangeVisibility();
        }

        //if (Input.GetButton("Fire2")) script.flipCard();
    }
}
