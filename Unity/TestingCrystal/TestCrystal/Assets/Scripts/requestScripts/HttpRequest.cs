using UnityEngine;
using System.Collections;

/**
* @purpose Handle http request from user's intent 
**/
public class HttpRequest {

  /**
   * Variables
   */

  public string intent{private get; set;}
  private Response myObject; 

  /**
 * Constructor
 *
 * @publie
 */

  public HttpRequest(){
    this.intent = "";
  }

  /**
   * Call Crystal Cloud API
   *
   * @return {Object}
   * @error {Object}
   * @public
   */

  public IEnumerator httpCall () {
    string url = "http://ec2-34-207-95-183.compute-1.amazonaws.com/" + this.intent;
    WWW www = new WWW(url);
    yield return www;

    try{
      myObject = (www.error == null)
        ? JsonUtility.FromJson<Response>(www.text) 
        : new Response("There was an error");     
    }
    catch(System.ArgumentException e){
      Debug.Log(e); 
      myObject = new Response("There was an error");
    }

    yield return myObject;
  }

}
