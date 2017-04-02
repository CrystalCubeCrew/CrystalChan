//
// API.AI Unity SDK Sample
// =================================================
//
// Copyright (C) 2015 by Speaktoit, Inc. (https://www.speaktoit.com)
// https://www.api.ai
//
// ***********************************************************************************************************************
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with
// the License. You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on
// an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the
// specific language governing permissions and limitations under the License.
//
// ***********************************************************************************************************************

using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Reflection;
using ApiAiSDK;
using ApiAiSDK.Model;
using ApiAiSDK.Unity;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;

public class ApiAiModuleCrystalChan : MonoBehaviour
{

    public Text answerTextField;
    public Text inputTextField;
    private ApiAiUnity apiAiUnity = new ApiAiUnity();
    private AudioSource aud;
    public AudioClip listeningSound;
    public CrystalChanPlayer crystal;

    private ApiAiUnity apiAiUnity2;
    public string global;


    private readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
    };

    private readonly Queue<Action> ExecuteOnMainThread = new Queue<Action>();

    // Use this for initialization
    public IEnumerator Start()
    {
        // check access to the Microphone
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            throw new NotSupportedException("Microphone using not authorized");
        }

        ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) =>
        {
            return true;
        };
        //access for api.ai fro speech recognition
        const string ACCESS_TOKEN = "3485a96fb27744db83e78b8c4bc9e7b7";

        var config = new AIConfiguration( ACCESS_TOKEN, SupportedLanguage.English);

        apiAiUnity = new ApiAiUnity();
        apiAiUnity.Initialize(config);

        apiAiUnity.OnError += HandleOnError;
        apiAiUnity.OnResult += HandleOnResult;

        //crystal api.ai initialization
        const string ACCESS_TOKEN2 = "de08ee3db8064a96b78aae533587369a";

        var config2 = new AIConfiguration(ACCESS_TOKEN2, SupportedLanguage.English);

        apiAiUnity2 = new ApiAiUnity();
        apiAiUnity2.Initialize(config2);

        apiAiUnity2.OnError += HandleOnError;
        apiAiUnity2.OnResult += HandleOnResult;

    }

    //when we get return information from api call
    void HandleOnResult(object sender, AIResponseEventArgs e)
    {
        RunInMainThread(() => {
            var aiResponse = e.Response;
            if (aiResponse != null)
            {
                Debug.Log(aiResponse.Result.ResolvedQuery);
                //test to grab actions from the intent json string <WHEN SUJEN IS DONE, ADD API.AI RESPONE TYPE PARSER HERE> -> aiResponse.Result.Action
                Debug.Log("INTENT ACTION IS: " + aiResponse.Result.ResolvedQuery);
                writeResponseToFile(aiResponse.Result.ResolvedQuery);
                               
                AIResponse response = apiAiUnity2.TextRequest(aiResponse.Result.ResolvedQuery);
                var output = "";
                if (response != null)
                {
                    Debug.Log("Resolved query: " + response.Result.ResolvedQuery);
                    output = JsonConvert.SerializeObject(response, jsonSettings);

                    Debug.Log("Result: " + output);

                }
                else
                {
                    Debug.LogError("Response is null");
                }

                //notify crystal to send intent to cloud and determine and play animation ans response
                StartCoroutine(crystal.playRequiredReaction(output));

            }
            else
            {
                Debug.LogError("Response is null");
            }
        });
    }

    void writeResponseToFile(String response)
    {
         try{
            using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(@"Desktop\crystalFile.txt", true))
            {
                file.WriteLine(response);
            }
        }
        catch (Exception e)
        {

        }
    }

    //if we encounter error, display and send error to determine action
    void HandleOnError(object sender, AIErrorEventArgs e)
    {
        RunInMainThread(() => {
            Debug.LogException(e.Exception);
            Debug.Log(e.ToString());
            // answerTextField.text = e.Exception.Message;
            Debug.Log("error");
            writeResponseToFile("error on handle");
            //if error occurs while getting intent, then let crystalknow error has occurred
            crystal.setAnimationStrategy("shrug");
            crystal.playAnimation();
            Debug.Log("determine log");
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (apiAiUnity != null)
        {
            apiAiUnity.Update();
        }

        // dispatch stuff on main thread
        while (ExecuteOnMainThread.Count > 0)
        {
            ExecuteOnMainThread.Dequeue().Invoke();
        }
    }

    private void RunInMainThread(Action action)
    {
        ExecuteOnMainThread.Enqueue(action);
    }

    public void PluginInit()
    {

    }


    //begin listening to user
    public void StartListening()
    {
        Debug.Log("StartListening");

        aud = GetComponent<AudioSource>();
        apiAiUnity.StartListening(aud);

    }

    //stop listening to what the user is saying
    public void StopListening()
    {

        try
        {
            Debug.Log("StopListening");
            Debug.Log("Audio is currently"+aud.clip);

            apiAiUnity.StopListening();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
//--------------------------------------------------- might not need what is below ------------------------------------------------------
    //send text to formulate intent to api.ai
    public void SendText()
    {
        var text = inputTextField.text;

        Debug.Log(text);

        AIResponse response = apiAiUnity.TextRequest(text);

        if (response != null)
        {
            Debug.Log("Resolved query: " + response.Result.ResolvedQuery);
            var outText = JsonConvert.SerializeObject(response, jsonSettings);

            Debug.Log("Result: " + outText);

        }
        else
        {
            Debug.LogError("Response is null");
        }

    }

    public void StartNativeRecognition()
    {
        try
        {
            apiAiUnity.StartNativeRecognition();
            writeResponseToFile("started");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            writeResponseToFile("error in native recogn");

        }
    }
}
