﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUIScript : MonoBehaviour , IPointerEnterHandler
{
    private string textureName { get; set; }
    private bool isVisible;
    private bool faceDown;
    private Button button;
    private PreviewCardScript previewButton;
    private GameController gameController;
    private Card myCard;
    public bool isHandCard;
   
    //public FoeCardScript FoeCard;

    // Use this for initialization
    void Awake()
    {
        textureName = "Textures/events/eventCard2";
        isVisible = true;
        faceDown = true;
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(Click);
        flipCard();

        // Store preview Card
        previewButton = GameObject.FindGameObjectWithTag("PreviewCard").GetComponent<PreviewCardScript>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hovered over a card");
        previewButton.textureName = textureName;
        previewButton.ChangeTexture();
    }

	// Update is called once per frame
	void Update()
    {
    }

    void Click()
    {
        // If we have a card from our hand 
        if (isHandCard)
        {
            // you can check gameState either here or gameController or both

            print("hello");
            gameController.selectedCard = myCard;

            //might need to get rid of the ui card 
            //Destroy(gameObject);
        }
    }

    public void ChangeVisibility()
    {
        isVisible = !isVisible;
        gameObject.SetActive(isVisible);
    }

    public void flipCard()
    {
        /*
        if (faceDown)
        {
            faceDown = false;
            button.image.overrideSprite = frontImage;
        }
        else
        {
            faceDown = true;
            button.image.overrideSprite = BackImage;
        }
        */
    }

    public void ChangeTexture()
    {
        button.image.overrideSprite = Resources.Load<Sprite>(textureName);
    }
}
