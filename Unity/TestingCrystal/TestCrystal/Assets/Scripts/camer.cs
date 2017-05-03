using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class camer : MonoBehaviour
{
    WebCamTexture back;
    WebCamTexture _webcamtex;
    // new ip 34.206.165.219
    private const string URL = "http://34.206.165.219/findUser";
    private const string HAND_URL = "http://34.206.165.219/checkImage";
    WebCamTexture webCamTexture;
    private bool login;
    private const String ID = "crystal_chan_6";

    public string intent { private get; set; }
    private User myObject;
    private Response camObject;
    public CrystalChanPlayer crystal;
    //added all below
    public bool isHand, userIssuedMusicToPlay;
    public float startTime, endTime;
    public bool musicLeftToPlay, sendNext, toggle;

    public SpeechRecognizerDemo srd;

    void Start()
    {
        _webcamtex = new WebCamTexture();
        Renderer _renderer = GetComponent<Renderer>();
        _renderer.material.mainTexture = _webcamtex;
        _webcamtex.Play();
        login = musicLeftToPlay= false;
       
        //added

        startTime = endTime = Time.realtimeSinceStartup;
        sendNext = toggle = false;

    }
    // Update is called once per frame
    void Update()
    {
        startTime = Time.realtimeSinceStartup;
        if (login)
        {
            Debug.Log("calling capture");
            login = false;
            StartCoroutine(CaptureTextureAsPNG(ID));
            return;
        }else if ((crystal.myAudio.isPlaying &&( startTime > endTime)) || (musicLeftToPlay && (startTime > endTime))) //added
        {
            startTime = Time.realtimeSinceStartup;
            endTime = startTime + 2f; //was 1 now 1.5f
            StartCoroutine(sendPhotoToCloudForHandInfo()); //added 
            //StartCoroutine(pixelRatioIsGreat());
        }
    }


    //added
    public IEnumerator pixelRatioIsGreat()
    {
        yield return new WaitForSeconds(3);
        Texture2D _TextureFromCamera = new Texture2D(GetComponent<Renderer>().material.mainTexture.width,
        GetComponent<Renderer>().material.mainTexture.height);
        _TextureFromCamera.SetPixels((GetComponent<Renderer>().material.mainTexture as WebCamTexture).GetPixels());
        _TextureFromCamera.Apply();
        byte[] bytes = _TextureFromCamera.EncodeToPNG();
        string encodedText = System.Convert.  ToBase64String(bytes);
        Debug.Log("grabbed image");

        Color[] colorpx =  _TextureFromCamera.GetPixels();
        //Debug.Log("pixel length is "+colorpx.Length);
        int length = colorpx.Length;
        float flo = 5 / colorpx.Length;

        //Debug.Log(" >>>> length is " + length+"with division "+flo);
        Color basis = _TextureFromCamera.GetPixel((int)(colorpx.Length / 2), (int)(colorpx.Length / 2));
        double d;
        int count = 0;

        //Debug.Log("red is "+basis.r+" blue is "+ basis.b+"green is "+ basis.g);
        
        //see how close in color each pixel is, if it is close count++
        for (int i=1; i < colorpx.Length; i++)
        {
            d = Math.Sqrt(Math.Pow((colorpx[i - 1].r - basis.r),2) + Math.Pow((colorpx[i - 1].g - basis.g),2) + Math.Pow((colorpx[i - 1].b - basis.b), 2));
            if ((d / Math.Sqrt((255) ^ 2 + (255) ^ 2 + (255) ^ 2))*100 <= 2)
            {
                count++;
            }
        }
        
        if (((double)count / length) * 100 > 55)
        {
            isHand = true;
            Debug.Log("POSSSIBLE HANDDDS!!!");
            sendNext = true;
        }else if (sendNext)
        {//send image to cloud to see if it is a hand or not and do the proper functions
            //StartCoroutine(sendPhotoToCloudForHandInfo(encodedText));  commentedout
            sendNext = false;
        }

        if (crystal.myAudio == null || crystal.myAudio.time == 0)
        {
            musicLeftToPlay = false;
        }

        Debug.Log("Count is " + count + "   percent of count/pixels is " + ((double)count / length)*100);

        yield return 0;
    }

    private IEnumerator sendPhotoToCloudForHandInfo() //was String encodedText
    {
        sendNext = false;
        //-------------added ---------------------
        musicLeftToPlay = true;
        Texture2D _TextureFromCamera = new Texture2D(GetComponent<Renderer>().material.mainTexture.width,
        GetComponent<Renderer>().material.mainTexture.height);
        _TextureFromCamera.SetPixels((GetComponent<Renderer>().material.mainTexture as WebCamTexture).GetPixels());
        _TextureFromCamera.Apply();
        byte[] bytes = _TextureFromCamera.EncodeToPNG();
        string encodedText = System.Convert.ToBase64String(bytes);
        Debug.Log("grabbed image");
        //--------------------------------------------
        WWWForm form = new WWWForm();
        form.AddField("file", encodedText);
        form.AddField("fileName", "testHandImage.png");
        form.AddField("machineId", ID);
        // Upload to a cgi script
        WWW w = new WWW(HAND_URL, form); //need to change url to something else
        yield return w;
        if (!string.IsNullOrEmpty(w.error))
        {
            print(w.error);
            Debug.Log("Error in image hand capture is " + w.error);
           
        }
        else
        {
            print("Finished Uploading Hand Screenshot");
            try
            {
                camObject = (w.error == null)
                  ? JsonUtility.FromJson<Response>(w.text)
                  : new Response("There was an error");
                if (camObject.response == null )
                {
                    throw new System.ArgumentException();
                }

                Debug.Log("object is -> " + camObject);
                Debug.Log("response is -> " + camObject.response);

                if (camObject.response.ToLower().Equals("hand"))
                {
                    if (!toggle)
                    {
                        crystal.myAudio.Pause();
                        crystal.setAnimationStrategy("idle");
                        crystal.playAnimation();
                        toggle = true;
                    }
                    else
                    {
                        if (crystal.myAudio.time < crystal.myAudio.clip.length && crystal.myAudio.time != 0)
                        {
                            srd.getSpeechPlugin().IncreaseMusicVolumeByValue(50);
                            crystal.myAudio.volume = 1.0f;
                            crystal.myAudio.UnPause();
                            //crystal.myAudio.PlayScheduled(crystal.myAudio.time);
                            crystal.setAnimationStrategy("music");
                            crystal.playAnimation();
                            toggle = false;
                        }
                        else
                        {
                            musicLeftToPlay = false;
                        }
                    }
                }

                if(crystal.myAudio == null || crystal.myAudio.time == 0)
                {
                    musicLeftToPlay = false;

                }

            }
            catch (System.ArgumentException e)
            {
                Debug.Log(e);
                camObject = new Response("There was an error");

            
            }
            if (crystal.myAudio == null || crystal.myAudio.time == 0)
            {
                musicLeftToPlay = false;
            }

            yield return camObject;
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
        Debug.Log("url is -" + URL);

        yield return w;
        if (!string.IsNullOrEmpty(w.error))
        {
            print(w.error);
            Debug.Log("Error in image capture is " + w.error);

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
                if ( myObject.firstName == null || myObject.firstName.ToLower().Equals(""))
                {
                    throw new System.ArgumentException();
                }
                crystal.currentUser.firstName = myObject.firstName;
                crystal.currentUser.lastName = myObject.lastName;
                crystal.currentUser.userId = myObject.userId;
                Debug.Log("object is -> " + myObject);

                Debug.Log("response is -> " + myObject.firstName);
                crystal.setAnimationStrategy("wave");
                crystal.PlayTextToSpeechWithAnimation("Hello " + myObject.firstName + " " + myObject.lastName + ". Welcome to your "
                    + "Crystal Cube!");
            }
            catch (System.ArgumentException e)
            {
                Debug.Log(e);
                myObject = new User("There was an error");

                crystal.setAnimationStrategy("shrug");
                crystal.PlayTextToSpeechWithAnimation("Sorry, I am not sure who you are.");
            }


            yield return myObject;
        }
    }

    public void setLogin(bool newLogin)
    {
        login = newLogin;
    }


}