using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraPlayer : MonoBehaviour {

	public float dampTime;
	public Transform target;

	private SceneManager sceneManager;
	private GameObject player;
	private Vector3 velocity = Vector3.zero;
	
	void Start () 
	{
		sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		player = sceneManager.getGameObjectPlayer();
		target = player.transform;
	}

	void Update () 
	{
		if(target) 
		{
	        Vector3 point = camera.WorldToViewportPoint(target.position);
	        Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
	        Vector3 destination = transform.position + delta;		
	        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
   	 	}
	}
}

       /* public float distance = 3.0f;
        public float height = 2.0f;
        public float damping = 4.0f;

        /*void Update () 
        {
        	Vector3 wantedPosition;
			wantedPosition = target.TransformPoint(distance*8.0f, height, 0.5f);
			transform.position = Vector3.Lerp (transform.position, wantedPosition, Time.deltaTime * damping);
		}*/