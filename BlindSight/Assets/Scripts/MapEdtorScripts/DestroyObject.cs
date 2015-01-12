/* Mark Courtney
 * C09588817
 * Destroy Object
 * 
 * Allows the user to remove any object within a scene; provided
 * the object isn't a wall or floor.
*/

using UnityEngine;
using System.Collections;

public class DestroyObject : MonoBehaviour {

	Ray ray;
	RaycastHit hit;
	
	void Update () {
		
		// Right click to destroy objects in the scene
		if(Input.GetMouseButtonDown(1))
		{
			// Get the ray from the mouse position to the object on screen
			// And the object the ray hits
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			hit = new RaycastHit();
			
			if(Physics.Raycast(ray, out hit, 1000)) 
		    {
				// Delete objects except floor
				// Embedded statements as the OR operator doesn't work as intended
				if(hit.transform.tag != "Floor")
					if(hit.transform.tag != "Wall")
						Destroy(hit.transform.gameObject);
			}
		}
	}
}