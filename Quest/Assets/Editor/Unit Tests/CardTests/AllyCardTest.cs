using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class AllyCardTest{

	[Test]
	public void getBattlePoints_returnsBattlePointsif_bpBuffisZero(){
		var ally = new AllyCard("Ally Card", "Ayrton", "", 5, 5, "Special", "quest", "ally", 0, 0, new NoBuff());
		Assert.AreEqual(ally.battlePoints, ally.getBattlePoints("quest", new List<Player>()));
	}

	[Test]
	public void getBattlePoints_returnsBattlePoints_Adds_allyEffectBuffOnQuest(){
		var ally = new AllyCard("Ally Card", "Ayrton", "", 5, 5, "Special", "quest", "ally", 1, 1, new BuffOnQuestEffect());
		Assert.AreEqual(6, ally.getBattlePoints("quest", new List<Player>()));
	}

	[Test]
	public void getBattlePoints_returnsBattlePoints_Adds_allyEffectBuffOnCardInPlay(){
		List<Player> players = new List<Player>();
		var ally1 = new AllyCard("Ally Card", "Ayrton", "", 5, 5, "Special", "", "ally", 1, 1, new BuffOnCardInPlayEffect());
		var ally2 = new AllyCard("Ally Card", "ally", "", 0, 0, "Special", "quest", "", 0, 0, new NoBuff());
		Player player1 = new Player("ahmed", new List<Card>(), new iStrategyPlayer());
		players.Add(player1);
		player1.activeAllies.Add(ally2);

		Assert.AreEqual(6, ally1.getBattlePoints("other quest", players));
	}

	[Test]
	public void getFreeBid_returnsBidif_bidBuffisZero(){
		var ally = new AllyCard("Ally Card", "Ayrton", "", 5, 5, "Special", "quest", "ally", 0, 0, new NoBuff());
		Assert.AreEqual(ally.freeBid, ally.getFreeBid("quest", new List<Player>()));

	}

	[Test]
	public void getFreeBid_returnsBidPoints_Adds_allyEffectBuffOnQuest(){
		var ally = new AllyCard("Ally Card", "Ayrton", "", 5, 5, "Special", "quest", "ally", 1, 1, new BuffOnQuestEffect());
		Assert.AreEqual(6, ally.getFreeBid("quest", new List<Player>()));

	}

	[Test]
	public void getFreeBid_returnsBidPoints_Adds_allyEffectBuffOnCardInPlay(){
		List<Player> players = new List<Player>();
		var ally1 = new AllyCard("Ally Card", "Ayrton", "", 5, 5, "Special", "", "ally", 1, 1, new BuffOnCardInPlayEffect());
		var ally2 = new AllyCard("Ally Card", "ally", "", 0, 0, "Special", "quest", "ally", 0, 0, new NoBuff());
		Player player1 = new Player("ahmed", new List<Card>(), new iStrategyPlayer());
		players.Add(player1);
		player1.activeAllies.Add(ally2);

		Assert.AreEqual(6, ally1.getFreeBid("other quest", players));
	}

	[Test]
	public void AllyCard_CreatedWithSpecial_ReturnsTrue(){
		var ally = new AllyCard("Ally Card", "Ayrton", " ", 10, 0, "Special", "Quest", "Ally", 0, 0, new NoBuff());
		Assert.IsTrue(ally.hasSpecial());
	}

	[Test]
	public void AllyCard_CreatedWithAllyEffect_ReturnsTrue()
	{
		var ally = new AllyCard("Ally Card", "Ayrton", " ", 10, 0, "Special", "Quest", "Ally", 0, 0, new NoBuff());
		Assert.IsNotNull(ally.allyEffect);
	}

	[Test]
	public void AllyCard_CreatedWithoutSpecial_ReturnsFalse(){
		var ally = new AllyCard("Ally Card", "Ayrton", " ", 10, 0, "", "Quest", "Ally", 0, 0, new NoBuff());
		Assert.IsFalse(ally.hasSpecial());
	}

	[Test]
	public void AllyCard_CreatedWithGiven_WillHaveTheVariables() {

		var ally = new AllyCard("Ally Card", "Ayrton", " ", 10, 2, "Special", "Quest", "Ally", 0, 0, new NoBuff());
		ally.display();
		Assert.AreEqual("Ally Card", ally.type);
		Assert.AreEqual("Ayrton", ally.name);
		Assert.AreEqual(" ", ally.texturePath);
		Assert.AreEqual(10, ally.battlePoints);
		Assert.AreEqual("Special", ally.special);
		Assert.AreEqual(2, ally.freeBid);
		Assert.AreEqual("Special", ally.special);
		Assert.AreEqual("Quest", ally.specialQuest);
		Assert.AreEqual("Ally", ally. specialAlly);
		Assert.AreEqual(0, ally.bidBuff);
		Assert.AreEqual(0, ally.bpBuff);
		// Use the Assert class to test conditions.
	}
}
