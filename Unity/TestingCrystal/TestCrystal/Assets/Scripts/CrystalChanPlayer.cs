using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalChanPlayer : MonoBehaviour
{

    private Animator crystal = null;
    public IPlayerAnimator playerAnimator = null;
    public ApiAiModuleCrystalChan cy;
    private VoiceRSSTextToSpeech tts;
    public bool recordingStarted;
    HttpRequest httpTest;


    // Use this for initialization, runs at beginning of game
    void Start()
    {
        //playerAnimator = new IdleAnimation();
        crystal = gameObject.GetComponent<Animator>();
        tts = gameObject.GetComponent<VoiceRSSTextToSpeech>();
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

    internal void playError()
    {
        setAnimationStrategy("shrug");
        playAnimation();
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
    public IEnumerator determineAction(string json)
    {
        //type of animation and intent to be voiced and animated by crystal
        string actiontype = parseIntent(json);
        //determine the animation action that should be played _> (Current;y action type is "weather" but should be changed to Actiontype when sujen done"
        setAnimationStrategy("weather");

        //send intent to crystal cloud -- dummy weather----> should be actionType passed when sujen gets apiai done.
        CoroutineWithData cd = new CoroutineWithData(this, getTextFromCloud("weather"));
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
    public string parseIntent(string json)
    {
        if(json != null)
        {
             if (json.Contains("weather"))
             {
                 return "weather";
             }else if (json.Contains("todo"))
            {
                 return "todo";
             }
            else if (json.Contains("music"))
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
    }

    public void stopAnimation()
    {
        setAnimationStrategy("idle");
    }
}
