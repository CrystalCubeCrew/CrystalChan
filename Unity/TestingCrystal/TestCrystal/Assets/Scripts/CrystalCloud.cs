using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalCloud : MonoBehaviour {

    public string URL = "http://ec2-34-207-95-183.compute-1.amazonaws.com/crystalRequest";

    private const String ID = "crystal_chan_6";

    public string intent { private get; set; }
    private CloudResponseObject myObject;
    public CrystalChanPlayer crystal;

    public IEnumerator sendTextToCrystalCloud(string whatUserSaid)
    {

        WWWForm form = new WWWForm();
        form.AddField("speech", whatUserSaid);
        //form.AddField("machineId", id);
        // Upload to a cgi script
        form.AddField("userId", crystal.currentUser.userId); //added
        form.AddField("machineId", ID);  //added
        WWW w = new WWW(URL, form);
        yield return w;
        if (!string.IsNullOrEmpty(w.error))
        {
            Debug.Log("Error in sending what user said to cloud with error" + w.error);
            crystal.setAnimationStrategy("shrug");
            crystal.PlayTextToSpeechWithAnimation("Sorry, I dont understand what you are saying.");
        }
        else
        {
            try
            {
                Debug.Log("returned text is " + w.text);
                myObject = (w.error == null)
                  ? JsonUtility.FromJson<CloudResponseObject>(w.text)
                  : new CloudResponseObject("There was an error");

                crystal.currentResponse.response = myObject.response;
                crystal.currentResponse.intent = myObject.intent;

            }
            catch (System.ArgumentException e)
            {
                Debug.Log(e);
                Debug.Log("Couldn't get json from the cloud");
                crystal.setAnimationStrategy("shrug");
                crystal.PlayTextToSpeechWithAnimation("Sorry, I dont understand what you are saying.");
            }

            yield return myObject;
        }
    }
}
