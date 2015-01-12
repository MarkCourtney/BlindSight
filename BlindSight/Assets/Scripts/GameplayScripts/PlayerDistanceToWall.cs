/* Mark Courtney
 * C09588817
 * Player Distance To Wall
 * 
 * This class checks the distance to a wall facing in the users
 * direction. A sound clip is played to tell the user how
 * far they are from the wall.
*/

using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerDistanceToWall : GamePadController {
	
	public AudioClip[] distanceToWall;
	
	Ray raycastToWall;
	RaycastHit info;
	
	float dist;

	
	void Update () {

		gamePadState = GamePad.GetState(0);
		
		raycastToWall = new Ray(transform.position + transform.forward * 0.01f, transform.forward);
		Debug.DrawRay(transform.position + transform.forward * 0.01f, transform.forward * 20);

		// Only allow hint if audio is not currently on
		// Raycasts from the players position in the players direction
		if(Physics.Raycast(raycastToWall, out info, 20) && !audio.isPlaying)
		{
			dist = Vector3.Distance(transform.position, info.transform.position);
			
			if(Input.GetKeyDown(KeyCode.Space) || checkTriggerPressed(gamePadState))
			{
				if(dist < 1)
				{
					playAudioClip(distanceToWall[0]);
				}
				else if(dist < 2)
				{
					playAudioClip(distanceToWall[1]);
				}
				else if(dist < 3)
				{
					playAudioClip(distanceToWall[2]);
				}
				else if(dist < 4)
				{
					playAudioClip(distanceToWall[3]);
				}
				else if(dist < 5)
				{
					playAudioClip(distanceToWall[4]);
				}
				else if(dist >= 5)
				{
					playAudioClip(distanceToWall[5]);
				}
			}
		}
	}
}