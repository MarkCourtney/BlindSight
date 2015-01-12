/* Mark Courtney
 * C09588817
 * Create Maps Directory
 * 
 * This class checks if the Maps directory and Customs/Pre-Made
 * sub directories have been created. If not the Documents folder
 * is located on the users machine and creates the relevant directories.
 * If the directories are present then this process is skipped.
*/


using UnityEngine;
using System;
using System.IO;
using System.Collections;

public class CreateMapsDirectory : MonoBehaviour {
	
	string path;
	string folderName;
	
	// Create the Map folders in MyDocuments
	void Start () {
		
		// Only create these folders if they can't be found
		
		// For first time start up
		// Create a new folder in MyDocuments called Maps
		folderName = "Maps";
		path = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + '/' + folderName;

		// If the path to Maps doesn't exist then create it
		if(!Directory.Exists(path))
			Directory.CreateDirectory(path);
		
		
		// Create a folder called Pre-Made Maps in the Maps folder
		folderName = "Pre-Made Maps";
		path = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Maps/" + folderName;


		// If the path to Pre-Made Maps doesn't exist then create it
		if(!Directory.Exists(path))
			Directory.CreateDirectory(path);
		
		
		// Create a folder called Custom Maps in the Maps folder
		folderName = "Custom Maps";
		path = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Maps/" + folderName;

		// If the path to Custom Maps doesn't exist then create it
		if(!Directory.Exists(path))
			Directory.CreateDirectory(path);
	}
}