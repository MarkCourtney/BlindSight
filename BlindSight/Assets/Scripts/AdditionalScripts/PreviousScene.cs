/* Mark Courtney
 * C09588817
 * InteractWithPassiveObjects
 * 
 * Class that determines what the previous level was
 * Set as a global class to ensure all scenes have
 * access to level variable
*/

using UnityEngine;
using System.Collections;

public class PreviousScene : MonoBehaviour {

	// Static int to ensure the value doesn't change across scenes
	public static int currentLevel;
	
	void Start () 
	{
		currentLevel = Application.loadedLevel;
	}
}