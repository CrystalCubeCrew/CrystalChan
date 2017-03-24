using UnityEngine;
using UnityEditor;
using NUnit.Framework;


[TestFixture]
public class CrystalChanTest
{

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
        CrystalScript.setAnimationStrategy("idle");
        

    }


    [Test]
    public void ifCrystalIsIdleNoOtherAnimationsShouldBePlayingButIdle()
    {

        CrystalScript.setIdleAction();

        Assert.True((CrystalScript.playerAnimator.GetType()).Equals(new IdleAnimation().GetType()));

    }

    [Test]
    public void ifSetAnimationIsNewsThenNewsAnimationIsPlayed()
    {

        CrystalScript.setAnimationStrategy("news");
        CrystalScript.playAnimation();

        Assert.True((CrystalScript.playerAnimator.GetType()).Equals(new NewsFeedAnimation().GetType()));
    }
    [Test]
    public void ifSetAnimationIstodoThenToDoAnimationIsPlayed()
    {
        CrystalScript.setAnimationStrategy("todo");
        CrystalScript.playAnimation();

        Assert.True((CrystalScript.playerAnimator.GetType()).Equals(new ToDoAnimation().GetType()));

    }

    [Test]
    public void ifSetAnimationIstodoIsThenStoppedThenIdleAnimationIsPlayedAndToDoIsNot()
    {
        CrystalScript.setAnimationStrategy("todo");
        CrystalScript.playAnimation();

        CrystalScript.stopAnimation();

        Assert.False((CrystalScript.playerAnimator.GetType()).Equals(new ToDoAnimation().GetType()));

    }

    [Test]
    public void ifSetAnimationIsWeatherIsThenStoppedThenIdleAnimationIsPlayedAndWeatherIsNot()
    {
        CrystalScript.setAnimationStrategy("weather");
        CrystalScript.playAnimation();

        CrystalScript.stopAnimation();

        Assert.False((CrystalScript.playerAnimator.GetType()).Equals(new WeatherAnimation().GetType()));

    }
}
