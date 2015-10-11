using System;
using System.Collections;
using System.Collections.Generic;

public class FSMState
{
	protected Dictionary<PlayerActions, PlayerStates> transitions = new Dictionary<PlayerActions, PlayerStates>();
	protected PlayerStates ID;
	
	public PlayerStates getID()
	{
		return ID;
	}
	
	public FSMState (PlayerStates newID)
	{
		ID = newID;
	}
	
	public void addTransition(PlayerActions newAction,PlayerStates newState)
	{
		transitions.Add(newAction,newState);
	}
	
	public PlayerStates validateNewAction(PlayerActions newAction)
	{
		if(transitions.ContainsKey(newAction))
		{
			return transitions[newAction];
		}
		
		return PlayerStates.NULL;
	}
}

