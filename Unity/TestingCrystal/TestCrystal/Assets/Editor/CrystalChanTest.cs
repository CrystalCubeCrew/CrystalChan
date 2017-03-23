using UnityEngine;
using UnityEditor;
using NUnit.Framework;


[TestFixture]
public class CrystalChanTest {

    GameObject Crystal;
    Animator CrystalAnimator;
    CrystalChanPlayer CrystalScript;


    [SetUp]
    public void SetUp()
    {
        Crystal = GameObject.FindGameObjectWithTag("CrystalModel");
        CrystalScript = Crystal.GetComponent<CrystalChanPlayer>();
        CrystalAnimator = Crystal.GetComponent<Animator>(); //unity game animator

        CrystalScript.setAnimator(CrystalAnimator);

    }

	[Test]
	public void EditorTest() {
		//Arrange
		var gameObject = new GameObject();

		//Act
		//Try to rename the GameObject
		var newGameObjectName = "My game object";
		gameObject.name = newGameObjectName;

		//Assert
		//The object has a new name
		Assert.AreEqual(newGameObjectName, gameObject.name);
	}


    [Test]
    public void ifCrystalIsIdleNoOtherAnimationsShouldBePlaying()
    {
        CrystalScript.setIdleAction();

        Assert.False(CrystalAnimator.GetBool("isShrugging"));
        Assert.False(CrystalAnimator.GetBool("isCalculating"));
        Assert.False(CrystalAnimator.GetBool("isNews"));
        Assert.False(CrystalAnimator.GetBool("isDoing"));

    }

    [Test]
    public void ifSetAnimationIsNewsThenNewsAnimationIsPlayed()
    {
         CrystalScript.setAnimation("news");

        Assert.True(CrystalAnimator.GetBool("isNews"));
       
    }
}
