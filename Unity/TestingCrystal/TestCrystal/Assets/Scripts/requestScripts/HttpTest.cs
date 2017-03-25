using UnityEngine;
using System.Collections;

public class HttpTest : MonoBehaviour {

  HttpRequest httpTest = new HttpRequest();
    public string intent;
  // Use this for initialization
  IEnumerator Start () {
    //Assign httpRequest Object intent 
    httpTest.intent = intent;

    //Get Response object from coroutine
    CoroutineWithData cd = new CoroutineWithData(this, httpTest.httpCall() );
    yield return cd.coroutine;
    Response response = (Response) cd.result;
    
    //Do something depended on return
    if(response.error == null){
      Debug.Log("result is " + response.response);
    }
    else{
      Debug.Log("result is " + response.error);
    }
   
  }

}
