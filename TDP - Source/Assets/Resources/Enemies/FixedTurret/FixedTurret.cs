using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterHealthPanelManager))]

public class FixedTurret : Enemy {

	//Have to be taken from the resources folder.  
	private static GameObject turret;
	private static GameObject rollingTurret;

	public enum TurretPosition
	{
		TOP,
 		BOTTOM
	}

	public static FixedTurret Create (float xLocation, TurretPosition position) {
		//Get the turret from the Resources folder.  
		if (turret == null) {
			turret = Resources.Load ("Enemies/FixedTurret/FixedTurret") as GameObject;
			if (turret == null)
				Debug.LogError ("Enemies/FixedTurret/FixedTurret could not be loaded!");
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
			createdTurret.GetComponent <FixedTurret> ().SetPosition (position);
			//Initialize the turret.  
			createdTurret.GetComponent <FixedTurret> ().InitializeCharacter ();
			//Return the turret.  
			return createdTurret.GetComponent <FixedTurret> ();
		} else {
			Debug.LogError ("FixedTurret does not exist!");
			return null;
		}
	}

	//Instance variables.  
	[SerializeField] private Sprite boltSprite = null;
	//private Color boltColor = Color.black;
	//Less value means faster.  
	private float fireRate = 5;
	//More value means faster.  
	private float fireSpeed = 8;
	//Local position.  
	private TurretPosition localPosition;

	protected override void InitializeEnemy() {
		//Change fireRate and bolt color depending on the current level.  
		fireRate = Mathf.Clamp((fireRate) / (1 + (Mathf.Log (LevelGenerator.instance.currentLevel) / 6f)), .1f, 30) / 5f;
		fireSpeed = Mathf.Clamp(fireSpeed * (1f + (Mathf.Log (LevelGenerator.instance.currentLevel) / 6f)), .1f, 30) / 2f;
	}

	//Used for when the static Create method wants to set the local position for the turret.  
	public void SetPosition(TurretPosition position) {
		localPosition = position;
		if (localPosition == TurretPosition.BOTTOM)
			transform.localScale = new Vector3 (transform.localScale.x, -transform.localScale.y, transform.localScale.z);
	}

	protected override IEnumerator EnemyControl() {
		StartCoroutine (FireRepeatedly ());
		yield return null;
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
		Transform shooter = transform.GetChild (0), target = shooter.GetChild (0);
		Projectile bolt = Projectile.Create (boltSprite, shooter.position, GetCombatantID());
		//bolt.transform.GetChild (0).GetComponent <SpriteRenderer> ().color = boltColor;
		bolt.SetLightState (true);
		bolt.Initialize (target.position, fireSpeed, enemyAttackingPower);
	}

}