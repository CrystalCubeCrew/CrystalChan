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

        CrystalScript.setAnimationStrategy("news");
        CrystalScript.playAnimation();
        Assert.True(CrystalAnimator.GetBool("isNews"));
    }
    [Test]
    public void ifSetAnimationIstodoThenToDoAnimationIsPlayed()
    {
        CrystalScript.setAnimationStrategy("todo");
        CrystalScript.playAnimation();

        Assert.False(CrystalAnimator.GetBool("isIdle"));
        Assert.True(CrystalAnimator.GetBool("isDoing"));

    }

    [Test]
    public void ifSetAnimationIstodoIsThenStoppedThenIdleAnimationIsPlayedAndToDoIsNot()
    {
        CrystalScript.setAnimationStrategy("todo");
        CrystalScript.playAnimation();
        CrystalScript.stopAnimation();


        Assert.True(CrystalAnimator.GetBool("isIdle"));
        Assert.False(CrystalAnimator.GetBool("isDoing"));

    }

    [Test]
    public void ifSetAnimationIsWeatherIsThenStoppedThenIdleAnimationIsPlayedAndWeatherIsNot()
    {
        CrystalScript.setAnimationStrategy("weather");
        CrystalScript.playAnimation();
        CrystalScript.stopAnimation();

        Assert.True(CrystalAnimator.GetBool("isIdle"));
        Assert.False(CrystalAnimator.GetBool("isWeather"));

    }
}
