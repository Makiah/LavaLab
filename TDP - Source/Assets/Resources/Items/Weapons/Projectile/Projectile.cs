
/*
 * Author: Makiah Bennett
 * Created 14 September 2015
 * Last edited: 14 September 2015
 * 
 * 9/14 - Created.  Should have a constructor with velocity, heading, and other projectile properties.  
 * 
 * This class should manage all functions relating to being attacked, moving as a result of being attacked, UI functions
 * such as health bars, and any other examples that can be used.  
 * 
 * 
 */


using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public class Projectile : MonoBehaviour {
	
	//Static properties.  
	private static GameObject basicProjectile = null;

	public static Projectile Create(Sprite sprite, Vector3 location) {
		//Attempt to load the basic projectile prefab since Unity won't serialize static variables).  
		if (basicProjectile == null) {
			basicProjectile = Resources.Load ("Items/Weapons/Projectile/Projectile") as GameObject;
			if (basicProjectile == null)
				Debug.LogError ("Failed to load \"Items/Weapons/Projectile/Projectile\"");
		}

		//Check to make sure that the projectile requirements are satisfied.  
		if (basicProjectile != null) {
			GameObject instantiatedProjectile = (GameObject)(Instantiate (basicProjectile, location, Quaternion.identity));
			instantiatedProjectile.transform.GetChild (0).GetComponent <SpriteRenderer> ().sprite = sprite;
			instantiatedProjectile.GetComponent <BoxCollider2D> ().size = sprite.bounds.size;
			return instantiatedProjectile.GetComponent <Projectile> ();
		} else {
			Debug.LogError ("Basic Projectile is null!");
			return null;
		}
	}

	public static Projectile Create(Sprite sprite, Vector3 location, string guidToIgnore) {
		Projectile p = Create (sprite, location);
		p.ignoreGUID = guidToIgnore;
		return p;
	}

	//Instance variables
	protected Rigidbody2D rb2d;
	private bool notificationSent = false;
	protected float power;
	protected GameObject playerObject;
	public string ignoreGUID = "";

	public virtual void Initialize(Vector2 positionToFireToward, float speed, float power) {
		playerObject = InstanceDatabase.GetPlayerReference ();
		//Get the Rigidbody component so that physics can be used.  
		rb2d = GetComponent <Rigidbody2D> ();

		//Calculate the heading to the fire location.  
		float radianAngleToTarget = Mathf.Atan2 ((positionToFireToward.y - transform.position.y) , (positionToFireToward.x - transform.position.x));
		float degreeAngleToTarget = radianAngleToTarget * Mathf.Rad2Deg;

		//Turn to the heading and move in that direction.  
		transform.localRotation = Quaternion.Euler(new Vector3 (0, 0, degreeAngleToTarget));
		rb2d.velocity = new Vector2 (speed * Mathf.Cos (ScriptingUtilities.DegreesToRadians(degreeAngleToTarget)), speed * Mathf.Sin (ScriptingUtilities.DegreesToRadians(degreeAngleToTarget)));

		//Set the strength of the arrow.  
		this.power = power;

		//Start the coroutine that checks when the arrow should be destroyed (takes up memory space)
		StartCoroutine (DestroyIfDistanceFromPlayer());
	}

	//If the distance to the player is too large, then destroy the projectile (used to avoid memory loss, but could be disabled for a more accurate setting).  
	IEnumerator DestroyIfDistanceFromPlayer() {
		while (true) {
			if (Vector2.Distance (transform.position, playerObject.transform.position) > 20) {
				Destroy (this.gameObject);
			}
			yield return null;
		}
	}

	//When something hits the projectile (or the projectile hits something)
	void OnTriggerEnter2D (Collider2D externalTrigger) {
		//Even this boolean seems pointless, it is actually required.  Destroy() does not destroy the object on that frame, and DestroyImmediate causes
		//strange side effects, so this is an easier way of dealing with the issue.  
		//For some reason, I have to check to make sure that the collider is a trigger (even though the method header already implies this.  
		if (notificationSent == false) {
			//GetComponentInParent checks recursively to find the desired component (really useful)
			GameObject externalGameObject = externalTrigger.gameObject;
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

				//Make sure that we are not attacking ourself.  
				if (combatant.GetCombatantID ().Equals (ignoreGUID) == false) {
					//Attack the other combatant.  
					combatant.GetHealthController ().YouHaveBeenAttacked (power);
					notificationSent = true;
					Destroy (this.gameObject);
				}
			}
		} 

		if (notificationSent == false && externalTrigger.gameObject.layer == LayerMask.NameToLayer("Ground")) {
			Destroy (this.gameObject);
		}
	}

}
