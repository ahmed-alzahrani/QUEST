# Should I participate in this Tournament?
This CPU will participate in this Tournament if it is a Tournament at Camelot (bonus shields is 3).

# What should I play in a Tournament?
This CPU will play their strongest valid combination.

# Should I sponsor this Quest?
This CPU will sponsor the quest if EITHER of the following two conditions are met:

Condition1: Condition 1 returns true if both of its following sub-conditions are met:
Condition1a: I have valid cards in my hand to play this quest (enough foes, -1 if there's a test).
Condition1b: More than one players can rank up from this quest.

Condition2: The CPU has 6 or more Foes in their hand and can set up a valid quest. (Needs to flush foes from his hand)

# How do I setup a Quest?

Final Foe Stage: This CPU sets up the final stage to be the strongest possible Foe Encounter possible.

Second Last Stage: This CPU will set up the second last stage to be a test stage if possible.

Earlier Foe Encounters: All other foe encounters, for stages 1 -> x-1 or 1 -> x-2 where x is the number of stages in the quest and depending on whether or not a test stage was possible. This CPU sets up the previous stages in a backward order to be just their strongest foe at each stage.

# Should I participate in this Quest?
This CPU will ALWAYS elect to participate in a Quest.

# How do I play a final foe stage?
This CPU will play its strongest possible combination of cards.

# How do I play a test stage?
This CPU will only bid one time, and this is what he will bid from his hand:

- Test Cards
- Duplicate Weapons
- Any Foe with 30 or less BP

# How do I play an earlier foe stage?
This CPU plays ALL allies from their hand, an Amour if possible, and their weakest weapon.
