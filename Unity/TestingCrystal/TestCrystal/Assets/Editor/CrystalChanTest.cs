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

        Assert.True((CrystalScript.playerAnimator.GetType()).Equals(new NewsFeedAnimation().GetType()));
    }
    [Test]
    public void ifSetAnimationIstodoThenToDoAnimationIsPlayed()
    {
        CrystalScript.setAnimationStrategy("todo");

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


    //parse intent method tests:

    [Test]
    public void jsonContainsWeatherThenOutputIsWeather()
    {
        string json = "Weather Intent";

        Assert.AreEqual("weather", CrystalScript.determineAction(json, null) );
    }

    [Test]
    public void jsonContainsToDoThenOutputIsToDo()
    {
        string json = "todo animation";

        Assert.AreEqual("todo", CrystalScript.determineAction(json, null));
    }

    [Test]
    public void jsonDoesntContainAnyCommandsThenOutputIsShrugged()
    {
        string json = "bleh animation";

        Assert.AreEqual("shrug", CrystalScript.determineAction(json,null));

    }

    [Test]
    public void jsonNotRecognizedAsStringAndIsNULLThenOutputIsShrugged()
    {
        string json = null;

        Assert.AreEqual("shrug", CrystalScript.determineAction(json,null));
    }


    //isString method tests

    [Test]
    public void ifObjectCanBeconvertedToStringThenReturnTrue()
    {
        object obj = "i am a string";

        Assert.True(CrystalScript.isString(obj));
    }

    [Test]
    public void ifObjectIsNullIsStringThenReturnFalse()
    {
        object obj = null;

        Assert.False(CrystalScript.isString(obj));
    }

    [Test]
    public void ifObjectCannotBeConvertedToStringThenReturnFalse()
    {
        object obj = 1;

        Assert.False(CrystalScript.isString(obj));
    }


}
