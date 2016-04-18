/*
 * This interface should be implemented by any class that can carry items and attack with them.  It inherits from ICombatant.  
 * 
 */


using UnityEngine;
using System.Collections;

public interface ICanHoldItems : ICombatant {
	void OnRefreshCurrentWeaponMoves(Item itemInUse);
	void ExternalJumpAction (int num);
}
