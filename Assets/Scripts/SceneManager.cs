using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour {
	
	public int sceneID;

	public GameObject playerPrefab;
	
	private string currentPlayerPrefabName = "PlayerGhost01";
	private GameObject player;
	private Player playerScript;
	
	private InputMapping inputMapping;
	private LevelsAttributes levelsAttributes;
	
	private GUIStyle nameStyle;
	private Rect namePlayer, coinsRect;
	
	void Start () 
	{
		inputMapping = new InputMapping();
		levelsAttributes = new LevelsAttributes();
		
		sceneID = LevelsAttributes.currentLevelId;
		
		#if UNITY_ANDROID
			inputMapping.idController = "Android";

			instantiatePlayer(sceneID, PlayerCharacter.GHOST, levelsAttributes.getInitialPositionFromID(sceneID), 
							  inputMapping.idController, getKeysBasedOnController(inputMapping.idController) );	
		
			CustomGUI gui = GameObject.Find ("CustomGUI").GetComponent<CustomGUI>();
			gui.initializeButtonsAndroid();
		#endif
		
		#if UNITY_STANDALONE_WIN || UNITY_WEBPLAYER
			string idController = inputMapping.idController;
			instantiatePlayer(sceneID, PlayerCharacter.GHOST, levelsAttributes.getInitialPositionFromID(sceneID), 
							  idController, getKeysBasedOnController(idController) );	
		#endif
		
		SizeFixer sizeFixer = new SizeFixer();
		namePlayer = sizeFixer.getRectFixResolution(1900, 20, 300, 200);
		coinsRect = sizeFixer.getRectFixResolution(1600, 20, 100, 50);
		
		nameStyle = new GUIStyle();
		nameStyle.fontSize = 17;
	}
	
	void Update () 
	{
	
	}
	
	void OnGUI() 
	{
        GUI.Label(namePlayer, "Ghost", nameStyle);
		GUI.Label(coinsRect, playerScript.getCollectedCoins().ToString(), nameStyle);
    }
	
	private void instantiatePlayer(int sceneID, PlayerCharacter playerCharacter, Vector3 position, string idController, List<KeyCode> listKeys)
	{
		player = (GameObject)Instantiate(playerPrefab, position, playerPrefab.transform.rotation);
		
		playerScript = player.GetComponent<Player>();
		playerScript.initializeInputManager(idController, listKeys);
	}

	public string getCurrentPlayerPrefabName() 
	{
		return currentPlayerPrefabName;
	}
	
	public List<KeyCode> getKeysBasedOnController(string id)
	{
		switch ( id )
		{
			case "Keyboard":
			{
				return inputMapping.keysController1;
			}
			case "Controller":
			{
				return inputMapping.keysControllerGP1;
			}
		}
		
		return new List<KeyCode>();
	}

	public void restartLevel()
	{
		StartCoroutine( startWait(2.0f, StringUtils.getLevelNumber(sceneID)) );
		playerScript.GetComponent<Player>().enabled = false;
	}
	
	public void advanceLevel()
	{
		sceneID += 1;
		LevelsAttributes.currentLevelId = sceneID;
		
		StartCoroutine( startWait(2.0f, StringUtils.getLevelNumber(sceneID)) );
		playerScript.GetComponent<Player>().enabled = false;
	}

	private IEnumerator startWait(float seconds, string level)
	{
		yield return StartCoroutine( waitSeconds(seconds, level) );
	}
	
	private IEnumerator waitSeconds(float seconds, string level)
	{
		yield return new WaitForSeconds(seconds);
		Application.LoadLevel("Level" + level + "-Ghost");
	}
	
	public Player getPlayer()
	{
		return playerScript;
	}
	
	public GameObject getGameObjectPlayer()
	{
		return player;
	}
	
	public LevelsAttributes getLevelsAttributes()
	{
		return levelsAttributes;
	}
}
