# COMP 3004 Weekly Submission PDF

This is the weekly submission PDF for Group 14 in COMP 3004 (winter 18).

This PDF is to be handed in by 930:AM EST January 30th, 2018

Note: The feature numbers used correspond with those that can be found on our Trello here:

https://trello.com/invite/b/C37Sfr8e/69d55c2ae5222847d7b927bb87d5f605/comp3004

Note: This is an invite link for you to see our Trello board, if for some reason the link isn't working, please contact us.

# Statement About Team Issues

Either:

#a) This week our team had NO issues to report in terms of participation etc.

b) This week our team had the following issues to report in terms of participation:

# Completed Feature Table

Each of us is to complete a table showing the features (each feature should be COMPLETE and PLANNED) being worked on, with a unique ID (F#) and a short description.

For a COMPLETED feature, also include the time and date of the last commit in the repository corresponding to this feature.

Use the space below to enter a table under your name, .md tables can be made easily at this link: https://www.tablesgenerator.com/markdown_tables

Note: Did you finish a feature/are you working on a feature that isn't mapped out in Trello? MAP IT OUT IN TRELLO! These tables need to refer to specific Features and tests within those features that are being worked on.

***Ahmed Al-Zahrani (100900855):***

| #F1001 | Adventure Deck needs created upon game start                                 | T1           | commit a4ae77f (Jan 29th, 2018). |
|--------|------------------------------------------------------------------------------|--------------|----------------------------------|
| #F1005 | Story Deck needs to be created upon game start                               | T1           | commit a4ae77f (Jan 29th, 2018). |
| #F1006 | Rank Deck needs to be created upon game start                                | T1           | commit a4ae77f (Jan 29th, 2018). |
| #F1000 | Fully Implement the initial game condition including linking logic to the UI | Planned      | n/a                              |
| #F0001 | Implement the Card abstract superclass                                       | n/a          | commit a4ae77f (Jan 29th, 2018). |
| #F0007 | Implement the Rank Card class                                                | #F1006 -> T1 | commit a4ae77f (Jan 29th, 2018). |
| #F0010 | Implement Weapon Card class                                                  | #F1001 -> T1 | commit a4ae77f (Jan 29th, 2018). |

***Cheldon Mahon (101001843):***

***Mohamed Kazma (101019719):***

| #5002  | Setup the complete foundations of the UI of the game  |   | commit ee8943c |
|--------|-------------------------------------------------------|---|----------------|

***Julian Greppin (100899233):***

| #F5001 | Design card and implement spacing algorithm                          |           | commit 929ea28 |
|--------|------------------------------------------------------------------------------|--------------|----------------------------------|

## Tests Table

If a feature is complete, but not tested, it's not complete. Here we each have a table to show for each test we're working on, its unique ID (T#), a short description, and the list of features (completed IDs) relevent to the test. Also, the time and date of the last commit corresponding to this test.

Note: Finished a feature but it lacks testing? YOUR NEXT JOB IS TESTING!

Again, tables can be formatted here: https://www.tablesgenerator.com/markdown_tables

***Ahmed Al-Zahrani (100900855):***

- Although some completed features have been tested informally (IE: T1 for #F1001). For example, I can print out the created Adventure Deck on start of the game by clicking on the game object that corresponds to the adventure deck. However, I have e-mailed JP re: The C# alternative he would like us to use an alternative to JUnit.

Once I recieve an answer, hopefully for next week I can update this section with specific formal testing for the features I'm currently working on as well as the features completed this week.

***Cheldon Mahon (101001843):***

***Mohamed Kazma (101019719):***

Most of the UI has been tested by running the game with worst case scenearios for cards etc... in addition to hardcoding a
template that will be dynamically built in the future.

***Julian Greppin (100899233):***

# For Each Team Member:

***Ahmed Al-Zahrani (100900855):***

1. The Number of Hours spent on the Project since the lecture of (Lecture): 20-22

2. The IDs of Features I have completed this week?

Completed Features:

F0001 - Implement Card superclass
F0007 - Implement Rank Card class (including all sub tasks)
F0010 - Implement Weapon Card class (including all sub tasks)
F0002-01 - Implement Ally Card constructor / class file
F0003-01 - Implement Amour Card constructor / class file
F0004-01 - Implement Event Card constructor / class file
F0005-01 - Implement Foe Card constructor / class file
F0006-01 - Implement Quest Card constructor / class file
F0007-01 - Implement Rank Card constructor / class file
F0008-01 - Implement Test Card constructor / class file
F0009-01 - Implement Tournament Card constructor / class file
F0011-01 - Implement Deck constructor / class file
F1001 - Adventure Deck is properly created upon game start
F1005 - Story Deck is properly created upon game start
F1006 - Rank Deck is properly created upon game start

F*** - Uploaded new weekly update template to our repo
F*** - Fleshed out Trello board with relevant User stories / sub-stories pertinent to the logic work done this year


3. The IDs of the features you are planning to tackle from Jan 30 to Feb 6th?

Planned to tackle:

F1000 - Implement initial game condition (including all sub tasks) / dependent on UI team unblocking
          F1003 - Query player for # of players in game
          F1004 - Deal out hands corresponding to # of players
          F1007 - Merge initialized game state on logic side with initialized game UI

F0011 - Imlpement Deck object (including all sub tasks):
          F0011-02 - Ensure deck can be randomly drawn from
          F0011-03 - Ensure discard pile properly tracks discarded cards
          F0011-04 - Ensure deck can be re-shuffled from discard if it runs out
          F0011-05 - Ensure a specific card can be drawn from the deck by name

Planning:
  - Plan out the logic of a single user's turn and create relevent user stories on Trello to track this loop.

***Cheldon Mahon (101001843):***

1. The Number of Hours spent on the Project since the lecture of (Lecture):

2. The IDs of Features I have completed this week?

3. The IDs of the features you are planning to tackle from Jan 30 to Feb 6th?

***Mohamed Kazma (101019719):***

1. The Number of Hours spent on the Project since the lecture of (Lecture): 10-12

2. The IDs of Features I have completed this week?

Completed Features:

F5002 - Setup Default UI foundations
Worked with julian Greppin on the ui and card sorting

3. The IDs of the features you are planning to tackle from Jan 30 to Feb 6th?

F5005 - Setup active ally and weapon cards
F5007 - Setup preview cards 
F5008 - Setup added active temporary cards

***Julian Greppin (100899233):***

1. The Number of Hours spent on the Project since the lecture of (Lecture): 10~

2. The IDs of Features I have completed this week?
F5001
Worked with Mohamed Kazma on his completed features.

3. The IDs of the features you are planning to tackle from Jan 30 to Feb 6th?
