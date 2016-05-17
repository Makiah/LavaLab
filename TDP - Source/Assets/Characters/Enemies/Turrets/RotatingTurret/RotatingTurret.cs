using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterHealthPanelManager))]

public class RotatingTurret : Turret {

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

		//Setting the turret position randomly.  
		if (Random.Range (0, 2) == 0)
			SetPosition (TurretPosition.BOTTOM);
		else
			SetPosition (TurretPosition.TOP);
		
		//Disable the laser.  
		SetLaserState(false);
	}

	protected override IEnumerator EnemyControl() {
		StartCoroutine (PointAtPlayer());
		StartCoroutine(base.EnemyControl ());
		yield return null;
	}

	public override void SetPosition(TurretPosition position) {
		base.SetPosition (position);
		if (localPosition == TurretPosition.TOP)
			transform.localScale = new Vector3 (transform.localScale.x, -transform.localScale.y, transform.localScale.z);
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