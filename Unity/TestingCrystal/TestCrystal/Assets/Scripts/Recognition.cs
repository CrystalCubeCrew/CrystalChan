using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using System;

public class Recognition : MonoBehaviour {

    //listens for specific words
    KeywordRecognizer keywordRecog;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    private ApiAiModuleCrystalChan listener;


    void Start()
    {
        //initilize stuff
        keywords.Add("Hey Crystal", () =>
         {
             startRecordingVoice();

         });
        keywords.Add("stop", () =>
        {
            stopRecordingVoice();

        });


        keywordRecog = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecog.OnPhraseRecognized += keywordRecognOnPhraseRecog;
        keywordRecog.Start();

        listener =gameObject.GetComponent<ApiAiModuleCrystalChan>();


    }

    private void stopRecordingVoice()
    {
        Debug.Log("Stopped Recording...");
       listener.StopListening();
      // listener.SendText();
    }

    void keywordRecognOnPhraseRecog(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;

        if(keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    public void startRecordingVoice()
    {
        Debug.Log("Started Recording...");
        if(listener == null)
        {
            Debug.Log("NO OBJECT INSTANCTE");
        }
       listener.StartListening();
    }
}
