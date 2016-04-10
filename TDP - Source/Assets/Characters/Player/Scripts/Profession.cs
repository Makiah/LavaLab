
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script manages the properties of each race that will be used in the game (male character, female character, wizard, etc.  
 * Resources are loaded based on the path that is given for each element, and then placed into the game.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class Profession {
	//Components that do not depend on gender.
	public Sprite icon;
	public readonly Sprite maleHead, femaleHead, body, arm1, arm2, leg1, leg2;
	public string name;
	public int professionID;
	public ResourceReferenceWithStack[] initialObjects;

	//Profession constructor
	public Profession(string resourcesPath, string ctorName, int ctorProfessionID, ResourceReferenceWithStack[] ctorInitialObjects) {
		//Load sprite resources from the Resources folder.  
		icon = Resources.Load <Sprite> (resourcesPath + "Icon");

		//Sprites
		maleHead = Resources.Load <Sprite> (resourcesPath + "Male Head");
		femaleHead = Resources.Load <Sprite> (resourcesPath + "Female Head");
		body = Resources.Load <Sprite> (resourcesPath + "Body");
		leg1 = Resources.Load <Sprite> (resourcesPath + "Leg1");
		leg2 = Resources.Load <Sprite> (resourcesPath + "Leg2");
		arm1 = Resources.Load <Sprite> (resourcesPath + "Arm1");
		arm2 = Resources.Load <Sprite> (resourcesPath + "Arm2");

		//ID requirements
		professionID = ctorProfessionID;
		name = ctorName;

		//Put each initial item in the hotbar.  
		if (ctorInitialObjects != null) {
			initialObjects = ctorInitialObjects;
		}
	}

}
