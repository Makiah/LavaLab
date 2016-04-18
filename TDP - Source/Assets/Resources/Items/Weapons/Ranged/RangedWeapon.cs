using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CatchCo;

public class RangedWeapon : Item {
	//The only piece of information required for the weapon besides the collider.  
	[SerializeField] private Sprite projectileSprite = null;
	[SerializeField] private int damage = 5, speed = 5;
	private Transform shooter, target;

	[ExposeMethodInEditor]
	public override void InitializeItem() {
		if (transform.FindChild ("Shooter") != null) {
			shooter = transform.FindChild ("Shooter");
		} else {
			if (transform.GetChild (0) != null) {
				shooter = transform.GetChild (0);
			} else {
				Debug.Log ("There is no shooter transform on object " + gameObject.name + " using object instead");
				shooter = transform;
			}
		}

		if (shooter != null) {
			if (shooter.FindChild ("Target") != null) {
				target = shooter.FindChild ("Target");
			} else if (shooter.GetChild(0) != null) {
				target = shooter.GetChild (0);
			}
		} 

		if (target == null) {
			Debug.Log ("Since there is no shooter on object " + gameObject.name + " using 1 space to the right of the object as target");
			target = new GameObject ("Target").transform;
			target.SetParent (shooter);
			target.localPosition = new Vector3 (1, 0, 0);
		}
	}

	public override MovementAndMethod[] GetPossibleActionsForItem () {
		if (movementTriggerPair != null && movementTriggerPair.Length > 0) {
			MovementAndMethod[] moves = new MovementAndMethod[movementTriggerPair.Length];
			//Convert the whole array into the movement and method thing.  The constructor takes care of the initialization.  
			for (int i = 0; i < moves.Length; i++) {
				moves [i] = new MovementAndMethod (movementTriggerPair [i].attack, movementTriggerPair [i].method, movementTriggerPair [i].worksMidair);
			}
			//Return the created array.  
			return moves;

		} else {
			Debug.Log ("Weapon " + gameObject.name + " does not have any moves or triggers!");
			return null;
		}
	}

	//Called when something is supposed to happen.  
	public override void InfluenceEnvironment(MovementAndMethod.PossibleMovements actionKey) {
		attachedCharacterInput.GetActualClass ().Action1 += Shoot;
	}

	[ExposeMethodInEditor]
	private void Shoot() {
		Projectile projectile = Projectile.Create(projectileSprite, shooter.position, attachedCharacterInput.GetCombatantID());
		projectile.Initialize (target.position, speed, damage);
	}

}
