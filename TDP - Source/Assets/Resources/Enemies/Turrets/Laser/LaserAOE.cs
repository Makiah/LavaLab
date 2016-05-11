using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserAOE : MonoBehaviour {
	private BoxCollider2D aoe;
	private IEnumerator dpsCoroutine;

	public void EnableLaserDamage() {
		aoe = GetComponent <BoxCollider2D> ();
		if (dpsCoroutine == null) {
			dpsCoroutine = DamagePerSecond ();
			StartCoroutine (dpsCoroutine);
		} else {
			Debug.LogError ("The laser cannot be started a second time!");
		}
	}

	public void DisableLaserDamage() {
		if (dpsCoroutine != null) {
			StopCoroutine (dpsCoroutine);
			dpsCoroutine = null;
		}
	}
		
	//Will hold all of the ICombatants that the laser trigger sees.  
	private List <ICombatant> affectedByLaser = new List <ICombatant> ();

	void OnTriggerEnter2D(Collider2D other) {
		GameObject externalGameObject = other.gameObject;
		if (externalGameObject.layer == LayerMask.NameToLayer ("Fighting")) {
			ICombatant combatant;
			if (externalGameObject.GetComponent <ICombatant> () != null)
				combatant = externalGameObject.GetComponent <ICombatant> ();
			else if (externalGameObject.transform.parent != null && externalGameObject.GetComponentInParent <ICombatant> () != null)
				combatant = externalGameObject.GetComponentInParent <ICombatant> ();
			else {
				//Exit if no ICombatant is present.  
				Debug.Log ("No ICombatant");
				return;
			}
			//Attack the other combatant.  
			if (combatant != null) {
				Debug.Log("Added " + combatant.GetActualClass().gameObject.name);
				affectedByLaser.Add (combatant);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		GameObject externalGameObject = other.gameObject;
		if (externalGameObject.layer == LayerMask.NameToLayer ("Fighting")) {
			ICombatant combatant;
			if (externalGameObject.GetComponent <ICombatant> () != null)
				combatant = externalGameObject.GetComponent <ICombatant> ();
			else if (externalGameObject.transform.parent != null && externalGameObject.GetComponentInParent <ICombatant> () != null)
				combatant = externalGameObject.GetComponentInParent <ICombatant> ();
			else {
				//Exit if no ICombatant is present.  
				Debug.Log ("No ICombatant");
				return;
			}
			//Attack the other combatant.  
			if (affectedByLaser.Contains (combatant)) {
				Debug.Log ("Removed " + combatant.GetActualClass ().gameObject.name);
				affectedByLaser.Remove (combatant);
			}
		}
	}

	//The coroutine that will damage the player once every half second.  
	IEnumerator DamagePerSecond() {
		if (aoe != null) {
			int loop = 0;
			while (true) {
				foreach (ICombatant iC in affectedByLaser) {
					iC.GetHealthController ().YouHaveBeenAttacked (6);
					Debug.Log ("Damaged " + iC.GetActualClass ().gameObject.name);
				}
				loop++;
				//Wait half a second.  
				yield return new WaitForSeconds(.25f);
			}
		} else {
			Debug.LogError ("Laser AOE was null!");
		}
	}
}
