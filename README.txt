COMP 3004 QUEST Team #14
Ahmed Al-Zahrani 100900855
Mohamed Kazma 101019719
Julian Greppin 100899233

Foreword:
"./" refers to the current directory in which this README.txt file is located.
In order to run this game and produce proper Log files, the game must be run from within
Unity itself.

How to start the game:
Navigate to the directory: ./Quest/Assets/Scenes and open the MainMenu.unity file with Unity.
From within unity, press the "Play" button, located at the top-middle of the window next to the "Pause" button.
This will launch the game in the "Game" tab of unity, you will be presented with our Main Menu.
A good way to know for certain that the game has begun is that the animated fire effect to the
left of the menu will be active.
Press the Hot-Seat Game button on the main menu to begin the game.

How to test the different Scenarios:

Within ./Quest/Assets/Resources/TextAssets/Scenarios you will find a folder for each scenario. Each folder includes the following:
          - Scenariox_brief.txt which gives an overview of what the scenario proves and how it executes
          - ScenarioxStory.txt which is a txt file that stores the order in which we want to rig the Story deck
          - ScenarioxAdventure.txt which is a txt file that stores the order in which we want to rig the Adventure deck

          Note** Scenario 1 and 3 both also outline in their _brief file a way to play the scenario with 2 CPUS (1 of each strategy) and what playing the scenario with these CPU proves


To change which Scenario the game is currently running, please uncomment the corresponding code
snippet in GameController.cs and comment out the scenario you no longer wish to use.



Located at approximately line 463 of GameController.CS
The following is a snippet of what this section's code looks like:
    //cards = File.ReadAllLines(Directory.GetCurrentDirectory() + "/Assets/Resources/TextAssets/Scenarios/Scenario2/Scenario2Adventure.txt");
     //cardsStory = File.ReadAllLines(Directory.GetCurrentDirectory() + "/Assets/Resources/TextAssets/Scenarios/Scenario2/Scenario2Story.txt");

Where the Strategy pattern is implemented?

1. QUEST/Quest/Assets/Scripts/Interfaces/iStrategy  Is the working directory for our player strategies, human players as well as our fully functioning AI player both use this strategy pattern.

2. QUEST/Quest/Assets/Scripts/Interfaces/iStory Is the working directory for our Story Card interfaces which also use a strategy pattern so that the game can interface with a story card regardless of its type.


Game Controls:

To Draw a Story Card click the story deck in the center of the board, marked with the letter S.
Once a story card has been drawn, depending on the card, you may receive one of the following prompts:

-The yes/no prompt used for polling the players for participating/sponsoring/etc..

-The keyboard prompt which describes the input required of the player, this
 is used to set up the game as well as during during Quests when bidding on Test Stages.

-The Card prompt which prompts the player to select cards from their hand to add to the panel.
 This is used throughout the game and is how the player plays cards.

Ally Cards will not enter the Card prompt panel and will instead be played onto the field.

How to use Mordred:
First, press the number key associated with the player who is playing Mordred.
Second, press the number key associated with the player who's Ally Card you wish to destroy.
Third, press the number key associated with the Ally Card you wish to destroy.

For example:
Player 2 has played Merlin and Player 1 wishes to use his Mordred card to remove it.
Player 1 presses the numbers 1 2 and 1 on the keyboard.
This removes the Mordred Card from Player 1's hand and removes Player 2's Merlin Ally Card from play.

This can be done at any point of the game in compliance with the rules.
If any of the 3 inputs required are invalid, the input is rejected and you must begin entering the 3 inputs again.

GUI:

On the left of the screen there are several boxes which display the players' information:
(ie. Name , BP** , Number of cards in hand , player rank , number of shields).
**The bracketed number next to BP is what the player has active for the current quest or tournament (ie. Allies or Amours)
The current active player has a distinct green outline around their green border

At the top of the board is the quest information:
-The Story Card in play (including Quests)
-The current Stage's stage number
-The quest cards of the stage, once the stage is over, these cards are flipped face up for the players to see.
-
Located in the middle of the board are the decks and discard piles 
In order from left to right, they are:
Adventure Deck, Adventure Discard, Story Deck, Story Discard, and the Rank Deck

At the right of the screen is the Card Preview Area. When you wish to get a closer look at a given card, 
hover over it with the cursor and it will be displayed in this area.

At the bottom of the screen is the current player's hand

Located to the left of the current player's hand is their current rank and number of shields.
