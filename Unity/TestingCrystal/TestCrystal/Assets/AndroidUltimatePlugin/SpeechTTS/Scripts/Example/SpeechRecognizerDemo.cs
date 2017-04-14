using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using AUP;
using System.Threading;

public class SpeechRecognizerDemo : MonoBehaviour {

	private const string TAG = "[SpeechRecognizerDemo]: ";

	private SpeechPlugin speechPlugin;	
	private Dispatcher dispatcher;
    public CrystalChanPlayer crystal;
    public bool saidHey;
    public ApiAiModuleCrystalChan cloudService;




    // Use this for initialization
    void Start (){
		dispatcher = Dispatcher.GetInstance();
		speechPlugin = SpeechPlugin.GetInstance();
		speechPlugin.SetDebug(0);
		speechPlugin.Init();

		AddSpeechPluginListener();
        crystal = gameObject.GetComponent<CrystalChanPlayer>();
        saidHey = false;
        cloudService = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ApiAiModuleCrystalChan>();
	}

	private void OnEnable(){
		AddSpeechPluginListener();
	}

	private void OnDisable(){
		RemoveSpeechPluginListener();
	}

	private void AddSpeechPluginListener(){
		if(speechPlugin!=null){
			//add speech recognizer listener
			speechPlugin.onReadyForSpeech+=onReadyForSpeech;
			speechPlugin.onBeginningOfSpeech+=onBeginningOfSpeech;
			speechPlugin.onEndOfSpeech+=onEndOfSpeech;
			speechPlugin.onError+=onError;
			speechPlugin.onResults+=onResults;
			speechPlugin.onPartialResults+=onPartialResults;

		}
	}

	private void RemoveSpeechPluginListener(){
		if(speechPlugin!=null){
			//remove speech recognizer listener
			speechPlugin.onReadyForSpeech-=onReadyForSpeech;
			speechPlugin.onBeginningOfSpeech-=onBeginningOfSpeech;
			speechPlugin.onEndOfSpeech-=onEndOfSpeech;
			speechPlugin.onError-=onError;
			speechPlugin.onResults-=onResults;
			speechPlugin.onPartialResults-=onPartialResults;
		}
	}

	private void OnApplicationPause(bool val){
		if(speechPlugin!=null){
			if(val){
				RemoveSpeechPluginListener();
			}else{
				AddSpeechPluginListener();
			}
		}
	}

	public void StartListening(){
        crystal.startedListening = true;

        bool isSupported = speechPlugin.CheckSpeechRecognizerSupport();
        Debug.Log("BEEP:  started listening -----------------------------------------------------------------------------------");

        if (isSupported){
            Debug.Log("It is supposrted");
            //number of possible results
            //Note: sometimes even you put 5 numberOfResults, there's a chance that it will be only 3 or 2
            //it is not constant.

            // enable beep
            speechPlugin.EnableBeep(true);
            speechPlugin.IncreaseMusicVolumeByValue(50);

			// enable offline
			//speechPlugin.EnableOffline(true);

			// enable partial Results
			speechPlugin.EnablePartialResult(false);
			
			int numberOfResults = 1;  //was 5
			speechPlugin.StartListening(numberOfResults);
			
			//by activating this, the Speech Recognizer will start and you can start Speaking or saying something 
			//speech listener will stop automatically especially when you stop speaking or when you are speaking 
			//for a long time
		}else{
			Debug.Log(TAG + "Speech Recognizer not supported by this Android device ");
		}
	}

	public void StartListeningNoBeep(){
        crystal.startedListening = true;

        Debug.Log("Speech Plug in is: " + speechPlugin);
		bool isSupported = speechPlugin.CheckSpeechRecognizerSupport();
        Debug.Log("started listening -----------------------------------------------------------------------------------");
		if(isSupported){
			//number of possible results
			//Note: sometimes even you put 5 numberOfResults, there's a chance that it will be only 3 or 2
			//it is not constant.

			// disable beep
			speechPlugin.EnableBeep(false);
            speechPlugin.DecreaseMusicVolumeByValue(100);

			// enable offline
			//speechPlugin.EnableOffline(true);

			// enable partial Results
			speechPlugin.EnablePartialResult(false);
			
			int numberOfResults = 1; //was 5
			speechPlugin.StartListening(numberOfResults);
			///speechPlugin.StartListeningNoBeep(numberOfResults,true);
			
			//by activating this, the Speech Recognizer will start and you can start Speaking or saying something 
			//speech listener will stop automatically especially when you stop speaking or when you are speaking 
			//for a long time
		}else{
			Debug.Log(TAG + "Speech Recognizer not supported by this Android device ");
		}
	}

	//cancel speech
	public void CancelSpeech(){
		if(speechPlugin!=null){
			bool isSupported = speechPlugin.CheckSpeechRecognizerSupport();

			if(isSupported){			
				speechPlugin.Cancel();
			}
		}

		Debug.Log( TAG + " call CancelSpeech..  ");
	}

	public void StopListening(){
		if(speechPlugin!=null){
			speechPlugin.StopListening();
		}
		Debug.Log( TAG + " StopListening...  ");
	}

	public void StopCancel(){
		if(speechPlugin!=null){
			speechPlugin.StopCancel();
		}
		Debug.Log( TAG + " StopCancel...  ");
	}

	private void OnDestroy(){
		RemoveSpeechPluginListener();
		speechPlugin.StopListening();
	}

	private void UpdateStatus(string status){

        Debug.LogError("status currently is ."+status);

        if (status.ToLower().Equals("error_recognizer_busy") || status.ToLower().Equals("error_speech_timeout")||
            status.ToLower().Equals("error_no_match"))
        {
             speechPlugin.Cancel();
            speechPlugin.CancelInvoke();
            saidHey = false;
            crystal.startedListening = false;
            crystal.recordingStarted = false;

            if (status.ToLower().Equals("error_speech_timeout") || status.ToLower().Equals("error_no_match"))
            {
                crystal.endtime = Time.realtimeSinceStartup + 1;  //wait a few before you record again
               // speechPlugin.CancelInvoke();
                crystal.timeOut = true;
            }
          
            
            /*   speechPlugin.Cancel();
               speechPlugin.DestroySpeechController();

               saidHey = false;
               crystal.startedListening = false;
               crystal.recordingStarted = false;*/

        }

        /*
                if (statusText!=null){
                    statusText.text = String.Format("Status: {0}",status);	
                }*/
    }

    //SpeechRecognizer Events
    private void onReadyForSpeech(string data){
		dispatcher.InvokeAction(
			()=>{
				if(speechPlugin!=null){
					//Disables modal
					speechPlugin.EnableModal(false);	
				}

				UpdateStatus(data.ToString());
			}
		);
	}

	private void onBeginningOfSpeech(string data){
		dispatcher.InvokeAction(
			()=>{
				UpdateStatus(data.ToString());
			}
		);
	}

	private void onEndOfSpeech(string data){
		dispatcher.InvokeAction(
			()=>{
				UpdateStatus(data.ToString());
			}
		);
	}

	private void onError(int data){
		dispatcher.InvokeAction(
			()=>{
				SpeechRecognizerError error = (SpeechRecognizerError)data;
				UpdateStatus(error.ToString());
                Debug.LogError("animation error, cannot be played...");

                /*if(resultText!=null){
					resultText.text =  "Result: Waiting for result...";
				}*/
            }
        );
	}

	private void onResults(string data){
		dispatcher.InvokeAction(
			()=>{
				
					string[] results =  data.Split(',');
					Debug.Log( TAG + " nom result length " + results.Length);
                Debug.Log("saidhey is " + saidHey);

                //when you set morethan 1 results index zero is always the closest to the words the you said
                //but it's not always the case so if you are not happy with index zero result you can always 
                //check the other index

                string whatToSay  = results.GetValue(0).ToString();
                bool crystalWasSpokenOf = false; // user said hey crystal
                //sample on checking other results
                foreach (string possibleResults in results)
                {
                    Debug.Log(TAG + " possibleResults " + possibleResults);
                    if (saidHey == false && (possibleResults.ToLower().Equals("hey crystal") || possibleResults.ToLower().Equals("crystal")))
                    {
                        crystalWasSpokenOf = true;
                    }
                  }

                    if (saidHey == false && crystalWasSpokenOf)
                    {
                         crystalWasSpokenOf = false;
                        crystal.setAnimationStrategy("wave");
                        crystal.playAnimation();
                        saidHey = true;
                        crystal.recordingStarted = true;
                        crystal.startedListening = true;
                        crystal.waitToRecord(.3f);

                    }
                    else if (saidHey)
                    {
                        
                        Debug.Log("WHAT TO PARSE your result is " + whatToSay);
                        saidHey = false;
                        crystal.startedListening = true;
                    StartCoroutine(crystal.playRequiredReaction(whatToSay));
                    //StartCoroutine(cloudService.SendText(whatToSay));

                }
                    else
                    {
                       
                            crystal.startedListening = false;
                    crystal.recordingStarted = false; //added

                }

            }
		);
	}

    public void sendToCloud(string whatToSay)
    {
        //cloudService.SendText(whatToSay);
        crystal.playRequiredReaction(whatToSay);
        //StartCoroutine(cloudService.SendText(whatToSay)); - makes error!
    }

	private void onPartialResults( string data ){

    }

	//SpeechRecognizer Events
}