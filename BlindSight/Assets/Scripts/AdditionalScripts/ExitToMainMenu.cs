/* Mark Courtney
 * C09588817
 * ExitToMainMenu
 * 
 * Allows the user to return to the main menu from any point
 * in the software. This provides a safeguard for users with 
 * visual impairments incase they get lost in the menu systems.
*/

using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class ExitToMainMenu : GamePadController {

	public AudioClip exitClip;
	bool exit = false;

	FadeInOut fIO;

	void Update(){
	
		fIO = GetComponent<FadeInOut> ();

		gamePadState = GamePad.GetState(0);

		if (checkEscape (gamePadState)) {
			audio.clip = exitClip;
			audio.Play ();

			fIO.loadLevel ("Menu");
			GamePad.SetVibration (0, 0, 0);
		}
	}
}