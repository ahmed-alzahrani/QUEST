using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class WeaponsTest{

  [Test]
	public void WeaponCard_CreatedWithGiven_WillHaveTheVariables() {

		var weapon = new WeaponCard("Weapon Card", "Horse", " ", 10);
		weapon.display();
		Assert.AreEqual("Weapon Card", weapon.type);
		Assert.AreEqual("Horse", weapon.name);
		Assert.AreEqual(" ", weapon.texturePath);
		Assert.AreEqual(10, weapon.battlePoints);

		// Use the Assert class to test conditions.
	}
}
