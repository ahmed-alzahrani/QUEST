using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Maybe we can have the description of a card where when we click a card it shows its description data on the screen
public class RankDeckController : MonoBehaviour
{
    public Button button;
    public GameObject gameControl;
    private GameController gameLogic;
    private Deck rankDeck;

	// Use this for initialization
	void Start ()
    {
      gameLogic = gameControl.GetComponent<GameController>();
      rankDeck = gameLogic.rankDeck;
      button = gameObject.GetComponent<Button>();
      button.onClick.AddListener(display);
	}

	// Update is called once per frame
	void Update ()
    {
    }
  public void display(){
    rankDeck.display();
  }
}
