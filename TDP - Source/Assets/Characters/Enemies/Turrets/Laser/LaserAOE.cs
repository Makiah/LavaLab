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

	//The coroutine that will damage the player once every half second.  
	//NOTE: THIS WILL damage the turret unless the collider is not touching it.  
	IEnumerator DamagePerSecond() {
		if (aoe != null) {
			int loop = 0;
			while (true) {
				foreach (ICombatant iC in AOEUtilities.GetComponentsInArea <ICombatant> (aoe)) {
					if (iC.GetCombatantID ().Equals (GetComponentInParent <ICombatant> ().GetCombatantID ()) == false) {
						iC.GetHealthController ().YouHaveBeenAttacked (6);
						Debug.Log ("Damaged " + iC.GetActualClass ().gameObject.name);
					}
				}
				loop++;
				//Wait a quarter of a second.  
				yield return new WaitForSeconds(.25f);
			}
		} else {
			Debug.LogError ("Laser AOE was null!");
		}
	}
}
