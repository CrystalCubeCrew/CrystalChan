using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;


/// <summary>
/// <author>Jefferson Reis</author>
/// <explanation>Works only on Android. To test, change the platform to Android.</explanation>
/// </summary>

public class GoogleTextToSpeech : MonoBehaviour
{

    public string words = "Hello";

    IEnumerator Start()
    {
        AudioSource myAudio = gameObject.GetComponent<AudioSource>();
        string url = "http://api.voicerss.org/?key=d94ce83f767b429e95b0be81eb9c1962&hl=en-us&c=wav&src=" + words;
        WWW www = new WWW(url);
        yield return www;

        Debug.LogError("returned is " + www.text);
        myAudio.clip = www.GetAudioClip(false, true, AudioType.WAV);
        //myAudio.Play();
        myAudio.Play();


    }

    void OnGUI()
    {
        words = GUI.TextField(new Rect(Screen.width / 2 - 200 / 2, 10, 200, 30), words);
        if (GUI.Button(new Rect(Screen.width / 2 - 150 / 2, 40, 150, 50), "Speak"))
        {
            StartCoroutine(Start());
        }
    }


}//closes the class