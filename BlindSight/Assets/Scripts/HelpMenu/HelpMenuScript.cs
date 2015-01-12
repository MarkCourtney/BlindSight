/* Mark Courtney
 * C09588817
 * Help Menu Script
 * 
 * The functionality for the help menu is developed in this 
 * class. Each option is given a sound clip for when it is 
 * selected. Extra audio is given to describe each chosen menu
 * item. The main menu, control schemes and player types are
 * broken down and described for the user.
*/

using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class HelpMenuScript : GamePadController {
	
	public GameObject[] helpMenuArray, menuDescriptionArray, controlSchemeArray, playerTypesArray;
	public GameObject environmentBuilderControls, navigationControls, helpMenuTitle;
	public AudioClip[] maleHelpMenuSounds, maleMenuDescriptionSounds, maleControlSchemeSounds, maleManualSounds, malePlayerTypeSounds;
	public AudioClip[] femaleHelpSounds;
	AudioClip[] chosenSounds;
	
	FadeInOut fIO;
	int helpMenuSelection, menuDescriptionSelection, controlSchemeSelection, manualSelection, playerTypeSelection;
	
	bool exitToMainMenu, showingControl;
	
	State state;
	
	enum State {
		
		HelpMenu,
		MenuDescription,
		ControlScheme,
		PlayerTypes
	}
	

	void Start () {
		
		fIO = GetComponent<FadeInOut>();
		
		helpMenuSelection = 0;
		menuDescriptionSelection = 0;
		controlSchemeSelection = 0;
		
		chosenSounds = maleHelpMenuSounds;
		
		state = State.HelpMenu;
		
		exitToMainMenu = false;
		
		playAudioClip(chosenSounds[0]);
	}
	


	// Turn off GUI elements
	void disableMenu(GameObject[] array)
	{
		foreach(GameObject objects in array)
		{
			objects.renderer.enabled = false;	
		}
	}

	// Turn on GUI elements
	void enableMenu(GameObject[] array)
	{
		foreach(GameObject objects in array)
		{
			objects.renderer.enabled = true;	
		}
	}


	// Change to a new state
	void changeState(State newState)
	{
		state = newState;
	}


	// Reset the current selection to the top menu option
	void resetSelection(int selection)
	{
		selection = 0;
	}


	// Change between the different menu audio sounds
	void changeAudioSelection(AudioClip[] audioSounds)
	{
		chosenSounds = audioSounds;
	}
	
	
	void Update () {
		
		gamePadState = GamePad.GetState(0);
		
		if(exitToMainMenu && !audio.isPlaying)
		{
			fIO.loadLevel("Menu");
		}
		
		if(state == State.HelpMenu)
		{
			if(!audio.isPlaying)
			{
				// Scroll down the menu, only if the scene isn't fading out
				if(isThumbStickDown(gamePadState) && fIO.fadeOut == false)
				{
					helpMenuSelection++;
					
					if(helpMenuSelection > helpMenuArray.Length - 1)
						helpMenuSelection = 0;
					
					playAudioClip(chosenSounds[helpMenuSelection]);
				}
				
				// Scroll up the menu, only if the scene isn't fading out
				if(isThumbStickUp(gamePadState) && fIO.fadeOut == false)
				{
					helpMenuSelection--;
					
					if(helpMenuSelection < 0)
						helpMenuSelection = helpMenuArray.Length - 1;
					
					playAudioClip(chosenSounds[helpMenuSelection]);
				}
			}

			
			if((checkButtonPressed(gamePadState) || checkConfirmPressed()) && !audio.isPlaying)
			{
				if(helpMenuSelection == 0)
				{
					// Turn on the Menu Description Page
					enableMenu(menuDescriptionArray);
					disableMenu(helpMenuArray);
					
					// Change the audio to match the menu descriptions
					changeAudioSelection(maleMenuDescriptionSounds);
					
					// Change the code so the code knows where to take the functionality from
					changeState(State.MenuDescription);
					
					// Reset the menu so the first option is selected
					menuDescriptionSelection = 0;
					
					// Play the first audio clip
					playAudioClip(chosenSounds[menuDescriptionSelection]);
					
				}
				else if(helpMenuSelection == 1)
				{
					enableMenu(controlSchemeArray);
					disableMenu(helpMenuArray);
					
					changeAudioSelection(maleControlSchemeSounds);
					
					controlSchemeSelection = 0;
					changeState(State.ControlScheme);
					
					playAudioClip(chosenSounds[controlSchemeSelection]);
				}
				else if(helpMenuSelection == 2)
				{
					enableMenu(playerTypesArray);
					disableMenu(helpMenuArray);
					
					changeAudioSelection(malePlayerTypeSounds);
					
					playerTypeSelection = 0;
					changeState(State.PlayerTypes);
					
					playAudioClip(chosenSounds[playerTypeSelection]);
				}
				else if(helpMenuSelection == 3)
				{
					playAudioClip(chosenSounds[helpMenuSelection + 1]);
					exitToMainMenu = true;
				}
			}
			
			
			for(int i = 0; i < helpMenuArray.Length; i++)
			{
				// Color the menus based on the current menu selection
				helpMenuArray[i].renderer.material.color = Color.white;
				helpMenuArray[helpMenuSelection].renderer.material.color = Color.green;
			}
		}
		
		else if(state == State.MenuDescription)
		{
			if(!audio.isPlaying)
			{
				// Scroll down the menu, only if the scene isn't fading out
				if(isThumbStickDown(gamePadState) && fIO.fadeOut == false)
				{
					menuDescriptionSelection++;
					
					if(menuDescriptionSelection > menuDescriptionArray.Length - 1)
						menuDescriptionSelection = 0;
					
					playAudioClip(chosenSounds[menuDescriptionSelection]);
				}
				
				// Scroll up the menu, only if the scene isn't fading out
				if(isThumbStickUp(gamePadState) && fIO.fadeOut == false)
				{
					menuDescriptionSelection--;
					
					if(menuDescriptionSelection < 0)
						menuDescriptionSelection = menuDescriptionArray.Length - 1;
					
					playAudioClip(chosenSounds[menuDescriptionSelection]);
				}
			}
			
			
			if((checkButtonPressed(gamePadState) || checkConfirmPressed()) && !audio.isPlaying)
			{
				// Play audio clips 
				if(menuDescriptionSelection != 5)
				{
					playAudioClip(chosenSounds[menuDescriptionSelection + 6]);
				}
				// Return to the help menu
				else if(menuDescriptionSelection == 5)
				{
					enableMenu(helpMenuArray);
					disableMenu(menuDescriptionArray);
					
					changeState(State.HelpMenu);
					
					changeAudioSelection(maleHelpMenuSounds);
					
					helpMenuSelection = 0;
					playAudioClip(chosenSounds[helpMenuSelection]);
				}
			}
			
			
			for(int i = 0; i < menuDescriptionArray.Length; i++)
			{
				// Color the menus based on the current menu selection
				menuDescriptionArray[i].renderer.material.color = Color.white;
				menuDescriptionArray[menuDescriptionSelection].renderer.material.color = Color.green;
			}
		}
		
		else if(state == State.ControlScheme)
		{
			if(!audio.isPlaying)
			{
				// Scroll down the menu, only if the scene isn't fading out
				if(isThumbStickDown(gamePadState) && fIO.fadeOut == false)
				{
					controlSchemeSelection++;
					
					if(controlSchemeSelection > controlSchemeArray.Length - 1)
						controlSchemeSelection = 0;
					
					playAudioClip(chosenSounds[controlSchemeSelection]);
				}
				
				// Scroll up the menu, only if the scene isn't fading out
				if(isThumbStickUp(gamePadState) && fIO.fadeOut == false)
				{
					controlSchemeSelection--;
					
					if(controlSchemeSelection < 0)
						controlSchemeSelection = controlSchemeArray.Length - 1;
					
					playAudioClip(chosenSounds[controlSchemeSelection]);
				}
			}
			
			if(controlSchemeSelection == 1 && checkEscape(gamePadState))
			{

			}


			if((checkButtonPressed(gamePadState) || checkConfirmPressed()) && !audio.isPlaying)
			{
				if(controlSchemeSelection == 0)
				{
					if(checkButtonPressed(gamePadState) && !audio.isPlaying)
					{
						disableMenu(controlSchemeArray);
						navigationControls.renderer.enabled = true;
						helpMenuTitle.renderer.enabled = false;
					}
					else if(checkEscape(gamePadState) && !audio.isPlaying)
					{
						enableMenu(controlSchemeArray);
						navigationControls.renderer.enabled = false;
						helpMenuTitle.renderer.enabled = true;
					}
				}
				else if(controlSchemeSelection == 1)
				{
					if(showingControl && !audio.isPlaying)
					{
						enableMenu(controlSchemeArray);
						environmentBuilderControls.renderer.enabled = false;
						helpMenuTitle.renderer.enabled = true;
						showingControl = false;
					}
					else
					{
						disableMenu(controlSchemeArray);
						environmentBuilderControls.renderer.enabled = true;
						helpMenuTitle.renderer.enabled = false;
						showingControl = true;
					}

				}
				else if(controlSchemeSelection == 2)
				{
					enableMenu(helpMenuArray);
					disableMenu(controlSchemeArray);
					
					changeState(State.HelpMenu);
					
					changeAudioSelection(maleHelpMenuSounds);
					
					helpMenuSelection = 0;
					playAudioClip(chosenSounds[helpMenuSelection]);
				}
			}
			
			
			for(int i = 0; i < controlSchemeArray.Length; i++)
			{
				// Color the menus based on the current menu selection
				controlSchemeArray[i].renderer.material.color = Color.white;
				controlSchemeArray[controlSchemeSelection].renderer.material.color = Color.green;
			}
		}
		
		else if(state == State.PlayerTypes)
		{
			if(!audio.isPlaying)
			{
				// Scroll down the menu, only if the scene isn't fading out
				if(isThumbStickDown(gamePadState) && fIO.fadeOut == false)
				{
					playerTypeSelection++;
					
					if(playerTypeSelection > playerTypesArray.Length - 1)
						playerTypeSelection = 0;
					
					playAudioClip(chosenSounds[playerTypeSelection]);
				}
				
				// Scroll up the menu, only if the scene isn't fading out
				if(isThumbStickUp(gamePadState) && fIO.fadeOut == false)
				{
					playerTypeSelection--;
					
					if(playerTypeSelection < 0)
						playerTypeSelection = playerTypesArray.Length - 1;
					
					playAudioClip(chosenSounds[playerTypeSelection]);
				}
			}
			
			
			if((checkButtonPressed(gamePadState) || checkConfirmPressed()) && !audio.isPlaying)
			{
				if(playerTypeSelection == 0)
				{
					playAudioClip(chosenSounds[playerTypeSelection + 4]);
				}
				if(playerTypeSelection == 1)
				{
					playAudioClip(chosenSounds[playerTypeSelection + 4]);
				}
				if(playerTypeSelection == 2)
				{
					playAudioClip(chosenSounds[playerTypeSelection + 4]);
				}
				if(playerTypeSelection == 3)
				{
					enableMenu(helpMenuArray);
					disableMenu(playerTypesArray);
					
					changeState(State.HelpMenu);
					
					changeAudioSelection(maleHelpMenuSounds);
					
					helpMenuSelection = 0;
					playAudioClip(chosenSounds[helpMenuSelection]);
				}
			}
			
			
			for(int i = 0; i < playerTypesArray.Length; i++)
			{
				// Color the menus based on the current menu selection
				playerTypesArray[i].renderer.material.color = Color.white;
				playerTypesArray[playerTypeSelection].renderer.material.color = Color.green;
			}
		}
	}
}