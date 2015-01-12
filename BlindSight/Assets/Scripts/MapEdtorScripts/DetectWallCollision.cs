/* Mark Courtney
 * C09588817
 * Detect Wall Collisions
 * 
 * Detects if an object is colliding with the walls when it's
 * final position is being set. In most cases objects shouldn't 
 * collide, but in special conditions objects will need to be 
 * attached to the wall to give a correct depiction of a room.
 * For example doors and light switch needs to be fitted onto 
 * the walls.
*/

using UnityEngine;
using System.Collections;

public class DetectWallCollision : MonoBehaviour {
	
	GameObject objectHit;
	
	public static bool collide = false;
	

	// If an objects collision box collides with a wall
	// Execute the OnCollisionEnter once
	void OnCollisionEnter(Collision collision)
	{
		// Special condition for door, light switch and window
		if(collision.gameObject.name.Contains("Door") || collision.gameObject.name.Contains("Window") || collision.gameObject.name.Contains("Switch"))
		{
			collide = false;
		}
		// Determine if an object is clipping the wall
		else if(collision.gameObject.name == CameraEditObject.objectHit.gameObject.name)
		{
			collide = true;	
		}
	}


	// If an objects collision box stops colliding with a wall
	// Execute the OnCollisionExit once
	void OnCollisionExit(Collision collision)
	{
		// Special condition for door, light switch and window
		if(collision.gameObject.name == "Door" || collision.gameObject.name == "LightSwitch" || collision.gameObject.name == "Window"  || collision.gameObject.name == "TargetDoor" || collision.gameObject.name == "TargetWindow")
		{
			collide = true;
		}
		// Determine if an object is no longer clipping
		else if(collision.gameObject.name == CameraEditObject.objectHit.gameObject.name)
		{
			collide = false;
		}
	}
	
	public bool checkClipping()
	{
		return collide;
	}
}