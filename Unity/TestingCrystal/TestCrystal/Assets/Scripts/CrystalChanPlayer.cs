using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalChanPlayer : MonoBehaviour
{

    private Animator crystal = null;
    public IPlayerAnimator playerAnimator = null;
    public ApiAiModuleCrystalChan cy;
    private GoogleTextToSpeech tts;
    public bool recordingStarted;

    // Use this for initialization, runs at beginning of game
    void Start()
    {
        //playerAnimator = new IdleAnimation();
        crystal = gameObject.GetComponent<Animator>();
        tts = gameObject.GetComponent<GoogleTextToSpeech>();
        setAnimationStrategy("idle");
        StartCoroutine(cy.Start());
        recordingStarted = false;


    }

    // Update is called once per frame
    void Update()
    {

        if (recordingStarted == true)
            gameObject.GetComponent<Recognition>().currentTime = Time.realtimeSinceStartup;

        if (gameObject.GetComponent<Recognition>().currentTime > gameObject.GetComponent<Recognition>().endTime && recordingStarted == true)
        {
            cy.StopListening();
            recordingStarted = false;
        }
        else if (recordingStarted == true)
        {
            Debug.LogAssertion("currentTime: " + gameObject.GetComponent<Recognition>().currentTime + " endtime " + gameObject.GetComponent<Recognition>().endTime);
        }
    }

    private bool informationChanged()
    {
        return true;
    }

    //set all non idle actions to false, so idle can only be played and other actions are locked
    public void setIdleAction()
    {
        setAnimationStrategy("idle");
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
    public JsonFile my;
    internal void determineAction(string outText)
    {
        my = JsonUtility.FromJson<JsonFile>(outText);
        if (outText.Contains("greeting"))
        {
            setAnimationStrategy("shrug");
            playAnimation();
        }

        TextToSpeech(retrieveTextAudioFromCloudBasedOnIntent(outText));
    }

    private void TextToSpeech(string v)
    {
        tts.words = v;
        tts.playTTS();
    }

    private string retrieveTextAudioFromCloudBasedOnIntent(string intent)
    {
        return "Hey John and Chet";
    }

    //setter for the animator (for testing purposes)
    public void setAnimator(Animator animator)
    {
        crystal = animator;
    }


    public void playAnimation()
    {
        //crystal.SetBool("isIdle", false);
        // playerAnimator.playAnimation(crystal);
        crystal.Play("Shrug", 0);
    }

    public void stopAnimation()
    {
        setAnimationStrategy("idle");
    }
}
