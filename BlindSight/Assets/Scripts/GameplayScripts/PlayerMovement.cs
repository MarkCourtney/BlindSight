/* Mark Courtney
 * C09588817
 * Player Movement
 * 
 * Allows the user to move about the environment. FixedUpdate 
 * is used to ensure even movements over time. It ensures the 
 * user can accurately measure the distance they cover.
*/

using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerMovement : GamePadController
{
	Vector3 curPos, newPos;
	Vector3 forward, backward, right, left;
	Quaternion currentLook, targetLook;
	bool playAudio = false;


	void movePlayer(Vector3 forward, float speed)
	{
		// Only allow the user to move when there's no audio playing
		if(!audio.isPlaying)
		{
			rigidbody.MovePosition(rigidbody.position + (forward * Time.deltaTime * speed));
		}
	}

	// Use a fixed update for calculatin rigidbody forces
	void FixedUpdate ()
	{	
		gamePadState = GamePad.GetState(0);


		// Move the character forward
		if(((Input.GetKey(KeyCode.W) || gamePadState.ThumbSticks.Left.Y > 0.8f) && rigidbody.velocity.magnitude < 0.05f))
		{	
			movePlayer(transform.forward, 1.25f);
		}

		// Move the character backward
		// Slow the rate of movement
		else if((Input.GetKey(KeyCode.S) || gamePadState.ThumbSticks.Left.Y < -0.8f) && rigidbody.velocity.magnitude < 0.05f)
		{	
			movePlayer(-transform.forward, .75f);
		}

		// Stops any force being applied to the character
		else if(!Input.anyKey && rigidbody.velocity.magnitude > 0)
		{
			if(rigidbody.velocity.magnitude > 0)
			{
				rigidbody.velocity = Vector3.zero;
			}
			else
			{
				rigidbody.AddForce(-transform.forward);
			}
		}

	}
}