using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterHealthPanelManager))]

public abstract class Turret : Enemy {

	public enum TurretPosition
	{
		TOP,
		BOTTOM
	}

	//Instance variables.  
	[SerializeField] protected Sprite boltSprite = null;
	//private Color boltColor = Color.black;
	//Less value means faster.  
	protected float fireRate = 3;
	//More value means faster.  
	protected float fireSpeed = 8;
	protected TurretPosition localPosition;
	protected Transform shooter, target, laser;

	protected override void InitializeEnemy() {
		//Change fireRate and bolt color depending on the current level.  
		fireRate = Mathf.Clamp((fireRate) / (1 + (Mathf.Log (LevelGenerator.instance.currentLevel) / 6f)), .1f, 30);
		fireSpeed = Mathf.Clamp(fireSpeed * (1f + (Mathf.Log (LevelGenerator.instance.currentLevel) / 6f)), .1f, 30);
		//Declare these individually.  
		shooter = transform.FindChild("Shooter"); 
		target = shooter.GetChild (0);
		laser = transform.FindChild ("Laser");
		//Disable the laser.  
		SetLaserState(false);
	}

	//Used for when the static Create method wants to set the local position for the turret.  
	public virtual void SetPosition(TurretPosition position) {
		localPosition = position;
		if (localPosition == TurretPosition.TOP)
			transform.localScale = new Vector3 (transform.localScale.x, -transform.localScale.y, transform.localScale.z);
	}

	protected override IEnumerator EnemyControl() {
		StartCoroutine(FireRepeatedly ());
		yield return null;
	}

	private IEnumerator FireRepeatedly() {
		//Get the animator component.  
		Animator anim = transform.GetChild (0).GetComponent <Animator> ();

		//Count the number of bullet shots and use a laser depending on the number.  
		int bulletShots = 0;

		while (true) {
			//Every so often, the turret should fire a laser.  
			if (Vector2.Distance (player.position, transform.position) < 20) {
				if (bulletShots >= 4 && Random.Range (0, 2) == 0 && LevelGenerator.instance.currentLevel > 2) {
					yield return new WaitForSeconds (.75f);
					bulletShots = 0;
					SetLaserState (true);
					yield return new WaitForSeconds (1.5f);
					SetLaserState (false);
				} else {
					anim.SetTrigger ("Shoot");
					Action1 += Attack;
					bulletShots++;
					yield return new WaitForSeconds (fireRate);
				}
			}
			yield return null;
		}
	}

	protected void SetLaserState(bool state) {
		laser.gameObject.SetActive (state);
		if (state)
			laser.GetComponent <LaserAOE> ().EnableLaserDamage ();
		else
			laser.GetComponent <LaserAOE> ().DisableLaserDamage ();
	}

	protected override void Attack() {
		Projectile bolt = Projectile.Create (boltSprite, shooter.position, GetCombatantID());
		//bolt.transform.GetChild (0).GetComponent <SpriteRenderer> ().color = boltColor;
		bolt.Initialize (target.position, fireSpeed, enemyAttackingPower);
	}

}