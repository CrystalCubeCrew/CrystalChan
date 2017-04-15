using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class camer : MonoBehaviour {
    WebCamTexture back;
    WebCamTexture _webcamtex;
    public string URL = "http://ec2-34-207-95-183.compute-1.amazonaws.com/findUser";
    WebCamTexture webCamTexture;
    private bool login;
    private const String ID = "crystal_chan_6";

    public string intent { private get; set; }
    private User myObject;
    public CrystalChanPlayer crystal;

    void Start()
    {
        _webcamtex = new WebCamTexture();
        Renderer _renderer = GetComponent<Renderer>();
        _renderer.material.mainTexture = _webcamtex;
        _webcamtex.Play();
        login = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (login)
        {
            Debug.Log("calling capture");
            login = false;
            StartCoroutine(CaptureTextureAsPNG(ID));
            return;
        }
    }
    public IEnumerator CaptureTextureAsPNG(string id)
    {
        Debug.Log("in capture");

        //yield return new WaitForEndOfFrame();
        Debug.Log("waited for end of frame");
        Texture2D _TextureFromCamera = new Texture2D(GetComponent<Renderer>().material.mainTexture.width,
        GetComponent<Renderer>().material.mainTexture.height);
        _TextureFromCamera.SetPixels((GetComponent<Renderer>().material.mainTexture as WebCamTexture).GetPixels());
        _TextureFromCamera.Apply();
        byte[] bytes = _TextureFromCamera.EncodeToPNG();
        string encodedText = System.Convert.ToBase64String(bytes);
        Debug.Log("grabbed image");

        WWWForm form = new WWWForm();
        form.AddField("file", encodedText);
        form.AddField("fileName", "testImage.png");
        form.AddField("machineId", id);
        // Upload to a cgi script
        WWW w = new WWW(URL, form);
        yield return w;
        if (!string.IsNullOrEmpty(w.error))
        {
            print(w.error);
            Debug.Log("Error in image capture is " + w.error);
            //crystal.recordingStarted = false;
            //crystal.startedListening = false;
            //added
            crystal.setAnimationStrategy("shrug");
            crystal.PlayTextToSpeechWithAnimation("Sorry, I am not sure who you are.");
        }
        else
        {
            print("Finished Uploading Screenshot");
            try
            {
                myObject = (w.error == null)
                  ? JsonUtility.FromJson<User>(w.text)
                  : new User("There was an error");

                crystal.currentUser.firstName = myObject.firstName;
                crystal.currentUser.lastName = myObject.lastName;
                Debug.Log("object is -> " + myObject);

                Debug.Log("response is -> " + myObject.firstName );
                crystal.setAnimationStrategy("wave");
                crystal.PlayTextToSpeechWithAnimation("Hello " + myObject.firstName + " " + myObject.lastName + ". Welcome to your "
                    + "Crystal Cube!");
            }
            catch (System.ArgumentException e)
            {
                Debug.Log(e);
                myObject = new User("There was an error");
            }

            yield return myObject;
        }
    }

    public void setLogin(bool newLogin)
    {
        login = newLogin;
    }


}
