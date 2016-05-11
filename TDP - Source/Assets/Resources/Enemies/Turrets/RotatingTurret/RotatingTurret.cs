using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterHealthPanelManager))]

public class RotatingTurret : Turret {

	//Have to be taken from the resources folder.  
	private static GameObject turret;

	public static RotatingTurret Create (float xLocation, Turret.TurretPosition position) {
		//Get the turret from the Resources folder.  
		if (turret == null) {
			turret = Resources.Load ("Enemies/Turrets/RotatingTurret/RotatingTurret") as GameObject;
			if (turret == null)
				Debug.LogError ("Enemies/Turrets/RotatingTurret/RotatingTurret could not be loaded!");
		}

		//Determine the y location of the turret based on the position.  
		if (turret != null) {
			float yLocation = 0;
			switch (position) {
			case Turret.TurretPosition.BOTTOM: 
				yLocation = -3.2f;
				break;
			case Turret.TurretPosition.TOP: 
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
	private float fireThreshold = 45;

	protected override void InitializeEnemy() {
		//Change fireRate and bolt color depending on the current level.  
		fireRate = Mathf.Clamp((fireRate) / (1 + (Mathf.Log (LevelGenerator.instance.currentLevel) / 6f)), .1f, 30);
		fireSpeed = Mathf.Clamp(fireSpeed * (1f + (Mathf.Log (LevelGenerator.instance.currentLevel) / 6f)), .1f, 30);
		//Declare these individually.  
		shooter = transform.FindChild("Shooter Mount").FindChild("Shooter"); 
		target = shooter.GetChild (0);
		laser = transform.FindChild ("Shooter Mount").FindChild ("Laser");
		//Disable the laser.  
		SetLaserState(false);
	}

	protected override IEnumerator EnemyControl() {
		StartCoroutine (PointAtPlayer());
		StartCoroutine(base.EnemyControl ());
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
				if (localPosition == Turret.TurretPosition.TOP)
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

}