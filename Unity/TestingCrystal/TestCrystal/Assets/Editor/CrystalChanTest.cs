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
        Debug.LogError("Animator is " + CrystalAnimator);
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
        Debug.LogError("news is " + CrystalAnimator.GetBool("isNews"));
        Assert.True(CrystalAnimator.GetBool("isNews"));
    }
    [Test]
    public void ifSetAnimationIstodoThenToDoAnimationIsPlayed()
    {
        CrystalScript.setAnimationStrategy("todo");
        CrystalScript.playAnimation();

        Debug.LogError("todo is " + CrystalAnimator.GetBool("isDoing"));

        Assert.False(CrystalAnimator.GetBool("isIdle"));
        Assert.True(CrystalAnimator.GetBool("isDoing"));

    }
}
