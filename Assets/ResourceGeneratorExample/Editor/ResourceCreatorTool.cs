/* ResourceCreatorTool.cs - Placed in Editor folder
 * 
 * REQUIRES: JsonHelper class for serialistion of lists (also placed in Editor folder)
 * 
 * Written by: [Trello Username] dannyt8, 7 Feb 2017
 * Description: An editor tool to create new Resources
 * 
 * Changing variables:
 * When changing variables in the Resource class, changes to the editor tool are required to reflect these changes.
 * New variables need to be added and functions need to be altered.
 * 1. Go to the variables section in the OnGUI() function.
 * 2. Enter the new variable(s) following the format and layout of the existing variables.
 * 	  Note: the fields reflect the input that they receive (i.e. IntField will only accept ints).
 * 	  These will represent the input fields in the inspector.
 * 3. Go to the ClearEntries() function.
 * 4. Enter the new variable(s) following the format and layout of the existing variables.
 * 	  Note: these will be the default variables that will be inserted into the input fields when a new resource is added.
 * 5. Go to the AddToList() function.
 * 6. Enter the new variable(s) following the format and layout of the existing variables.
 * 	  Note: the list will be serialised.
 * 
 * Important: The JsonHelper class is required to help serialise a list to write to a .json file. The class uses
 * 			  an array to serialise into Json, hence the need to convert our dynamic list into an array.
 * 		Code copied from: http://www.boxheadproductions.com.au/deserializing-top-level-arrays-in-json-with-unity/
 * 
 * Important: Ensure that the path and fileName variables are correct if an error occurs.
 * Important: Refresh list if you've made changes to the file directly. And be sure to make valid changes.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ResourceCreatorTool : EditorWindow {

	// our own reference for variables
	Resource referenceResource = new Resource();

	// List of current resources as a reference
	public List<Resource> resources = new List<Resource>();

	// serialisation setting variables
	bool serialisationFoldOut = false;
	bool readableFile = true;
	bool readFileOnLoad = true; // read from file when tool loads. May not be favourable as path might not exist
	public string fileName = "ResourceList";
	public string path = "Assets/";

	// temporary style (needed)
	GUIStyle tempStyle = new GUIStyle();

	// to display lists
	bool listDisplay = false;
	bool resourceSlot = false;
	Vector2 scrollPos;

	// reference to the editor window
	static EditorWindow window;

	// Open the window
	[MenuItem ("Window/NanoTools/Resource Creator")]
	public static void Initialize() {
		window = EditorWindow.GetWindow (typeof(ResourceCreatorTool));
	}

	// Constructor
	public ResourceCreatorTool() {
		// Change tab name
		this.titleContent = new GUIContent ("Resource Creator");
		try {
			if (readFileOnLoad) {
				DeserialiseList ();
			}
		} catch (FileNotFoundException e) {
			Debug.Log ("File not found in directory. Creating new...");
			SerialiseList ();
		}
	}

	// Draw GUI
	void OnGUI() {
		EditorGUILayout.BeginVertical ();

		// Header
		EditorGUILayout.Space ();
		tempStyle.fontSize = 15; // Change GUI Style
		tempStyle.alignment = TextAnchor.MiddleCenter;
		EditorGUILayout.LabelField ("Resource Creator", tempStyle); // Show Label
		tempStyle = new GUIStyle ();
		EditorGUILayout.Space ();

		// **** Variables ****
		referenceResource.id = EditorGUILayout.IntField ("ID", referenceResource.id); // show variables
		referenceResource.name = EditorGUILayout.TextField ("Name", referenceResource.name);
		referenceResource.description = EditorGUILayout.TextField ("Description", referenceResource.description);
		referenceResource.source = EditorGUILayout.TextField ("Source", referenceResource.source);
		// Enter new variables here:


		// Buttons
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Add")) { // Show Button
			AddToList();
			SerialiseList ();
			DeserialiseList ();
		}

		if (GUILayout.Button ("Refresh List")) { // Show Button
			DeserialiseList ();
		}
		EditorGUILayout.EndHorizontal ();


		// Serialisation settings
		EditorGUILayout.Space ();
		// Create fold out
		serialisationFoldOut = EditorGUILayout.Foldout (serialisationFoldOut, "Serialisation Settings", true);
		if (serialisationFoldOut) {
			EditorGUI.indentLevel++; // indent

			// Show serialisation setting variables
			fileName = EditorGUILayout.TextField ("File Name", fileName);
			path = EditorGUILayout.TextField ("Path", path);
			readableFile = EditorGUILayout.Toggle("Is Readable", readableFile);
			readFileOnLoad = EditorGUILayout.Toggle ("Read File On Load", readFileOnLoad);

			EditorGUI.indentLevel--; // un-indent
		}
		EditorGUILayout.Space ();


		// Resource List Display
		listDisplay = EditorGUILayout.Foldout (listDisplay, "Resource List", true);
		if (listDisplay) {
			scrollPos = EditorGUILayout.BeginScrollView (scrollPos, false, false);
			foreach (Resource resource in resources) {
				EditorGUI.indentLevel++; // indent

				resourceSlot = EditorGUILayout.Foldout (resourceSlot, resource.id + ": " + resource.name, true);
				if (resourceSlot) {
					EditorGUI.indentLevel++; // indent

					EditorGUILayout.LabelField ("ID", resource.id.ToString ());
					EditorGUILayout.LabelField ("Name", resource.name);
					EditorGUILayout.LabelField ("Description", resource.description);
					EditorGUILayout.LabelField ("Source", resource.source);

					EditorGUI.indentLevel--; // un-indent
				}

				EditorGUI.indentLevel--; // un-indent
			}
			EditorGUILayout.EndScrollView ();
		}

		EditorGUILayout.EndVertical ();
	}

	// --------- GUI related functions ---------

	// Clears all text from the input fields in the window and increments id by 1
	void ClearEntries() {
		referenceResource.id += 1;
		referenceResource.name = "";
		referenceResource.description = "";
		referenceResource.source = "";
		// Enter new variables here:

	}

	// --------- Serialisation functions ---------

	// Serialises the current list of items
	void SerialiseList() {
		string fileString = JsonHelper.ToJson (ConvertToArray(), readableFile);
		File.WriteAllText (path + fileName + ".json", fileString);
	}

	// Deserialises the file from the given directory
	void DeserialiseList() {
		string fileString = File.ReadAllText (path + fileName + ".json");
		Debug.Log ("Deserialise: " + fileString);
		ConvertToList (JsonHelper.FromJson<Resource> (fileString));
	}

	// --------- Serialisation Helper functions ---------

	// Converts our list into an arry for the JsonHelper class
	Resource[] ConvertToArray() {
		Resource[] resourceArray = new Resource[resources.Count];
		// Add everything to the array
		for (int i = 0; i < resources.Count; i++) {
			resourceArray [i] = resources [i];
		}

		return resourceArray;
	}

	// Converts the array from the JsonHelper Class
	void ConvertToList(Resource[] array) {
		resources.Clear ();
		// Add everything to the list
		for (int i = 0; i < array.Length; i++) {
			resources.Add (array [i]);
		}
	}

	// Adds entry to list
	void AddToList() {
		Resource resource = new Resource ();

		// Assign values
		resource.id = referenceResource.id;
		resource.name = referenceResource.name;
		resource.description = referenceResource.description;
		resource.source = referenceResource.source;
		// Enter new variables here:


		// Check for if entry already exists
		if (!CheckResourceDuplicate (resource)) {
			resources.Add (resource);
			Debug.Log ("Resource Added: " + resource.name);
			ClearEntries ();
		}
	}

	// Checks if the entry id or name already exists
	bool CheckResourceDuplicate(Resource resource) {
		foreach (Resource source in resources) {
			if (resource.id == source.id) {
				Debug.Log ("ID: " + resource.id + " is already in use by; " + source.id + ": " + source.name);
				return true;
			} else if (resource.name.ToLower ().Equals (source.name.ToLower ())) {
				Debug.Log ("Name: " + resource.name + " is already in use by; " + source.id + ": " + source.name);
				return true;
			}
		}

		return false;
	}
}

//// Resource Type Class
//[System.Serializable]
//public class Resource {
//	public int id = 0;
//	public string name = "";
//	public string description = "";
//	public string source = "";
//}

