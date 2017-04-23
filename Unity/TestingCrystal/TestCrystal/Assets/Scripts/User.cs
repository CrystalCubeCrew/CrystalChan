using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string firstName;
    public string lastName;
    public string response;
    public string error;
    public string userId;

    /**
    * Constructor 
    **/
    public User(string error)
    {
        this.error = error;
        userId = "none";
        firstName = lastName = response = null;
    }


}
