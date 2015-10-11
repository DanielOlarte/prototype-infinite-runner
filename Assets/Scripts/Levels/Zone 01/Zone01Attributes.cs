using UnityEngine;
using System;
using System.Collections.Generic;

public class Zone01Attributes
{
	public string DIRECTION_CHANGER = "DirectionChanger";
	public string RIGHT = "Right";
	public string LEFT = "Left";
	
	public string BAD_TREE = "BadTree";
	public string BAD_TREE_CHANGE = "BadTreeChange";
	public string CHUZOS = "Chuzos";
	public string CHUZOS_CHANGE = "ChuzosChange";
	
	public string WALL_UP = "WallUp";
	public string WALL_DOWN = "WallDown";
	
	public string INCLINATE = "Inclinate";
	
	private BadTree badTree = new BadTree();
	private Chuzos chuzos = new Chuzos();
	private Inclinate inclinate = new Inclinate();
	
	private List<Vector3> initialPositions;
	
	public Zone01Attributes ()
	{
		initialPositions = new List<Vector3>();
		initialPositions.Add(new Vector3(0.0f, 9.0f, -225.0f));
		initialPositions.Add(new Vector3(0.0f, 9.0f, -225.0f));
	}
	
	public Vector3 getPosition(int index) 
	{
		return initialPositions[index];
	}
	
	public void actBadTree(ref bool isDead)
	{
		badTree.action(ref isDead);
	}
	
	public void actChuzos(ref bool isDead)
	{
		chuzos.action(ref isDead);
	}

	public void actInclinate(ref bool isSliding)
	{
		inclinate.action(ref isSliding);
	}
}

