/* Mark Courtney
 * C09588817
 * Hint System
 * 
 * The hint systems functionality is developed in this class.
 * A user is given three distinct hints based on the various 
 * details of the room if they wish.
*/


using UnityEngine;
using System.Collections;
using XInputDotNetPure;

//http://answers.unity3d.com/questions/42321/can-i-reference-a-gameobject-in-a-script-attached.html

public class HintSystem : GamePadController {
	
	public GameObject loadLevel, interactiveObject;
	public AudioClip[] distanceFromStartHint;
	public AudioClip[] distanceToWall;
	
	LoadLevelScript lL;
	
	float distToObject;
	int currentHint;
	
	string roomShape;

	GameObject player;
	Vector3 startingPosition;
	float distanceFromStart;


	
	Ray raycastToWall;
	RaycastHit info;
	
	float distToWall;


	void Start () {

		distToObject = 0.0f;

		player = GameObject.FindGameObjectWithTag("Player");

		startingPosition = player.transform.position;
		
		interactiveObject = GameObject.FindGameObjectWithTag("InteractiveObject");
	}


	void Update () {
	
		gamePadState = GamePad.GetState(0);

		// Only allow a hint if audio isn't currently playing
		if(checkBumperPressed(gamePadState) && !audio.isPlaying)
		{	
			distanceFromStart = Vector3.Distance(player.transform.position, startingPosition);

			// Output audio based on distance from starting location
			if(distanceFromStart < 5.0f)
			{
				playAudioClip(distanceFromStartHint[0]);
			}
			else if(distanceFromStart <= 10.0f)
			{
				playAudioClip(distanceFromStartHint[1]);
			}
			else if(distanceFromStart <= 15.0f)
			{
				playAudioClip(distanceFromStartHint[2]);
			}
			else if(distanceFromStart >= 15.0f)
			{
				playAudioClip(distanceFromStartHint[3]);
			}
		}
		

		if(checkTriggerPressed(gamePadState) && !audio.isPlaying)
		{
			raycastToWall = new Ray(transform.position + transform.forward * 0.01f, transform.forward);

			if(Physics.Raycast(raycastToWall, out info, 20))
			{
				distToWall = Vector3.Distance(transform.position, info.transform.position);

				// Output audio based on distance from wall
				// In the facing direction
				if(distToWall < 1)
				{
					playAudioClip(distanceToWall[0]);
				}
				else if(distToWall < 2)
				{
					playAudioClip(distanceToWall[1]);
				}
				else if(distToWall < 3)
				{
					playAudioClip(distanceToWall[2]);
				}
				else if(distToWall < 4)
				{
					playAudioClip(distanceToWall[3]);
				}
				else if(distToWall < 5)
				{
					playAudioClip(distanceToWall[4]);
				}
				else if(distToWall >= 5)
				{
					playAudioClip(distanceToWall[5]);
				}
			}
		}
	}
}
