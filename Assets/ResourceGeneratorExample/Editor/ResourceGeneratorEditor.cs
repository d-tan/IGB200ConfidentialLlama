/* ResourceGeneratorEditor.cs (was called RenewableResourceEditor.cs) - Placed in the Editors folder
 * 
 * Written by: Mahir Muhammed, Added to Trello on 23 Jul, 2016
 * 		Edited by: [Trello Username] dannyt8, 13 Feb 2017
 * Description: A custom editor for the Resource generator. Used to select what resources can be gathered from the generator.
 * 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(ResourceGenerator))]
public class ResourceGeneratorEditor : Editor
{

	// Serialisation settings
	public string path = "Assets/";
	public string fileName = "ResourceList";
	bool serialiseSettings = false;
	string fileString = "";

	// Reference resources list
	Resource[] resourceArray;
	string[] displayArray;

	// Selecting resources
	// Harvest
	bool showHarvestResources = false;
	int harvestListSize = 0;
	List<bool> showHarvests = new List<bool>();

	// Destroy
	bool showDestroyResources = false;
	int destroyListSize = 0;
	List<bool> showDestroys = new List<bool>();

	// Reference ResourceGenerator script
	ResourceGenerator resourceGenerator;

	// Constructor
	public ResourceGeneratorEditor () {
		Deserialise (); // Read from .json file
	}

	void OnEnable() {
		resourceGenerator = target as ResourceGenerator; // get ResourceGenerator script

		harvestListSize = resourceGenerator.harvestResources.Count; // keep size
		destroyListSize = resourceGenerator.destroyResources.Count;

		Deserialise (); // Read from .json file
	}
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

		// Resource Type selection
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Select Resources Here");
//      index = EditorGUILayout.Popup(index, (string[])RenewableResourceEditorWindow.resourceTypes.ToArray(typeof(string)));
//		resourceGenerator.resourceIndex =  EditorGUILayout.Popup(resourceGenerator.resourceIndex, displayArray);

		// Group Harvest stuff
		showHarvestResources = EditorGUILayout.Foldout (showHarvestResources, "Harvest Resources", true);
		if (showHarvestResources) {
			EditorGUI.indentLevel++;

			// check if list is null
			if (resourceGenerator.harvestResources == null) {
				resourceGenerator.harvestResources = new List<ResourceMeta> ();
			}

			// Size input
			harvestListSize = EditorGUILayout.IntField ("List Size", harvestListSize);

			// Update list sizes
			UpdateList (harvestListSize, showHarvests);
			UpdateList (harvestListSize, resourceGenerator.harvestResources);

			// Show all elements
			for (int i = 0; i < harvestListSize; i++) {
				// Group each resource
				showHarvests [i] = EditorGUILayout.Foldout (showHarvests [i], i.ToString (), true);
				if (showHarvests [i]) {
					EditorGUI.indentLevel++;

					// Resource selection
					resourceGenerator.harvestResources [i].index = EditorGUILayout.Popup (resourceGenerator.harvestResources [i].index, displayArray);
					resourceGenerator.harvestResources [i].resource = resourceArray [resourceGenerator.harvestResources [i].index];

					// min, max settings
					EditorGUILayout.BeginHorizontal ();
					resourceGenerator.harvestResources [i].min = EditorGUILayout.IntField ("Min Collectable", resourceGenerator.harvestResources [i].min);
					resourceGenerator.harvestResources [i].max = EditorGUILayout.IntField ("Max Collectable", resourceGenerator.harvestResources [i].max);
					EditorGUILayout.EndHorizontal ();

					// Min > max detection
					Debug.Assert (resourceGenerator.harvestResources [i].min <= resourceGenerator.harvestResources [i].max, "Min cannot be greater than Max.");
					if (resourceGenerator.harvestResources [i].min > resourceGenerator.harvestResources [i].max) {
						resourceGenerator.harvestResources [i].min = resourceGenerator.harvestResources [i].max;
					}

					EditorGUI.indentLevel--;
				}
			}

			EditorGUI.indentLevel--;
		}

		// Group Destroy stuff
		showDestroyResources = EditorGUILayout.Foldout (showDestroyResources, "Destroy Resources", true);
		if (showDestroyResources) {
			EditorGUI.indentLevel++;

			// Check if list is null
			if (resourceGenerator.destroyResources == null) {
				resourceGenerator.destroyResources = new List<ResourceMeta> ();
			}

			// Size input
			destroyListSize = EditorGUILayout.IntField ("List Size", destroyListSize);

			// Update list sizes
			UpdateList (destroyListSize, showDestroys);
			UpdateList (destroyListSize, resourceGenerator.destroyResources);

			// Show all elements
			for (int i = 0; i < destroyListSize; i++) {
				// Group each resource
				showDestroys [i] = EditorGUILayout.Foldout (showDestroys [i], i.ToString (), true);
				if (showDestroys [i]) {
					EditorGUI.indentLevel++;

					// Resource selection
					resourceGenerator.destroyResources [i].index = EditorGUILayout.Popup (resourceGenerator.destroyResources [i].index, displayArray);
					resourceGenerator.destroyResources [i].resource = resourceArray [resourceGenerator.destroyResources [i].index];

					// min, max settings
					EditorGUILayout.BeginHorizontal ();
					resourceGenerator.destroyResources [i].min = EditorGUILayout.IntField ("Min Collectable", resourceGenerator.destroyResources [i].min);
					resourceGenerator.destroyResources [i].max = EditorGUILayout.IntField ("Max Collectable", resourceGenerator.destroyResources [i].max);
					EditorGUILayout.EndHorizontal ();

					// Min > max detection
					Debug.Assert (resourceGenerator.destroyResources [i].min <= resourceGenerator.destroyResources [i].max, "Min cannot be greater than Max.");
					if (resourceGenerator.destroyResources [i].min > resourceGenerator.destroyResources [i].max) {
						resourceGenerator.destroyResources [i].min = resourceGenerator.destroyResources [i].max;
					}

					EditorGUI.indentLevel--;
				}
			}

			EditorGUI.indentLevel--;
		}

		// Resource Creator tool button
		EditorGUILayout.BeginHorizontal ();
        if (GUILayout.Button("Add Resource"))
        {
//            RenewableResourceEditorWindow.Init();
			ResourceCreatorTool.Initialize ();
        }
		if (GUILayout.Button ("Refresh List")) {
			Deserialise ();
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.HelpBox ("When in doubt:\n 1. Check serialisation settings to see if the file name and location is correct.\n 2. Ensure that at least 1 item is added to the serialised file AND IDs match in Inventory Master.\n 3. Refresh list.\n 4. Apply prefab (if applicable)", MessageType.Info);

		// Serialisation Settings
		EditorGUILayout.Space ();
		EditorGUILayout.HelpBox ("Don't forget to check the serialisation settings if you're getting an error about file location.", MessageType.Info);
		serialiseSettings = EditorGUILayout.Foldout (serialiseSettings, "Serialisation Settings", true);
		if (serialiseSettings) {
			fileName = EditorGUILayout.TextField ("File Name", fileName);
			path = EditorGUILayout.TextField ("Path", path);
		}
    }

	// Reads from file of given path and name, converts to array with JsonHelper class and stores as reference
	void Deserialise() {
		fileString = File.ReadAllText (path + fileName + ".json"); // Read file
		resourceArray = JsonHelper.FromJson<Resource> (fileString); // convert to array

		displayArray = new string[resourceArray.Length]; // Create string array for resource selection
		for (int i = 0; i < displayArray.Length; i++) {
			displayArray[i] = resourceArray [i].id.ToString () + ": " + resourceArray [i].name; // assign strings
		}
	}

	void UpdateList(int listSize, List<int> indexList, int min = 0, int max = 20, int defaultIndex = 0) {
		if (listSize >= min && listSize <= max) {
			Debug.Log (indexList);
			if (indexList.Count < listSize) {
				for (int i = indexList.Count; i < listSize; i++) {
					indexList.Add (defaultIndex);
				}
			} else if (indexList.Count > listSize) {
				for (int i = indexList.Count - 1; i > listSize; i--) {
					indexList.RemoveAt (i);
				}
			}
		} else {
			EditorGUILayout.HelpBox ("Invalid List Size. We only allow between " + min + " and " + max +  " Resources.", MessageType.Warning);
		}
	}

	void UpdateList(int listSize, List<ResourceMeta> metaList, int min = 0, int max = 20) {
		if (listSize >= min && listSize <= max) {
			if (metaList.Count < listSize) {
				for (int i = metaList.Count; i < listSize; i++) {
					metaList.Add (new ResourceMeta());
				}
			} else if (metaList.Count > listSize) {
				for (int i = metaList.Count; i > listSize; i--) {
					metaList.RemoveAt (i - 1);
				}
			}
		} else {
			EditorGUILayout.HelpBox ("Invalid List Size. We only allow between " + min + " and " + max +  " Resources.", MessageType.Warning);
		}
	}

	void UpdateList(int listSize, List<bool> boolList) {
		if (listSize >= 0) {
			if (boolList.Count < listSize) {
				for (int i = boolList.Count; i < listSize; i++) {
					boolList.Add (false);
				}
			} else if (boolList.Count > listSize) {
				for (int i = boolList.Count; i > listSize; i--) {
					boolList.RemoveAt (i - 1);
				}
			}
		}
	}

}