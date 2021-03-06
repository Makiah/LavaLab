﻿
/*
 * Author: Makiah Bennett
 * Created 27 September 2015
 * Last edited: 27 September 2015
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class MammothScript : Enemy {

	protected override void InitializeEnemy() {}

	protected override void Attack() {
		AttemptToAttackAfterCompletedAnimation ();
	}
	
	void AttemptToAttackAfterCompletedAnimation () {
		ActionsOnAttack += AttackEnemyInFocus;
	}
	
	void AttackEnemyInFocus () {
		//Use the RaycastAttackUtilites class.  
		CharacterHealthPanelManager resultingHealthPanelManager = LinecastingUtilities.FindEnemyViaLinecast (
			transform.position, 
			distToEnemyLength, 
			yOffsetToEnemy, 
			enemyWithinAreaBounds, 
			GetFacingDirection(), 
			GetCombatantID()
		);

		if (resultingHealthPanelManager != null) {
			resultingHealthPanelManager.gameObject.GetComponent <Character> ().ApplyKnockback (new Vector2 (enemyKnockbackPower.x * GetFacingDirection (), enemyKnockbackPower.y));
			resultingHealthPanelManager.YouHaveBeenAttacked (enemyAttackingPower);
		}

	}
	
}
