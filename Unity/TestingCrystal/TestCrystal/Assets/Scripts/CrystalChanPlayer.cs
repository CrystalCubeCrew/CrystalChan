using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalChanPlayer : MonoBehaviour {

    private Animator crystal;
    private IPlayerAnimator playerAnimator;


	// Use this for initialization, runs at beginning of game
	void Start () {
        //playerAnimator = new IdleAnimation();
        crystal = gameObject.GetComponent<Animator>();
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
	}

    private bool informationChanged()
    {
        return true;
    }

    //set all non idle actions to false
    public void setIdleAction()
    {
        crystal.SetBool("isIdle", true);

        crystal.SetBool("isShrugging", false);
        crystal.SetBool("isCalculating", false);
        crystal.SetBool("isNews", false);
        crystal.SetBool("isDoing", false);

    }

    //sets the animation based on the IPlayerAnimator
    public void setAnimation(String animationToPlay)
    {
        crystal.SetBool("isIdle", false);
        crystal.SetBool("isNews", true);

    }
    //setter for the animator (for testing purposes)
    public void setAnimator(Animator animator)
    {
        crystal = animator;
    }
}
