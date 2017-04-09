using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using System.Threading;

/// <summary>
/// <author>Jefferson Reis</author>
/// <explanation>Works only on Android. To test, change the platform to Android.</explanation>
/// </summary>

public class PlaySongs : MonoBehaviour
{

    public string words = "Hello";
    public CrystalChanPlayer ccp;

    public IEnumerator playSong(String link)
    {
        Debug.Log("in playSong");
        AudioSource myAudio = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
        ccp = GameObject.FindGameObjectWithTag("CrystalModel").GetComponent<CrystalChanPlayer>();
        Debug.Log("audio source is "+myAudio+" crystal is "+ccp);
        string url = link;
        WWW www = new WWW(url);
        Debug.Log("got url");
        yield return www;
        try
        {
            Debug.Log("trying... "+myAudio.clip);
            myAudio.clip = www.GetAudioClip(false, true, AudioType.WAV);
            Debug.Log("Audio is " + myAudio);
            Debug.Log("AudioClip is " + myAudio.clip);
            myAudio.Play();
            ccp.setAnimationStrategy("music");
            ccp.playAnimation();
            ccp.recordingStarted = false;
            ccp.startedListening = false;
        }
        catch (Exception e)
        {
            Debug.Log("Error: Connect connect to text to speech api - " + e.ToString());
            words = "";
            ccp.playError();
        }


    }

    public void playASong()
    {
        StartCoroutine(playSong("http://d2xb16qkudqwc7.cloudfront.net/starboy2.wav"));
    }
}//closes the class