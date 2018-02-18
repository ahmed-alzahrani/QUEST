# COMP 3004 Weekly Submission PDF

This is the weekly submission PDF for Group 14 in COMP 3004 (winter 18).

Note: The feature numbers used correspond with those that can be found on our Trello here:

https://trello.com/invite/b/C37Sfr8e/69d55c2ae5222847d7b927bb87d5f605/comp

Note: This is an invite link for you to see our Trello board, if for some reason the link isn't working,
please contact us.

# Statement About Team Issues

Either:

# a) This week our team had NO issues to report

# in terms of participation etc.

b) This week our team had the following issues to report in terms of participation:

# Completed Feature Table

Each of us is to complete a table showing the features (each feature should be COMPLETE and
PLANNED) being worked on, with a unique ID (F#) and a short description.

For a COMPLETED feature, also include the time and date of the last commit in the repository
corresponding to this feature.

Use the space below to enter a table under your name, .md tables can be made easily at this link:
https://www.tablesgenerator.com/markdown_tables

Note: Did you finish a feature/are you working on a feature that isn't mapped out in Trello? MAP IT
OUT IN TRELLO! These tables need to refer to specific Features and tests within those features that
are being worked on.

Ahmed Al-Zahrani (100900855):

| #F | Watched NUnit tutorials / did some NUnit excercises to understand unit testing in C#                                                                                                                                                                            | N/A | N/A                              |
|----|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-----|----------------------------------|
| #F | Planned out scenarios to be tested in demo version of game                                                                                                                                                                                                      | N/A | N/A                              |
| #F | Implemented beta version of CPU strategy 1                                                                                                                                                                                                                      | N/A | commit 533c825 (Feb 15th, 2018). |
| #F | Implemented beta version of CPU strategy 2                                                                                                                                                                                                                      | N/A | commit 533c825 (Feb 15th, 2018). |
| #F | Implemented Event behaviors                                                                                                                                                                                                                                     | N/A | commit 500037c (Feb 13th, 2018). |
| #F | Tweaked the architecture of the game, Players now have as a member variable a reference to the game they are in, this allows not only players to elegantly draw from the decks but will assist in the implementation of quest context and field context effects | N/A | commit 500037c (Feb 13th, 2018). |
| #F | Added unique texture paths to each card in deckBuilder                                                                                                                                                                                                          | N/A | commit 500037c(Feb 13th, 2018)   |
| #F | Tweaked Player behavior to include checks for rank changes when shields are awarded / taken away, drawing from the adventure deck as well as the CPU strategy for resolving scenario where the player has drawn <12 cards                                       | N/A | commit 533c825 (Feb 15th, 2018). |

Cheldon Mahon (101001843): Tournament logic framework
Mohamed Kazma (101019719):
| #5013  | Setup UI prefab instantiation                                      |   |   |
|--------|--------------------------------------------------------------------|---|---|
| #5014  | Setup the prefab cards                                             |   |   |
| #5016  | Setup texture names of all cards and load the textures dynamically |   |   |
| #5017  | Setup active and non active cards                                  |   |   |
| #5018  | Implement adding a card to a quest or tournament                   |   |   |
| #5019  | Creating UI user prompts                                           |   |   |
| #5020  | create boolean prompts                                             |   |   |
| #5021  | create input prompts                                               |   |   |
| #5022  | create panel prompts                                               |   |   |

Julian Greppin (100899233): Set up quest logic and human interaction with scripting framework, currently not ready to be implemented.
							Worked with Mohamed Kazma on UI design and set up.
							Spent some time working with Cheldon Mahon on Tournament logic framework


## Tests Table

If a feature is complete, but not tested, it's not complete. Here we each have a table to show for
each test we're working on, its unique ID (T#), a short description, and the list of features
(completed IDs) relevent to the test. Also, the time and date of the last commit corresponding to
this test.

Note: Finished a feature but it lacks testing? YOUR NEXT JOB IS TESTING!

Again, tables can be formatted here: https://www.tablesgenerator.com/markdown_tables

Ahmed Al-Zahrani (100900855):
Cheldon Mahon (101001843): No tournament testing yet. Tournaments aren't finished
Mohamed Kazma (101019719):
UI testing can be checked using logs and visual result 

Julian Greppin (100899233):

# For Each Team Member:

Ahmed Al-Zahrani (100900855):

1.The Number of Hours spent on the Project since the lecture of (Lecture): 25
2.The IDs of Features I have completed this week:

Can be found within the feature table above.

3.The IDs of the features you are planning to tackle from Feb16th to March 1st?

Having spent a good chunk of this week learning about writing Unit tests with NUnit, starting on Saturday my primary focus will be to begin writing comprehensive unit tests for the
C# scripts we have. Because we have (untested, beta versions of) the Event behaviors, CPU Strategies, are some minor refactoring away from the player Strategy (between work done by Mohamed and Julian) and Julian/Cheldon are respectively working on the Quest/Tournament behavior, a lot of the logic of the game (again, untested and beta) is in place.

This means our focus across the reading week is following:

1. Writing comprehensive tests to ensure integrity of the C# script logic we've already Implemented (My primary focus starting this weekend)
2. Integrate fully the C# logic with the UI elements to deliver the full experience in hot-seat play
3. Integrate the additional Quest and Card in Field context sensitive ally abilities (Working with Mo Kazma on this)
4. Assisting Julian and Cheldon with any issues they may have regarding the logic for Quest/Tournament behavior respectively
5. Preparing our rigged demo version of the game, ensuring proper logging and that all scenarios we wanna show are covered (team concern.)

So that entails 3 of the 5 major points we have left to cover that I will have a hand in, as already mentioned starting tomorrow (Saturday the 17th) my primary focus will be unit testing.


Cheldon Mahon (101001843):
1.The Number of Hours spent on the Project since the lecture of (Lecture): 16
2.The IDs of Features I have completed this week? None
3.The IDs of the features you are planning to tackle from Feb 16th to Mar 1?
	To finish the tournament logic that I started last week.


Mohamed Kazma (101019719):
1.The Number of Hours spent on the Project since the lecture of (Lecture): 20
2.The IDs of Features I have completed this week?
#5013 -> #5022

3.The IDs of the features you are planning to tackle in the reading week?
i am going to be helping out more with the code logic especially the quest logic with some UI integration 


Julian Greppin (100899233):
1.The Number of Hours spent on the Project since the lecture of (Lecture): 12
2.The IDs of Features I have completed this week? Progress on #1005 & #1013
3.The IDs of the features you are planning to tackle from Feb 16th to Mar 1?
	I plan on acting as the intermediary the scripting portion of the project and the Unity user interface, interconnecting the two and completing related features required for the game to begin playing with human interaction.