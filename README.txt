COMP 3004 QUEST Team #14
Ahmed Al-Zahrani 100900855
Mohamed Kazma 101019719
Julian Greppin 100899233


TO START THE GAME:

Go into quest/assets/scenes and open the MainMenu scene and then start using the arrow like button on the too then run a hotseat game.

TO TEST THE DIFFERENT SCENARIOS:

Within QUEST/Quest/Assets/Resources/TextAssets/Scenarios you will find a folder for each scenario. Each folder includes the following:
          - Scenariox_brief.txt which gives an overview of what the scenario proves and how it executes
          - ScenarioxStory.txt which is a txt file that stores the order in which we want to rig the Story deck
          - ScenarioxAdventure.txt which is a txt file that stores the order in which we want to rig the Adventure deck

          Note** Scenario 1 and 3 both also outline in their _brief file a way to play the scenario with 2 CPUS (1 of each strategy) and what playing the scenario with these CPU proves


Once you've understood the scenario, simply go into the Start function in gameController and look for these lines, comment out all except the one that pertains to the scenario you want to run

"STARTING FROM LINE ~ 463"
(THIS IS HOW THE CODE WILL LOOK LIKE THE DIFFERENCE WILL BE THE NAME OF THE SCENARIO)
ex:     //cards = File.ReadAllLines(Directory.GetCurrentDirectory() + "/Assets/Resources/TextAssets/Scenarios/Scenario2/Scenario2Adventure.txt");
        //cardsStory = File.ReadAllLines(Directory.GetCurrentDirectory() + "/Assets/Resources/TextAssets/Scenarios/Scenario2/Scenario2Story.txt");

Where the Strategy pattern is implemented?

1. QUEST/Quest/Assets/Scripts/Interfaces/iStrategy to implement the different strategies for players as well as different types of CPUS (IStrategyCPU1 and IStrategyCPU2 are
   the subclasses).

2. QUEST/Quest/Assets/Scripts/Interfaces/iStory to be able to treat the execution of any Story card be it event/tourney/quest in the same way within our game controller


Game Controls:

To Draw a Story Card click the story deck to draw a story card.
After drawing a story card there are generally 3 types of ui prompts:
the yes/no (boolean) check for players(participation or sponsoring etc...) those can be visually answered using the buttons
the keyboard userinput. That is almost entirely done in the game setup to get number of players and human ones as well as the strategies of each of the cpus.
the card panel user input. That is used for when a player needs to add a card so adding a card to a quest or tournament or discarding cards etc...
Allies always just get activated instead of being added to the prompt's card panel

Mordred:
first of all input for which player is playing mordred (1 , 2 , 3 , 4) (number of players)
second which player to activate mordred against (1 , 2 , 3 , 4) (number of players)
third which ally this player we want to remove (1 , 2 , 3 , 4 , ... 9)  // I HAVE NO IDEA IF THIS IS CORRECT OR NOT CHECK WITH JULIAN

GUI:

On the left, there is a bar of all the players information (Name , BP(rank + allies and in parenthesis what the player has active for the current quest or tournament) , Number of cards
in hand , player rank , number of shields). If you are the current player (its your turn), you will have an outline around your gui status information.

On the top of board, There is the quest information (The quest card in play or story card , The stage number if he is in a quest , and the quest cards if the stage is not over 
they are face down and the end of a stage they are face up until you select a new card then the next stage is started).

The middle of board, has the decks and discard piles (adventure deck , adventure deck discard pile , story deck , story deck discard pile , rank deck).

On the right there is the preview card, if you hover over cards, they will show up on the preview card to be able to read more about them.

On the bottom left, there is the shields and the rank card 

On the bottom, there is each player's hands  
