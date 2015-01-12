/* Mark Courtney
 * C09588817
 * Load Level Script
 * 
 * This script takes a .txt file, depending on whether
 * it's a custom or pre-made map, and loads all the assests
 * in the correct position, orientation and scale.
*/ 

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class LoadLevelScript : MonoBehaviour {
	
	StreamReader sr;
	
	public List<string> gameObjectName;
	public List<Vector3> gameObjectPosition;
	public List<Quaternion> gameObjectRotation;
	public List<Vector3> gameObjectScale;
	public Material wallMaterial;
	
	public string[] vectorPoints, quatPoints, scaleValues;
	public string temp, roomShape, roomMode;
	
	string mapName;
	string path;
	string folderName;
	
	int level;
	
	
	// Ensures the level loads before gameplay can begin
	void Awake () {

		// 0 relates to Main Menu scene
		if (PreviousScene.currentLevel == 0) {
			// Find the path to where maps are stored
			folderName = "Pre-Made Maps";
			
			// Get the map name that was selected
			mapName = MenuSelection.selectedMap;
		}
		
		// 4 relates to Custom Maps scene
		else if (PreviousScene.currentLevel == 4) 
		{
			// Find the path to where maps are stored
			folderName = "Custom Maps";
			
			// Get the map name that was selected
			mapName = CustomMapsChoice.selectedMap;
		}

		
		// Lists for names and positions
		gameObjectName = new List<string>();
		gameObjectPosition = new List<Vector3>();
		
		
		// Strings for each line in a file and what the line is split into
		string fileLine;
		string[] fileLineSplit;
		
		// Set the path for loading a file
		path = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Maps/" + folderName + "/" + mapName;

		// Determine the file to read
 		sr = new StreamReader(path);


		// Read the first two lines of the file
		roomShape = sr.ReadLine();
		roomMode = sr.ReadLine();
		
		while((fileLine = sr.ReadLine()) != null)
		{
			// Pass name, position, rotation and scale into the fileListSplit
			fileLineSplit = fileLine.Split(':');
			
			// Extract and add the name of the object to gameObjectName
			temp = fileLineSplit[0];

			// Convert the character object to a player object
			if(temp == "MaleMiddleAged" || temp == "FemaleElderly" || temp == "MaleChild")
			{
				temp = "Player";
			}
			
			gameObjectName.Add(temp);
			
			
			// Extract and add the position of the object to gameObjectPosition
			temp = fileLineSplit[1];
			temp =  Regex.Replace(temp, @"[()!@#$%_]", "");
			vectorPoints = temp.Split(',');
			

			// If the game object is the character then set the height
			if(fileLineSplit[0] == "MaleMiddleAged")
			{
				vectorPoints[1] = "1.5";
			}
			else if(fileLineSplit[0] == "FemaleElderly")
			{
				vectorPoints[1] = "1.25";
			}
			else if(fileLineSplit[0] == "Child")
			{
				vectorPoints[1] = "1";
			}
			
			gameObjectPosition.Add(new Vector3(	float.Parse(vectorPoints[0]), 
												float.Parse(vectorPoints[1]), 
												float.Parse(vectorPoints[2])));

			// Extract and add the rotation of the object to gameObjectRotation
			temp = fileLineSplit[2];
			temp =  Regex.Replace(temp, @"[()!@#$%_]", "");
			quatPoints = temp.Split(',');
			gameObjectRotation.Add(new Quaternion(	float.Parse(quatPoints[0]), 
													float.Parse(quatPoints[1]), 
													float.Parse(quatPoints[2]),
													float.Parse(quatPoints[3])));
			
			// Extract and add the rotation of the object to gameObjectScale
			temp = fileLineSplit[3];
			temp =  Regex.Replace(temp, @"[()!@#$%_]", "");
			scaleValues = temp.Split(',');


			// If the game object is the character then set the height
			if(fileLineSplit[0] == "MaleMiddleAged")
			{
				scaleValues[1] = "1.5";
			}
			else if(fileLineSplit[0] == "FemaleElderly")
			{
				scaleValues[1] = "1.25";
			}
			else if(fileLineSplit[0] == "Child")
			{
				scaleValues[1] = "1";
			}

			gameObjectScale.Add(new Vector3(float.Parse(scaleValues[0]), 
											float.Parse(scaleValues[1]), 
											float.Parse(scaleValues[2])));
		}
		
		
		// Instanciate all of the objects 
		for(int i = 0; i < gameObjectName.Count; i++)
		{
			// Use the resources folder which contains all the prefabs
			// Instanciate each game object in the scene
			GameObject temp = Instantiate(Resources.Load(gameObjectName[i]), gameObjectPosition[i], gameObjectRotation[i]) as GameObject;	
			
			// Scale the object to the desired length, width and height
			temp.transform.localScale = gameObjectScale[i];
		}

		GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
		foreach (GameObject w in walls)
		{
			w.renderer.material = wallMaterial;
		}

		//GameObject wall = GameObject.FindGameObjectWithTag("Wall");
		//w.renderer.material = wallMaterial;
	}
}