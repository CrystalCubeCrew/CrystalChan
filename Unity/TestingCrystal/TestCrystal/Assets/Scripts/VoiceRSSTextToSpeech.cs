using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;

/// <summary>
/// <author>Jefferson Reis</author>
/// <explanation>Works only on Android. To test, change the platform to Android.</explanation>
/// </summary>

public class VoiceRSSTextToSpeech : MonoBehaviour
{

    public string words = "Hello";

    IEnumerator playTextToSpeech()
    {
        AudioSource myAudio = gameObject.GetComponent<AudioSource>();
        // Remove the "spaces" in excess
        Regex rgx = new Regex("\\s+");
        // Replace the "spaces" with "% 20" for the link Can be interpreted
        string result = rgx.Replace(words, "%20");
        string url = "http://api.voicerss.org/?key=476c0791e4894892bd699c016d788668&hl=en-us&c=wav&src=\"" + result+"\"";
        WWW www = new WWW(url);
        yield return www;
        try
        {
            //Debug.LogError("returned is " + www.text);
            myAudio.clip = www.GetAudioClip(false, true, AudioType.WAV);
            //myAudio.Play();
            myAudio.Play();
            gameObject.GetComponent<CrystalChanPlayer>().playAnimation();
        }catch(Exception e){
            Debug.Log("Error: Connect connect to text to speech api - "+ e.ToString());
            gameObject.GetComponent<CrystalChanPlayer>().playError();
        }
       

    }

    internal void playTTS()
    {
        StartCoroutine(playTextToSpeech());
    }
}//closes the class