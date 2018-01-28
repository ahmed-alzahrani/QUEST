using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUIScript : MonoBehaviour
{
    public Sprite frontImage;
    public Sprite BackImage;

    private bool isVisible;
    private bool faceDown;
    private Button button;
   
    //public FoeCardScript FoeCard;

    // Use this for initialization
    void Awake()
    {
       // Debug.Log("spam");
        isVisible = true;
        faceDown = true;
        //FoeCard = new FoeCardScript("foe", "foeeee", 10, 20);
        button = gameObject.GetComponent<Button>();
        //button.onClick.AddListener(FoeCard.display)
        flipCard();
        //ChangeVisibility();
        
    }
	
	// Update is called once per frame
	void Update()
    {
        // Debug.Log("mega spam");
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
    }
}
