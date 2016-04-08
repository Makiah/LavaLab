
/*
 * Author: Makiah Bennett
 * Last edited: 27 September 2015
 * 
 * The ResourceDatabase controls the items that are used during the game, by defining the items beforehand during the 
 * InitializeDatabase part of the EventManager script. 
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceDatabase : MonoBehaviour {

	public static List <ResourceReference> masterItemList = new List<ResourceReference> ();
	public static List <ItemCombination> masterItemCombinationList = new List<ItemCombination> ();

	public static List <Profession> gameProfessions = new List <Profession> ();
	
	public static void InitializeDatabase() {

		/******************************************* ITEMS *******************************************/
		//Tools
		//masterItemList.Add (new ResourceReference (ResourceReference.ItemType.GameTool, "Wooden Sword", "A weak sword, but useful for survival.", 0, "Weapons/Wooden/WoodenSword/"));

		/******************************************* RACES *******************************************/
		//Agent (Default)
		gameProfessions.Add (new Profession ("Professions/Agent/", "Agent", 0, null));
	}

	public static Profession GetRaceByParameter(string specifiedName) {
		for (int i = 0; i < gameProfessions.Count; i++) {
			if (gameProfessions[i].name == specifiedName) 
				return gameProfessions[i];
		}

		return null;
	}

	public static Profession GetRaceByParameter(int specifiedID) {
		for (int i = 0; i < gameProfessions.Count; i++) {
			if (gameProfessions[i].professionID == specifiedID) 
				return gameProfessions[i];
		}
		
		return null;
	}

	public static ResourceReference GetItemByParameter(string specifiedName) {
		for (int i = 0; i < masterItemList.Count; i++) {
			if (masterItemList[i].itemScreenName == specifiedName) 
				return masterItemList[i];
		}
		
		return null;
	}
	
	public static ResourceReference GetItemByParameter(ResourceReference.ItemType toolType, int specifiedID) {
		for (int i = 0; i < masterItemList.Count; i++) {
			if (toolType == masterItemList[i].itemType)
				if (masterItemList[i].localGroupID == specifiedID) 
					return masterItemList[i];
		}
		
		return null;
	}

	public static int GetNumberOfItemsInGame() {
		return masterItemList.Count;
	}

	public static int GetNumberOfProfessionsInGame() {
		return gameProfessions.Count;
	}

}
