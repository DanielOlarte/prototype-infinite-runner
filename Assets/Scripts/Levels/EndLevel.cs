using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour {

	void Start () 
	{
	
	}
	
	void Update () 
	{
	
	}
	
	void OnTriggerEnter(Collider collisionInfo) 
	{
		if(collisionInfo.gameObject.tag.Equals("Player"))
		{
			Player player = collisionInfo.gameObject.GetComponent<Player>();
			player.setFinishedLevel();
		}
	}
}
