using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudResponseObject {

    public string response;
    public string intent;
    //public string error;

    /**
    * Constructor 
    **/
    public CloudResponseObject(string error)
    {
      //  this.error = error;
    }

    public void setAllFieldsToNull()
    {
        intent = response = null;
    }

}
