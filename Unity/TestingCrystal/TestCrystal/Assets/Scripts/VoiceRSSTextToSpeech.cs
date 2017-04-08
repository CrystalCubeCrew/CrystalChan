using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;

/// <summary>
/// <author>Jefferson Reis</author>
/// <explanation>Works only on Android. To test, change the platform to Android.</explanation>
/// </summary>

public class PlayMusic : MonoBehaviour
{

    public string words = "Hello";
    public CrystalChanPlayer ccp;

    public IEnumerator playSong(String link)
    {
        AudioSource myAudio = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
        // Remove the "spaces" in excess
        Regex rgx = new Regex("\\s+");
        // Replace the "spaces" with "% 20" for the link Can be interpreted
        string result = rgx.Replace(words, "%20");
        string url = link;
        WWW www = new WWW(url);
        Debug.Log("got url");
        yield return www;
        try
        {
            //Debug.LogError("returned is " + www.text);
            myAudio.clip = www.GetAudioClip(false, true, AudioType.WAV);
            //myAudio.Play();
            myAudio.Play();
            ccp.playAnimation();
        }catch(Exception e){
            Debug.Log("Error: Connect connect to text to speech api - "+ e.ToString());
            words = "";
            ccp.playError();
        }
       

    }

    public void playASong()
    {
        StartCoroutine(playSong("http://d2xb16qkudqwc7.cloudfront.net/you_and_i.m4a"));
    }
}//closes the class