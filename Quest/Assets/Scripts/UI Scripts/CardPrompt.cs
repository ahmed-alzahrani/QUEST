using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//REMOVING FROM PANELSSSS!!!!!!!!!

[System.Serializable]
public class CardPrompt
{
    //for UI card panel prompt
    public GameObject cardPanel;
    public GameObject chosenCardsPanel;
    public Text userMessage;
    public Text totalBP;
    public Button submitButton;
    public List<Card> selectedCards;
    public List<GameObject> UICardsSelected;

    public bool isActive;
    public bool doneAddingCards;

    public void Setup()
    {
        submitButton.onClick.AddListener(() => doneAddingCards = true);
        selectedCards = new List<Card>();
        UICardsSelected = new List<GameObject>();
        isActive = false;
        doneAddingCards = false;
    }

    void Submit() { doneAddingCards = true; }

    public void Activate(string message)
    {
        cardPanel.SetActive(true);
        userMessage.text = message;
        isActive = true;
        totalBP.text = "BP: " + 0;
    }

    public void Deactivate()
    {
        cardPanel.SetActive(false);
        userMessage.text = "";
        isActive = false;
        doneAddingCards = false;
        selectedCards.Clear();
        totalBP.text = "";
        
        /////

        for (int i = 0; i < UICardsSelected.Count; i++)
        {
            Object.Destroy(UICardsSelected[i]);
        }

        UICardsSelected.Clear();
    }

    //Check card for removal (true if we removed a card false otherwise)
    public bool CheckCard(Card card)
    {
        for (int i = 0; i < selectedCards.Count; i++)
        {
            if (card == selectedCards[i])
            {
                RemoveCard(i);
                return true;
            }
        }
        return false;
    }

    //Add a card to the panel
    public void AddCard(GameObject card)
    {
        CardUIScript script = card.GetComponent<CardUIScript>();
        script.isHandCard = true;

        selectedCards.Add(script.myCard);
        UICardsSelected.Add(card);

        CalculateTotalBP();

        card.transform.SetParent(chosenCardsPanel.transform);
    }

    //Remove card from panel
    public void RemoveCard(int i)
    {
        selectedCards.RemoveAt(i);
        Object.Destroy(UICardsSelected[i]);
        UICardsSelected.RemoveAt(i);

        CalculateTotalBP();
    }

    //NEEDS CHANGESSSSSSSSSSSS!!!!!!!!!!!!!!
    public void CalculateTotalBP()
    {
        int total = 0;

        strategyUtil util = new strategyUtil();

        for (int i = 0; i < selectedCards.Count; i++)
        {
            if (selectedCards[i].type == "Foe Card")
            {
                FoeCard foe = (FoeCard)selectedCards[i];

                if (QuestState.currentQuest != null)
                {
                    total += util.getContextBP(foe, QuestState.currentQuest.foe);
                }
                else
                {
                    total += foe.minBP;
                }
            }
            else if (selectedCards[i].type == "Weapon Card")
            {
                WeaponCard weapon = (WeaponCard)selectedCards[i];
                total += weapon.battlePoints;
            }
            else if (selectedCards[i].type == "Ally Card")
            {
                AllyCard ally = (AllyCard)selectedCards[i];
                total += ally.battlePoints;
            }
            else if (selectedCards[i].type == "Amour Card")
            {
                AmourCard amour = (AmourCard)selectedCards[i];
                total += amour.battlePoints;
            }
        }

        totalBP.text = "BP: " + total.ToString();
    }

}
