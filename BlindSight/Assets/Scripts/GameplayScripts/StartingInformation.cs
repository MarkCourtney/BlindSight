/* Mark Courtney
 * C09588817
 * Starting Information
 * 
 * Provides an audio clip that will give the user an idea of 
 * what they are trying to achieve in that specific environment.
 * The class finds the interactive object in the scene and tells
 * the user to find that specific object.
*/

using UnityEngine;
using System.Collections;

public class StartingInformation : GamePadController {

	public AudioClip[] objectiveClips;
	public AudioClip[] roomShapeClips;

	GameObject loadLevel;
	LoadLevelScript lLS;
	string roomShape;
	AudioClip firstClip, secondClip;

	// Use this for initialization
	void Start () {

		// Access LoadLevel object and the script LoadLevelScript
		loadLevel = GameObject.Find("LoadLevel");
		lLS = loadLevel.GetComponent<LoadLevelScript>();

		roomShape = lLS.roomShape;

		if(lLS.roomMode == "Objective Based")
		{
			GameObject interactiveObject = GameObject.FindGameObjectWithTag("InteractiveObject");

			if(interactiveObject.name == "Kettle(Clone)")
			{
				firstClip = objectiveClips[0];
			}
			else if(interactiveObject.name == "LightSwitch(Clone)")
			{
				firstClip = objectiveClips[1];
			}
			else if(interactiveObject.name == "Sink(Clone)")
			{
				firstClip = objectiveClips[2];
			}
			else if(interactiveObject.name == "Telephone(Clone)")
			{
				firstClip = objectiveClips[3];
			}
			else if(interactiveObject.name == "TargetDoor(Clone)")
			{
				firstClip = objectiveClips[4];
			}
			else if(interactiveObject.name == "TargetWindow(Clone)")
			{
				firstClip = objectiveClips[5];
			}

			if(roomShape == "Square")
			{
				secondClip = roomShapeClips[0];
			}
			else if(roomShape == "Rectangle")
			{
				secondClip = roomShapeClips[1];
			}
			else if(roomShape == "LShaped")
			{
				secondClip = roomShapeClips[2];
			}
			else if(roomShape == "Cross")
			{
				secondClip = roomShapeClips[3];
			}

			StartCoroutine(playMultipleClips(firstClip, secondClip));
		}
	}
}
