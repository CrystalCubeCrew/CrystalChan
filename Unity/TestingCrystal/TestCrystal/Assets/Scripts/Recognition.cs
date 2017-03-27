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


    //work around variables 
    public float start, end;
    public bool first;

    void Start()
    {
        currentTime = endTime = start = end = 0;
        first = true;
        //initilize stuff

        //make crystal listen after 4 seconds of start up
        start = Time.realtimeSinceStartup;
        end = (float)(currentTime + 4);

        /* keywords.Add("Hey Crystal", () =>
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
         keywordRecog.Start(); */


    }


    //update method only used for work around
    void Update()
    {
        start = Time.realtimeSinceStartup;
        if (start > end && first )
        {
            first = false;
            cy.StartListening();
            currentTime = start = Time.realtimeSinceStartup;
            endTime = (float)(currentTime + 3);
            gameObject.GetComponent<CrystalChanPlayer>().recordingStarted = true;

            //when hey crystal is said, play wave animation
            gameObject.GetComponent<CrystalChanPlayer>().setAnimationStrategy("wave");
            gameObject.GetComponent<CrystalChanPlayer>().playAnimation();
        }
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
