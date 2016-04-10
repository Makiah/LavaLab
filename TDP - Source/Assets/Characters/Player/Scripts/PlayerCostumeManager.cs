
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script controls the initial player setup, such as instantiating the race components on to the player itself, then 
 * updating the item in use by the player.  This script controls item switching, as well as armor, etc.  Added by the Hotbar
 * Manager.  
 * 
 */


using UnityEngine;
using System.Collections;

public class PlayerCostumeManager : MonoBehaviour {

	/************************************************** INITIALIZATION **************************************************/

	void OnEnable() {
		InitializationSequence.InitializeCostume += InitializeSpriteChildren;
	}
	
	void OnDisable() {
		InitializationSequence.InitializeCostume -= InitializeSpriteChildren;
	}


	/************************************************ COSTUME MANAGEMENT ************************************************/

	//Main player action
	private PlayerAction mainPlayerAction;
	//SpriteRenderer child components
	private SpriteRenderer head, body, idleArm, idleHand, occupiedArm, occupiedHand, topLeg, topFoot, bottomLeg, bottomFoot;
	//The prefab of the item will be childed to this object.  
	private Transform item;

	void InitializeSpriteChildren() {
		mainPlayerAction = transform.parent.parent.gameObject.GetComponent <PlayerAction> ();
		//Just setting up the basic costume.  
		body = transform.FindChild("Body").GetComponent <SpriteRenderer> ();
		head = transform.FindChild ("Head").GetComponent <SpriteRenderer> ();
		idleArm = transform.FindChild ("Arms").FindChild ("Idle Arm").GetComponent <SpriteRenderer> ();
		idleHand = idleArm.transform.FindChild ("Hand").GetComponent <SpriteRenderer> ();
		occupiedArm = transform.FindChild ("Arms").FindChild ("Occupied Arm").GetComponent <SpriteRenderer> ();
		occupiedHand = occupiedArm.transform.FindChild ("Hand").GetComponent <SpriteRenderer> ();
		topLeg = transform.FindChild ("Legs").FindChild ("Top Leg").GetComponent <SpriteRenderer> ();
		topFoot = topLeg.transform.FindChild ("Foot").GetComponent <SpriteRenderer> ();
		bottomLeg = transform.FindChild ("Legs").FindChild("Bottom Leg").GetComponent <SpriteRenderer> ();
		bottomFoot = bottomLeg.transform.FindChild ("Foot").GetComponent <SpriteRenderer> ();
		item = occupiedHand.transform.FindChild ("Item");

		Profession currentPlayerProfession = GameData.GetPlayerProfession ();
		UpdatePlayerProfession (currentPlayerProfession);
	}

	//Used when a player profession is changed.  
	public void UpdatePlayerProfession(Profession profession) {
		//Update with common gender sprites
		body.sprite = profession.body;
		idleArm.sprite = profession.arm1;
		idleHand.sprite = profession.arm2;
		occupiedArm.sprite = profession.arm1;
		occupiedHand.sprite = profession.arm2;
		topLeg.sprite = profession.leg1;
		topFoot.sprite = profession.leg2;
		bottomLeg.sprite = profession.leg1;
		bottomFoot.sprite = profession.leg2;

		//Gender check.  
		if (GameData.GetChosenGender() == GameData.Gender.MALE) {
			head.sprite = profession.maleHead;
		} else {
			head.sprite = profession.femaleHead;
		}

		//Add the initial items for the profession to the inventory.  
		if (profession.initialObjects != null) {
			for (int i = 0; i < profession.initialObjects.Length; i++) {
				PlayerInventory.instance.AssignNewItemToBestSlot (profession.initialObjects [i]);
			}
		}
	}

	//Called by HotbarManager when a new hotbar item is selected.
	public void UpdatePlayerItem(GameObject prefabSelectedInHotbar) {

		//Deletes the previous item that had existed before this new item.  
		if (item.childCount != 0) {
			if (item.childCount > 1) {
				Debug.Log("There was more than one object being held by the player.");
			}

			for (int i = 0; i < item.childCount; i++) {
				Destroy (item.GetChild (i).gameObject);
			}
		}

		//Instantiating the new item (even if the new item is null).  
		if (prefabSelectedInHotbar != null) {
			GameObject createdItem = (GameObject)Instantiate (prefabSelectedInHotbar);
			createdItem.transform.SetParent (item);
			createdItem.transform.localPosition = Vector2.zero; 
			createdItem.transform.localScale = new Vector3(prefabSelectedInHotbar.transform.localScale.x, prefabSelectedInHotbar.transform.localScale.y, 1);//transform.parent.localScale * createdItem.transform.localScale;
			createdItem.transform.localRotation = transform.parent.localRotation;

			if (prefabSelectedInHotbar.GetComponent <Item> () != null) {
				prefabSelectedInHotbar.GetComponent <Item> ().SetAttachedCharacterInput (mainPlayerAction);
				mainPlayerAction.OnRefreshCurrentWeaponMoves (prefabSelectedInHotbar.GetComponent <Item> ());
			} else {
				mainPlayerAction.OnRefreshCurrentWeaponMoves (null);
			}

		} else {
			mainPlayerAction.OnRefreshCurrentWeaponMoves(null);
		}

	}

}

