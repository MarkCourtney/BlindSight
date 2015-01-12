/* Mark Courtney
 * C09588817
 * InteractWithPassiveObjects
 * 
 * This class provides functionality for collisions with
 * passive objects. Vibrations are applied to the controller
 * if a collision occurs. Play an audio clips responding
 * to the object collided with.
*/

using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class InteractWithPassiveObjects : GamePadController {

	Ray rayForward, rayForwardRight, rayForwardLeft, rayBackward, rayBackwardRight, rayBackwardLeft;
	RaycastHit objectHit;
	GameObject[] activePassiveObjects;
	public GameObject[] allPassiveObjects;
	public bool hitting;
	Vector3 velocity;
	Vector3 forwardRightOffSet, forwardLeftOffSet, backwardOffSet;
	float time;
	public AudioClip[] hitObjectFront, hitObjectBehind;


	void Start () {
	
		hitting = false;
		activePassiveObjects = GameObject.FindGameObjectsWithTag("PassiveObject");
	}



	// Vibrate the controller
	IEnumerator vibrateOverTime(float left, float right)
	{
		while (time < 1) 
		{
			time += 0.00002f;
			GamePad.SetVibration (0, left, right);
		}

		if(time > 1)
		{
			GamePad.SetVibration (0, 0, 0);
			time = 0;
			yield return null;
		}
	}

	// Vibrate the controller if an object is collided with
	// And if the raycast is pointed at an object
	IEnumerator checkObjectHit(RaycastHit hit, AudioClip[] sounds)
	{
		for(int i = 0; i < allPassiveObjects.Length; i++)
		{
			if(objectHit.transform.name.Contains(allPassiveObjects[i].transform.name) && !audio.isPlaying)
			{
				playAudioClip(sounds[i]);

				if(Physics.Raycast(rayForward, out objectHit, 5))
				{
					StartCoroutine(vibrateOverTime(1, 1));
				}
			}
		}
		yield return null;
	}


	// Determine if objects collide
	void OnCollisionEnter()
	{
		// Check if a raycast has hit an object forward or behind
		if(Physics.Raycast(rayForward, out objectHit, 8) && !hitting)
		{
			StartCoroutine(checkObjectHit(objectHit, hitObjectFront));
		}
		
		if(Physics.Raycast(rayBackward, out objectHit, 8) && !hitting)
		{
			StartCoroutine(checkObjectHit(objectHit, hitObjectBehind));
		}

		hitting = true;
	}


	void OnCollisionExit()
	{
		hitting = false;
	}



	void Update () {
	
		gamePadState = GamePad.GetState(0);

		Debug.DrawRay(transform.position, new Vector3(transform.forward.x, -1, transform.forward.z) * 3, Color.blue);

		//forwardRightOffSet = new Vector3(transform.forward.x + transform.right.x * 0.5f, -1, transform.forward.z + transform.right.z * 0.5f);
		//forwardLeftOffSet = new Vector3(transform.forward.x - transform.right.x * 0.5f, -1, transform.forward.z - transform.right.z * 0.5f);
		//backwardOffSet = new Vector3(-transform.forward.x + transform.right.x * 0.5f, -1, -transform.forward.z + transform.right.z * 0.5f);

		rayForward = new Ray(transform.position, new Vector3(transform.forward.x, -1, transform.forward.z));
		//rayForwardRight = new Ray (transform.position, forwardRightOffSet);
		//rayForwardLeft = new Ray (transform.position, forwardLeftOffSet);

		rayBackward = new Ray(transform.position, new Vector3(-transform.forward.x, -1, -transform.forward.z));
	}
}
