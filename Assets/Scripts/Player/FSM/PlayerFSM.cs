using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
	private List<FSMState> fsmStates;
	private PlayerStates currentState = PlayerStates.RUNNING;
	
	public PlayerFSM ()
	{
		fsmStates = new List<FSMState>();
		initializeDefStates();
	}
	
	public bool validateNewAction(PlayerActions newAction)
	{				
		FSMState current = getCurrentState();
		
		PlayerStates newState = current.validateNewAction(newAction);
		
		if(newState.Equals(PlayerStates.NULL))
		{
			return false;	
		}
		else
		{
			currentState = newState;
			return true;	
		}
	}
	
	public void setCurrentState(PlayerStates newState)
	{
		currentState = newState;
	}
	
	public FSMState getCurrentState()
	{
		return fsmStates.Find(
            delegate(FSMState st)
            {
                return st.getID() == currentState;
            }
        );
	}
	
	public void initializeDefStates()
	{
		FSMState fall = new FSMState(PlayerStates.FALLING);
		fall.addTransition(PlayerActions.RUN, PlayerStates.RUNNING);
		fall.addTransition(PlayerActions.JUMP_INPUT, PlayerStates.FALL_JUMP);
		fsmStates.Add(fall);
		
		FSMState run = new FSMState(PlayerStates.RUNNING);
		run.addTransition(PlayerActions.JUMP_INPUT, PlayerStates.JUMPING);
		run.addTransition(PlayerActions.FALL, PlayerStates.FALLING);			
		fsmStates.Add(run);
		
		FSMState jump = new FSMState(PlayerStates.JUMPING);
		jump.addTransition(PlayerActions.FALL, PlayerStates.FALLING);		
		jump.addTransition(PlayerActions.JUMP_INPUT, PlayerStates.DOUBLE_JUMPING);	
		jump.addTransition(PlayerActions.WALL_SLIDE, PlayerStates.WALL_SLIDING);	
		fsmStates.Add(jump);
		
		FSMState doubleJump = new FSMState(PlayerStates.DOUBLE_JUMPING);
		doubleJump.addTransition(PlayerActions.FALL, PlayerStates.FALLING);	
		doubleJump.addTransition(PlayerActions.WALL_SLIDE, PlayerStates.WALL_SLIDING);
		fsmStates.Add(doubleJump);
		
		FSMState fallJump = new FSMState(PlayerStates.FALL_JUMP);	
		fallJump.addTransition(PlayerActions.FALL, PlayerStates.RUNNING);
		fsmStates.Add(fallJump);

		FSMState wallSliding = new FSMState(PlayerStates.WALL_SLIDING);	
		wallSliding.addTransition(PlayerActions.RUN, PlayerStates.RUNNING);
		wallSliding.addTransition(PlayerActions.JUMP_INPUT, PlayerStates.JUMPING);
		fsmStates.Add(wallSliding);
	}
}

