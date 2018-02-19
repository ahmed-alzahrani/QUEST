using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class TournamentTest{

  [Test]
	public void TournamentCard_CreatedWithGiven_WillHaveTheVariables() {

		var tournament = new TournamentCard("Tournament Card", "Tournament at Camelot", " ", 3);
		tournament.display();
		Assert.AreEqual("Tournament Card", tournament.type);
		Assert.AreEqual("Tournament at Camelot", tournament.name);
		Assert.AreEqual(" ", tournament.texturePath);
		Assert.AreEqual(3, tournament.shields);
    Assert.IsNotNull(tournament.tournament);

		// Use the Assert class to test conditions.
	}
}
