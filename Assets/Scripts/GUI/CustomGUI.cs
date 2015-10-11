using System;

using UnityEngine;
using System.Collections.Generic;

public class CustomGUI : MonoBehaviour
{
	public GameObject buttonPrefab;
	public InGameGUI inGameGUI;
	
	public Texture jump;
	public Texture attack;
	
	private SceneManager sceneManager;
	
	void Start()
	{
		sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		
		#if UNITY_ANDROID
			inGameGUI = GetComponent<InGameAndroidGUI>();
			inGameGUI.createButtons(buttonPrefab, jump, attack);
			
		#endif
		
		#if UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
			inGameGUI = GetComponent<InGameGUI>();
		#endif

	}
	
	void Update()
	{
		inGameGUI.checkEvents();
	}
	
	#if UNITY_ANDROID
		public void initializeButtonsAndroid()
		{
			sceneManager.getPlayer().GetComponent<Player>().initializeButtonsAndroid(inGameGUI.getButtons());
		}
	#endif

}

