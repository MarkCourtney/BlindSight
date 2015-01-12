/* Mark Courtney
 * C09588817
 * Menu Selection
 * 
 * The hub where other parts of the system can be accessed. 
 * Functionality for the main menu system is developed here. 
 * The menu can transition to the options, custom and pre-made
 * maps, environment builder and help menu. Every menu item 
 * has a audio cue attached to describe the current selection
 * and more information if that item is selected.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class MenuSelection : GamePadController {

	public GameObject optionMusicChoice, optionVoiceChoice, playReturn;
	public GameObject[] optionsList, menuList, mapList;
	
	public AudioClip[] maleMainMenuSounds, maleOptionSounds, malePlaySounds;
	public AudioClip[] femaleMainMenuSounds, femaleOptionSounds, femalePlaySounds;
	AudioClip[] chosenMainMenuSounds, chosenOptionSounds, chosenPlaySounds;
	
	Quaternion currentRot;
	Vector3 targetRot;
	int menuSelection, optionSelection, mapSelection, totalItems;
	int playMapX, playMapY, mod;
		
	TextMesh voiceText, musicText;
	public static string currentVoice, currentMusic, selectedMap;
	
	bool exiting;

	Ray mousePosition;
	RaycastHit hit;
	string menuItemHit;

	FadeInOut fIO;
	State state;
	
	
	// All the possible states
	enum State {
		
		MainMenu,
		Options,
		MapMenu,
	}




	void Start () {

		// Get the FadeInOut class
		fIO = GetComponent<FadeInOut>();
		
		// The intial state is the main menu
		state = State.MainMenu;
		
		currentVoice = "";
		currentMusic = "";
		selectedMap = "";

		exiting = false;


		currentRot = transform.rotation;
		
		// Make the initial selection the first option
		menuSelection = 0;
		optionSelection = 0;
		mapSelection = 0;
		playMapY = 265;
		
		
		// Get the total items in the first menu
		totalItems = GameObject.FindGameObjectsWithTag("MenuItem").Length;
		
		// Return the TextMesh's text
		musicText = (TextMesh)optionMusicChoice.GetComponent(typeof(TextMesh));
		voiceText = (TextMesh)optionVoiceChoice.GetComponent(typeof(TextMesh));
		
		
		// Set the audio to male by default
		chosenMainMenuSounds = maleMainMenuSounds;
		chosenOptionSounds = maleOptionSounds;
		chosenPlaySounds = malePlaySounds;
		
		// Play the play menu clip
		playAudioClip (chosenMainMenuSounds[0]);

		// Stop any vibrations that may have carried over from the previous map
		GamePad.SetVibration(0, 0, 0);
	}



	
	// Method to change between menu states
	void changeState(State newState)
	{
		state = newState;
	}

	
	// Method for rotating the camera
	void rotateCamera(Vector3 direction)
	{
		transform.rotation = Quaternion.Slerp(currentRot, Quaternion.Euler(direction), Time.deltaTime * 3);
	}
	
	
	
	void Update () {

		// Variables for storing user input
		mousePosition = Camera.main.ScreenPointToRay (Input.mousePosition);
		gamePadState = GamePad.GetState(0);


		// Holds the current rotation of the camera
		// Used in slerping the camera to different menu items
		currentRot = transform.rotation;


		// Mouse Input for visual users
		// Allows sighted users to choose menu items much quicker via mouse input
		if(Physics.Raycast(mousePosition, out hit, 10) && Input.GetMouseButtonDown(0))
		{
			if(!hit.transform.name.Equals(null))
			{
				for(int i = 0; i < menuList.Length; i++)
				{
					if(menuList[i].transform.name.Contains(hit.transform.name))
					{
						menuSelection = i;
					}
				}

				for(int i = 0; i < mapList.Length; i++)
				{

					if(mapList[i].transform.name.Contains(hit.transform.name))
					{
						mapSelection = i;
						print("Maps");
					}
				}


				// -2 so that it doesnt' allow the mouse options list 
				// to choose AudioChoice or VoiceChoice
				// It overrides the correct selection
				for(int i = 0; i < optionsList.Length - 2; i++)
				{
					if(optionsList[i].transform.name.Contains(hit.transform.name))
					{
						optionSelection = i;
					}
				}
			}
		}


		


		
		// Contains logic for main menu
		// Acts as central hub for the whole program
		// Allows easy access back to this menu from any point
		// Can choose to access all map types, create maps, options, help and exit
		if(state.Equals(State.MainMenu))
		{
			// After the menu selection has been changed, audio will play
			// Don't allow any menu movement till the audio has finished
			if(!audio.isPlaying)
			{
				// Scroll down the menu, only if the scene isn't fading out
				if(isThumbStickDown(gamePadState) && fIO.fadeOut == false)
				{
					menuSelection++;
					
					if(menuSelection > totalItems -1)
						menuSelection = 0;
					
					playAudioClip(chosenMainMenuSounds[menuSelection]);
				}
				
				// Scroll up the menu, only if the scene isn't fading out
				if(isThumbStickUp(gamePadState) && fIO.fadeOut == false)
				{
					menuSelection--;
					
					if(menuSelection < 0)
						menuSelection = totalItems - 1;
					
					playAudioClip(chosenMainMenuSounds[menuSelection]);
				}
			}
			
			// Play Map
			if(menuSelection == 0)
			{
				// Rotate the camera to point at the chosen text
				rotateCamera(new Vector3(-2, 0, 0));
				
				if(checkButtonPressed(gamePadState) || checkConfirmPressed())
				{
					// Find the total number of items in the options menu
					
					totalItems = GameObject.FindGameObjectsWithTag("MapItem").Length;
					mapSelection = 0;
					
					changeState(State.MapMenu);
					
					StartCoroutine(playMultipleClips(chosenMainMenuSounds[6], chosenPlaySounds[0]));
				}
			}
			
			
			// Custom Map
			if(menuSelection == 1)
			{
				rotateCamera(new Vector3(0, 0, 0));
			
				// Start fading out
				if(checkButtonPressed(gamePadState) || checkConfirmPressed())
				{	
					currentVoice = voiceText.text;
					fIO.loadLevel("CustomMaps");
					
					// Custom Map currently unavailable
					playAudioClip(chosenMainMenuSounds[7]);
				}
			}
			
			
			// Create Map
			if(menuSelection == 2)
			{
				rotateCamera(new Vector3(2, 0, 0));
			
				// Start fading out
				if(checkButtonPressed(gamePadState) || checkConfirmPressed())
				{
					// Place the voice Male or Female into a variable
					currentVoice = voiceText.text;
					
					// Load the scene for map dimensions
					fIO.loadLevel("MapDimensions");

					playAudioClip(chosenMainMenuSounds[8]);
				}
			}
			
			
			// Options
			if(menuSelection == 3)
			{
				rotateCamera(new Vector3(4, 0, 0));
			
				if(checkButtonPressed(gamePadState) || checkConfirmPressed())
				{
					// Change the state to the options menu
					// Find the total number of items in the options menu for scrolling through the menu
					
					totalItems = GameObject.FindGameObjectsWithTag("OptionsItem").Length;
					optionSelection = 0;
					
					
					// Determine if music has been turned on or off
					// Change the audio clip based on this
					if(musicText.text == "On")
					{
						changeState(State.Options);
						
						if(chosenMainMenuSounds != null)
						{				
							StartCoroutine(playMultipleClips(chosenMainMenuSounds[9], chosenOptionSounds[optionSelection]));
						}
					}
					else
					{
						changeState(State.Options);
						
						if(chosenMainMenuSounds != null)
						{
							StartCoroutine(playMultipleClips(chosenMainMenuSounds[9], chosenOptionSounds[3]));
						}
					}
				}
			}
			
			
			// Options
			if(menuSelection == 4)
			{
				rotateCamera(new Vector3(6, 0, 0));
			
				if(checkButtonPressed(gamePadState) || checkConfirmPressed())
				{
					// Place the voice Male or Female into a variable
					currentVoice = voiceText.text;
					
					// Load the scene for map dimensions
					fIO.loadLevel("Help");	

					playAudioClip(chosenMainMenuSounds[10]);
				}
			}
			
			
			// Exit
			if(menuSelection == 5)
			{
				rotateCamera(new Vector3(8, 0, 0));
			
				if(checkButtonPressed(gamePadState) || checkConfirmPressed())
				{
					if(!audio.isPlaying)
					{	
						exiting = true;

						// Exit the program audio
						playAudioClip(chosenMainMenuSounds[11]);
					}
					
				}
			}

			for(int i = 0; i < menuList.Length; i++)
			{
				// Color the menus based on the current menu selection
				menuList[i].renderer.material.color = Color.white;
				menuList[menuSelection].renderer.material.color = Color.green;
			}
		}







		
		
		// Contains logic for options menu
		// Allows the user to choose where audio is on/off
		// And whether the voice is male/female
		else if(state.Equals(State.Options))
		{
			if(!audio.isPlaying)
			{
				if(isThumbStickDown(gamePadState) && fIO.fadeOut == false)
				{
					optionSelection++;
					
					if(optionSelection > totalItems -1)
						optionSelection = 0;
					
					// If music is set to off, play that clip
					// Otherwise play the music on clip
					if(optionSelection == 0 && musicText.text == "Off")
						playAudioClip(chosenOptionSounds[3]);
					else
						playAudioClip(chosenOptionSounds[optionSelection]);
				}
				if(isThumbStickUp(gamePadState) && fIO.fadeOut == false)
				{
					optionSelection--;
					
					if(optionSelection < 0)
						optionSelection = totalItems - 1;
					
					// If music is set to off, play that clip
					// Otherwise play the music on clip
					if(optionSelection == 0 && musicText.text == "Off")
						playAudioClip(chosenOptionSounds[3]);
					else
						playAudioClip(chosenOptionSounds[optionSelection]);
				}
				
			}
			
			
			for(int i = 0; i < optionsList.Length; i++)
			{
				optionsList[i].renderer.material.color = Color.white;
				optionsList[optionSelection].renderer.material.color = Color.green;
			}
			

			// Audio On/Off
			if(optionSelection == 0)
			{
				rotateCamera(new Vector3(0, 90, 0));
		
				if(checkButtonPressed(gamePadState) || checkConfirmPressed())
				{
					// Turn the music text to off
					// Play the audio clip with music off
					if(musicText.text == "On")
					{
						musicText.text = "Off";
						
						playAudioClip(chosenOptionSounds[3]);
						
						
						// Mute all audio sources in a scene
						
						chosenMainMenuSounds = null;
						chosenOptionSounds = null;
						chosenPlaySounds = null;
					}
					
					// Turn the music text to on
					// Play the audio clip with music on
					else
					{
						musicText.text = "On";
						
						// Apply the male audio sounds if male is the chosen option
						if(voiceText.text == "Male")
						{
							chosenMainMenuSounds = maleMainMenuSounds;
							chosenOptionSounds = maleOptionSounds;
							chosenPlaySounds = malePlaySounds;
							
							playAudioClip(chosenOptionSounds[optionSelection]);
							
							voiceText.text = "Male";
						}
					
						// Apply the female audio sounds if male is the chosen option
						else
						{
							chosenMainMenuSounds = femaleMainMenuSounds;
							chosenOptionSounds = femaleOptionSounds;
							chosenPlaySounds = femalePlaySounds;
							
							playAudioClip(chosenOptionSounds[optionSelection]);
							
							voiceText.text = "Female";
						}
					}
				}
			}
			
				
			// Voice Male/Female
			else if(optionSelection == 1)
			{
				rotateCamera(new Vector3(4,90,0));
				
				if(checkButtonPressed(gamePadState) || checkConfirmPressed())
				{
					// Change the audio to female sounds
					// Change the text to female and play the audio clip
					if(voiceText.text == "Male" && musicText.text == "On")
					{
						chosenMainMenuSounds = femaleMainMenuSounds;
						chosenOptionSounds = femaleOptionSounds;
						chosenPlaySounds = femalePlaySounds;
						
						playAudioClip(chosenOptionSounds[optionSelection]);
						
						voiceText.text = "Female";
					}
					
					// Change the audio to male sounds
					// Change the text to male and play the audio clip
					else if(voiceText.text == "Female" && musicText.text == "On")
					{
						chosenMainMenuSounds = maleMainMenuSounds;
						chosenOptionSounds = maleOptionSounds;
						chosenPlaySounds = malePlaySounds;
						
						playAudioClip(chosenOptionSounds[optionSelection]);
						
						voiceText.text = "Male";
					}
				}
			}
			

			// Return to main menu
			else if(optionSelection == 2)
			{
				rotateCamera(new Vector3(8,90,0));
				
				if(checkButtonPressed(gamePadState) || checkConfirmPressed())
				{
					// Change the state to the main menu
					// Find the total number of items in the main menu for scrolling through the menu
					changeState(State.MainMenu);
					totalItems = GameObject.FindGameObjectsWithTag("MenuItem").Length;
					menuSelection = 0;
					
					StartCoroutine(playMultipleClips(chosenOptionSounds[optionSelection], chosenMainMenuSounds[menuSelection]));
				}
			}
		}
		
		





		// Contains logic for map menu
		// Cann access a range of pre-made maps
		else if(state.Equals(State.MapMenu))
		{
			if(!audio.isPlaying)
			{
				if(isThumbStickDown(gamePadState) && fIO.fadeOut == false)
				{
					mapSelection++;
					
					if(mapSelection > totalItems - 1)
						mapSelection = 0;
					
					playAudioClip(chosenPlaySounds[mapSelection]);
				}
				if(isThumbStickUp(gamePadState) && fIO.fadeOut == false)
				{
					mapSelection--;
					
					if(mapSelection < 0)
						mapSelection = totalItems - 1;
					
					playAudioClip(chosenPlaySounds[mapSelection]);
				}
			}


			// Determines if the camera rotates left or right
			mod = mapSelection%2;

			if(mod == 0)
			{
				playMapY = 265;	
			}
			else if(mod == 1)
			{
				playMapY = 275;
			}

			playMapX = mapSelection/2;



			// Rotate the camera depending on the map selection
			for(int z = 0; z < mapList.Length; z++)
			{
				print(z);
				rotateCamera(new Vector3(playMapX * 3, playMapY, 0));
			}



			if(mapSelection == 6)
			{
				rotateCamera(new Vector3(9, 270, 0));
			}

			else if(mapSelection == 7)
			{
				rotateCamera(new Vector3(12, 270, 0));

				// Return to the main menu screen
				if(checkButtonPressed(gamePadState) || checkConfirmPressed())
				{
					totalItems = GameObject.FindGameObjectsWithTag("MenuItem").Length;
					menuSelection = 0;	
					
					changeState(State.MainMenu);
					
					if(chosenMainMenuSounds != null)
					{
						StartCoroutine(playMultipleClips(chosenPlaySounds[8], chosenMainMenuSounds[0]));
					}
				}
			}
			

			// Transfer to the navigation tool, loading that particular map
			if((checkButtonPressed(gamePadState) || checkConfirmPressed()) && mapSelection != 7)
			{
				fIO.loadLevel("Gameplay");

				selectedMap =  mapList[mapSelection].name + ".txt";
				print(selectedMap);
			}
			
			
			for(int i = 0; i < mapList.Length; i++)
			{
				mapList[i].renderer.material.color = Color.white;
				mapList[mapSelection].renderer.material.color = Color.green;
			}
		}



		// At any point press Escape or the right mouse button to return to the Play Map option
		if(checkEscape(gamePadState) || Input.GetMouseButtonDown(1))
		{
			// Stop the fade out
			fIO.fadeOut = false;
			fIO.alpha = 0;
			
			totalItems = GameObject.FindGameObjectsWithTag("MenuItem").Length;

			// Stop any audio that is current playing
			audio.Stop();


			// Change the state back to Pre-Made Map in the Main Menu
			changeState(State.MainMenu);
			menuSelection = 0;

			// Play the Pre-Made Map option
			playAudioClip(chosenMainMenuSounds[0]);
		}


		// Exit the program
		if(exiting && !audio.isPlaying)
		{
			Application.Quit();	
		}
	}
}