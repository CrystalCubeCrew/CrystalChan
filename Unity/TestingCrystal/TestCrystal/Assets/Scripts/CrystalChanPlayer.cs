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
    HttpRequest httpTest;


    // Use this for initialization, runs at beginning of game
    void Start()
    {
        //playerAnimator = new IdleAnimation();
        crystal = gameObject.GetComponent<Animator>();
        tts = gameObject.GetComponent<GoogleTextToSpeech>();
        httpTest = new HttpRequest();
        setAnimationStrategy("idle");
        StartCoroutine(cy.Start());
        recordingStarted = false;


    }

    // Update is called once per frame
    void Update()
    {
        //start listening timer if "hey crystal" was said
        if (recordingStarted == true)
            gameObject.GetComponent<Recognition>().currentTime = Time.realtimeSinceStartup;

        //stop listening when we have listened for entime-current time about of seconds
        if (gameObject.GetComponent<Recognition>().currentTime > gameObject.GetComponent<Recognition>().endTime && recordingStarted == true)
        {
            cy.StopListening();
            recordingStarted = false;
        }
        //display check to see how much time we have left to wait till we cannot speak anymore
        else if (recordingStarted == true)
        {
            Debug.LogAssertion("currentTime: " + gameObject.GetComponent<Recognition>().currentTime + " endtime " + gameObject.GetComponent<Recognition>().endTime);
        }
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

    //mehtod determine the animation action that should be played
    public IEnumerator determineAction(string outText)
    {

        string actiontype = parseIntent(outText);
        //determine the animation action that should be played
        setAnimationStrategy(actiontype);

        //send intent to crystal cloud
        CoroutineWithData cd = new CoroutineWithData(this, getTextFromCloud("weather"));
        yield return cd.coroutine;
        //grab reponse speech fromcrystal cloud and play it
        TextToSpeech((string)cd.result);
    }

    //determine the itent based on the json string 
    private string parseIntent(string outText)
    {
        if (outText.Contains("weather"))
        {
            return "weather";
        }
        else
        {
            return "shrug";
        }
    }


    //play audio of the string "text" allow in unity using rss api
    private void TextToSpeech(string textToPlay)
    {
        tts.words = textToPlay;
        tts.playTTS();
    }


    IEnumerator getTextFromCloud(string intent)
    {
        //Assign httpRequest Object intent 
        httpTest.intent = intent;

        //Get Response object from coroutine
        CoroutineWithData cd = new CoroutineWithData(this, httpTest.httpCall());
        yield return cd.coroutine;
        Response response = (Response)cd.result;

        //Do something depended on return
        if (response.error == null)
        {
            Debug.Log("result is " + response.boii);
            yield return response.boii;
        }
        else {
            Debug.Log("result is " + response.error);
        }

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
