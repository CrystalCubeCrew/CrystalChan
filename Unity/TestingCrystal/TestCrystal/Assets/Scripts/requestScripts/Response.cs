
/**
* @purpose Json serialization (Json -> Object) 
**/

public class Response {

	public string response;
  public string error;

  /**
  * Constructor 
  **/
  public Response (string error) {
    this.error = error;
  }


}
