/* Mark Courtney
 * C09588817
 * Camera Edit Object
 * 
 * This class allows the user to click on objects in
 * order to manipulate them. When the object is held 
 * and the mouse position is over the floor the object
 * will be placed in the position of the mouse. The user
 * can then press Enter to place the object permanently.
 * The object can later be altered if desired.
*/

using UnityEngine;
using System.Collections;
using System;

public class CameraEditObject : MonoBehaviour {

	public static GameObject objectHit;
	GameObject[] walls;
	
	public GUIStyle style;
	public AudioClip alert;
	
	Ray ray, positionRay;
	RaycastHit hit;
	
	Vector3 mousePos, objectPos, transformRot;
	
	float adjustSpeed = 5;
	float y, timer;
	
	public bool held, snapMode, hideWalls, wallCollide;

	SaveScene ss;
	
	void Awake()
	{
		snapMode = true;
		held = false;
		hideWalls = false;
		wallCollide = false;
	}
	
	void Start () {

		ss = GetComponent<SaveScene> ();

		Screen.showCursor = true;
		
		transformRot = Vector3.zero;
		
		walls = GameObject.FindGameObjectsWithTag("Wall");

		foreach(GameObject wall in walls)
		{
			wall.layer = LayerMask.NameToLayer("Ignore Raycast");
		}
	}
	
	
	
	// Display a message in the center of the screen
	IEnumerator displayMessage(string message)
	{
		timer += Time.deltaTime;
			
		if(timer < 7)
		{
			GUILayout.BeginArea(new Rect(Screen.width/2 - 200, Screen.height/2 - 40, 500, 200));
			GUILayout.Label(message, style);
			GUILayout.EndArea();
			
			yield return null;
		}
		else
		{
			timer = 0;
			yield return null;
		}
	}

	
	void Update () 
	{
		// Get the current mouse position
		mousePos = Input.mousePosition;
		
		
		// Stop the object from entering the GUI
		if(mousePos.x < 150)
			mousePos.x = 150;
		
		
		// Create the ray using the mouse position to world coordinates
		// And the object the cast will hit
		ray = Camera.main.ScreenPointToRay(mousePos);
		
		hit = new RaycastHit();
		
		
		// Left click to produce a ray from the camera to the mouse location
		// Don't allow clicking of objects when saving
		if(Input.GetMouseButtonDown(0) && !held && !ss.saving)
		{
			if(Physics.Raycast(ray, out hit, 1000)) 
		    {
				// Get the object hit by the raycast, as long as it's not the floor
				// Embedded statements as the OR operator didn't work
				if(hit.transform.tag != "Floor")
				{
					if(hit.transform.tag != "Wall")
					{
						objectHit = hit.collider.gameObject;
						
						held = true;
					}
				}
		    }
		}
		

		// If an object has been clicked on
		if(held)
		{
			// If the object is deleted while being held turn off the held bool
			// Otherwise it assumes an object is still being held
			if(objectHit.Equals(null))
			{
				held = false;
			}

			
			// If the raycast doesnt hit anything
			// Set the position a certain distance from the camera
			if(Physics.Raycast(ray, out hit, 1000)) {}
			else
			{	
				objectHit.transform.position = ray.origin + ray.direction * 10;	
			}
				
				
			// Ignore the raycast so that the item can be moved with the mouse
			objectHit.layer = 0;
			
			// Turn off kinematic so that the 
			objectHit.rigidbody.isKinematic = false;
			
			// Get all the objects hit by the raycast
			RaycastHit[] rayHits = Physics.RaycastAll(ray, 100);
			

			// Iterate through all the objects hit
			foreach(RaycastHit rH in rayHits)
			{
				// Only allow the ray to hit the floor
				
				if(rH.transform.tag == "Floor")
				{
					if(objectHit.name == "Telephone")
					{
						y = 1.45f;
					}
					else if(objectHit.name.Contains("Window"))
					{
						y = 1.8f;
					}
					else if(objectHit.name == "Kettle")
					{
						y = 1.45f;
					}
					else if(objectHit.name == "LightSwitch")
					{
						y = 1.5f;
					}
					else if(objectHit.name.Contains("Fire"))
					{
						y = 0.05f;
					}
					else
					{
						y = objectHit.collider.bounds.extents.y;
					}
					
					//Place the position of the object on the position of the floor
					objectHit.transform.position = new Vector3(rH.point.x + (rH.point.x * 0.01f), y, rH.point.z  + (rH.point.z * 0.01f));
				}
			}
			
			
			
			
			// Faster/Slower orientation of the object
			if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				adjustSpeed = 1;
			else
				adjustSpeed = 3;
			
			
			// If snap mode enabled turn rotate the object by 45 degree angles
			if(snapMode)
			{
				if(Input.GetKeyDown(KeyCode.RightArrow))
					transformRot += Vector3.up * 45;
				if(Input.GetKeyDown(KeyCode.LeftArrow))
					transformRot += Vector3.down * 45;
				
				objectHit.transform.rotation = Quaternion.Euler(transformRot);
			}
			else
			{
				// Turn the chair to more accurently suit the world
				if(Input.GetKey(KeyCode.RightArrow))
					objectHit.transform.Rotate(Vector3.up * adjustSpeed);
				if(Input.GetKey(KeyCode.LeftArrow))
					objectHit.transform.Rotate(Vector3.down  * adjustSpeed);
			}
			
			// Finish moving the object
			if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
			{
				if(DetectWallCollision.collide)
				{
					wallCollide = true;
					
					audio.clip = alert;
					audio.Play();
				}
				else
				{
					wallCollide = false;
					objectHit.layer = 0;
					objectHit.rigidbody.isKinematic = true;
					
					held = false;
				}
			}
		}


		
		
		
		// Hide the walls
		if(hideWalls)
		{
			foreach(GameObject wall in walls)
			{
				wall.renderer.enabled = false;
				wall.collider.enabled = true;
			}
		}
		// Display the walls
		else
		{
			foreach(GameObject wall in walls)
			{
				wall.renderer.enabled = true;
				wall.collider.enabled = true;
			}
		}
	}
}