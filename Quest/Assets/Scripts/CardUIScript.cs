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

    // Use this for initialization
    void Awake()
    {
        isVisible = true;
        faceDown = true;
        button = gameObject.GetComponent<Button>();
        //button.onClick.AddListener(Click);
        flipCard();
        //ChangeVisibility();
    }
	
	// Update is called once per frame
	void Update()
    {
        if (Input.GetButton("Fire2")) flipCard();
    }

    void Click()
    {
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
