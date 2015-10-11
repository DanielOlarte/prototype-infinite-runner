using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
	public const float RIGHT = 1.0f;
	public const float LEFT = -1.0f;
	public const float UP = 1.0f;
	public const float DOWN = -1.0f;

	public float speed = 36.0f; 
	private float maxSpeed = 36.0f;
	
	public float speedSliding = 20.0f;
	private float maxSpeedSliding = 20.0f;
	
	public float jumpSpeed = 19.0f;
	private float jumpDownSpeed = 8.0f;
	private float doubleJumpSpeed = 15.0f;
	private float doubleJumpDownSpeed = 4.0f;
	
	public float gravity = 27.0f;
	public float direction = RIGHT;
	
	private int collectedCoins = 0;
	private bool isDead = false;
	private bool finishedLevel;
	
	public Vector3 playerSpeed = new Vector3(0.0f, 0.0f, 0.0f); 
	
	private int jumpCounter = 0;
	public bool isGrounded = false;
	public bool isOnWallUp = false;
	public bool isOnWallDown = false;
	public bool isSliding = false;

	public float rayDistance;
	
	public float lastJumpTime;
	
	public Vector3 impact = Vector3.zero;

	private SceneManager sceneManager;
	private CharacterController controller;
	private PlayerFSM fsm;
	private InputManager inputManager;
	
	private Zone01Attributes zone01;
	
	void Start () 
	{
		sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		controller = GetComponent<CharacterController>();
		fsm = GetComponent<PlayerFSM>();
		zone01 = sceneManager.getLevelsAttributes().getZone01();
		
		rayDistance = controller.height * .7f + controller.radius;
	}

	void Update () 
	{
		Vector3 velocity = transform.forward * direction * speed;
		
		isGrounded = checkGroundCollision();
		checkForwardCollision();
		changeState();
		applyPhysics(ref velocity);
		
		CollisionFlags collisionFlags = movePlayer(velocity);
		if ( (collisionFlags & CollisionFlags.CollidedAbove) != 0 ) 
		{
			if (playerSpeed.y > 0) 
			{
            	playerSpeed.y = -playerSpeed.y;
        	}
		}
		
		if (transform.position.x != 0)
		{
			Vector3 newPosition = transform.position;
			newPosition.x = 0;
			transform.position = newPosition;
		}
		
		checkIsDead();
	}
	
	void changeState() 
	{
		checkSlidingState();
		
		if(inputManager.getKeyDown(PlayerKeys.JUMP))
		{
			if(isGrounded)
			{
				if(fsm.validateNewAction(PlayerActions.JUMP_INPUT))
				{
					jump(jumpSpeed, UP);
				}
			}
			else if(isOnWallUp || isOnWallDown)
			{
				if(fsm.validateNewAction(PlayerActions.JUMP_INPUT))
				{
					float jumpDirection = isOnWallUp ? UP : DOWN ;
					float jumpSpeedWall = isOnWallUp ? jumpSpeed : jumpDownSpeed ;
					direction = (direction == RIGHT) ? LEFT : RIGHT; 
					jump(jumpSpeedWall, jumpDirection);
				}
			}
			else
			{
				doubleJump();
			}
			
		}  
		else if (isOnWallUp && !isGrounded)
		{
			fsm.validateNewAction(PlayerActions.WALL_SLIDE);
		}

		else if (!isGrounded)
		{
			fsm.validateNewAction(PlayerActions.FALL);
		}
		
		else if (isGrounded)
		{
			fsm.validateNewAction(PlayerActions.RUN);
		}
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit) 
	{
		String colliderName = hit.collider.name;
		
		if (colliderName.Equals(zone01.BAD_TREE)) 
		{
			zone01.actBadTree(ref isDead);
		} 
		
		if (colliderName.Equals(zone01.CHUZOS)) 
		{
			zone01.actChuzos(ref isDead);
		} 

		if (colliderName.Equals(zone01.INCLINATE)) 
		{
			zone01.actInclinate(ref isSliding);
		}
		
		if (colliderName.Contains(zone01.DIRECTION_CHANGER) ) 
		{
			if ( colliderName.Substring(zone01.DIRECTION_CHANGER.Length).Equals(zone01.LEFT) ) 
			{
				direction = (direction == RIGHT) ? LEFT : RIGHT;
			}
			
			if ( colliderName.Substring(zone01.DIRECTION_CHANGER.Length).Equals(zone01.RIGHT) ) 
			{
				direction = (direction == LEFT) ? RIGHT : LEFT;
			}
		}
    }
	
	private bool checkGroundCollision()
	{
		RaycastHit hitInfo;
		Debug.DrawRay(transform.position, -transform.up, Color.red);
		bool isOnGround = false;
		
     	if( Physics.Raycast(transform.position, -transform.up , out hitInfo, rayDistance) ) 
		{
			string colliderName = hitInfo.collider.name;
			if ( colliderName.Equals(zone01.INCLINATE) || colliderName.Equals("Cube") ) 
			{
				Vector3 axis = Vector3.Cross(-transform.up, -hitInfo.normal);
				float angle = 0.0f;
				
	    		if(axis != Vector3.zero)
				{
					angle = Mathf.Atan2(Vector3.Magnitude(axis), Vector3.Dot(-transform.up, -hitInfo.normal));
					transform.RotateAround(axis, angle);	
				}
			}
			
			isOnGround = colliderName.Equals(zone01.WALL_UP) ? false : true;
			isOnGround = colliderName.Equals(zone01.WALL_DOWN) ? false : true;
		}
		
		isSliding = isOnGround ? isSliding : false;
		return isOnGround;
	}
	
	private void checkForwardCollision()
	{
		RaycastHit hitInfo;
		Debug.DrawRay(transform.position, transform.forward * direction, Color.green);
		
		isOnWallUp = false;
		isOnWallDown = false;
		
		if(Physics.Raycast(transform.position, transform.forward * direction , out hitInfo, rayDistance) )
		{
			String colliderName = hitInfo.collider.name;
			
			if ( colliderName.Equals(zone01.WALL_UP) || colliderName.Equals(zone01.WALL_DOWN) ) 
			{
				Vector3 axis = Vector3.Cross(direction*transform.forward, -hitInfo.normal);
				float angle = 0.0f;
				
	    		if(axis != Vector3.zero)
				{
					angle = Mathf.Atan2(Vector3.Magnitude(axis), Vector3.Dot(direction*transform.forward, -hitInfo.normal));
					transform.RotateAround(axis, angle);	
				}

				isOnWallUp = colliderName.Equals(zone01.WALL_UP) ? true : false;
				isOnWallDown = colliderName.Equals(zone01.WALL_DOWN) ? true : false;
				jumpCounter = 0;
			}
		}
	}
	
	private void jump(float jumpTempSpeed, float direction)
	{
		playerSpeed.y = jumpTempSpeed*direction;
		isSliding = false; 
		jumpCounter += 1;
		lastJumpTime = Time.time;
	}
	
	private bool doubleJump()
	{
		if ( jumpCounter > 0 && Time.time > lastJumpTime + 0.25f ) 
		{
			playerSpeed.y += doubleJumpSpeed;
			lastJumpTime = Time.time;
			jumpCounter = 0;
			return true;
		}
		return false;
	}
	
	private void applyPhysics(ref Vector3 velocity)
	{
		if (!isSliding) 
		{
			playerSpeed.y -= gravity * Time.deltaTime;
			velocity.y = playerSpeed.y; 
		}
	}
	
	private CollisionFlags movePlayer(Vector3 velocity)
	{
		CollisionFlags collisionFlags = CollisionFlags.None;
		
		if (impact.sqrMagnitude > 0.2f) 
		{
			impact.y -= gravity * Time.deltaTime;
			collisionFlags = controller.Move(impact * Time.deltaTime);
			impact = Vector3.Lerp(impact, Vector3.zero, 5*Time.deltaTime);
			speed = 0.0f;
		} 
		else 
		{
			collisionFlags = controller.Move(velocity * Time.deltaTime);
			speed = Mathf.Lerp(speed, maxSpeed, 2*Time.deltaTime);
			speedSliding = Mathf.Lerp(speedSliding, maxSpeedSliding, 2*Time.deltaTime);
		}
		
		playerSpeed.z = velocity.z;
		return collisionFlags;	
	}
	
	private void checkSlidingState()
	{
		if (isSliding || controller.isGrounded)
		{
			impact.y = 0.0f;
			playerSpeed.y = 0.0f;
		}
	}
	
	private void checkIsDead()
	{
		if( isDead )
		{
			sceneManager.restartLevel();
		}
		
		if( finishedLevel )
		{
			sceneManager.advanceLevel();
		}
	}
	
	public void initializeInputManager(string id, List<KeyCode> keys)
	{
		switch ( id )
		{
			case "Android":
			{
				inputManager = GetComponent<ControllerAndroid>();
				GetComponent<ControllerAndroid>().enabled = true;
				break;
			}
			default:
			{
				inputManager = GetComponent<InputManager>();
				break;
			}
		}
		inputManager.setController(id);
		inputManager.setKeys(keys);
	}
	
	#if UNITY_ANDROID
		public void initializeButtonsAndroid(List<Button> buttons)
		{
			inputManager.addButtons(buttons);
		}
	#endif
	
	public bool isPlayerDead()
	{
		return isDead;
	}
	
	public PlayerFSM getFSM()
	{
		return fsm;
	}	
	
	public void addCoins(int number = 1)
	{
		collectedCoins += number;
	}
	
	public int getCollectedCoins()
	{
		return collectedCoins;
	}
	
	public void setFinishedLevel(bool finished = true)
	{
		finishedLevel = finished;
	}
}
