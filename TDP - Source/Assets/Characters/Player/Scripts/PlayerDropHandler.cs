
/*
 * Author: Makiah Bennett
 * Last edited: 11 September 2015
 * 
 * This script pretty much just sends a continual linecast in front of the player, and if an object is found and matches the specified
 * criteria, the script will then assign that item to the best available slot.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class PlayerDropHandler : MonoBehaviour {

	/************************************************** DROP HANDLER **************************************************/
	//When an item drop hits the player.  
	void OnTriggerEnter2D(Collider2D externalTrigger) {
		if (externalTrigger.gameObject.GetComponent <Drop> () != null && PlayerInventory.instance.IsInitialized())
			PickupItem (externalTrigger.gameObject.GetComponent <Drop> ());
	}

	public void PickupItem(Drop item) {
		//This does not check the resourcereference property of the attached script as a comparison, only the tag.  Consider changing later.  
		if (item.gameObject.CompareTag ("ExpNodule")) {
			transform.parent.gameObject.GetComponent <PlayerHealthPanelManager> ().OnExperienceNodulePickedUp ();
			Destroy (item.gameObject);
		} else if (item.gameObject.CompareTag ("Coin")) {
			transform.parent.gameObject.GetComponent <PlayerHealthPanelManager> ().OnCoinPickedUp(1);
			Destroy (item.gameObject);
		} else {
			ResourceReferenceWithStack pendingObject = item.localResourceReference;
			if (! PlayerInventory.instance.AssignNewItemToBestSlot(pendingObject)) {
				Debug.LogError("ERROR WHEN ASSIGNING OBJECT TO INVENTORY");
			} else {
				Destroy (item);
			}
		}
	}

}
