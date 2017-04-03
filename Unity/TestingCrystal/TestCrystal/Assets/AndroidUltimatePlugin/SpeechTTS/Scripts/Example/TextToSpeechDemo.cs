﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using AUP;

public class TextToSpeechDemo : MonoBehaviour {

	private const string TAG = "[TextToSpeechDemo]: ";

	/*public InputField inputField;

	public Text statusText;
	public Text ttsDataActivityStatusText;
	public Text localeText;
	public Slider localeSlider;

	public Text pitchText;
	public Slider pitchSlider;

	public Text speechRateText;
	public Slider speechRateSlider;

	public Text volumeText;
	public Slider volumeSlider;*/

	private SpeechPlugin speechPlugin;
	private TextToSpeechPlugin textToSpeechPlugin;
	private float waitingInterval = 2f;
    public CrystalChanPlayer crystal;

	private Dispatcher dispatcher;

	private void Awake(){
		dispatcher = Dispatcher.GetInstance();

		speechPlugin = SpeechPlugin.GetInstance();
		speechPlugin.SetDebug(0);
		
		textToSpeechPlugin = TextToSpeechPlugin.GetInstance();
		textToSpeechPlugin.SetDebug(0);
		textToSpeechPlugin.Initialize();
		
		textToSpeechPlugin.OnInit+=OnInit;
		textToSpeechPlugin.OnChangeLocale+=OnSetLocale;
		textToSpeechPlugin.OnStartSpeech+=OnStartSpeech;
		textToSpeechPlugin.OnEndSpeech+=OnEndSpeech;
		textToSpeechPlugin.OnErrorSpeech+=OnErrorSpeech;
	}

	// Use this for initialization
	void Start (){
		CheckTTSDataActivity();
		UpdateSettingsValue();
        crystal = GameObject.FindGameObjectWithTag("CrystalModel").GetComponent<CrystalChanPlayer>();
	}

	private void OnApplicationPause(bool val){
		//for text to speech events
		if(textToSpeechPlugin!=null){
			if(textToSpeechPlugin.isInitialized()){
				if(val){
					textToSpeechPlugin.UnRegisterBroadcastEvent();
				}else{
					textToSpeechPlugin.RegisterBroadcastEvent();
				}
			}
		}
	}

	private void UpdateSettingsValue(){
		UpdateSpeechLocaleSetting();
		UpdatePitchSetting();
		UpdateSpeechRateSetting();
		UpdateVolumeSetting();
	}

	private void WaitingMode(){
		UpdateStatus("Waiting...");
	}

	private void UpdateStatus(string status){
		/*if(statusText!=null){
			statusText.text = String.Format("Status: {0}",status);	
		}*/
        Debug.Log("status: " + status);
	}

	private void UpdateTTSDataActivityStatus(string status){
        /*if(ttsDataActivityStatusText!=null){
			ttsDataActivityStatusText.text = String.Format("TTS Data Activity Status: {0}",status);
		}*/
        Debug.Log("tts data activity status: " + status);

    }

    private void UpdateLocale(SpeechLocale locale){
        /*if(localeText!=null){
			localeText.text = String.Format("Locale: {0}",locale);
			textToSpeechPlugin.SetLocale(locale);
		}*/
        Debug.Log("locale text: " + locale);

    }

    private void UpdatePitch(float pitch){
        //if(pitchText!=null){
        //pitchText.text = String.Format("Pitch: {0}",pitch);
        Debug.Log("pitch: " + pitch);

        textToSpeechPlugin.SetPitch(pitch);
		//}
	}

	private void UpdateSpeechRate(float speechRate){
		//if(speechRateText!=null){
			//speechRateText.text = String.Format("Speech Rate: {0}",speechRate);
			textToSpeechPlugin.SetSpeechRate(speechRate);
        Debug.Log("speech rate: " + speechRate);

        //}
    }

    private void UpdateVolume(int volume){
		//if(volumeText!=null){
			//volumeText.text = String.Format("Volume: {0}",volume);
			textToSpeechPlugin.IncreaseMusicVolumeByValue(volume);
        Debug.Log("volume: " + volume);

        //}
    }

    public void SpeakOut(){
        //if(inputField!=null){
        string whatToSay = crystal.getWhatToSay();
			string utteranceId  = "test-utteranceId";

			if(textToSpeechPlugin.isInitialized()){
				UpdateStatus("Trying to speak...");
				Debug.Log(TAG + "SpeakOut whatToSay: " + whatToSay  + " utteranceId " + utteranceId);
				textToSpeechPlugin.SpeakOut(whatToSay,utteranceId);
			}
		//}
	}

	//checks if speaking
	public bool IsSpeaking(){
		return textToSpeechPlugin.IsSpeaking();
	}

	private void CheckTTSDataActivity(){
		if(textToSpeechPlugin!=null){
			if(textToSpeechPlugin.CheckTTSDataActivity()){
				UpdateTTSDataActivityStatus("Available");
			}else{
				UpdateTTSDataActivityStatus("Not Available");
			}
		}
	}

	public void SpeakUsingAvailableLocaleOnDevice(){

		//on this example we will use spain locale
		TTSLocaleCountry ttsLocaleCountry = TTSLocaleCountry.SPAIN;
		
		//check if available
		bool isLanguageAvailanble =  textToSpeechPlugin.CheckLocale(ttsLocaleCountry);
		
		if(isLanguageAvailanble){
			string countryISO2Alpha = textToSpeechPlugin.GetCountryISO2Alpha(ttsLocaleCountry);
			
			//set spain language
			textToSpeechPlugin.SetLocaleByCountry(countryISO2Alpha);
			Debug.Log(TAG + "locale set," + ttsLocaleCountry.ToString() + "locale is available");

			SpeakOut();
		}else{
			Debug.Log(TAG + "locale not set," + ttsLocaleCountry.ToString() + "locale is  notavailable");
		}
	}

	private void OnDestroy(){
		//call this of your not going to used TextToSpeech Service anymore
		textToSpeechPlugin.ShutDownTextToSpeechService();
	}

	public void OnLocaleSliderChange(){
		Debug.Log(TAG + "OnLocaleSliderChange");
		UpdateSpeechLocaleSetting();
	}

	private void UpdateSpeechLocaleSetting(){
		//if(localeSlider!=null){
		//	SpeechLocale locale = (SpeechLocale)localeSlider.value;
		//	UpdateLocale(locale);
		//}
	}

	private void UpdatePitchSetting(){
		//if(pitchSlider!=null){
		//	float pitch = pitchSlider.value;
		//	UpdatePitch(pitch);
		//}
	}
	
	public void OnPitchSliderChange(){
		Debug.Log(TAG + "OnPitchSliderChange");
		UpdatePitchSetting();
	}
	
	public void OnSpeechRateSliderChange(){
		Debug.Log(TAG + "OnSpeechRateSliderChange");
		UpdateSpeechRateSetting();
	}
	
	private void UpdateSpeechRateSetting(){
		//if(speechRateSlider!=null){
		///	float speechRate = speechRateSlider.value;
		//	UpdateSpeechRate(speechRate);
		//}
	}
	
	public void OnVolumeSliderChange(){
		Debug.Log(TAG + "OnLocaleSliderChange");
		UpdateVolumeSetting();
	}
	
	private void UpdateVolumeSetting(){
		//if(volumeSlider!=null){
		//	int volume = (int)volumeSlider.value;
		//	UpdateVolume(volume);
		//}
	}

	private void OnInit(int status){
		dispatcher.InvokeAction(
			()=>{
				Debug.Log(TAG + "OnInit status: " + status);

				if(status == 1){
					UpdateStatus("init speech service successful!");

					//get available locale on android device
					//textToSpeechPlugin.GetAvailableLocale();

					UpdateLocale(SpeechLocale.US);
					UpdatePitch(1f);
					UpdateSpeechRate(1f);

					CancelInvoke("WaitingMode");
					Invoke("WaitingMode",waitingInterval);
				}else{
					UpdateStatus("init speech service failed!");

					CancelInvoke("WaitingMode");
					Invoke("WaitingMode",waitingInterval);
				}
			}
		);
	}

	private void OnSetLocale(int status){
		dispatcher.InvokeAction(
			()=>{
				Debug.Log(TAG + "OnSetLocale status: " + status);
				if(status == 1){
					//float pitch = Random.Range(0.1f,2f);
					//textToSpeechPlugin.SetPitch(pitch);
				}
			}
		);
	}
	
	private void OnStartSpeech(string utteranceId){
		dispatcher.InvokeAction(
			()=>{
				UpdateStatus("Start Speech...");
				Debug.Log(TAG + "OnStartSpeech utteranceId: " + utteranceId);

				if(IsSpeaking()){
					UpdateStatus("speaking...");
				}
			}
		);
	}
	
	private void OnEndSpeech(string utteranceId){
		dispatcher.InvokeAction(
			()=>{
				UpdateStatus("Done Speech...");
				Debug.Log(TAG + "OnDoneSpeech utteranceId: " + utteranceId);
                crystal.recordingStarted = false;
				CancelInvoke("WaitingMode");
				Invoke("WaitingMode",waitingInterval);
			}
		);
	}
	
	private void OnErrorSpeech(string utteranceId){
		dispatcher.InvokeAction(
			()=>{
				UpdateStatus("Error Speech...");
                crystal.recordingStarted = false;
                CancelInvoke("WaitingMode");
				Invoke("WaitingMode",waitingInterval);

				Debug.Log(TAG + "OnErrorSpeech utteranceId: " + utteranceId);
			}
		);
	}
}
