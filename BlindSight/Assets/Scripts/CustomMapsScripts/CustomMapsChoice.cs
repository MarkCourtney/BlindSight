/* Mark Courtney
 * C09588817
 * CustomMapsChoice
 * 
 * Provides GUI functionality in order to choose a custom map.
 * Maps can be played or deleted from this class. The maps are 
 * displayed using a scroll bar system.
*/


using UnityEngine;
using System;
using System.IO;
using System.Collections;
using XInputDotNetPure;

public class CustomMapsChoice : GamePadController {
	
	DirectoryInfo directory;
	FileInfo[] files;
	FadeInOut fIO;
	
	public GUIStyle style;
	
	Vector2 scrollPos;
	Rect windowRect;
	
	public static string selectedMap;
	string path;
	string folderName;

	void Start () {
		
		Screen.showCursor = true;
		
		// Get the folder name the files are located in
		folderName = "Custom Maps";
		path = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Maps/" + folderName;
		
		// Extract the names of all the files
		directory = new DirectoryInfo(path);
		files = directory.GetFiles("*.txt");
		
		selectedMap = "Select A Map";
	}
	

	// Display a list of custom levels
	void showCustomLevels(int id) {
		
		// Horizontal layer that displays the map name, play and delete options
		GUILayout.BeginHorizontal();
		
		GUILayout.Label(selectedMap, style);

		// Transition to the navigation tool with the selected map
		if(GUILayout.Button("Play", GUILayout.Width(windowRect.width / 5)))
		{
			// If a map hasn't been selected the else statement runs
			if(selectedMap.Contains(".txt"))
			{
				Application.LoadLevel("Gameplay");
			}
			else
			{
				selectedMap = "Choose a valid Map";
			}
		}


		// Delete a map thats is highlighted
		// Repopulate the list without the deleted map
		if(GUILayout.Button("Delete", GUILayout.Width(windowRect.width / 5)))
		{
			File.Delete(directory + "/" + selectedMap);
			files = directory.GetFiles("*.txt");
		}
		
		// Space the buttons to the right side of the window
		GUILayout.Space(25);
		GUILayout.EndHorizontal();
		GUILayout.Space(6);
		
		// Scroll menu, contains all the files located in the directory path
		scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(windowRect.width-20), GUILayout.Height(windowRect.height-20));


		// Determine which bus was clicked and place that into selected map variable
		for(int i = 1; i < files.Length + 1; i++)
		{
			if(GUILayout.Button(files[i-1].Name, GUILayout.Width(Screen.width / 4 - 50), GUILayout.Height(20)))
			{
				// Place the name of the selected map in a variable
				selectedMap = files[i-1].Name;
			}
		}
		
		GUILayout.EndScrollView();
	}
	
	
	void OnGUI() {

		// Define a size for the scroll menu
		windowRect = new Rect(Screen.width / 2.5f, Screen.height / 4, Screen.width / 4, 250);


		// Create a window with the levels contained inside it
		// Name the window Custom Maps
		GUILayout.Window(0, windowRect, showCustomLevels, "Custom Maps");
	}


	void Update() {

		gamePadState = GamePad.GetState(0);

		if(checkEscape(gamePadState))
		{
			fIO.loadLevel("MainMenu");
		}
	}
}