﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalChanPlayer : MonoBehaviour
{

    private Animator crystal = null;
    public IPlayerAnimator playerAnimator = null;
    public ApiAiModuleCrystalChan cy;
    //public VoiceRSSTextToSpeech tts;
    public bool recordingStarted;
    public SpeechRecognizerDemo srd;
    public TextToSpeechDemo tts;
    HttpRequest httpTest;
    public bool haveWaited;
    public AudioClip beep;
    private string whatToSay;

    //timer things
    float startTime, endtime;

    // Use this for initialization, runs at beginning of game
    void Start()
    {
        //playerAnimator = new IdleAnimation();
        crystal = gameObject.GetComponent<Animator>();
        // tts = gameObject.GetComponent<VoiceRSSTextToSpeech>();
        tts = gameObject.GetComponent<TextToSpeechDemo>();
        httpTest = new HttpRequest();
        setAnimationStrategy("idle");
        StartCoroutine(cy.Start());
        recordingStarted = haveWaited= false;
        //StartCoroutine(cy.Start());
        startTime = Time.realtimeSinceStartup;
        endtime = startTime + 2;
        gameObject.GetComponent<AudioSource>().mute = false;
        //srd.Start();
        //srd.StartListening();

    }

    // Update is called once per frame
    void Update()
    {
        //srd.StartListening();
        //start listening timer if "hey crystal" was said        
        //stop listening when we have listened for entime-current time about of secondss
        startTime = Time.realtimeSinceStartup;
        Debug.Log("start: " + startTime +"end: "+ endtime);
        if (startTime > endtime && !recordingStarted)
        {
            startTime = Time.realtimeSinceStartup;
            endtime = startTime + 4;
            srd.StartListeningNoBeep();
            //recordingStarted = true;
        }else if(recordingStarted && haveWaited)
        {
            startTime = Time.realtimeSinceStartup;
            endtime = startTime + 4;
            haveWaited = false;
            
        }


    }

    public void waitToRecord(float timeToWait)
    {
        /*startTime = Time.realtimeSinceStartup;
         endtime = startTime + timeToWait;

         while(startTime < endtime)
         {
             Debug.Log("WAITING__________start: " + startTime + "end: " + endtime+"_____________________");

             startTime = Time.realtimeSinceStartup;
         }*/
        haveWaited = true;
        Debug.Log("SHOULD BEEEP NOWWW!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        srd.StartListening();
        // AudioSource.PlayClipAtPoint(beep, gameObject.GetComponent<Transform>().position);
    }

    public void waitSomeTime(float time)
    {
        startTime = Time.realtimeSinceStartup;
        endtime = startTime + time;

        while (startTime < endtime)
        {
            Debug.Log("SHE IS SPEAKING___________: " + startTime + "end: " + endtime + "_____________________");

            startTime = Time.realtimeSinceStartup;
        }
    }

    internal void playError()
    {
        setAnimationStrategy("shrug");
        playAnimation();
        whatToSay = "Sorry, I don't know what that means.";
        tts.SpeakOut();
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
    public IEnumerator playRequiredReaction(string json)
    {
        //type of animation and intent to be voiced and animated by crystal
        string actiontype = determineAction(json);
        //determine the animation action that should be played _> (Current;y action type is "weather" but should be changed to Actiontype when sujen done"
        setAnimationStrategy(actiontype);

        Debug.Log("IN DETERMINE ACTION");

        //send intent to crystal cloud -- dummy weather----> should be actionType passed when sujen gets apiai done.
        CoroutineWithData cd = new CoroutineWithData(this, getTextFromCloud(actiontype));
        yield return cd.coroutine;
        //grab reponse speech fromcrystal cloud and play it
        Debug.LogError("Return is of type" + cd.result); //ERROR CHECK HERE IF CD.RESULT IS OF TYPE COROUTINE WE GET ERROR SO SHRUG HERE

        if (isString(cd.result))
        {
            PlayTextToSpeechWithAnimation((string)cd.result);
        }  
        else
        {
            playError();
        }


       
    }


    //checks to see if return response is a string type that crystal can say
    public bool isString(object result)
    {
        var objectConversion = result as string;
        return (objectConversion != null);
    }

    //determine the itent based on the json string 
    public string determineAction(string json)
    {
        json = json.ToLower();
        if(json != null)
        {
             if (json.Contains("weather intent"))
             {
                 return "weather";
             }else if (json.Contains("todo"))
            {
                 return "todo";
             }
            else if (json.Contains("music intent"))
            {
                return "music";
            }
            else if (json.Contains("news"))
            {
                return "news";
            }
            else if (json.Contains("wave"))
            {
                return "wave";
            }
            else if (json.Contains("math"))
            {
                return "math";
            }
            else if (json.Contains("idle"))
            {
                return "idle";
            }
        }
       
        
            return "shrug";
    
    }


    //play audio of the string "text" allow in unity using rss api
    public void PlayTextToSpeechWithAnimation(string textToPlay)
    {
        whatToSay = textToPlay;
        Debug.Log("RESULT TO SPEAK TTS IS " + textToPlay);
        tts.SpeakOut();
        // tts.words = textToPlay;
        //tts.playTTS();
       // waitSomeTime(2);
    }

    public string getWhatToSay()
    {
        return whatToSay;
    }

    IEnumerator getTextFromCloud(string intent)
    {
        //Assign httpRequest Object intent 
        httpTest.intent = intent;

        //Get Response object from coroutine
        CoroutineWithData cd = new CoroutineWithData(this, httpTest.httpCall());
        yield return cd.coroutine;
        Response res = (Response)cd.result;
        //Do something depended on return
        if (res.error == null)
        {
            Debug.Log("result is " + res.response);
            yield return res.response;
        }
        else {
            Debug.Log("result is " + res.error);
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
        playerAnimator.playAnimation(crystal);
        setIdleAction();
    }

    public void stopAnimation()
    {
        setAnimationStrategy("idle");
    }
}
