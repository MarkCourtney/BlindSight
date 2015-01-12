/* Mark Courtney
 * C09588817
 * Player Look
 * 
 * Allows rotation of the camera to change the orientation
 * of the user.
*/

using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerLook : GamePadController
{

	public AudioClip shuffle;
	Vector3 currentRot;
	float totalRot;

	void rotateCamera(Vector3 targetLook)
	{
		transform.parent.Rotate(targetLook);
	}
	
	void FixedUpdate ()
	{	
		gamePadState = GamePad.GetState(0);

		totalRot = Vector3.Angle(currentRot, transform.right/2);
		//print("Angle: " + totalRot);
		//Debug.Log(transform.position);

		if(totalRot > 135 || totalRot < 45)
		{
			playAudioClip(shuffle);
			currentRot = transform.forward;
		}

		else if(gamePadState.ThumbSticks.Right.X < -0.8f || Input.GetKey(KeyCode.A))
		{
			rotateCamera(-Vector3.up);
		}
		else if(gamePadState.ThumbSticks.Right.X > 0.8f || Input.GetKey(KeyCode.D))
		{
			rotateCamera(Vector3.up);
		}
		else
		{
			currentRot = transform.forward;
		}
	}
}