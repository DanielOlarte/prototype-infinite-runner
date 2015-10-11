using UnityEngine;
using System.Collections;

public class StringUtils {

	public static string getLevelNumber(int id)
	{
		string level = "";
		id += 1;
		
		if ( id < 10 )
		{
			level += "0";
		}
		
		level += id.ToString();

		return level;
	}
}
