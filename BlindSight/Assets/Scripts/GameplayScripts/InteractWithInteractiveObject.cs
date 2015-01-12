/* Mark Courtney
 * C09588817
 * Menu Selection
 * 
 * This class allows the user to interact with objects located
 * in an environment. If an object is passive a sound clip will 
 * be played to determine what the user is beside. Where as if
 * the object is interactive the controller will vibrate to 
 * inform the user of this specific object. 
*/

//http://stackoverflow.com/questions/5188561/signed-angle-between-two-3d-vectors-with-same-origin-within-the-same-plane-reci

using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class InteractWithInteractiveObject : GamePadController {

	GameObject interactiveObject;
	public AudioClip[] audioClips;
	public AudioClip completeMap;

	Camera camera;
	
	Vector3 screenPos;
	float dist;
	bool environmentComplete;
	FadeInOut fIO;

	Vector3 playerDir, desiredDir;
	float angle;
	float time;
	bool oscillate;
	float currentDist;
	
	void Start () {

		fIO = GetComponent<FadeInOut>();

		// Find the interactive object in the scene
		interactiveObject = GameObject.FindGameObjectWithTag("InteractiveObject");


		// Destroy this script if the room is 
		if(interactiveObject == null)
		{
			Destroy(gameObject.GetComponent("InteractWithInteractiveObject"));
		}

		camera = Camera.main;
		
		screenPos = Vector3.zero;
		dist = 0.0f;

		oscillate = false;
	}


	/****************************************
	 * The functionality within the 		*
	 * calculateAngle() method for creating *
	 * an angle between the current and 	*
	 * desired vectors is taken from the 	*
	 * web link at the top of this file	 	*
	 * **************************************/

	// Calculate the angle between the player look vector 
	// and the angle required to look at the interactive object
	void calculateAngle()
	{
		angle = Vector3.Angle(playerDir.normalized, desiredDir.normalized);
		Vector3 cross = Vector3.Cross (transform.forward, desiredDir.normalized);

		// Makes the angle a negative 
		// Determines if object is left or right 
		if(cross.y > 0.0f)
		{
			angle = -angle;
		}
	}



	// Implementation of multiple vibrations type
	void vibrateBasedOnDistance()
	{
		// If the distance to the object is less than 5
		// Then vibrate based on the rotation of the character
		// In relation to the angle
		if(dist < 5)
		{
			if(angle > 120 || angle < -120)
			{
				GamePad.SetVibration(0, .1f, .1f);
			}
			else if(angle > 35)
			{
				GamePad.SetVibration(0, (1 - (dist /5)), 0);
			}
			else if(angle < -35)
			{
				GamePad.SetVibration(0, 0, (1 - (dist /5)));
			}
			else
			{
				GamePad.SetVibration(0, (1 - (dist /5)), (1 - (dist /5)));
			}
		}
		else
		{
			GamePad.SetVibration(0, 0, 0);
		}
	}



	void vibrateOscillationIncreaseIntervals()
	{
		if(oscillate)
		{
			if(time > currentDist + 1)
			{
				oscillate = false;
				GamePad.SetVibration(0, 0, 0);
				time = 0;
			}
			else if(angle > 120 || angle < -120)
			{
				GamePad.SetVibration(0, .1f, .1f);
			}
			else if(angle > 35)
			{
				GamePad.SetVibration(0, (1 - (dist /5)), 0);
			}
			else if(angle < -35)
			{
				GamePad.SetVibration(0, 0, (1 - (dist /5)));
			}
			else
			{
				GamePad.SetVibration(0, (1 - (dist /5)), (1 - (dist /5)));
			}
		}

		else if(time > dist)
		{
			oscillate = true;
			currentDist = dist;
		}
		else
		{
			GamePad.SetVibration(0, 0, 0);
		}
	}






	void vibrateOscillationIntervals()
	{
		if(oscillate)
		{
			if(time > currentDist + 1)
			{
				oscillate = false;
				GamePad.SetVibration(0, 0, 0);
				time = 0;
			}
			else if(angle > 120 || angle < -120)
			{
				GamePad.SetVibration(0, .1f, .1f);
			}
			else if(angle > 35)
			{
				GamePad.SetVibration(0, 0.5f, 0);
			}
			else if(angle < -35)
			{
				GamePad.SetVibration(0, 0, 0.5f);
			}
			else
			{
				GamePad.SetVibration(0, 0.5f, 0.5f);
			}
		}
		
		else if(time > dist)
		{
			oscillate = true;
			currentDist = dist;
		}
		else
		{
			GamePad.SetVibration(0, 0, 0);
		}
	}



	
	// Update is called once per frame
	void Update () {

		gamePadState = GamePad.GetState(0);

		time += Time.deltaTime;

		playerDir = transform.forward;
		desiredDir = interactiveObject.transform.position - transform.position;

		// Distance from the interactive object
		// Used in calculating vibration rate
		dist = Vector3.Distance(transform.position, interactiveObject.transform.position) /2;


		calculateAngle ();

		// Can turn off each of the vibrations methods at any time
		//vibrateBasedOnDistance();
		//vibrateOscillationIncreaseIntervals();
		vibrateOscillationIntervals();

		// If the object is within 1.5 meters and visible on screen
		// Then the statement is true
		if(dist < 1.5f && interactiveObject.renderer.isVisible && !environmentComplete)
		{
			screenPos = camera.WorldToScreenPoint(interactiveObject.transform.position);
			
			// By getting the bounds of the screen take the middle third
			// Determine if the object is in this third
			if(screenPos.x > Screen.width / 3 && screenPos.y < Screen.width - Screen.width / 3)
			{
				// Pressed to interact with the object
				if(checkButtonPressed(gamePadState))
				{
					if(interactiveObject.name == "Kettle(Clone)")
					{
						interactiveObject.audio.volume = 1;
						interactiveObject.audio.clip = audioClips[0];
					}
					else if(interactiveObject.name == "LightSwitch(Clone)")
					{
						GameObject[] lights = GameObject.FindGameObjectsWithTag("PassiveObject");
						foreach(GameObject l in lights)
						{
							if(l.name == "LightSource(Clone)")
							{
								if(l.light.intensity == 2)
								{
									l.light.intensity = 0.5f;
								}
								else if(l.light.intensity == 0.5f)
								{
									l.light.intensity = 2f;
								}
							}
						}
					}
					else if(interactiveObject.name == "Sink(Clone)")
					{
						GameObject tapParticleSystem = GameObject.Find("WaterParticleSystem");
						print(tapParticleSystem);
						print(audioClips[2]);
						interactiveObject.audio.clip = audioClips[2];

						// Turn on/off the particle system
						// Indicates the tap is running
						if(tapParticleSystem.renderer.enabled == true)
						{
							tapParticleSystem.renderer.enabled = false;
						}
						else if(tapParticleSystem.renderer.enabled == false)
						{
							tapParticleSystem.renderer.enabled = true;
						}
					}
					else if(interactiveObject.name == "Telephone(Clone)")
					{
						interactiveObject.audio.clip = audioClips[3];
					}
					else if(interactiveObject.name == "TargetDoor(Clone)")
					{
						interactiveObject.audio.clip = audioClips[4];
					}
					else if(interactiveObject.name == "TargetWindow(Clone)")
					{
						// Turn the volume up for the wind noise
						// Indicates the window has been opened
						interactiveObject.audio.volume = 1;
					}

					playAudioClip(completeMap);
					environmentComplete = true;
				}
			}
		}
	}
}
