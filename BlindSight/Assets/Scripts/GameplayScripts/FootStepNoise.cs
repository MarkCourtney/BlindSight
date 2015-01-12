using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class FootStepNoise : GamePadController {

	Vector3 curPos, newPos;
	Vector3 forward, backward, right, left;
	Quaternion currentLook, targetLook;
	public AudioClip footStep;
	float time = 0;
	bool playAudio = false;

	
	void Update ()
	{	
		gamePadState = GamePad.GetState(0);

		if(gamePadState.ThumbSticks.Left.Y > 0.8f && transform.parent.rigidbody.velocity.magnitude < 0.0001f)
		{	
			time += Time.deltaTime;
		}
		else if(gamePadState.ThumbSticks.Left.Y < -0.8f && transform.parent.rigidbody.velocity.magnitude < 0.0001f)
		{
			time += Time.deltaTime * 0.6f;
		}

		if(time > 0.8)
		{
			playAudioClip(footStep);
			
			time = 0;
		}
	}
}