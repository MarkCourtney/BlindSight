/* Mark Courtney
 * C09588817
 * Camera Position
 * 
 * The position of the camera is determined in this class. 
 * It can be zoomed, rotated and positioned in a location
 * that provides a vertical view of the environment.
*/

using UnityEngine;
using System.Collections;

public class CameraPosition : MonoBehaviour {
	
	Quaternion prevRotation, defaultRot;
	Vector3 prevPosition, defaultPos, origin;
	float distToCenter;
	
	bool topDown;
	
	void Start () {
		

		if (MapDimensionsGUI.roomShape == "LShaped") 
		{
			origin = new Vector3(-MapDimensionsGUI.floorLength / 5, 0, MapDimensionsGUI.floorLength / 5);
			transform.position = transform.position + new Vector3((MapDimensionsGUI.floorLength - MapDimensionsGUI.floorWidth) / -2, 0, 0);
		}
		else 
		{
			origin = new Vector3(0, 0, 0);
		}

		defaultPos = transform.position;
		defaultRot = transform.rotation;

		
		topDown = false;
		
		// Place a default value in dist so that the camera can intially zoom
		distToCenter = 10.0f;
	}
	
	
	void rotateCam(Vector3 direction)
	{
		transform.RotateAround(origin, direction, 90 * Time.deltaTime);
	}
	
	
	void zoomCam(Vector3 location)
	{
		transform.position += location * Time.deltaTime * 5;
	}
	
	
	void Update () {
		
		// Rotate the camera around the center of the floor
		if(Input.GetKey(KeyCode.A))
		{
			rotateCam(Vector3.up);
		}
		else if(Input.GetKey(KeyCode.D))
		{
			rotateCam(Vector3.down);
		}
		
		
		// Zoom the camera in and out
		// Can't exceed certain ranges
		if(Input.GetKey(KeyCode.Q) && distToCenter < 20.0f)
		{
			distToCenter = Vector3.Distance(transform.position, origin);
			zoomCam(-transform.forward);
		}
		else if(Input.GetKey(KeyCode.E) && distToCenter > 8.0f)
		{
			distToCenter = Vector3.Distance(transform.position, origin);
			zoomCam(transform.forward);
		}
		
		
		// Center the camera to on top of the floor
		// Allows for a different angle for placing objects
		if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !topDown)
		{
			// Ensures the camera doesn't keep resetting when W is pressed
			topDown = true;
			
			transform.position = Vector3.up * 10;
			transform.rotation = Quaternion.Euler(90, 0, 0);
		}
		
		// Return to the default view
		else if((Input.GetKeyDown(KeyCode.S) ||Input.GetKeyDown(KeyCode.DownArrow)) && topDown)
		{
			// Ensures the user can't keep reseting the camera when in default mode
			topDown = false;
			
			transform.position = defaultPos;
			transform.rotation = defaultRot;
		}
	}
}