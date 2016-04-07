
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
using CatchCo;

public class Projectile : MonoBehaviour {
	
	//Static properties.  
	[SerializeField] private static GameObject basicProjectile = null;

	public static Projectile Create(Sprite sprite, Vector3 location) {
		//Attempt to load the basic projectile prefab since Unity won't serialize static variables).  
		if (basicProjectile == null) {
			basicProjectile = Resources.Load ("Weapons/Projectile/Projectile") as GameObject;
			if (basicProjectile == null)
				Debug.LogError ("Failed to load \"Weapons/Projectile/Projectile\"");
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

	//Instance variables
	protected Rigidbody2D rb2d;
	private bool notificationSent = false;
	protected float power;
	protected GameObject playerObject;

	public virtual void Initialize(Vector3 positionToFireToward, float velocity, float power) {
		//Get the Rigidbody component so that physics can be used.  
		rb2d = GetComponent <Rigidbody2D> ();

		//Calculate the heading to the fire location.  
		float radianAngleToTarget = Mathf.Atan2 ((positionToFireToward.y - transform.position.y) , (positionToFireToward.x - transform.position.x));
		float degreeAngleToTarget = ScriptingUtilities.RadiansToDegrees (radianAngleToTarget);

		//Turn to the heading and move in that direction.  
		transform.localRotation = Quaternion.Euler(new Vector3 (0, 0, degreeAngleToTarget));
		rb2d.velocity = new Vector2 (velocity * Mathf.Cos (ScriptingUtilities.DegreesToRadians(degreeAngleToTarget)), velocity * Mathf.Sin (ScriptingUtilities.DegreesToRadians(degreeAngleToTarget)));

		//Set the strength of the arrow.  
		this.power = power;

		//Start the coroutine that checks when the arrow should be destroyed (takes up memory space)
		StartCoroutine (DestroyIfDistanceFromPlayer());
	}

	public virtual void Initialize(float degree, float velocity, float power) {
		//Get the Rigidbody component so that physics can be used.  
		rb2d = GetComponent <Rigidbody2D> ();

		//Turn to the heading and move in that direction.  
		transform.localRotation = Quaternion.Euler(new Vector3 (0, 0, degree));
		rb2d.velocity = new Vector2 (velocity * Mathf.Cos (ScriptingUtilities.DegreesToRadians(degree)), velocity * Mathf.Sin (ScriptingUtilities.DegreesToRadians(degree)));

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

	void OnTriggerEnter2D (Collider2D externalTrigger) {
		//Make sure that the second parent of the transform exists.  
		if (externalTrigger.transform.parent != null && externalTrigger.transform.parent.parent != null) {
			//Check to see whether it exists.  
			if (externalTrigger.transform.parent.parent.GetComponent <CharacterHealthPanelManager> () != null && notificationSent == false) {
				externalTrigger.transform.parent.parent.GetComponent <CharacterHealthPanelManager> ().YouHaveBeenAttacked (power);
				notificationSent = true;
				Destroy (this.gameObject);
			}
		}

	}

}
