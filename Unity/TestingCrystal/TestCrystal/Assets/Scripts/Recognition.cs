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
    public ApiAiModuleCrystalChan cy;

    public float currentTime;
    public float endTime;
    void Start()
    {
        currentTime = endTime = 0;
        //initilize stuff
        keywords.Add("Hey Crystal", () =>
         {
             //add animation to play when you say hey crystal
             cy.StartListening();
             currentTime = Time.realtimeSinceStartup;
             endTime = (float)(currentTime + 3);
             gameObject.GetComponent<CrystalChanPlayer>().recordingStarted = true;

             //when hey crystal is said, play wave animation
             gameObject.GetComponent<CrystalChanPlayer>().setAnimationStrategy("wave");
             gameObject.GetComponent<CrystalChanPlayer>().playAnimation();
         });


        keywordRecog = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecog.OnPhraseRecognized += keywordRecognOnPhraseRecog;
        keywordRecog.Start();


    }



    void keywordRecognOnPhraseRecog(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;

        if(keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }


}
