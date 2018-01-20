using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Maybe we can have the description of a card where when we click a card it shows its description data on the screen 
public class GameController : MonoBehaviour
{
    public Button button;
    public CardUIScript script;

	// Use this for initialization
	void Start ()
    {
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
