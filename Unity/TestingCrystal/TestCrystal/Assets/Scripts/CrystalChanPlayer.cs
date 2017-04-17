using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalChanPlayer : MonoBehaviour
{

    private Animator crystal = null;
    public IPlayerAnimator playerAnimator = null;
   // public ApiAiModuleCrystalChan cy;
    //public VoiceRSSTextToSpeech tts;
    public bool recordingStarted;
    public SpeechRecognizerDemo srd;
    public TextToSpeechDemo tts;
    HttpRequest httpTest;
    public bool haveWaited, startedListening, timeOut;
    public AudioClip beep;
    private string whatToSay;
    public PlaySongs music;
    public AudioSource myAudio;
    private const String ID = "crystal_chan_6";
    public camer crystalCam;
    public CrystalCloud cloud;

    public User currentUser;
    public CloudResponseObject currentResponse;

    //timer things
    public float startTime, endtime;

    // Use this for initialization, runs at beginning of game
    void Start()
    {
        myAudio = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
        crystal = gameObject.GetComponent<Animator>();
        tts = gameObject.GetComponent<TextToSpeechDemo>();
        music = gameObject.GetComponent<PlaySongs>();
        httpTest = new HttpRequest();
        setAnimationStrategy("idle");
       // StartCoroutine(cy.Start());
        //cy.initializeSendText();  //make send text run once so cloud is initialized
        recordingStarted = haveWaited= startedListening = timeOut = false;
        startTime = Time.realtimeSinceStartup;
        endtime = startTime + 1;
        gameObject.GetComponent<AudioSource>().mute = false;
        currentUser = new User("none");
        currentResponse = new CloudResponseObject("no response has been given");
    }

    // Update is called once per frame
    void Update()
    {
        
        //start listening timer if "hey crystal" was said        
        //stop listening when we have listened for entime-current time about of secondss
        startTime = Time.realtimeSinceStartup;
        //Debug.Log("start: " + startTime +"end: "+ endtime);
       // Debug.Log("We have "+ (startTime > endtime)+" "+ !recordingStarted+" "+" "+!myAudio.isPlaying+" "+ !startedListening );
       if (startTime > endtime && !recordingStarted && !myAudio.isPlaying && !startedListening)
        {
            Debug.Log("Listening once again...");
            startTime = Time.realtimeSinceStartup;
            endtime = startTime + 2;
            startedListening = true;
            srd.StartListeningNoBeep();
          
            //recordingStarted = true;
        }else if(recordingStarted && haveWaited)
        {
            startTime = Time.realtimeSinceStartup;
            endtime = startTime + 2;
            haveWaited = false;

        }else
        {
            if ( timeOut)
            {
                Debug.Log("We have " + (startTime > endtime) + " " + !recordingStarted + " " + " " + !myAudio.isPlaying + " " + !startedListening);
                timeOut = false;
                startedListening = false;
            }
        }
    


    }

    public void waitToRecord(float timeToWait)
    {
        haveWaited= true;
        Debug.Log("SHOULD BEEEP NOWWW!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        srd.StartListening();
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
            case "login":
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

//revised classes -----------------------------------------------------------------------
    public IEnumerator playRequiredReaction(string whatUserSaid)
    {

        Debug.Log("Called " + whatUserSaid);
        CoroutineWithData cd = new CoroutineWithData(this, cloud.sendTextToCrystalCloud(whatUserSaid));

        yield return cd.coroutine;


        //grab reponse speech fromcrystal cloud and play it
        //Debug.LogError("Return is of type" + cd.result); //ERROR CHECK HERE IF CD.RESULT IS OF TYPE COROUTINE WE GET ERROR SO SHRUG HERE
        if((whatUserSaid.Contains("log in") || whatUserSaid.Contains("login") || whatUserSaid.Contains("log me in")))
        {
            setAnimationStrategy(determineAction("login"));
            crystalCam.setLogin(true);

        }
        else if ((currentResponse != null
            && currentResponse.intent != null && currentResponse.response != null))
        {
            Debug.Log("Response is : " + currentResponse.response + " whilst intent is " + currentResponse.intent);
            setAnimationStrategy(determineAction((currentResponse.intent)));
            completeRequiredActionBasedOnResponse(currentResponse);
        }
        else
        {
            playError();
        }
        currentResponse.setAllFieldsToNull();


    }

    private void completeRequiredActionBasedOnResponse(CloudResponseObject currentResponse)
    {

        if(currentResponse.intent.ToLower().Equals("math intent"))
        {
            currentResponse.response = MathCommand.getResultOf(currentResponse.response);
            PlayTextToSpeechWithAnimation(currentResponse.response);
        }
        else if(currentResponse.intent.ToLower().Equals("music intent"))
        {
            //play music 
            music.playASong(currentResponse.response);
        }
        else
        {
            PlayTextToSpeechWithAnimation(currentResponse.response);
        }
        

    }

    //determine the itent based on the json string 
    public string determineAction(string action)
    {
        if (action != null)
        {
            action = action.ToLower();
            if (action.Contains("weather intent"))
            {
                return "weather";
            }
            else if (action.Contains("todo intent"))
            {
                return "todo";
            }
            else if (action.Contains("music intent"))
            {
                return "music";
            }
            else if (action.Contains("news intent"))
            {
                return "news";
            }
            else if (action.Contains("wave"))
            {
                return "wave";
            }

            else if (action.Contains("idle"))
            {
                return "idle";
            }
            else if (action.Contains("log in") || action.Contains("login") || action.Contains("log me in"))
            {
                //send to cloud and recieve json info, play tts of crystal saying "hellom USERNAME"
                return "wave";
            }
            else if(action.Contains("math intent")) {

                    return "math";

                }
            }
        


        return "shrug";

    }

    //-----------------------------------------------------------------------------------------------------------


    //checks to see if return response is a string type that crystal can say
    public bool isString(object result)
    {
        var objectConversion = result as string;
        return (objectConversion != null);
    }

    //play audio of the string "text" allow in unity using rss api
    public void PlayTextToSpeechWithAnimation(string textToPlay)
    {
        whatToSay = textToPlay;
        Debug.Log("RESULT TO SPEAK TTS IS " + textToPlay);
        playAnimation();
        tts.SpeakOut();

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
            Debug.Log("result error is " + res.error);
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
