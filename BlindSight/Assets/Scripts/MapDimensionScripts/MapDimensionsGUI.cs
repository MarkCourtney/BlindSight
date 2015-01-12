/* Mark Courtney
 * C09588817
 * Map Dimensions GUI
 * 
 * Provides the functionality behind the user interface
 * that allows the user to select the room they wish to 
 * create. The type of room and it's dimensions are selected
 * in this interface.
 * 
 * At any point the user can return to the main menu by
 * pressing the Escape key.
*/

using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class MapDimensionsGUI : MonoBehaviour {
	
	public AudioClip[] returnToMainMenu;
	public Texture lShaped, rectangle, crossSection;
	
	public static float floorWidth, floorLength;
	float textAlpha, timer;
	int textFieldWidth;
	
	public static string roomShape;
	
	string floorLengthString, floorWidthString, voice;
	string errorMessage;
	
	public GUIStyle titleStyle, noteStyle, headingStyle, fieldStyle, lShapedStyle, rectangleStyle, crossStyle;
	
	bool shapeSelected, checkErrorMessage;
	
	FadeInOut fIO;

	
	// Use this for initialization
	void Start () {
		
		voice = MenuSelection.currentVoice;
		
		fIO = GetComponent<FadeInOut>();
		
		Screen.showCursor = true;
		
		floorWidth = floorLength = 0;
		
		textFieldWidth = 50;
		
		textAlpha = 1.0f;
		timer = 0.0f;
		
		floorLengthString = "";
		floorWidthString = "";
		
		shapeSelected = false;
		checkErrorMessage = false;
		
		if(voice == "Male")
		{
			audio.clip = returnToMainMenu[0];
		}
		else if (voice == "Female")
		{
			audio.clip = returnToMainMenu[1];
		}
		
		audio.Play();
	}


	// Display a message in the center of the screen
	IEnumerator displayMessage(string message)
	{
		timer += Time.deltaTime;
		print (timer);

		if(timer < 7)
		{
			print(message);
			GUILayout.BeginArea(new Rect(Screen.width/2 - 150, Screen.height/1.3f, 500, 200));
			GUILayout.Label(message, noteStyle);
			GUILayout.EndArea();
			
			yield return null;
		}
		else
		{
			checkErrorMessage = false;
			timer = 0;
			yield return null;
		}
	}


	// 
	void setMessage(string message, bool messageOn)
	{
		errorMessage = message;
		checkErrorMessage = messageOn;
	}

	
	
	void OnGUI()
	{
		if (checkErrorMessage) 
		{
			StartCoroutine(displayMessage(errorMessage));
		}

		if(!shapeSelected)
		{
			GUILayout.BeginArea(new Rect(Screen.width/3 - 100, Screen.height/2 - 100, Screen.width/2, 250));
			
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Select the type of room to build", headingStyle);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			
			GUILayout.EndArea();
			
			
			GUILayout.BeginArea(new Rect(Screen.width/4.5f, Screen.height/2, Screen.width/1.5f, 250));
			GUILayout.BeginHorizontal();
			
			if(GUILayout.Button(lShaped, lShapedStyle, GUILayout.Width(200), GUILayout.Height(200)))
			{
				roomShape = "LShaped";
				shapeSelected = true;
			}
			
			GUILayout.FlexibleSpace();
			if(GUILayout.Button(rectangle, rectangleStyle, GUILayout.Width(200), GUILayout.Height(200)))
			{
				roomShape = "Rectangle";
				shapeSelected = true;	
			}
			
			GUILayout.FlexibleSpace();
			if(GUILayout.Button(crossSection, crossStyle, GUILayout.Width(200), GUILayout.Height(200)))
			{
				roomShape = "Cross";
				shapeSelected = true;	
			}
			
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		
		else
		{		
			// Fade the text in and out with the background
			// Not directly affecting the fadeIn/Out code
			
			if(fIO.fadeOut)
			{
				textAlpha -= 0.008f;
			}
			
			// Fade the color of the text in and out
			GUI.color = new Color(1, 1, 1, textAlpha);
			
			
			GUILayout.BeginArea(new Rect(Screen.width/2 - 150, Screen.height/3f, Screen.width/1.5f, 250));
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			// Labels with instructions
			GUILayout.Label("Input Floor Length & Width", titleStyle, GUILayout.Width(50));
			
			GUILayout.BeginArea(new Rect(90, 35, 200, 50));
			
			GUILayout.Label("(Dimensions are in feet)", noteStyle, GUILayout.Width(10));
			GUILayout.EndArea();
			
			
			
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			
			
			// Textfields for width and length in feet
			
			GUILayout.BeginArea(new Rect(100, 60, 120, 250));
			
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			
			
			GUILayout.Label("Length", headingStyle);
			GUILayout.Label("Width", headingStyle);
			
			
			GUILayout.EndVertical();
			
			GUILayout.BeginVertical();					
			

			floorLengthString = GUILayout.TextField(floorLengthString, 2, GUILayout.Width(textFieldWidth));
		 	floorWidthString = GUILayout.TextField(floorWidthString, 2, GUILayout.Width(textFieldWidth));
			
			// Only allow numbers between 0 and 9, otherwise replace with the old string
			floorLengthString = Regex.Replace(floorLengthString, @"[^0-9 ]", "");
			floorWidthString = Regex.Replace(floorWidthString, @"[^0-9 ]", "");
			
			
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
			
			
			GUILayout.BeginArea(new Rect(100, 120, 120, 250));
			
			// When the Create button is clicked
			// Load the map editor
			// All the relevant data gets passed into the MapEditor class
			if(GUILayout.Button("Create", GUILayout.Height(30)))
			{
				// Only allow transition to MapEditor if a width and length have been defined
				if(!floorLengthString.Equals("") && !floorWidthString.Equals(""))
				{
					// If the room shape is cross or Lshaped
					// Ensure the length must be greater than width

					if(float.Parse(floorWidthString) >= float.Parse(floorLengthString) && (roomShape == "Cross" || roomShape == "LShaped"))
					{
						setMessage("For a " + roomShape + " room, Length must be longer than Width", true);
					}
					
					else if(float.Parse(floorWidthString) > 10 && (roomShape == "Cross" || roomShape == "LShaped"))
					{
						setMessage("For a " + roomShape + " room, Width must not exceed 10 feet", true);
					}
					
					else if(float.Parse(floorLengthString) > 20 && (roomShape == "Cross" || roomShape == "LShaped"))
					{
						setMessage("For a " + roomShape + " room, Length must not exceed 20 feet", true);
					}
					else
					{
						fIO.loadLevel("MapEditor");
						// Set the width and length to be used in MapEditorGUI
						floorWidth = float.Parse(floorWidthString);
						floorLength = float.Parse(floorLengthString);
					}
				}
			}

			GUILayout.EndArea();
			GUILayout.EndArea();
		}
	}
	
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(shapeSelected)
			{
				shapeSelected = false;
			}
			else
			{
				Application.LoadLevel("Menu");
			}
		}	

	}
}