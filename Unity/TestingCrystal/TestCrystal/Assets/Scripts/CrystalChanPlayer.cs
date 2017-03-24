using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalChanPlayer : MonoBehaviour {

    private Animator crystal = null;
    private IPlayerAnimator playerAnimator = null;
    public ApiAiModuleCrystalChan cy;

    // Use this for initialization, runs at beginning of game
    void Start () {
        //playerAnimator = new IdleAnimation();
        crystal = gameObject.GetComponent<Animator>();
        setAnimationStrategy("idle");
        StartCoroutine(cy.Start());


    }
	
	// Update is called once per frame
	void Update () {
       if (informationChanged())
        {
            //call determine action method


        }
        else
        {
            setIdleAction();
        }

        if (crystal.GetBool("isShrugging"))
        {
            cy.StartListening();
        }
        else if(crystal.GetBool("isDoing"))
        {
            cy.StopListening();
        }
	}

    private bool informationChanged()
    {
        return true;
    }

    //set all non idle actions to false, so idle can only be played and other actions are locked
    public void setIdleAction()
    {
        crystal.SetBool("isIdle", true);

        crystal.SetBool("isShrugging", false);
        crystal.SetBool("isCalculating", false);
        crystal.SetBool("isNews", false);
        crystal.SetBool("isDoing", false);

    }

    //sets the animation based on the IPlayerAnimator
    public void setAnimationStrategy(String animationToPlay)
    {
        switch (animationToPlay)
        {
            case "shrug":
                playerAnimator = new ShrugAnimation();
                break;
            case "todo":
                playerAnimator = new ToDoAnimation();
                break;
            case "weather":
                playerAnimator = new WeatherAnimation();
                break;
            case "music":
                playerAnimator = new MusicAnimation();
                break;
            case "news":
                playerAnimator = new NewsFeedAnimation();
                break;
            case "wave":
                playerAnimator = new WaveAnimation();
                break;
            case "math":
                playerAnimator = new BasicMathAnimation();
                break;
            case "idle":
                playerAnimator = new IdleAnimation();
                break;
        }

    }
    //setter for the animator (for testing purposes)
    public void setAnimator(Animator animator)
    {
        crystal = animator;
    }


    public void playAnimation()
    {
        crystal.SetBool("isIdle", false);
        playerAnimator.playAnimation(crystal);
    }

    public void stopAnimation()
    {
        playerAnimator.stopAnimation(crystal);
        crystal.SetBool("isIdle", true);
    }
}
