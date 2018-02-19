using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

public class TestTest{

  [Test]
	public void TestCard_CreatedWithGiven_WillHaveTheVariables() {

		var test = new TestCard("Test Card", "Test of Valor", " ", 3);
		test.display();
		Assert.AreEqual("Test Card", test.type);
		Assert.AreEqual("Test of Valor", test.name);
		Assert.AreEqual(" ", test.texturePath);
		Assert.AreEqual(3, test.minimum);

		// Use the Assert class to test conditions.
	}
}
