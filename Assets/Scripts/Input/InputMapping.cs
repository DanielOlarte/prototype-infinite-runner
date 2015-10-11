using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputMapping {
	
	public List<KeyCode> keysController1 = new List<KeyCode>(){ KeyCode.Space, 
															    KeyCode.F,
															    KeyCode.E };
	public List<KeyCode> keysControllerGP1 = new List<KeyCode>(){ KeyCode.Joystick1Button5, 
															  	  KeyCode.Joystick1Button0, 
															   	  KeyCode.Joystick1Button2 };

	public string idController = "Keyboard";
	
	
	public InputMapping() 
	{
	}
}
