using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Required components (should be on the GameObject)
[RequireComponent(typeof(Collider2D))]

public class MeleeWeapon : Item {

	//Taken from dota (area of effect)
	private Collider2D aoe;
	//The only piece of information required for the weapon besides the collider.  
	[SerializeField] private int damage = 5;

	public override void InitializeItem() {
		aoe = GetComponent <Collider2D> ();
		aoe.enabled = false;
	}

	public override MovementAndMethod[] GetPossibleActionsForItem () {
		if (movementTriggerPair != null && movementTriggerPair.Length > 0) {
			MovementAndMethod[] moves = new MovementAndMethod[movementTriggerPair.Length];
			//Convert the whole array into the movement and method thing.  The constructor takes care of the initialization.  
			for (int i = 0; i < moves.Length; i++)
				moves [i] = new MovementAndMethod (movementTriggerPair [i].attack, movementTriggerPair [i].method, movementTriggerPair [i].worksMidair);
			//Return the created array.  
			return moves;
		} else {
			Debug.Log ("Weapon " + gameObject.name + " does not have any moves or triggers!");
			return null;
		}
	}

	//Called when something is supposed to happen.  
	public override void InfluenceEnvironment(MovementAndMethod.PossibleMovements actionKey) {
		//Go through each collider and see whether it is a fighting character.  
		foreach (ICombatant combatant in AOEUtilities.GetComponentsInArea <ICombatant> (aoe))
			if (combatant != null && combatant.GetCombatantID().Equals(attachedCharacterInput.GetCombatantID()) == false)
				combatant.GetHealthController ().YouHaveBeenAttacked (damage);
	}

}
