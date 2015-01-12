/* Mark Courtney
 * C09588817
 * Map Editor GUI
 * 
 * The environment build's user interface is provided in
 * this class. A range of objects can be placed in the scene
 * including passive, interactive and player objects.
 * 
 * Settings within for an evironment can be changed to suit a
 * map or for easier placement of objects. These settings 
 * include hiding the walls, snap mode for either free angles 
 * or 45 degree angles of movement for each object, deciding 
 * if the room is objective based or free roam.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class MapEditorGUI : MonoBehaviour {

	public GUIStyle style;

	public GameObject[] passiveGameObjects;
	public GameObject[] interactiveGameObjects;
	public GameObject[] characterGameObjects;

	List<string> passiveObjectNames = new List<string>();
	List<string> interactiveObjectNames = new List<string>();
	List<string> characterObjectNames = new List<string>();

	
	GameObject playerObject, newObject;
	Ray ray;
	
	Quaternion quaternion;
	Vector3 mousePos;
	
	int buttonHeight;
	float timer;
	
	public string currentRoomMode = "Objective Based";
	string currentSnapMode = "On";
	string currentWallsMode = "Off";
	string escapeText = "Exit the Map Editor?";
	string objectHitName = "";
	
	bool passiveItems, interactiveItems, playerTypes, exitEditor, errorMessage;


	CameraEditObject cEO;
	SaveScene sS;
	
	void Start () {

		passiveObjectNames.Add("Bin");
		passiveObjectNames.Add("Chair");
		passiveObjectNames.Add("Counter Small");
		passiveObjectNames.Add("Counter Large");
		passiveObjectNames.Add("Door");
		passiveObjectNames.Add("Fire");
		passiveObjectNames.Add("Table Small");
		passiveObjectNames.Add("Table Large");
		passiveObjectNames.Add("Window");

		interactiveObjectNames.Add("Door");
		interactiveObjectNames.Add("Kettle");
		interactiveObjectNames.Add("Light Switch");
		interactiveObjectNames.Add("Sink");
		interactiveObjectNames.Add("Telephone");
		interactiveObjectNames.Add("Window");

		characterObjectNames.Add("Male");
		characterObjectNames.Add("Female");
		characterObjectNames.Add("Child");



		cEO = GetComponent<CameraEditObject>();
		sS = GetComponent<SaveScene>();
		
		passiveItems = false;
		interactiveItems = false;
		playerTypes = false;
		exitEditor = false;
		errorMessage = false;
		
		// Set the default rotation of the objects spawned
		quaternion = Quaternion.identity;
		
		buttonHeight = 40;
		timer = 0.0f;
	}


	// Display a message in the center of the screen
	IEnumerator displayMessage(string message)
	{
		timer += Time.deltaTime;
		
		if(timer < 7)
		{
			GUILayout.BeginArea(new Rect(Screen.width/2 - 100, Screen.height/2 - 40, 500, 200));
			GUILayout.Label(message, style);
			GUILayout.EndArea();
			
			yield return null;
		}
		else
		{
			timer = 0;
			errorMessage = false;
			yield return null;
		}
	}

	
	void OnGUI() {


		if(cEO.held)
		{
			objectHitName = CameraEditObject.objectHit.transform.name;
		}

		mousePos = Input.mousePosition;
		
		// Set the minimum x and y positions
		// Used later for the ray
		mousePos.y = Screen.height / 1.5f;
		if(mousePos.x < 200)
			mousePos.x = 500;
		
		
		// Print a label to the screen when specific objects don't collide with the wall
		// These include: door, window and switches
		if((objectHitName.Contains("Door") || objectHitName.Contains("Window") || objectHitName.Contains("Switch")) && cEO.wallCollide)
		{
			timer += Time.deltaTime;
			
			if(timer < 5)
			{
				GUILayout.BeginArea(new Rect(Screen.width/2 - 100, Screen.height/2 - 40, 300, 200));

				GUILayout.Label("The object must be part of the wall", style);

				GUILayout.EndArea();
			}
			else
			{
				timer = 0.0f;
				cEO.wallCollide = false;	
			}
		}

		// For all other objects, that aren't meant to collide with a wall
		// print a string to the screen
		else if(cEO.wallCollide)
		{
			timer += Time.deltaTime;
			
			if(timer < 5)
			{
				GUILayout.BeginArea(new Rect(Screen.width/2 - 100, Screen.height/2 - 40, 300, 200));

				GUILayout.Label("The object must not collide with the wall", style);
				GUILayout.EndArea();
			}
			else
			{
				timer = 0.0f;
				cEO.wallCollide = false;	
			}
		}
		
		
		// Open the menu to exit from the editor
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			exitEditor = true;
		}
		// Allow the user to select if they wish to exit the editor
		if(exitEditor)
		{
			GUILayout.BeginArea(new Rect(Screen.width/2 - 25, Screen.height/2 - 40, 300, 200));
			GUILayout.Label(escapeText);
			
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Yes", GUILayout.Width(58)))
			{
				Application.LoadLevel("Menu");
			}
			if(GUILayout.Button("No", GUILayout.Width(58)))
			{
				exitEditor = false;
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		
		
		
		
		// GUI appears if the floor size is determined and an object isn't being held
		if(cEO.held || !sS.saving)
		{
			// Convert the mouses position to a ray
			// Used in clicking on GUI items
			ray = Camera.main.ScreenPointToRay(mousePos);


			// Begin a new layout area
			GUILayout.BeginArea(new Rect(0, 0, 250, 30));
			GUILayout.BeginHorizontal();


				// Colour code to indicate selected option
				if(passiveItems)
					GUI.color = Color.green;
				else
					GUI.color = Color.white;


				// Turning on/off the passive menu
				if(GUILayout.Button("Passive", GUILayout.Height(30)))
				{
					if(passiveItems)
					{
						passiveItems = false;
					}
					else
					{
						interactiveItems = false;
						playerTypes = false;
						passiveItems = true;
					}
				}
				
			
				// Colour code to indicate selected option
				if(interactiveItems)
					GUI.color = Color.green;
				else
					GUI.color = Color.white;
				
				GUILayout.Space(-4);

				// Turning on/off the interactive menu
				if(GUILayout.Button("Interactive", GUILayout.Height(30)))
				{
					if(interactiveItems)
						interactiveItems = false;
					else
					{
						passiveItems = false;
						playerTypes = false;
						interactiveItems = true;
					}
				}
				
				

				// Colour code to indicate selected option
				if(playerTypes)
					GUI.color = Color.green;
				else
					GUI.color = Color.white;
				
				GUILayout.Space(-4);

				// Turning on/off the Player Type menu
				if(GUILayout.Button("Player Types", GUILayout.Height(30)))
				{
					if(playerTypes)
						playerTypes = false;
					else
					{
						passiveItems = false;
						interactiveItems = false;
						playerTypes = true;
					}
				}
			
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
			
			
			
			// Instanciate passive objects based on which button is clicked
			GUI.color = Color.white;
			if(passiveItems)
			{
				GUILayout.BeginArea(new Rect(0, 30, 150, Screen.height));
					
					GUILayout.Space(4);

					// Scroll through each object of the passive object list
					for(int i = 0; i < passiveObjectNames.Count; i++)
					{
						GUILayout.Space(-4);
						if(GUILayout.Button(passiveObjectNames[i], GUILayout.Height(buttonHeight)))
						{
							// Instanciate the object
							newObject = Instantiate(passiveGameObjects[i], ray.origin + ray.direction * 10, quaternion) as GameObject;
							newObject.name = passiveGameObjects[i].transform.name;
						}
					}

				GUILayout.EndArea();
			}
		

	
			// Instanciate interactive objects based on which button is clicked
			if(interactiveItems)
			{
				GUILayout.BeginArea(new Rect(0, 30, 150, Screen.height));
					
					GUILayout.Space(4);

					// Scroll through each object of the passive object list
					for(int i = 0; i < interactiveObjectNames.Count; i++)
					{
						GUILayout.Space(-4);
						if(GUILayout.Button(interactiveObjectNames[i], GUILayout.Height(buttonHeight)))
						{
							newObject = Instantiate(interactiveGameObjects[i], ray.origin + ray.direction * 10, quaternion) as GameObject;
							newObject.name = interactiveGameObjects[i].transform.name;
							
							// All interactive objects have audio attached
							// It's on by default for the navigation tool
							// Turn this off for the environment builder
							newObject.audio.mute = true;
						}
					}

				GUILayout.EndArea();
			}
			
			if(playerTypes)
			{
				GUILayout.BeginArea(new Rect(0, 30, 150, Screen.height));

					GUILayout.Space(4);

					for(int i = 0; i < characterObjectNames.Count; i++)
					{
						GUILayout.Space(-4);

						if(GUILayout.Button(characterObjectNames[i], GUILayout.Height(buttonHeight)))
						{
							// If there is a single character on screen don't allow another
							if(GameObject.FindGameObjectsWithTag("Player").Length == 1)
							{
								errorMessage = true;
							}
							else
							{
								newObject = Instantiate(characterGameObjects[i], ray.origin + ray.direction * 10, quaternion) as GameObject;
								newObject.name = characterGameObjects[i].transform.name;
							}
						}
					}
				
				GUILayout.EndArea();
			}
		}

		// Print the error message to the screen
		if (errorMessage) 
		{
			StartCoroutine(displayMessage("Only one character allowed in each environment!"));
		}
		
		
		GUILayout.BeginArea(new Rect(Screen.width-200, 0, 200, Screen.height));

			// Change the gui color based on the hide walls option
			if(cEO.hideWalls)
				GUI.color = Color.cyan;
			else
				GUI.color = Color.yellow;	
			
			if(GUILayout.Button("Hide Walls " + currentWallsMode, GUILayout.Height(buttonHeight)))
			{
				// Change the walls to be hidden or shown
				if(cEO.hideWalls)
				{
					currentWallsMode = "Off";
					cEO.hideWalls = false;
				}
				else if(!cEO.hideWalls)
				{
					currentWallsMode = "On";
					cEO.hideWalls = true;
				}
			}
			
			
			// Change the gui color based on the current snap mode
			if(cEO.snapMode)
				GUI.color = Color.yellow;
			else
				GUI.color = Color.cyan;
			
			GUILayout.Space(-4);
			if(GUILayout.Button("Snap Mode " + currentSnapMode, GUILayout.Height(buttonHeight)))
			{
				// Change the current snap mode
				if(cEO.snapMode)
				{
					currentSnapMode = "Off";
					cEO.snapMode = false;
				}
				else if(!cEO.snapMode)
				{
					currentSnapMode = "On";
					cEO.snapMode = true;
				}
			}
			
			
			// Change the gui color based on the hide walls option
			if(currentRoomMode == "Objective Based")
				GUI.color = Color.yellow;
			else
				GUI.color = Color.cyan;
			
			GUILayout.Space(-4);
			if(GUILayout.Button("Room Type " + currentRoomMode, GUILayout.Height(buttonHeight)))
			{
				// Change the room type from objective based to free roam
				if(currentRoomMode == "Free Roam")
				{
					currentRoomMode = "Objective Based";
				}
				else if(currentRoomMode == "Objective Based")
				{
					currentRoomMode = "Free Roam";
				}
			}

		GUILayout.EndArea();
	}
}