using UnityEngine;
using System.Collections;

public class Turret : Enemy, IMethodReroute1 {

	//Have to be taken from the resources folder.  
	private static GameObject turret;
	private static GameObject rollingTurret;

	public enum TurretPosition
	{
		TOP,
		BOTTOM, 
		ROLLING
	}

	public static Turret Create (float xLocation, TurretPosition position) {
		//Get the turret from the Resources folder.  
		if (turret == null) {
			turret = Resources.Load ("Enemies/Turret/Turret") as GameObject;
			if (turret == null)
				Debug.LogError ("Enemies/Turret/Turret could not be loaded!");
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
			case TurretPosition.ROLLING: 
				//These guys can pretty much go anywhere and have a rigidbody 2d, so it doesn't really matter where they goe.  
				yLocation = -1f;
				break;
			}
			//Instantiate the turret
			GameObject createdTurret = (GameObject)(Instantiate (turret, new Vector3 (xLocation, yLocation, 0), Quaternion.identity));
			//If the turret is on the turret, rotate the turret mount.  
			if (position == TurretPosition.TOP)
				createdTurret.transform.localScale = new Vector3 (1, -1, 1);
			//Initialize the turret.  
			createdTurret.GetComponent <Turret> ().InitializeCharacter ();
			//Return the turret.  
			return createdTurret.GetComponent <Turret> ();
		} else {
			Debug.LogError ("Turret does not exist!");
			return null;
		}
	}

	//Instance variables.  
	[SerializeField] private Sprite boltSprite = null;
	private Color boltColor = Color.black;
	private float fireRate = 5;
	private float fireSpeed = 5;

	protected override void InitializeEnemy() {
		//Change fireRate and bolt color depending on the current level.  
		switch (LevelGenerator.instance.currentLevel) {
		case 0: 
			boltColor = Color.red;
			fireRate = 4;
			break;
		default: 
			boltColor = Color.black;
			fireRate = 5;
			break;
		}
	}

	protected override IEnumerator EnemyControl() {
		StartCoroutine (PointAtPlayer());
		StartCoroutine (FireRepeatedly ());
		yield return null;
	}

	//Always point at the player. 
	protected IEnumerator PointAtPlayer() {
		while (true) {
			//Avoid boxing and unboxing.  
			Vector2 directionVector;
			float zVal;
			Vector3 initialScale = transform.localScale;

			if (Vector2.Distance (player.position, transform.position) < 20) {
				//Calculate the direction vector, and normalize it (make 1 the largest value, and scale the opposite value appropriately).  
				directionVector = player.position - transform.position;
				directionVector.Normalize ();
				//Calculate the correct direction to point.  
				zVal = Mathf.Atan2 (directionVector.y, directionVector.x) * Mathf.Rad2Deg;
//				if (zVal < 0)
//					transform.localScale = new Vector3 (initialScale.x * -1, initialScale.y, initialScale.z);
//				else if (zVal >= 0)
//					transform.localScale = new Vector3 (initialScale.x, initialScale.y, initialScale.z);
				transform.GetChild (0).rotation = Quaternion.Euler (0f, 0f, -zVal);
			}
			yield return null;
		}
	}

	protected IEnumerator FireRepeatedly() {
		Animator anim = transform.GetChild (0).GetComponent <Animator> ();
		while (true) {
			if (Vector2.Distance (player.position, transform.position) < 20) {
				anim.SetTrigger ("Shoot");
			}
			yield return new WaitForSeconds (fireRate);
		}
	}

	//Used for the animator.  
	public void ReRoute1() {
		Attack ();
	}

	protected override void Attack() {
		Projectile bolt = Projectile.Create (boltSprite, transform.position);
		bolt.transform.GetChild (0).GetComponent <SpriteRenderer> ().color = boltColor;
		bolt.Initialize (transform.GetChild (0).localRotation.z, fireSpeed, enemyAttackingPower);
	}

}
