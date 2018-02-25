using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Maybe we can have the description of a card where when we click a card it shows its description data on the screen
public class StoryDeckController : MonoBehaviour
{
    public Button button;
    public GameObject gameControl;
    private GameController gameLogic;
    private Deck storyDeck;

	// Use this for initialization
	void Start ()
    {
      gameLogic = gameControl.GetComponent<GameController>();
      storyDeck = gameLogic.storyDeck;
      button = gameObject.GetComponent<Button>();
      button.onClick.AddListener(display);
	}

	// Update is called once per frame
	void Update ()
    {
    }
  public void display(){
  }
}
