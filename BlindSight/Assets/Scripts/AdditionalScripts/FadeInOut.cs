/* Mark Courtney
 * C09588817
 * FadeInOut
 * 
 * Functionality that fades between scenes. When a user
 * transitions to another scene the screen will fade out
 * the current scene and fade in the new.
*/

using UnityEngine;
using System.Collections;

public class FadeInOut : MonoBehaviour {
	
	public Texture2D backgroundColor;
	public float alpha;
	public bool fadeIn, fadeOut;
	string level;
	
	void Start () {
		
		fadeIn = true;
		fadeOut = false;
		alpha = 0.95f;
	}
	
	
	public void loadLevel(string levelName)
	{
		fadeIn = false;
		fadeOut = true;
		level = levelName;
	}
	
	void Update()	
	{
		if(alpha > 0.95f)
		{
			Application.LoadLevel(level);
		}
	}
	
	
	void drawTexture()
	{
		GUI.color = new Color(0, 0, 0, alpha);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundColor);
	}
	
	
	void OnGUI()
	{
		if(fadeIn)
		{
			alpha -= 0.0075f;
			drawTexture();
				
			if(alpha < 0)
			{
				alpha = 0;
				fadeIn = false;
			}
		}
		// Fade the screen from white to black
		if(fadeOut)
		{
			alpha += 0.0035f;
			drawTexture();
		}
	}
}