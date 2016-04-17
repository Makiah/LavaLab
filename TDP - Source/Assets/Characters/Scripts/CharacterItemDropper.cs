
/*
 * Author: Makiah Bennett
 * Last edited: 8 October 2015
 * 
 * This script is the base class for all objects that drop items when "killed", which includes trees (wood), 
 * boars (meat), and skeletons (bones, exp, coins).  These drops should be defined through the DropReferenceClass.
 * 
 * 10/8 - Added OnEnable and OnDisable to the base class, and made MakeReferences an abstract function.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterItemDropper : MonoBehaviour {

	void OnEnable() {
		InitializationSequence.InitializeEnemies += MakeReferences;
	}

	void OnDisable() {
		InitializationSequence.InitializeEnemies -= MakeReferences;
	}

	protected List <DropReferenceClass> drops;

	private DropReferenceClass expDrop, coinDrop;

	[SerializeField] protected int experienceToDrop = 0, cashToDrop = 0;

	private void MakeReferences () {
		//Define the default elements.  
		expDrop = new DropReferenceClass(ResourceDatabase.GetItemByParameter ("ExpNodule"), 1, 1, 1);
		coinDrop = new DropReferenceClass (ResourceDatabase.GetItemByParameter ("Coin"), 1, 1, 1);
	}

	//The character has to call this method to change the drops.  
	public void DefineDrops(List <DropReferenceClass> drops) {
		this.drops = drops;
	}

	//Drops all items around the enemy or character when some event is triggered.  
	public void DropItems() {
		if (drops != null) {
			for (int i = 0; i < drops.Count; i++) {
				if (Random.Range (0, drops [i].probabilityToDrop) == 0) {
					for (int q = 0; q < Random.Range(drops[i].minToDrop, drops[i].maxToDrop + 1); q++) {
						if (drops[i].dropReference != null) {
							Drop.Create(new ResourceReferenceWithStack(drops[i].dropReference, 1), transform.position, 0);
						} else {
							Debug.Log("DropReference " + i + " was null!!! (DropsItems)");
						}
					}
				}
			}
		} else {
			Debug.Log("Drops were null (DropsItems)");
		}

		if (experienceToDrop > 0) {
			for (int i = 0; i < experienceToDrop; i++) {
				Drop.Create (new ResourceReferenceWithStack (expDrop.dropReference, 1), transform.position, 0);
			}
		} else {
			Debug.Log("Did not drop any experience, experience to drop was 0. (DropsItems)");
		}

		if (cashToDrop > 0) {
			for (int i = 0; i < cashToDrop; i++) {
				Drop.Create (new ResourceReferenceWithStack (coinDrop.dropReference, 1), transform.position, 0);
			}
		} else {
			Debug.Log("Did not drop any experience, experience to drop was 0. (DropsItems)");
		}

	}

}
