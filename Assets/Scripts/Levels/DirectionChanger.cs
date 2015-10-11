using UnityEngine;
using System.Collections;

public class DirectionChanger : MonoBehaviour {

	void Start () 
	{
	
	}
	
	void Update () 
	{

	}
	
	void OnTriggerExit() 
	{
		collider.isTrigger = false;
	}
}
