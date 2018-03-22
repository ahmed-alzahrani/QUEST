﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUIScript : MonoBehaviour , IPointerEnterHandler
{
    //private string textureName { get; set; }
    private bool isVisible;
    private bool faceDown;
    private Button button;
    private PreviewCardScript previewButton;
    public Controller gameController;
    public Card myCard;
    public bool isHandCard;
   
    // Use this for initialization
    void Awake()
    {
        isVisible = true;
        faceDown = false;
        isHandCard = false;
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(Click);
        //flipCard();

        // Store preview Card
        previewButton = GameObject.FindGameObjectWithTag("PreviewCard").GetComponent<PreviewCardScript>();

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if (gameController == null)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<NetworkController>();
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Hovered over a card");
        if (myCard != null && myCard.texturePath != "")
        { 
            previewButton.textureName = myCard.texturePath;
        }
        previewButton.ChangeTexture();
    }

	// Update is called once per frame
	void Update()
    {
    }

    void Click()
    {
        // If we have a card from our hand 
        // this might change
        //maybe check for certain cards like mordred 
        if (isHandCard && (gameController.userInput.cardPrompt.isActive || gameController.userInput.discardPrompt.isActive))
        {
            // you can check gameState either here or gameController or both
            // add selected card then destroy the UI gameObject
            gameController.selectedCard = myCard;
            Destroy(gameObject);
        }
    }

    public void ChangeVisibility()
    {
        isVisible = !isVisible;
        gameObject.SetActive(isVisible);
    }

    public void flipCard()
    {
            //Debug.Log("flipped card");
            button.image.overrideSprite = Resources.Load<Sprite>("Textures/Backings/a_backing");
    }

    public void ChangeTexture()
    {
        //Debug.Log(myCard.texturePath);
        button.image.overrideSprite = Resources.Load<Sprite>(myCard.texturePath);
    }
}
