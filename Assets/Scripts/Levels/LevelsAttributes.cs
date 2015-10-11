using UnityEngine;
using System.Collections;

public class LevelsAttributes {
	
	public static int currentLevelId = 0;
	private Zone01Attributes zone01Attributes;

	public LevelsAttributes ()
	{
		zone01Attributes = new Zone01Attributes();
	}
	
	public Zone01Attributes getZone01() 
	{
		return zone01Attributes;
	}
	
	public Vector3 getInitialPositionFromID(int id) 
	{
		if ( id >= 0 && id < 10 )
		{
			return zone01Attributes.getPosition(id);
		}
		
		return Vector3.zero;
	}
}
