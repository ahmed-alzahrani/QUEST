using System.Collections;
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
   
    //public FoeCardScript FoeCard;

    // Use this for initialization
    void Awake()
    {
        textureName = "Textures/events/eventCard2";
        isVisible = true;
        faceDown = true;
        button = gameObject.GetComponent<Button>();
        //button.onClick.AddListener(FoeCard.display);
        flipCard();

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
        if (Input.GetButton("Fire2")) flipCard();
    }

    void Click()
    {
        print(name);
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
}
