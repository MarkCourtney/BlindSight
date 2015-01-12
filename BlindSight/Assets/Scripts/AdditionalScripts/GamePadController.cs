/* Mark Courtney
 * C09588817
 * GamePadController
 * 
 * This class contains a series of methods for
 * inputs from the keyboardall and the controller 
 * buttons and triggers.
 * 
 * This class also contains methods for playing
 * audio cues. This is the case due to all classes
 * inheriting the GamePadController also need 
 * audio to play based on the controller input.
*/

using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class GamePadController : MonoBehaviour {

	public GamePad gamePad;
	public GamePadState gamePadState;
	bool isStickUp = false;
	bool isStickDown = false;
	bool isButtonDown = false;
	bool isConfirmDown = false;
	bool isEscapeDown = false;
	bool isShoulderDown = false;
	bool isTriggerDown = false;


	// Checks if one of the triggers have pressed 
	public bool checkTriggerPressed(GamePadState state)
	{
		if (Input.GetKeyDown(KeyCode.Space) || ((state.Triggers.Left > 0.05f || state.Triggers.Right > 0.05f) && !isTriggerDown))
		{
			isTriggerDown = true;
			return true;
		} 
		else if (state.Triggers.Left > 0 || state.Triggers.Right > 0)
		{
			isTriggerDown = false;
			return false;
		} 
		else 
		{
			return false;
		}
	}

	// Checks if one of the bumpers have pressed
	public bool checkBumperPressed(GamePadState state)
	{
		if (Input.GetKeyDown(KeyCode.Return) || ((state.Buttons.LeftShoulder == ButtonState.Pressed || state.Buttons.RightShoulder == ButtonState.Pressed ) && !isShoulderDown))
		{
			isShoulderDown = true;
			return true;
		} 
		else if (state.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.RightShoulder == ButtonState.Released)
		{
			isShoulderDown = false;
			return false;
		} 
		else 
		{
			return false;
		}
	}


	// Checks for the return key or left mouse click
	public bool checkConfirmPressed()
	{
		if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
		{
			isConfirmDown = true;
			return true;
		}
		else 
		{
			return false;
		}
	}


	// Checks if the A, B, X, Y buttons have been pressed
	public bool checkButtonPressed(GamePadState state)
	{
		if ((state.Buttons.A == ButtonState.Pressed || state.Buttons.B == ButtonState.Pressed || state.Buttons.X == ButtonState.Pressed || state.Buttons.Y == ButtonState.Pressed) && !isButtonDown)
		{
			isButtonDown = true;
			return true;
		} 
		else if (state.Buttons.A == ButtonState.Released && state.Buttons.B == ButtonState.Released && state.Buttons.X == ButtonState.Released && state.Buttons.Y == ButtonState.Released)
		{
			isButtonDown = false;
			return false;
		} 
		else 
		{
			return false;
		}
	}

	// Checks if the Start, Select or Escape buttons have been pressed
	public bool checkEscape(GamePadState state)
	{
		if (Input.GetKeyDown(KeyCode.Escape) || ((gamePadState.Buttons.Start == ButtonState.Pressed || gamePadState.Buttons.Back == ButtonState.Pressed) && !isEscapeDown))
		{
			isEscapeDown = true;
			return true;
		} 
		else if (state.Buttons.Start == ButtonState.Released && state.Buttons.Back == ButtonState.Released)
		{
			isEscapeDown = false;
			return false;
		} 
		else 
		{
			return false;
		}
	}

	// Checks if the thumbstick is pulled up
	public bool isThumbStickUp(GamePadState state)
	{
		if ((state.ThumbSticks.Left.Y > 0.8f && !isStickUp) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
		{
			isStickUp = true;
			return true;
		} 
		else if (state.ThumbSticks.Left.Y <= 0.8f && state.ThumbSticks.Left.Y >= 0.0f) 
		{
			isStickUp = false;
			return false;
		} 
		else 
		{
			return false;
		}
	}


	// Checks if the thumbstick is pulled down
	public bool isThumbStickDown(GamePadState state)
	{
		if ((state.ThumbSticks.Left.Y < -0.8f && !isStickDown) || Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) 
		{
			isStickDown = true;
			return true;
		} 
		else if (state.ThumbSticks.Left.Y >= -0.8f && state.ThumbSticks.Left.Y <= 0.0f) 
		{
			isStickDown = false;
			return false;
		} 
		else 
		{
			return false;
		}
	}

	// Plays a single audio clip
	public void playAudioClip(AudioClip audioClip)
	{
		if (!audioClip.Equals(null)) 
		{
			audio.clip = audioClip;
			audio.Play ();	
		}
	}


	// Method for playing multiple audio clips in a row
	public IEnumerator playMultipleClips(AudioClip clip1, AudioClip clip2)
	{	

		if(clip1 != null)
		{
			audio.clip = clip1;
			audio.Play();
		
			yield return new WaitForSeconds(audio.clip.length + 0.1f);
		}
		
		// Only play the second clip if another audio clip isn't all ready playing
		// This scenario takes places when the user is quickly changing between menus
		
		if(!audio.isPlaying)
		{
			audio.clip = clip2;
			audio.Play();	
		}
	}
}