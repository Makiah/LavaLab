using UnityEngine;
using System.Collections;

public class Turret : Enemy {

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
		while (true) {
			if (Vector2.Distance (player.position, transform.position) < 20) {

			} else {
				yield return null;
			}
		}
	}

	protected override void Attack() {
		Projectile bolt = Projectile.Create (boltSprite, transform.position);
		bolt.transform.GetChild (0).GetComponent <SpriteRenderer> ().color = boltColor;
		bolt.Initialize (transform.GetChild (0).localRotation.z, fireSpeed, enemyAttackingPower);
	}

}
