using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterHealthPanelManager))]

public class RotatingTurret : Enemy {

	//Have to be taken from the resources folder.  
	private static GameObject turret;
	private static GameObject rollingTurret;

	public enum TurretPosition
	{
		TOP,
		BOTTOM
	}

	public static RotatingTurret Create (float xLocation, TurretPosition position) {
		//Get the turret from the Resources folder.  
		if (turret == null) {
			turret = Resources.Load ("Enemies/RotatingTurret/RotatingTurret") as GameObject;
			if (turret == null)
				Debug.LogError ("Enemies/RotatingTurret/RotatingTurret could not be loaded!");
		}

		//Determine the y location of the turret based on the position.  
		if (turret != null) {
			float yLocation = 0;
			switch (position) {
			case TurretPosition.BOTTOM: 
				yLocation = -3.2f;
				break;
			case TurretPosition.TOP: 
				yLocation = 3.2f;
				break;
			}
			//Instantiate the turret
			GameObject createdTurret = (GameObject)(Instantiate (turret, new Vector3 (xLocation, yLocation, 0), Quaternion.identity));
			//If the turret is on the turret, rotate the turret mount.  
			createdTurret.GetComponent <RotatingTurret> ().SetPosition (position);
			//Initialize the turret.  
			createdTurret.GetComponent <RotatingTurret> ().InitializeCharacter ();
			//Return the turret.  
			return createdTurret.GetComponent <RotatingTurret> ();
		} else {
			Debug.LogError ("RotatingTurret does not exist!");
			return null;
		}
	}

	//Instance variables.  
	[SerializeField] private Sprite boltSprite = null;
	//private Color boltColor = Color.black;
	//Less value means faster.  
	private float fireRate = 3;
	//More value means faster.  
	private float fireSpeed = 8;
	private float fireThreshold = 45;
	private TurretPosition localPosition;

	protected override void InitializeEnemy() {
		//Change fireRate and bolt color depending on the current level.  
		fireRate = Mathf.Clamp((fireRate) / (1 + (Mathf.Log (LevelGenerator.instance.currentLevel) / 6f)), .1f, 30);
		fireSpeed = Mathf.Clamp(fireSpeed * (1f + (Mathf.Log (LevelGenerator.instance.currentLevel) / 6f)), .1f, 30);
	}

	//Used for when the static Create method wants to set the local position for the turret.  
	public void SetPosition(TurretPosition position) {
		localPosition = position;
		if (localPosition == TurretPosition.TOP)
			transform.localScale = new Vector3 (transform.localScale.x, -transform.localScale.y, transform.localScale.z);
	}

	protected override IEnumerator EnemyControl() {
		StartCoroutine (PointAtPlayer());
		StartCoroutine (FireRepeatedly ());
		yield return null;
	}

	//Always point at the player. 
	private IEnumerator PointAtPlayer() {
		//Avoid boxing and unboxing.  
		Vector2 directionVector;
		float zVal = 0;
		Vector3 initialScale = transform.localScale;
		Vector3 flippedScale = new Vector3 (initialScale.x * -1, initialScale.y, initialScale.z);

		bool facingRight = true;

		//Constantly
		while (true) {
			if (Vector2.Distance (player.position, transform.position) < 20) {
				//Calculate the direction vector, and normalize it (make 1 the largest value, and scale the opposite value appropriately).  
				directionVector = player.position - transform.position;
				directionVector.Normalize ();

				//Check the direction of the 
				if (directionVector.x < 0) {
					transform.localScale = flippedScale;
					facingRight = false;
				} else {
					transform.localScale = initialScale;
					facingRight = true;
				}

				//Calculate the correct direction to point.  
				zVal = Mathf.Atan2 (directionVector.y, directionVector.x) * Mathf.Rad2Deg;

				//Flip it if the turret is on top.  
				if (localPosition == TurretPosition.TOP)
					zVal *= -1;

				//Calculate the actual value based off of the trig calculated previously.  
				if (facingRight)
					zVal *= -1;
				else
					zVal -= 180;

				zVal = Mathf.Clamp (zVal, -fireThreshold, 0);

				//HAS to be Quaternion.Euler, not the other way.  
				transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, zVal);
			}

			yield return null;
		}
	}

	private IEnumerator FireRepeatedly() {
		Animator anim = transform.GetChild (0).GetComponent <Animator> ();
		while (true) {
			if (Vector2.Distance (player.position, transform.position) < 20) {
				anim.SetTrigger ("Shoot");
				Action1 += Attack;
				yield return new WaitForSeconds (fireRate);
			}
			yield return null;
		}
	}

	protected override void Attack() {
		Transform shooter = transform.GetChild (0).GetChild (0), target = shooter.GetChild (0);
		Projectile bolt = Projectile.Create (boltSprite, shooter.position, GetCombatantID());
		//bolt.transform.GetChild (0).GetComponent <SpriteRenderer> ().color = boltColor;
		bolt.SetLightState (true);
		bolt.Initialize (target.position, fireSpeed, enemyAttackingPower);
	}

}