/* Mark Courtney
 * C09588817
 * Create Floor Walls
 * 
 * The room shape and dimensions are taken and a environment
 * is created based on these. There are 4 possible room shapes
 * including square, rectangle, L-shaped and cross section.
 * This class creates the floors and walls for whichever 
 * environment is being created.
*/

using UnityEngine;
using System.Collections;

public class CreateFloorWalls : MonoBehaviour {

	public Material wallMaterial;	
	public GameObject floor, wall, light;
	public string roomShape;
	
	// Must create the floor and walls first
	void Awake () {

		wall.renderer.material = wallMaterial;
		roomShape = MapDimensionsGUI.roomShape;
		
		float length = MapDimensionsGUI.floorLength;
		float width = MapDimensionsGUI.floorWidth;
		
		
		// Default rectangle room if they manage to bypass the map dimensions scene
		if(length == 0 || width == 0 || roomShape == "")
		{
			length = 8;
			width = 8;
			roomShape = "Square";
		}
		
		// Determine if the Rectangle room is Square shaped
		// Room shape used in the hint system
		else if(roomShape == "Rectangle" && length == width)
		{
			roomShape = "Square";		
		}
		
		
		Vector3 scale = Vector3.up;
		// Scale the width and length appropiately to the given length/width
		scale.x = length / 10;
		scale.z = width / 10;
		floor.transform.localScale = scale;
		
		
		
		GameObject f, w, l;
		
		if(roomShape == "Rectangle" || roomShape == "Square")
		{
			// Instanciate the floor
			f = Instantiate(floor, Vector3.zero, Quaternion.identity) as GameObject;
			f.name = floor.name;
			
			
			scale = Vector3.up;
			scale.z = length;
			scale.y = 3;
			wall.transform.localScale = scale;
			
			w = Instantiate(wall, new Vector3(0, 1.5f, width / 2), Quaternion.Euler(Vector3.up * 90)) as GameObject;
			w.name = wall.name;
			
			w = Instantiate(wall, new Vector3(0, 1.5f, -width / 2), Quaternion.Euler(Vector3.up * 90)) as GameObject;
			w.name = wall.name;
			
			
			
			scale = Vector3.up;
			scale.z = width;
			scale.y = 3;
			wall.transform.localScale = scale;
			
			w = Instantiate(wall, new Vector3(length/2, 1.5f, 0), Quaternion.identity) as GameObject;
			w.name = wall.name;
			
			w = Instantiate(wall, new Vector3(-length/2, 1.5f, 0), Quaternion.identity) as GameObject;
			w.name = wall.name;
			
			
			// Have one light if the room is square shaped or rectanguler with the length less than 30
			if(roomShape == "Square" || (roomShape == "Rectangle" && length < 30))
			{
				l = Instantiate(light, new Vector3(0, 10, 0),  Quaternion.identity) as GameObject;
				l.name = light.name;
			}
			else
			{
				l = Instantiate(light, new Vector3(length / 4, 10, 0),  Quaternion.identity) as GameObject;
				l.name = light.name;
				
				l = Instantiate(light, new Vector3(-length / 4, 10, 0),  Quaternion.identity) as GameObject;
				l.name = light.name;
			}
		}
		
		
		else if(roomShape == "LShaped")
		{
			// Make the first floor slightly smaller
			// Ensures the floors and thus the textures don't overlap
			scale.x = (length - width) / 10;
			scale.z = width / 10;
			floor.transform.localScale = scale;
			
			f = Instantiate(floor, new Vector3((width/2), 0, 0), Quaternion.identity) as GameObject;
			f.name = floor.name;
			
			
			
			scale.x = length / 10;
			scale.z = width / 10;
			floor.transform.localScale = scale;
			
			f = Instantiate(floor, new Vector3((width-length)/2, 0, (length-width)/2), Quaternion.Euler(new Vector3(0,90,0))) as GameObject;
			f.name = floor.name;

			if(length >= 13)
			{
				l = Instantiate(light, new Vector3(length/8, 10, 0),  Quaternion.identity) as GameObject;
				l.name = light.name;
				
				l = Instantiate(light, new Vector3((length - width) / -2, 10, (length - width) / 2),  Quaternion.identity) as GameObject;
				l.name = light.name;
			}
			else
			{
				l = Instantiate(light, new Vector3((length - width) / -2, 10, 0),  Quaternion.identity) as GameObject;
				l.name = light.name;
			}
			
			// End Walls
			scale = Vector3.up;
			scale.x = width;
			scale.y = 3;
			wall.transform.localScale = scale;
			
			w = Instantiate(wall, new Vector3(-(length-width)/2, 1.5f, length - width/2), Quaternion.identity) as GameObject;
			w.name = wall.name;
			
			w = Instantiate(wall, new Vector3(length/2, 1.5f, 0), Quaternion.Euler(new Vector3(0,90,0))) as GameObject;
			w.name = wall.name;
			
			
			// Longer Outer Walls
			scale = Vector3.up;
			scale.z = length;
			scale.y = 3;
			wall.transform.localScale = scale;
			
			w = Instantiate(wall, new Vector3(-length/2, 1.5f, (length-width)/2), Quaternion.identity) as GameObject;
			w.name = wall.name;
			
			w = Instantiate(wall, new Vector3(0, 1.5f, -width/2), Quaternion.Euler(new Vector3(0,90,0))) as GameObject;
			w.name = wall.name;
			
			
			// Shorter Outer Walls
			scale = Vector3.up;
			scale.x = width - length;
			scale.y = 3;
			wall.transform.localScale = scale;
			
			w = Instantiate(wall, new Vector3(width/2, 1.5f, width/2), Quaternion.identity) as GameObject;
			w.name = wall.name;
			
			w = Instantiate(wall, new Vector3(-(length/2-width), 1.5f, length/2), Quaternion.Euler(new Vector3(0,90,0))) as GameObject;
			w.name = wall.name;
		}
		
		
		else if(roomShape == "Cross")
		{	
			f = Instantiate(floor, Vector3.zero, Quaternion.Euler(new Vector3(0,90,0))) as GameObject;
			f.name = floor.name;
			
			// Adjust the scale so that two smaller floors can be created
			scale.x = (length/2 - width/2) / 10;
			scale.z = width / 10;
			floor.transform.localScale = scale;
			
			// Make two smaller floors that sit beside the longer floor
			f = Instantiate(floor, new Vector3((length+width)/4, 0, 0), Quaternion.identity) as GameObject;
			f.name = floor.name;
			
			f = Instantiate(floor, new Vector3(-(length+width)/4, 0, 0), Quaternion.identity) as GameObject;
			f.name = floor.name;
			
			
			l = Instantiate(light, new Vector3(0, 10, 0),  Quaternion.identity) as GameObject;
			l.name = light.name;
			
			
			// End Walls
			scale = Vector3.up;
			scale.z = width;
			scale.y = 3;
			wall.transform.localScale = scale;
			
			w = Instantiate(wall, new Vector3(0, 1.5f, -length/2), Quaternion.Euler(new Vector3(0,90,0))) as GameObject;
			w.name = wall.name;
			
			w = Instantiate(wall, new Vector3(0, 1.5f, length/2), Quaternion.Euler(new Vector3(0,90,0))) as GameObject;
			w.name = wall.name;
			
			w = Instantiate(wall, new Vector3(length/2, 1.5f, 0), Quaternion.identity) as GameObject;
			w.name = wall.name;
			
			w = Instantiate(wall, new Vector3(-length/2, 1.5f, 0), Quaternion.identity) as GameObject;
			w.name = wall.name;
			
			
			
			// Outer Walls
			scale = Vector3.up;
			scale.x = (length / 2) - (width / 2);
			scale.y = 3;
			wall.transform.localScale = scale;
			
			w = Instantiate(wall, new Vector3((length-scale.x)/2, 1.5f, width / 2), Quaternion.identity) as GameObject;
			w.name = wall.name;
			
			w = Instantiate(wall, new Vector3((length-scale.x)/2, 1.5f, -width / 2), Quaternion.identity) as GameObject;
			w.name = wall.name;
			
			w = Instantiate(wall, new Vector3(-(length-scale.x)/2, 1.5f, -width / 2), Quaternion.identity) as GameObject;
			w.name = wall.name;
			
			w = Instantiate(wall, new Vector3(-(length-scale.x)/2, 1.5f, width / 2), Quaternion.identity) as GameObject;
			w.name = wall.name;
			
			w = Instantiate(wall, new Vector3(width / 2, 1.5f, (length-scale.x)/2), Quaternion.Euler(new Vector3(0,90,0))) as GameObject;
			w.name = wall.name;
			
			w = Instantiate(wall, new Vector3(width / 2, 1.5f, -(length-scale.x)/2), Quaternion.Euler(new Vector3(0,90,0))) as GameObject;
			w.name = wall.name;
			
			w = Instantiate(wall, new Vector3(-width / 2, 1.5f, (length-scale.x)/2), Quaternion.Euler(new Vector3(0,90,0))) as GameObject;
			w.name = wall.name;
			
			w = Instantiate(wall, new Vector3(-width / 2, 1.5f, -(length-scale.x)/2), Quaternion.Euler(new Vector3(0,90,0))) as GameObject;
			w.name = wall.name;
		}
	}
}