using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIInput
{
    // user prompts
    public BooleanPrompt booleanPrompt;
    public KeyboardInputPrompt keyboardPrompt;
    public CardPrompt cardPrompt;
    public CardPrompt discardPrompt;

    // enabling UI
    public bool UIEnabled;

    //UIPanel
    //GET RID OF THIS PART
    //public GameObject foregroundPanel;

    /*
    //for boolean user check
    public GameObject booleanPanel;
    public Text userMessage;
    public Button yesButton;
    public Button noButton;
    public string buttonResult;
    */

    /*
    //for actual input user check
    public GameObject inputPanel;
    public Text userMessage1;
    public InputField KeyboardInput;
    */

    /*
    //for ui card panel prompt
    public GameObject cardPanel;
    public Text userMessage2;
    public int totalBPCount;
    public Text totalBP;
    public GameObject chosenCardsPanel;
    public Button submitButton;
    public List<Card> selectedCards;
    public List<GameObject> UICardsSelected;
    */

    ////for ui discard panel prompt
    //public GameObject discardPanel;
    //public Text userDiscardMessage;
    //public GameObject chosenDiscardsPanel;
    //public Button discardSubmitButton;
    //public List<Card> discardSelectedCards;
    //public List<GameObject> UIDiscardsSelected;

    public void SetupUI()
    {
        // Initialize UI
        booleanPrompt.Setup();
        cardPrompt.Setup();
        discardPrompt.Setup();

        UIEnabled = false;

        /*
        selectedCards = new List<Card>();
        UICardsSelected = new List<GameObject>();
        discardSelectedCards = new List<Card>();
        UIDiscardsSelected = new List<GameObject>();

        // setup event listeners
        yesButton.onClick.AddListener(Yes);
        noButton.onClick.AddListener(No);
        submitButton.onClick.AddListener(Submit);
        discardSubmitButton.onClick.AddListener(SubmitDiscard);
        buttonResult = "";
        */    
    }

   // void SubmitDiscard() { doneDiscardingCards = true; }
    //void Yes() { buttonResult = "Yes"; }
    //void No() { buttonResult = "No"; }

    public void ActivateDiscardCheck(string userMsg)
    {
        //Activate the prompt
        discardPrompt.Activate(userMsg);

        /*
        discardPanelUIEnabled = true;
        doneDiscardingCards = false;

        discardPanel.SetActive(true);
        userDiscardMessage.text = userMsg;
        */
    }

    public void DeactivateDiscardPanel()
    {
        //Deactivate prompt
        discardPrompt.Deactivate();

        /*
        discardPanel.SetActive(false);
        doneDiscardingCards = false;
        discardPanelUIEnabled = false;

        discardSelectedCards.Clear();

        Debug.Log(UIDiscardsSelected.Count);

        //Destroy panel cards
        for (int i = 0; i < UIDiscardsSelected.Count; i++)
        {
            Object.Destroy(UIDiscardsSelected[i]);
        }

        UIDiscardsSelected.Clear();
        */
    }

    public bool CheckDiscardCard(Card card)
    {
        //Check card in discard panel
        return discardPrompt.CheckCard(card);

        /*
        for (int i = 0; i < discardSelectedCards.Count; i++)
        {
            if (card == discardSelectedCards[i])
            {
                RemoveFromDiscardPanel(i);
                return true;
            }
        }
        return false;
        */
    }

    //MIGHT NOT NEED THIS ANYMORE
    public void RemoveFromDiscardPanel(int index)
    {
        //remove from discard panel
        discardPrompt.RemoveCard(index);
        /*
        // if u change ur mind
        //remove the discard selected card
        discardSelectedCards.RemoveAt(index);
        Object.Destroy(UIDiscardsSelected[index]);
        UIDiscardsSelected.RemoveAt(index);
        */
    }

    public void AddToUIDiscardPanel(GameObject card)
    {
        //add card to discard panel
        discardPrompt.AddCard(card);


        //CardUIScript script = card.GetComponent<CardUIScript>();
        //script.isHandCard = true;

        //discardSelectedCards.Add(script.myCard);
        //UIDiscardsSelected.Add(card);

        //card.transform.SetParent(chosenDiscardsPanel.transform);
    }

    public bool CheckCard(Card card)
    {
        //check a card in the card panel
        return cardPrompt.CheckCard(card);
  
        //for (int i = 0; i < selectedCards.Count; i++)
        //{
        //    if (card == selectedCards[i])
        //    {
        //        return RemoveFromCardUIPanel(i);
        //    }
        //}
        //return null;
    }

    public void AddToUICardPanel(GameObject card)
    {
        //Add card to card panel
        cardPrompt.AddCard(card);

        //CardUIScript script = card.GetComponent<CardUIScript>();
        //script.isHandCard = true;

        //selectedCards.Add(script.myCard);
        //UICardsSelected.Add(card);

        //CalculateTotalBP();

        ////add card to panel
        //card.transform.SetParent(chosenCardsPanel.transform);
    }

    public void RemoveFromCardUIPanel(int index)
    {
        //add to card panel
        cardPrompt.RemoveCard(index);


        //selectedCards.RemoveAt(index);

        //GameObject result = UICardsSelected[index];
        //UICardsSelected.RemoveAt(index);

        //CalculateTotalBP();
        //return result;
    }

    // WHAT IS THIS FORRRR!!!!!!!!!
    //Destroy's the children of a given game object by finding said object by its tag.
    public void DestroyChildren(string gameObjectTag)
    {
        Transform uiHand = GameObject.FindGameObjectWithTag(gameObjectTag).transform;

        foreach (Transform child in uiHand)
            while (uiHand.transform.childCount > 1)
            {
                GameObject.Destroy(child.gameObject);
            }
    }

    //NOT NEEDED ANYMORE
    public void CalculateTotalBP()
    {
        //int total = 0;

        //strategyUtil util = new strategyUtil();

        //for (int i = 0; i < selectedCards.Count; i++)
        //{
        //    if (selectedCards[i].type == "Foe Card")
        //    {
        //        FoeCard foe = (FoeCard)selectedCards[i];

        //        if (QuestState.currentQuest != null)
        //        {
        //            total += util.getContextBP(foe, QuestState.currentQuest.foe);
        //        }
        //        else
        //        {
        //            total += foe.minBP;
        //        }
        //    }
        //    else if (selectedCards[i].type == "Weapon Card")
        //    {
        //        WeaponCard weapon = (WeaponCard)selectedCards[i];
        //        total += weapon.battlePoints;
        //    }
        //    else if (selectedCards[i].type == "Ally Card")
        //    {
        //        AllyCard ally = (AllyCard)selectedCards[i];
        //        total += ally.battlePoints;
        //    }
        //    else if (selectedCards[i].type == "Amour Card")
        //    {
        //        AmourCard amour = (AmourCard)selectedCards[i];
        //        total += amour.battlePoints;
        //    }
        //}

        //totalBP.text = "BP: " + total.ToString();
    }


    public void ActivateBooleanCheck(string userMsg)
    {
        UIEnabled = true;

        //Deactivate other panels
        keyboardPrompt.Deactivate();
        cardPrompt.Deactivate();

        //Activate boolean prompt
        booleanPrompt.Activate(userMsg);

        /*
        buttonResult = "";
        booleanUIEnabled = true;
        keyboardInputUIEnabled = false;
        cardPanelUIEnabled = false;
        doneAddingCards = false;

        //foregroundPanel.SetActive(true);
        booleanPanel.SetActive(true);
        cardPanel.SetActive(false);
        inputPanel.SetActive(false);
        userMessage.text = userMsg;
        */
    }

    public void ActivateUserInputCheck(string userMsg)
    {
        UIEnabled = true;

        //Deactivate other panels
        cardPrompt.Deactivate();
        booleanPrompt.Deactivate();

        //Activate keyboard prompt panel
        keyboardPrompt.Activate(userMsg);

        /*
        keyboardInputUIEnabled = true;
        booleanUIEnabled = false;
        cardPanelUIEnabled = false;
        doneAddingCards = false;

        //foregroundPanel.SetActive(true);
        inputPanel.SetActive(true);
        cardPanel.SetActive(false);
        booleanPanel.SetActive(false);
        KeyboardInput.text = "";
        userMessage1.text = userMsg;
        */
    }

    public void ActivateCardUIPanel(string userMsg)
    {
        UIEnabled = true;

        //Deactivate other panels
        booleanPrompt.Deactivate();
        keyboardPrompt.Deactivate();

        //Activate card panel
        cardPrompt.Activate(userMsg);



        /*
        cardPanelUIEnabled = true;
        keyboardInputUIEnabled = false;
        booleanUIEnabled = false;
        doneAddingCards = false;

        //foregroundPanel.SetActive(true);
        cardPanel.SetActive(true);
        inputPanel.SetActive(false);
        booleanPanel.SetActive(false);
        userMessage2.text = userMsg;
        totalBP.text = "BP: " + totalBPCount.ToString();
        */
    }

    public void DeactivateUI()
    {
        //Deativate all prompts
        booleanPrompt.Deactivate();
        keyboardPrompt.Deactivate();
        cardPrompt.Deactivate();
        discardPrompt.Deactivate();

        UIEnabled = false;
        /*
        //foregroundPanel.SetActive(false);
        cardPanel.SetActive(false);
        inputPanel.SetActive(false);
        booleanPanel.SetActive(false);
        UIEnabled = false;
        keyboardInputUIEnabled = false;
        booleanUIEnabled = false;
        cardPanelUIEnabled = false;
        doneAddingCards = false;
        buttonResult = "";
        totalBP.text = "";
        totalBPCount = 0;
        selectedCards.Clear();

        //Destroy panel cards
        for (int i = 0; i < UICardsSelected.Count; i++)
        {
            Object.Destroy(UICardsSelected[i]);
        }

        UICardsSelected.Clear();
        */    
    }
}
