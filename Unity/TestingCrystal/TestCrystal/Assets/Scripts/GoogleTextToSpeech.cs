using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;

/// <summary>
/// <author>Jefferson Reis</author>
/// <explanation>Works only on Android. To test, change the platform to Android.</explanation>
/// </summary>

public class GoogleTextToSpeech : MonoBehaviour
{

    public string words = "Hello";

    IEnumerator Startt()
    {
        AudioSource myAudio = gameObject.GetComponent<AudioSource>();
        // Remove the "spaces" in excess
        Regex rgx = new Regex("\\s+");
        // Replace the "spaces" with "% 20" for the link Can be interpreted
        string result = rgx.Replace(words, "%20");
        string url = "http://api.voicerss.org/?key=12a3ff2575614e97b1b6120ff03fd3d3&hl=en-us&c=wav&src=\"" + result+"\"";
        WWW www = new WWW(url);
        yield return www;

        Debug.LogError("returned is " + www.text);
        myAudio.clip = www.GetAudioClip(false, true, AudioType.WAV);
        //myAudio.Play();
        myAudio.Play();


    }

    internal void playTTS()
    {
        StartCoroutine(Startt());
    }
}//closes the class