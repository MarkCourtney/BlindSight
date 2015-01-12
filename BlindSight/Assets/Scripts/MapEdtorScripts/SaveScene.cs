/* Mark Courtney
 * C09588817
 * Save Scene
 * 
 * Provides additional interfaces for saving an environment.
 * A name can be chosen for a map or saving can be cancelled
 * during the naming process. Any map that is saved will
 * be sent to the Custom Maps folder, shown below
 * 
 * /Documents/Maps/Custom Maps
 * 
 * The user is brought back to the main menu after a map has
 * been saved.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class SaveScene : MonoBehaviour {

 	public List<Vector3> gameObjectPositions;
	public List<GameObject> gameObjectNames;
	List<String> unwantedObjects = new List<String>();

	public GUIStyle style;

	string fileName;
	string path;
	string folderName;
	
	int totalInteractiveObjects, totalPlayers;
	float timer = 0;
	

	public bool saving;
	bool errorMessage;

	CreateFloorWalls cFW;
	StreamWriter sw;
	MapEditorGUI mEGUI;
	
	void Start() {
		
		cFW = GetComponent<CreateFloorWalls>();
		mEGUI = GetComponent<MapEditorGUI>();
		
		gameObjectPositions = new List<Vector3>();
		gameObjectNames = new List<GameObject>();

		errorMessage = false;
		saving = false;
		
		totalInteractiveObjects = 0;

		unwantedObjects.Add("PreRenderLight");
		unwantedObjects.Add("PreRenderCamera");
		unwantedObjects.Add("Camera");
		unwantedObjects.Add("Preview");
		unwantedObjects.Add("default");
		unwantedObjects.Add("Particle");
		unwantedObjects.Add("Scene");
		

		// Set the location to save custom maps
		fileName = "";
		folderName = "Custom Maps";
		path = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Maps/" + folderName;
	}
	
	
	// Display a message in the center of the screen
	IEnumerator displayMessage(string message)
	{
		timer += Time.deltaTime;
			
		if(timer < 7)
		{
			GUILayout.BeginArea(new Rect(Screen.width/2 - 200, Screen.height/2 - 40, 500, 200));
			GUILayout.Label(message, style);
			GUILayout.EndArea();
			
			yield return null;
		}
		else
		{
			errorMessage = false;
			timer = 0;
			yield return null;
		}
	}
	
	
	void OnGUI()
	{
		if(errorMessage)
		{
			if(totalPlayers != 1)
				StartCoroutine(displayMessage("Maps must contain at least and only one Player!"));	
			else if(totalInteractiveObjects != 1)
				StartCoroutine(displayMessage("Objective based maps must contain a single interactive object!"));	
		}
		// Save the positions of all the objects in the scene
		if(GUI.Button(new Rect(Screen.width - 200, 120, 200, 40), "Save Scene"))
		{
			GameObject[] gameObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
			
			totalInteractiveObjects = GameObject.FindGameObjectsWithTag("InteractiveObject").Length;
			totalPlayers = GameObject.FindGameObjectsWithTag("InteractiveObject").Length;


			// Ensures objective rooms have only one player and interactive object
			// Also that free roam option has only one player
			if((mEGUI.currentRoomMode == "Objective Based" && (totalInteractiveObjects == 1 || totalPlayers == 1))|| mEGUI.currentRoomMode == "Free Roam" && totalPlayers == 1)
			{
				foreach(GameObject activeObject in gameObjects)
				{
					// Return all the currently active items
					if (activeObject.activeInHierarchy)
					{
						// Check if match for bad stuff
						bool checkForMatch = unwantedObjects.Contains(activeObject.transform.name);
						print(activeObject.transform.name + checkForMatch);
						// Remove any objects that aren't needed for the navigation gameplay
						if(!checkForMatch)
						{
							gameObjectNames.Add(activeObject);
						}

					}
				}

				// If everything has been validated then transfer to the map name entry
				saving = true;
			}
			else
			{
				errorMessage = true;
			}
			
		}
	
	
		// When the saving button has been clicked
		if(saving)
		{
			// Text field with name of the map
			fileName = GUI.TextField(new Rect((Screen.width - 100) / 2, 30, 100, 20), fileName, 30);


			// Button for creating the map
			// When Create is clicked
			if(GUI.Button(new Rect(Screen.width / 2 + 50, 30, 50, 20), "Create"))
			{
				// Create and open the .txt file
				sw = new StreamWriter(path + '/' + fileName + ".txt");
				
				// Write the room shape on the first line
				sw.WriteLine(cFW.roomShape);
				sw.WriteLine(mEGUI.currentRoomMode);

				// Place objects in the text file
				foreach(GameObject gON in gameObjectNames)
				{
					print(gON.transform.name);

					// If items are lost in the virtual environment then don't add these to the complete level
					if(gON.transform.position.y > -1)
					{
						// Write the objects name, position and rotation to the file
						sw.Write(gON.transform.name + ":");
						sw.Write(gON.transform.position + ":");	
						sw.Write(gON.transform.rotation + ":");
						sw.WriteLine("(" + gON.transform.localScale.x + ", " + gON.transform.localScale.y + ", " + gON.transform.localScale.z + ")");
					}
				}

				// Close the newly create file
				sw.Close();
				
				
				// Return to the main menu after saving the environment
				Application.LoadLevel("Menu");
			}

			// Button for exiting back to the environment builder
			// When Create is clicked
			if(GUI.Button(new Rect(Screen.width / 2 + 100, 30, 50, 20), "Exit"))
			{
				// Remove any objects that were added when the save button was clicked
				// Otherwise while creating a file it will try access deleted objects
				gameObjectNames.Clear();

				saving = false;
			}
		}
	}
}